using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UI_LobbyPanel : UI_Controller
{
    const int MAXTURRETNUM = 23;
    const int MAXTURRETPRESETNUM = 8;
    const int MAXTCOLORWOOD = 6;
    const int MAXSTAGENUM = 23;

    //로비씬의 플레이어 스크립트
    LobbyPlayer lobbyPlayer;

    //터렛 이미지
    [SerializeField]
    Sprite[] turretSprite;

    //별 이미지
    [SerializeField]
    Sprite[] starSprite;

    //사운드 이미지
    [SerializeField]
    Sprite soundOnSprite;
    [SerializeField]
    Sprite soundOffSprite;

    enum Buttons
    {
        GameStartButton,    //게임시작버튼
        TurretSelectLeftArrowButton,    //터렛 선택 스크롤 <버튼
        TurretSelectRightArrowButton,    //터렛 선택 스크롤 >버튼
        BackLobbyMenuButton, //로비 메뉴로 돌아가기 버튼
        TurretButton0,  //터렛리스트 0~
        TurretButton1,
        TurretButton2,
        TurretButton3,
        TurretButton4,
        TurretButton5,
        TurretButton6,
        TurretButton7,
        TurretButton8,
        TurretButton9,
        TurretButton10,
        TurretButton11,
        TurretButton12,
        TurretButton13,
        TurretButton14,
        TurretButton15,
        TurretButton16,
        TurretButton17,
        TurretButton18,
        TurretButton19,
        TurretButton20,
        TurretButton21,
        TurretButton22, // ~22
        TurretRemoveButton0, //프리셋에 있는 터렛 삭제 버튼 0~
        TurretRemoveButton1,
        TurretRemoveButton2,
        TurretRemoveButton3,
        TurretRemoveButton4,
        TurretRemoveButton5,
        TurretRemoveButton6,
        TurretRemoveButton7, // ~7
        TurretPresetClearButton, //터렛 프리셋 비우기 버튼
        StageSelectLeftArrowButton,   //스테이지 선택 스크롤 <버튼
        StageSelectRightArrowButton,    //스테이지 선택 스크롤 >버튼
        StageStartButton,    //게임신 시작 버튼
        StageMapCloseButton,    //스테이지 선택 지도 끄기 버튼
        StageSelectButton,   //스테이지 선택 지도 켜기 버튼
        StageLeftArrowButton,   //스테이지 선택 지도 왼쪽 스크롤 끝으로
        StageRightArrowButton,   //스테이지 선택 지도 오른쪽 스크롤 끝으로
    }

    enum GameObjects
    {
        LobbyMenuPanel, //LobbyMenu UI 집합
        GameSettingPanel,   //GameSettring UI집합
        TurretSelectScrollView, //터렛 선택용 터렛 리스트 스크롤
        TurretPresetPanel0,   //0~
        TurretPresetPanel1,
        TurretPresetPanel2,
        TurretPresetPanel3,
        TurretPresetPanel4,
        TurretPresetPanel5,
        TurretPresetPanel6,
        TurretPresetPanel7,   //~7
        TurretPanel0,
        TurretPanel1,
        TurretPanel2,
        TurretPanel3,
        TurretPanel4,
        TurretPanel5,
        TurretPanel6,
        TurretPanel7,
        TurretPanel8,
        TurretPanel9,
        TurretPanel10,
        TurretPanel11,
        TurretPanel12,
        TurretPanel13,
        TurretPanel14,
        TurretPanel15,
        TurretPanel16,
        TurretPanel17,
        TurretPanel18,
        TurretPanel19,
        TurretPanel20,
        TurretPanel21,
        TurretPanel22,
        StageItem0, //스테이지 선택 패널 
        StageItem1,
        StageItem2,
        StageItem3,
        StageItem4,
        StageItem5,
        StageItem6,
        StageItem7,
        StageItem8,
        StageItem9,
        StageItem10,
        StageItem11,
        StageItem12,
        StageItem13,
        StageItem14,
        StageItem15,
        StageItem16,
        StageItem17,
        StageItem18,
        StageItem19,
        StageItem20,
        StageItem21,
        StageItem22,
        StageItem23,
        StageSelectPanel,    //스테이지 선택 패널
        StageContent    //스테이지 선택 토글 상위 오브젝트
    }

    enum TextMeshProUGUIs
    {
        TurretText0,    //터렛 선택패널 텍스트 0~
        TurretText1,
        TurretText2,
        TurretText3,
        TurretText4,
        TurretText5,
        TurretText6,
        TurretText7,
        TurretText8,
        TurretText9,
        TurretText10,
        TurretText11,
        TurretText12,
        TurretText13,
        TurretText14,
        TurretText15,
        TurretText16,
        TurretText17,
        TurretText18,
        TurretText19,
        TurretText20,
        TurretText21,
        TurretText22,
        TurretPresetText0,    //터렛 프리셋 텍스트 0~
        TurretPresetText1,
        TurretPresetText2,
        TurretPresetText3,
        TurretPresetText4,
        TurretPresetText5,
        TurretPresetText6,
        TurretPresetText7,   //~7
        TurretPresetCountText,   //터렛 프리셋의 최대 제한수와 현재 터렛수 텍스트
        ColorWoodText0,
        ColorWoodText1,
        ColorWoodText2,
        ColorWoodText3,
        ColorWoodText4,
        ColorWoodText5,
        StarNumText,    //총 별 개수
        StarNumText_Menu,   //메뉴패널 총 별 개수
        StageNumText,    //최대 클리어한 스테이지
        SelectedStageText   //선택한 스테이지
    }

    enum Images
    {
        TurretImage0,   //터렛 프리셋 이미지 0~
        TurretImage1,
        TurretImage2,
        TurretImage3,
        TurretImage4,
        TurretImage5,
        TurretImage6,
        TurretImage7,   //~7
        StageImage, //스테이지별 등급 휘장
        StageStarImage0,
        StageStarImage1,
        StageStarImage2,
        StageStarImage3,
        StageStarImage4,
        StageStarImage5,
        StageStarImage6,
        StageStarImage7,
        StageStarImage8,
        StageStarImage9,
        StageStarImage10,
        StageStarImage11,
        StageStarImage12,
        StageStarImage13,
        StageStarImage14,
        StageStarImage15,
        StageStarImage16,
        StageStarImage17,
        StageStarImage18,
        StageStarImage19,
        StageStarImage20,
        StageStarImage21,
        StageStarImage22,
        StageStarImage23,
        SelectedStageStarImage, //선택한 스테이지 별 정보
        SelectedStage   //선택한 스테이지 등급 이미지
    }

    enum ToggleGroups
    {
        StageSelectPanel    //스테이지 선택
    }

    enum Scrollbars
    {
        MapScrollbar    //스테이지 선택 스크롤 바
    }
    private void Update()
    {
        //UI 업데이트
        UpdateUI();
    }
    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));
        Bind<ToggleGroup>(typeof(ToggleGroups));
        Bind<Scrollbar>(typeof(Scrollbars));

        //로비 메뉴패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);

        //터렛 선택패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.BackLobbyMenuButton).gameObject, OnClickBackLobbyMenu, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickTurretSelectLeftArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickTurretSelectRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretPresetClearButton).gameObject, OnClickTurretPresetClearButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageStartButton).gameObject, OnClickStageStartButton, Define.UIEvent.Click);

        //스테이지 선택패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.StageMapCloseButton).gameObject, OnClickStageMapCloseButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageSelectButton).gameObject, OnClickStageMapOpenButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageRightArrowButton).gameObject, OnClickStageRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageLeftArrowButton).gameObject, OnClickStageLeftArrow, Define.UIEvent.Click);

        //유저정보 캐싱
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //로비패널 초기화
        
        //총합 별 개수 
        int totStarNum = 0;
        for (int i = 0; i <= userInfo.maxStageNum; i++)
        {
            //총합 별 개수 구하기
            totStarNum += userInfo.stageStarList[i].starNum;
        }

        GetTextMeshProUGUI((int)TextMeshProUGUIs.StarNumText).text = "X" + totStarNum.ToString();
        //메뉴패널 총 별 개수
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StarNumText_Menu).text = "X" + totStarNum.ToString();

        //최대 클리어한 스테이지
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = 
            "Stage " + userInfo.maxStageNum.ToString();

        for (int i = 0; i < MAXTCOLORWOOD; i++)
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.ColorWoodText0 + i).text = userInfo.colorWoodResource[i].ToString();
        }

        for (int i = 0; i < MAXTURRETPRESETNUM; i++)
        {
            //터렛 선택 해제 이벤트 초기화
            AddUIEvent(GetButton((int)Buttons.TurretRemoveButton0 + i).gameObject, i, OnClickTurretRemoveButton, Define.UIEvent.Click);
        }

        for (int i = 0; i < MAXTURRETNUM; i++)
        {
            //터렛 선택 이벤트 초기화
            AddUIEvent(GetButton((int)Buttons.TurretButton0 + i).gameObject, i, OnClickAddSelectTurret, Define.UIEvent.Click);

            //터렛 선택 Cost 정보 텍스트 초기화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretText0 + i).text = SystemManager.Instance.TurretJson.GetTurretData()[i].turretCost.ToString();

            //터렛 리스트 초기화
            if (i >= userInfo.maxTurretNum)
                GetGameobject((int)GameObjects.TurretPanel0 + i).SetActive(false);

        }
        
        //스테이지 패널
        for (int i = 0; i <= MAXSTAGENUM; i++)
        {
            if (i <= userInfo.maxStageNum)
            {
                GetGameobject((int)GameObjects.StageItem0 + i).SetActive(true);
                GetImage((int)Images.StageStarImage0 + i).sprite = starSprite[userInfo.stageStarList[i].starNum];
            }
            else
                GetGameobject((int)GameObjects.StageItem0 + i).SetActive(false);
        }

        //로비씬에서 hotelPino 게임오브젝트 찾아서 객체의 LobbyPlayer스크립트 가져오기
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //터렛 프리셋 초기화
        ResetTurretPreset();

        //선택 스테이지 정보 업데이트
        UpdateSelectedStageInfo();

        //GameSetting UI 비활성화
        GetGameobject((int)GameObjects.GameSettingPanel).SetActive(false);
    }

    /// <summary>
    /// 게임시작 버튼 클릭후 연출 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickGameStartButton(PointerEventData data)
    {
        //Lobby UI닫기
        GetGameobject((int)GameObjects.LobbyMenuPanel).SetActive(false);

        //플레이어 연출 애니메이션
        lobbyPlayer.animator.SetBool("gameStart", true);

        //카메라 연출 애니메이션
        lobbyPlayer.camAnimator.Play("CamAnim");
    }

    /// <summary>
    /// 게임시작 버튼 클릭후 연출 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickBackLobbyMenu(PointerEventData data)
    {
        //Lobby UI닫기
        GetGameobject((int)GameObjects.GameSettingPanel).SetActive(false);

        //플레이어 연출 애니메이션
        lobbyPlayer.animator.SetBool("gameStart", false);
        lobbyPlayer.animator.Play("Idle");

        //카메라 연출 애니메이션
        lobbyPlayer.camAnimator.Play("CamBackAnim");
    }

    /// <summary>
    /// 게임씬으로 이동 :김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param> 
    void OnClickStageStartButton(PointerEventData data)
    {
        SceneController.Instance.LoadScene(SceneController.Instance.gameSceneName);
    }

    /// <summary>
    /// UI 업데이트 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void UpdateUI()
    {
        //카메라 연출이 종료 되었을 경우 GameSetting UI생성 - LoobyMenu -> GameSetting
        if (lobbyPlayer.camAnimator.GetBool("finCameraWalk"))
        {
            //GameSetting UI 켜기
            GetGameobject((int)GameObjects.GameSettingPanel).SetActive(true);

            //한번만 실행되게
            lobbyPlayer.camAnimator.SetBool("finCameraWalk", false);
        }

        //카메라 연출이 종료 되었을 경우 LoobyMenu UI생성 - GameSetting -> LoobyMenu
        if (lobbyPlayer.camAnimator.GetBool("finCameraBack"))
        {
            //GameSetting UI 켜기
            GetGameobject((int)GameObjects.LobbyMenuPanel).SetActive(true);

            //한번만 실행되게
            lobbyPlayer.camAnimator.SetBool("finCameraBack", false);
        }
    }
    #region 터렛 선택
    /// <summary>
    /// 터렛 선택 스크롤 오른쪽 끝으로 이동 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickTurretSelectRightArrow(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretSelectScrollView).GetComponent<ScrollRect>().normalizedPosition = new Vector2(1, 0);
    }

    /// <summary>
    /// 터렛 선택 스크롤 왼쪽 끝으로 이동 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickTurretSelectLeftArrow(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretSelectScrollView).GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }

    /// <summary>
    /// 터렛 프리셋을 초기화 : 김현진
    /// </summary>
    void ResetTurretPreset()
    {
        //리스트 오름차순 갱신
        if (SystemManager.Instance.UserInfo.turretPreset.Count > 1)
            SystemManager.Instance.UserInfo.turretPreset.Sort();

        //텍스트 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretPresetCountText).text = SystemManager.Instance.UserInfo.turretPreset.Count + "/8";

        //터렛 프리셋 초기화
        for (int i = 0; i < MAXTURRETPRESETNUM; i++)
        {
            //패널 비활성화
            GetGameobject((int)GameObjects.TurretPresetPanel0 + i).SetActive(false);
        }

        //터렛 프리셋 갱신
        for (int i = 0; i < SystemManager.Instance.UserInfo.turretPreset.Count; i++)
        {
            //패널 활성화
            GetGameobject((int)GameObjects.TurretPresetPanel0 + i).SetActive(true);

            //이미지갱신
            GetImage((int)Images.TurretImage0 + i).sprite = turretSprite[SystemManager.Instance.UserInfo.turretPreset[i]];

            //텍스트갱신
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretPresetText0 + i).text =
                SystemManager.Instance.TurretJson.GetTurretData()[SystemManager.Instance.UserInfo.turretPreset[i]].turretCost.ToString();
        }
    }

    /// <summary>
    /// 터렛 프리셋에 선택한 터렛을 추가 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    /// <param name="idx">추가할 터렛 인덱스</param>
    void OnClickAddSelectTurret(PointerEventData data, int idx)
    {
        if (SystemManager.Instance.UserInfo.turretPreset.Count >= 8)
            return;

        if (!SystemManager.Instance.UserInfo.turretPreset.Contains(idx) && idx >= 0)
            SystemManager.Instance.UserInfo.turretPreset.Add(idx);

        //터렛 프리셋 갱신
        ResetTurretPreset();
    }

    /// <summary>
    /// 터렛 프리셋에서 터렛을 삭제 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    /// <param name="idx">삭제할 터렛 인덱스</param>
    void OnClickTurretRemoveButton(PointerEventData data, int idx)
    {
        //해당 인덱스 터렛 삭제
        if (idx >= 0)
            SystemManager.Instance.UserInfo.turretPreset.RemoveAt(idx);

        //터렛 프리셋 갱신
        ResetTurretPreset();
    }

    /// <summary>
    /// 터렛 프리셋 목록 모두 삭제 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickTurretPresetClearButton(PointerEventData data)
    {
        SystemManager.Instance.UserInfo.turretPreset.Clear();

        //터렛 프리셋 갱신
        ResetTurretPreset();
    }
    #endregion

    #region 스테이지 선택
    /// <summary>
    /// 스테이지 지도 끄기 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickStageMapCloseButton(PointerEventData data = null)
    {
        //선택한 스테이지 정보 캐싱
        UserInfo userInfo = SystemManager.Instance.UserInfo;
        GameObject SelectedToggle = GetToggleGroup((int)ToggleGroups.StageSelectPanel).GetFirstActiveToggle().gameObject;
        Sprite SelectedToggleImage = SelectedToggle.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;
        Sprite SelectedStarImage = SelectedToggle.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite;
        string SelectedStageNum = SelectedToggle.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;

        //선택한 스테이지 정보 유저 정보에 갱신
        userInfo.selectedStageNum = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));

        //패널에 선택한 스테이지 정보 갱신
        GetImage((int)Images.SelectedStage).sprite = SelectedToggleImage;
        GetImage((int)Images.SelectedStageStarImage).sprite = SelectedStarImage;
        GetTextMeshProUGUI((int)TextMeshProUGUIs.SelectedStageText).text = SelectedStageNum;

        if (GetGameobject((int)GameObjects.StageSelectPanel))
            GetGameobject((int)GameObjects.StageSelectPanel).SetActive(false);
    }

    /// <summary>
    /// 스테이지 지도 켜기 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickStageMapOpenButton(PointerEventData data)
    {
        if (GetGameobject((int)GameObjects.StageSelectPanel))
            GetGameobject((int)GameObjects.StageSelectPanel).SetActive(true);
    }

    /// <summary>
    /// 선택된 스테이지 정보 적용 : 김현진
    /// </summary>
    void UpdateSelectedStageInfo()
    {
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        Toggle toggle = GetGameobject((int)GameObjects.StageContent).transform.GetChild(userInfo.selectedStageNum).GetComponent<Toggle>();
        toggle.isOn = true;

        //닫고 패널에 정보 업데이트
        OnClickStageMapCloseButton();
    }

    /// <summary>
    /// 스테이지 선택 스크롤 오른쪽 끝으로 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickStageRightArrow(PointerEventData data)
    {
        GetScrollBar((int)Scrollbars.MapScrollbar).value = 1;
    }

    /// <summary>
    /// 스테이지 선택 스크롤 왼쪽 끝으로 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickStageLeftArrow(PointerEventData data)
    {
        GetScrollBar((int)Scrollbars.MapScrollbar).value = 0;
    }
    #endregion

}
