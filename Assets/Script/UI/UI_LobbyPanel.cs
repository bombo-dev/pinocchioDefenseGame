using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;

public class UI_LobbyPanel : UI_Controller
{
    const int MAXTURRETNUM = 23;
    const int MAXTURRETPRESETNUM = 8;
    const int MAXTCOLORWOOD = 6;
    const int MAXSTAGENUM = 40;

    //로비씬의 플레이어 스크립트
    LobbyPlayer lobbyPlayer;

    //터렛 이미지
    [SerializeField]
    Sprite[] turretSprite;

    //스테이지 이미지
    [SerializeField]
    Sprite[] stageSprite;

    //별 이미지
    [SerializeField]
    Sprite[] starSprite;

    [SerializeField]
    Sprite[] starSprite_Hard;

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
        StageStartButton,    //게임씬 시작 버튼
        StageMapCloseButton,    //스테이지 선택 지도 끄기 버튼
        StageSelectButton,   //스테이지 선택 지도 켜기 버튼
        StageLeftArrowButton,   //스테이지 선택 지도 왼쪽 스크롤 끝으로
        StageRightArrowButton,   //스테이지 선택 지도 오른쪽 스크롤 끝으로
        StoryButton, //스토리씬 시작 버튼
        NormalButton,   //노말모드 버튼
        HardButton      //하드모드 버튼
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
        StageItem24,
        StageItem25,
        StageItem26,
        StageItem27,
        StageItem28,
        StageItem29,
        StageItem30,
        StageItem31,
        StageItem32,
        StageItem33,
        StageItem34,
        StageItem35,
        StageItem36,
        StageItem37,
        StageItem38,
        StageItem39,
        StageItem40,
        StageSelectPanel,    //스테이지 선택 패널
        StageContent,    //스테이지 선택 토글 상위 오브젝트
        NormalSelectButtonImage,    //노말모드 선택 체크 토글 이미지
        HardSelectButtonImage,    //하드모드 선택 체크 토글 이미지
        NormalText, //노말 모드 텍스트
        HardText,   //하드 모드 텍스트
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
        HardStarNumText,    //총 별 개수 - Hard
        StarNumText_Menu,   //메뉴패널 총 별 개수
        HardStarNumText_Menu,   //메뉴패널 총 별 개수 - Hard
        StageNumText,    //최대 클리어한 스테이지
        HardStageNumText,//최대 클리어한 하드 스테이지
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
        StageStarImage24,
        StageStarImage25,
        StageStarImage26,
        StageStarImage27,
        StageStarImage28,
        StageStarImage29,
        StageStarImage30,
        StageStarImage31,
        StageStarImage32,
        StageStarImage33,
        StageStarImage34,
        StageStarImage35,
        StageStarImage36,
        StageStarImage37,
        StageStarImage38,
        StageStarImage39,
        StageStarImage40,
        SelectedStageStarImage, //선택한 스테이지 별 정보
        SelectedStage,   //선택한 스테이지 등급 이미지
        MapPanel    //맵 배경 이미지
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

        // UserInfo Load
        SaveLoad load = new SaveLoad();
        load.LoadUserInfo();

        //로비 메뉴패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StoryButton).gameObject, OnClickStoryButton, Define.UIEvent.Click);


        //터렛 선택패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.BackLobbyMenuButton).gameObject, OnClickBackLobbyMenu, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickTurretSelectLeftArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickTurretSelectRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretPresetClearButton).gameObject, OnClickTurretPresetClearButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageStartButton).gameObject, OnClickStageStartButton, Define.UIEvent.Click);

        //스테이지 선택패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.StageMapCloseButton).gameObject, OnClickStageMapCloseButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageSelectButton).gameObject, OnClickStageSelectButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageRightArrowButton).gameObject, OnClickStageRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageLeftArrowButton).gameObject, OnClickStageLeftArrow, Define.UIEvent.Click);

        AddUIEvent(GetButton((int)Buttons.NormalButton).gameObject, OnClickStageMapOpenButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.HardButton).gameObject, OnClickHardStageMapOpenButton, Define.UIEvent.Click);


        //유저정보 캐싱
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //로비패널 초기화
        
        //총합 별 개수 
        int totStarNum = 0;
        int totHardStarNum = 0;
        for (int i = 0; i <= userInfo.maxStageNum; i++)
        {
            //총합 별 개수 구하기
            totStarNum += userInfo.stageStarList[i].starNum;
        }
        for (int i = 0; i <= userInfo.maxStageNum_hard; i++)
        {
            //총합 별 개수 구하기 - 하드
            totHardStarNum += userInfo.stageStarList_hard[i].starNum;
        }

        //스테이지 이미지 바꾸기
        if (totStarNum <= 40)
            GetImage((int)Images.StageImage).sprite = stageSprite[0]; //브론즈
        else if(totStarNum <= 73)
            GetImage((int)Images.StageImage).sprite = stageSprite[1]; //실버
        else if (totStarNum <= 85)
            GetImage((int)Images.StageImage).sprite = stageSprite[2]; //골드
        else if (totStarNum <= 100)
            GetImage((int)Images.StageImage).sprite = stageSprite[3]; //플래티넘
        else if (totStarNum <= 122)
            GetImage((int)Images.StageImage).sprite = stageSprite[4]; //다이아
        else
            GetImage((int)Images.StageImage).sprite = stageSprite[5]; //마스터

        GetTextMeshProUGUI((int)TextMeshProUGUIs.StarNumText).text = "X" + totStarNum.ToString();
        GetTextMeshProUGUI((int)TextMeshProUGUIs.HardStarNumText).text = "X" + totHardStarNum.ToString();
        //메뉴패널 총 별 개수
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StarNumText_Menu).text = "X" + totStarNum.ToString();
        GetTextMeshProUGUI((int)TextMeshProUGUIs.HardStarNumText_Menu).text = "X" + totHardStarNum.ToString();

        //최대 클리어한 스테이지
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = 
            "max:" + userInfo.maxStageNum.ToString();
        //최대 클리어한 하드 스테이지
        GetTextMeshProUGUI((int)TextMeshProUGUIs.HardStageNumText).text =
            "max:" + userInfo.maxStageNum_hard.ToString();

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

        //로비씬에서 hotelPino 게임오브젝트 찾아서 객체의 LobbyPlayer스크립트 가져오기
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //터렛 프리셋 초기화
        ResetTurretPreset();

        //선택 스테이지 정보 업데이트
        //UpdateSelectedStageInfo();
        OnClickStageSelectButton();

        //닫고 패널에 정보 업데이트
        OnClickStageMapCloseButton();

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
    /// 스토리씬으로 이동 :김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param> 
    void OnClickStoryButton(PointerEventData data)
    {
        SceneController.Instance.LoadScene(SceneController.Instance.storySceneName);
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
        //---리스트 가격순 오름차순 갱신---
        Dictionary<int,int> tempTurretPreset = new Dictionary<int, int>();  // turretNum/Cost
        for (int i = 0; i < SystemManager.Instance.UserInfo.turretPreset.Count; i++)
        {
            tempTurretPreset.Add(SystemManager.Instance.UserInfo.turretPreset[i],
                SystemManager.Instance.TurretJson.GetTurretData()[SystemManager.Instance.UserInfo.turretPreset[i]].turretCost);
        }
        var sortTempTurretPreset = tempTurretPreset.OrderBy(x => x.Value);

        SystemManager.Instance.UserInfo.turretPreset.Clear();
        foreach (var dictionary in sortTempTurretPreset)
        {
            SystemManager.Instance.UserInfo.turretPreset.Add(dictionary.Key);
        }

        //---리스트 가격순 오름차순 갱신---

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

        // userinfo Save
        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();
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
        //1개만 남았을때 삭제 불가
        if (SystemManager.Instance.UserInfo.turretPreset.Count <= 1)
            return;

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
        if (SystemManager.Instance.UserInfo.turretPreset.Count > 1)
        {
            SystemManager.Instance.UserInfo.turretPreset.RemoveRange(1, SystemManager.Instance.UserInfo.turretPreset.Count - 1);
        }

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

        if (userInfo.selectMode == 0)   //노말
        {
            //선택한 스테이지 정보 유저 정보에 갱신
            userInfo.selectedStageNum = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));

            //패널에 선택한 스테이지 정보 갱신
            GetImage((int)Images.SelectedStage).sprite = SelectedToggleImage;
            GetImage((int)Images.SelectedStageStarImage).sprite = SelectedStarImage;
            GetTextMeshProUGUI((int)TextMeshProUGUIs.SelectedStageText).text = SelectedStageNum;

            if (GetGameobject((int)GameObjects.StageSelectPanel))
                GetGameobject((int)GameObjects.StageSelectPanel).SetActive(false);
        }
        else    //하드
        {
            //선택한 스테이지 정보 유저 정보에 갱신
            userInfo.selectedStageNum_hard = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));

            //패널에 선택한 스테이지 정보 갱신
            GetImage((int)Images.SelectedStage).sprite = SelectedToggleImage;
            GetImage((int)Images.SelectedStageStarImage).sprite = SelectedStarImage;
            GetTextMeshProUGUI((int)TextMeshProUGUIs.SelectedStageText).text = SelectedStageNum;

            if (GetGameobject((int)GameObjects.StageSelectPanel))
                GetGameobject((int)GameObjects.StageSelectPanel).SetActive(false);
        }

        EnableStageModeText();

        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();
    }

    /// <summary>
    /// 로비에서 스테이지 지도켜기 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickStageSelectButton(PointerEventData data = null)
    {
        //노말
        if (SystemManager.Instance.UserInfo.selectMode == 0)
        {
            OnClickStageMapOpenButton();
        }
        //하드
        else
        {
            OnClickHardStageMapOpenButton();
        }

        EnableStageModeText();
    }

    /// <summary>
    /// 스테이지 모드에 맞는 노말/하드모드 구분 텍스트 활성화 : 김현진
    /// </summary>
    void EnableStageModeText()
    {
        //노말
        if (SystemManager.Instance.UserInfo.selectMode == 0)
        {
            if (!GetGameobject((int)GameObjects.NormalText).activeSelf)
                GetGameobject((int)GameObjects.NormalText).SetActive(true);
            if (GetGameobject((int)GameObjects.HardText).activeSelf)
                GetGameobject((int)GameObjects.HardText).SetActive(false);
        }
        //하드
        else
        {
            if (GetGameobject((int)GameObjects.NormalText).activeSelf)
                GetGameobject((int)GameObjects.NormalText).SetActive(false);
            if (!GetGameobject((int)GameObjects.HardText).activeSelf)
                GetGameobject((int)GameObjects.HardText).SetActive(true);
        }
    }

    /// <summary>
    /// 스테이지 지도 켜기 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickStageMapOpenButton(PointerEventData data = null)
    {
        if (GetGameobject((int)GameObjects.StageSelectPanel))
            GetGameobject((int)GameObjects.StageSelectPanel).SetActive(true);

        //이전에 선택한 스테이지 정보 저장
        //선택한 스테이지 정보
        GameObject SelectedToggle = GetToggleGroup((int)ToggleGroups.StageSelectPanel).GetFirstActiveToggle().gameObject;
        string SelectedStageNum = SelectedToggle.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        if (SystemManager.Instance.UserInfo.selectMode == 1)   //하드
        {
            //선택한 스테이지 정보 유저 정보에 갱신
            SystemManager.Instance.UserInfo.selectedStageNum_hard = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));
        }

        //맵 배경 이미지
        if (GetImage((int)Images.MapPanel).gameObject.activeSelf)
            GetImage((int)Images.MapPanel).color = Color.white;

        //토글 이미지
        if (!GetGameobject((int)GameObjects.NormalSelectButtonImage).activeSelf)
            GetGameobject((int)GameObjects.NormalSelectButtonImage).SetActive(true);
        if (GetGameobject((int)GameObjects.HardSelectButtonImage).activeSelf)
            GetGameobject((int)GameObjects.HardSelectButtonImage).SetActive(false);
        //스테이지 정보 갱신
        SystemManager.Instance.UserInfo.selectMode = 0; //노말

        //스테이지 패널
        for (int i = 0; i <= MAXSTAGENUM; i++)
        {
            if (i <= SystemManager.Instance.UserInfo.maxStageNum)
            {
                GetGameobject((int)GameObjects.StageItem0 + i).SetActive(true);
                GetImage((int)Images.StageStarImage0 + i).sprite = starSprite[SystemManager.Instance.UserInfo.stageStarList[i].starNum];
            }
            else
                GetGameobject((int)GameObjects.StageItem0 + i).SetActive(false);
        }

        //선택한 스테이지 토글 정보
        UpdateSelectedStageInfo();
    }

    /// <summary>
    /// 스테이지 지도 켜기 - 하드: 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickHardStageMapOpenButton(PointerEventData data = null)
    {
        if (GetGameobject((int)GameObjects.StageSelectPanel))
            GetGameobject((int)GameObjects.StageSelectPanel).SetActive(true);

        //이전에 선택한 스테이지 정보 저장
        //선택한 스테이지 정보
        GameObject SelectedToggle = GetToggleGroup((int)ToggleGroups.StageSelectPanel).GetFirstActiveToggle().gameObject;
        string SelectedStageNum = SelectedToggle.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        if (SystemManager.Instance.UserInfo.selectMode == 0)    //노말
        {
            //선택한 스테이지 정보 유저 정보에 갱신
            SystemManager.Instance.UserInfo.selectedStageNum = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));
        }

        //맵 배경 이미지
        if (GetImage((int)Images.MapPanel).gameObject.activeSelf)
            GetImage((int)Images.MapPanel).color = new Color(1, 0.5f, 0.5f);

        //토글 이미지
        if (GetGameobject((int)GameObjects.NormalSelectButtonImage).activeSelf)
            GetGameobject((int)GameObjects.NormalSelectButtonImage).SetActive(false);
        if (!GetGameobject((int)GameObjects.HardSelectButtonImage).activeSelf)
            GetGameobject((int)GameObjects.HardSelectButtonImage).SetActive(true);
        //스테이지 정보 갱신
        SystemManager.Instance.UserInfo.selectMode = 1; //하드

        //스테이지 패널
        for (int i = 0; i <= MAXSTAGENUM; i++)
        {
            if (i == 0)
            {
                GetGameobject((int)GameObjects.StageItem0 + i).SetActive(false);
            }
            else if (i <= SystemManager.Instance.UserInfo.maxStageNum_hard)
            {
                GetGameobject((int)GameObjects.StageItem0 + i).SetActive(true);
                GetImage((int)Images.StageStarImage0 + i).sprite = starSprite_Hard[SystemManager.Instance.UserInfo.stageStarList_hard[i].starNum];
            }
            else
                GetGameobject((int)GameObjects.StageItem0 + i).SetActive(false);
        }

        //선택한 토글 정보
        UpdateSelectedStageInfo();
    }

    /// <summary>
    /// 선택된 스테이지 정보 적용 : 김현진
    /// </summary>
    void UpdateSelectedStageInfo()
    {
        UserInfo userInfo = SystemManager.Instance.UserInfo;
        Toggle toggle;

        if (userInfo.selectMode == 0)   //노말
            toggle = GetGameobject((int)GameObjects.StageContent).transform.GetChild(userInfo.selectedStageNum).GetComponent<Toggle>();
        else
            toggle = GetGameobject((int)GameObjects.StageContent).transform.GetChild(userInfo.selectedStageNum_hard).GetComponent<Toggle>();

        toggle.isOn = true;
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
