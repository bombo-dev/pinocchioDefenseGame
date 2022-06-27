using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_StageEndPanel : UI_Controller
{
    const int MAXTURRETPRESETNUM = 8;
    const int MAXREWARDNUM = 7;
    const int MAXTURRETNUM = 23;

    [SerializeField]
    Sprite[] turretSprite;  //터렛 이미지 모음

    [SerializeField]
    Sprite[] woodSprite;    //나무 이미지 모음

    [SerializeField]
    Sprite[] starSprite;    //별 이미지 모음

    [SerializeField]
    Sprite panelSpriteLight;
    [SerializeField]
    Sprite panelSpriteDark;

    enum TextMeshProUGUIs
    {
        StageText,  //현재 스테이지 정보
        ResultTimeText,  //걸린 시간
        TurretDamageText0,  //터렛 데미지 정보 0~
        TurretDamageText1,
        TurretDamageText2,
        TurretDamageText3,
        TurretDamageText4,
        TurretDamageText5,
        TurretDamageText6,
        TurretDamageText7,  //~7
        TurretCostText0,    //터렛 비용 정보 0~
        TurretCostText1,
        TurretCostText2,
        TurretCostText3,
        TurretCostText4,
        TurretCostText5,
        TurretCostText6,
        TurretCostText7,    //~7
        TurretNumText0,
        TurretNumText1, //터렛 개수 정보 0~
        TurretNumText2,
        TurretNumText3,
        TurretNumText4,
        TurretNumText5,
        TurretNumText6,
        TurretNumText7, //~7
        RewardNumText0, //보상 나무 개수 0~
        RewardNumText1,
        RewardNumText2,
        RewardNumText3,
        RewardNumText4,
        RewardNumText5,
        RewardNumText6,//~6
        TurretPresetText0,    //터렛 프리셋 텍스트 0~
        TurretPresetText1,
        TurretPresetText2,
        TurretPresetText3,
        TurretPresetText4,
        TurretPresetText5,
        TurretPresetText6,
        TurretPresetText7,   //~7
        TurretPresetCountText,   //터렛 프리셋의 최대 제한수와 현재 터렛수 텍스트
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
        newTurretText //새로운 터렛 추가 알림 텍스트
    }

    enum GameObjects
    {
        BattleAnalysisItem0,    //전투분석 패널 0~
        BattleAnalysisItem1,
        BattleAnalysisItem2,
        BattleAnalysisItem3,
        BattleAnalysisItem4,
        BattleAnalysisItem5,
        BattleAnalysisItem6,
        BattleAnalysisItem7,    //~7
        RewardPanel, //보상패널
        RewardItem0,    //보상 아이템 0~
        RewardItem1,
        RewardItem2,
        RewardItem3,
        RewardItem4,    
        RewardItem5,
        RewardItem6,//~6
        StageFailText,   //스테이지 실패
        StageClearText,   //스테이지 클리어
        StageResultPanel,    //패널 UI전체
        TurretPresetPanel0,   //0~
        TurretPresetPanel1,
        TurretPresetPanel2,
        TurretPresetPanel3,
        TurretPresetPanel4,
        TurretPresetPanel5,
        TurretPresetPanel6,
        TurretPresetPanel7,   //~7
        TurretSelectScrollView, //터렛 선택용 터렛 리스트 스크롤
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
        newTurretPanel  //새로 추가된 터렛 알리미
    }

    enum Sliders
    {
        DamageSlider0,  //데미지 슬라이더 0~
        DamageSlider1,
        DamageSlider2,
        DamageSlider3,
        DamageSlider4,
        DamageSlider5,
        DamageSlider6,
        DamageSlider7,  //~7
        CostSlider0,    //코스트 슬라이더 0~
        CostSlider1,
        CostSlider2,
        CostSlider3,
        CostSlider4,
        CostSlider5,
        CostSlider6,
        CostSlider7,    //~7
    }

    enum Images
    {
        TurretImage0,   //터렛 이미지 0~
        TurretImage1,
        TurretImage2,
        TurretImage3,
        TurretImage4,
        TurretImage5,
        TurretImage6,
        TurretImage7,   //~7
        RewardImage0,   //보상 나무 이미지 0~
        RewardImage1,
        RewardImage2,
        RewardImage3,
        RewardImage4,
        RewardImage5,
        RewardImage6,//~6
        StarRewardImage, //별 보상 이미지
        StageResultPanel,    //패널 UI전체
        StageResultBackGround,   //StageResultPanel 앞 패널
        TurretPresetImage0,   //터렛 프리셋 이미지 0~
        TurretPresetImage1,
        TurretPresetImage2,
        TurretPresetImage3,
        TurretPresetImage4,
        TurretPresetImage5,
        TurretPresetImage6,
        TurretPresetImage7,   //~7
        newTurretImage  //새로 추가된 터렛 이미지
    }

    enum Buttons
    {
        ReStartButton,  //다시시작 버튼
        ExitButton,  //나가기 버튼
        NextStageButton,  //다음 스테이지 버튼
        TurretSelectLeftArrowButton,    //터렛 선택 스크롤 <버튼
        TurretSelectRightArrowButton,    //터렛 선택 스크롤 >버튼
        TurretPresetClearButton, //터렛 프리셋 비우기 버튼
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
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Slider>(typeof(Sliders));
        Bind<Button>(typeof(Buttons));

        //이벤트 추가
        AddUIEvent(GetButton((int)Buttons.ReStartButton).gameObject, OnClickRestartButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.ExitButton).gameObject, OnClickExitButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.NextStageButton).gameObject, OnClickNextStageButton, Define.UIEvent.Click);

        //터렛 선택패널 UI 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickTurretSelectLeftArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickTurretSelectRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretPresetClearButton).gameObject, OnClickTurretPresetClearButton, Define.UIEvent.Click);

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
            if (i >= SystemManager.Instance.UserInfo.maxTurretNum)
                GetGameobject((int)GameObjects.TurretPanel0 + i).SetActive(false);

        }


        //게임오버
        if (SystemManager.Instance.GameFlowManager.gameState == GameFlowManager.GameState.StageFail)
        {
            GetGameobject((int)GameObjects.StageClearText).SetActive(false);
            GetButton((int)Buttons.NextStageButton).gameObject.SetActive(false);
        }
        else
        {
            GetGameobject((int)GameObjects.StageFailText).SetActive(false);

        }

        //터렛 프리셋 초기화
        ResetTurretPreset();

        //패널 비활성화
        GetGameobject((int)GameObjects.StageResultPanel).SetActive(false);

        //신규 터렛 획득 패널 비활성화
        GetGameobject((int)GameObjects.newTurretPanel).SetActive(false);

        //패널 활성화 코루틴 호출
        StartCoroutine("OnResultPanel");
    }

    /// <summary>
    /// 패널 정보 초기상태로 : 김현진
    /// </summary>
    void ResetStageEndPanel()
    {
        //싱글톤 정보 캐싱
        GameFlowManager gfm = SystemManager.Instance.GameFlowManager;
        ResourceManager rm = SystemManager.Instance.ResourceManager;

        for (int i = 0; i < MAXTURRETPRESETNUM; i++)
        {
            //패널 비활성화
            GetGameobject((int)GameObjects.BattleAnalysisItem0 + i).SetActive(false);
        }

        //보상패널 비활성화
        for (int i = 0; i < MAXREWARDNUM; i++)
        {
            GetGameobject((int)GameObjects.RewardItem0 + i).SetActive(false);
        }
        GetGameobject((int)GameObjects.RewardPanel).SetActive(false);

    }

    /// <summary>
    /// 패널 정보 업데이트 : 김현진
    /// </summary>
    void UpdateStageEndPanel()
    {
        //싱글톤 정보 캐싱
        GameFlowManager gfm = SystemManager.Instance.GameFlowManager;
        ResourceManager rm = SystemManager.Instance.ResourceManager;
        TurretManager tm = SystemManager.Instance.TurretManager;
        RewardManager rwm = SystemManager.Instance.RewardManager;
        UserInfo ui = SystemManager.Instance.UserInfo;

        //예외처리
        if (!gfm || !rm || !tm)
            return;

        //스테이지 텍스트
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageText).text = "Stage " + gfm.stage.ToString();
        //타임 텍스트
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ResultTimeText).text = 
            "Time: " + ((int)(gfm.stageTime / 60)).ToString() + " 분 " + ((int)(gfm.stageTime % 60)).ToString() + " 초 ";


        int maxDamage = 0;  //터렛이 준 데미지중 가장 큰 값
        int maxCost = 0;    //터렛 코스트 중 가장 큰 값

        for (int i = 0; i < rm.selectedTurretPreset.Count; i++)
        {
            //터렛 번호
            int turretNum = rm.selectedTurretPreset[i];

            //maxDamage구하기
            if (gfm.turretBattleAnalysisDic.ContainsKey(turretNum))
            {
                if (maxDamage < gfm.turretBattleAnalysisDic[turretNum])
                    maxDamage = gfm.turretBattleAnalysisDic[turretNum];
            }

            //maxCost구하기
            if (gfm.turretSummonAnalysisDic.ContainsKey(turretNum))
            {
                if (maxCost < (gfm.turretSummonAnalysisDic[turretNum] * tm.turretCostArr[turretNum]))
                    maxCost = (gfm.turretSummonAnalysisDic[turretNum] * tm.turretCostArr[turretNum]);
            }
        }

        //터렛 데미지 전투분석 정보 적용
        for (int i = 0; i < rm.selectedTurretPreset.Count; i++)
        {
            //터렛 번호
            int turretNum = rm.selectedTurretPreset[i];

            //딕셔너리에 포함되있는 데이터일 경우
            if (gfm.turretBattleAnalysisDic.ContainsKey(turretNum))
            {
                //패널 활성화
                GetGameobject((int)GameObjects.BattleAnalysisItem0 + i).SetActive(true);

                //터렛 이미지 정보
                GetImage((int)Images.TurretImage0 + i).sprite =
                    turretSprite[turretNum];

                //데미지 정보
                GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretDamageText0 + i).text =
                    gfm.turretBattleAnalysisDic[turretNum].ToString();

                //데미지 슬라이더 정보
                GetSlider((int)Sliders.DamageSlider0 + i).value = (float)gfm.turretBattleAnalysisDic[turretNum] / (float)maxDamage;


                //터렛 건설 개수 정보
                GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretNumText0 + i).text =
                    "X" + gfm.turretSummonAnalysisDic[turretNum].ToString();

                //터렛 가격 정보
                GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretCostText0 + i).text =
                    SystemManager.Instance.TurretManager.turretCostArr[turretNum] + " X " +
                    gfm.turretSummonAnalysisDic[turretNum] + " = " +
                    (tm.turretCostArr[turretNum] * gfm.turretSummonAnalysisDic[turretNum]);

                //터렛 가격 슬라이더 정보
                GetSlider((int)Sliders.CostSlider0 + i).value = (tm.turretCostArr[turretNum] * gfm.turretSummonAnalysisDic[turretNum]) / (float)maxCost;

            }

        }

        //----------보상정보 업데이트----------
        //클리어 상태일 경우
        if (gfm.gameState == GameFlowManager.GameState.StageClear)
        {
            //보상패널 활성화
            GetGameobject((int)GameObjects.RewardPanel).SetActive(true);

            //별 보상 이미지 교체
            GetImage((int)Images.StarRewardImage).sprite = starSprite[SystemManager.Instance.RewardManager.starRewardNum];

            for (int i = 0; i < MAXREWARDNUM - 1; i++)
            {
                //보상 존재할때 패널 활성화
                if (rwm.colorWoodReward[i] > 0)
                    GetGameobject((int)GameObjects.RewardItem0 + i).SetActive(true);
                else
                    continue;

                //보상 이미지
                GetImage((int)Images.RewardImage0 + i).sprite = woodSprite[i];
                //보상 개수
                GetTextMeshProUGUI((int)TextMeshProUGUIs.RewardNumText0 + i).text =
                    "X" + rwm.colorWoodReward[i];
            }


            //추가 터렛 보상 (마지막 인덱스에 터렛 보상 추가)
            if (rwm.turretReward.ContainsKey(gfm.stage)) //터렛 보상 존재할경우
            {
                //신규 터렛 획득 패널 활성화
                GetGameobject((int)GameObjects.newTurretPanel).SetActive(true);

                //이미 가지고 있는 터렛이 아닌경우
                if (rwm.getNewTurret)
                {
                    //터렛정보 업데이트
                    ui.maxTurretNum = rwm.turretReward[gfm.stage];

                    //패널갱신
                    //패널 활성화
                    GetGameobject((int)GameObjects.RewardItem6).SetActive(true);
                    //보상 이미지
                    GetImage((int)Images.RewardImage6).sprite = turretSprite[rwm.turretReward[gfm.stage] - 1];
                    //보상 개수
                    GetTextMeshProUGUI((int)TextMeshProUGUIs.RewardNumText6).text =
                        "X1";

                    rwm.getNewTurret = false;
                }

                //신규터렛 획득 연출
                GetImage((int)Images.newTurretImage).sprite = turretSprite[rwm.turretReward[gfm.stage] - 1];
                StartCoroutine(newTurretTyping(GetTextMeshProUGUI((int)TextMeshProUGUIs.newTurretText), "신규 터렛이 컬렉션에 추가되었습니다!", 0.03f));
            }

            //패널이미지 교체
            GetImage((int)Images.StageResultPanel).sprite = panelSpriteLight;
            GetImage((int)Images.StageResultBackGround).sprite = panelSpriteLight;
        }
        //게임오버 상태일 경우
        else
        {
            //패널이미지 교체
            GetImage((int)Images.StageResultPanel).sprite = panelSpriteDark;
            GetImage((int)Images.StageResultBackGround).sprite = panelSpriteDark;
        }

    }//end of UpdateStageEndPanel

    /// <summary>
    /// 씬 재로드하여 다시 시작 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickRestartButton(PointerEventData data)
    {
        //스테이지 선택
        SystemManager.Instance.UserInfo.selectedStageNum = SystemManager.Instance.GameFlowManager.stage;

        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();

        SceneController.Instance.LoadScene(SceneController.Instance.gameSceneName);
    }

    /// <summary>
    /// 로비씬으로 돌아가기 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickExitButton(PointerEventData data)
    {
        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();

        //씬 이동
        SceneController.Instance.LoadScene(SceneController.Instance.lobbySceneName);
    }

    /// <summary>
    /// 다음 스테이지로 이동 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickNextStageButton(PointerEventData data)
    {
        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();

        //예외처리
        if (SystemManager.Instance.UserInfo.maxStageNum < SystemManager.Instance.GameFlowManager.stage + 1)
            return;

        //유저정보 갱신
        SystemManager.Instance.UserInfo.selectedStageNum = SystemManager.Instance.GameFlowManager.stage + 1;
        save.SaveUserInfo();

        //씬 이동
        SceneController.Instance.LoadScene(SceneController.Instance.gameSceneName);
    }

    /// <summary>
    /// 일정시간 뒤 ResultPanel 활성화 후 정보 초기화 : 김현진
    /// </summary>
    IEnumerator OnResultPanel()
    {
        yield return new WaitForSeconds(2.5f);
        //패널 활성화
        GetGameobject((int)GameObjects.StageResultPanel).SetActive(true);

        //패널 정보 리셋
        ResetStageEndPanel();

        //패널 정보 업데이트
        UpdateStageEndPanel();
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
            GetImage((int)Images.TurretPresetImage0 + i).sprite = turretSprite[SystemManager.Instance.UserInfo.turretPreset[i]];

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


    /// <summary>
    /// 타이핑 효과 : 김현진
    /// </summary>
    /// <param name="typingText">타이핑 효과를 줄 텍스트</param>
    /// <param name="message">텍스트 문장</param>
    /// <param name="speed">타이핑 속도</param>
    IEnumerator newTurretTyping(TextMeshProUGUI typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        //대기
        yield return new WaitForSeconds(3.0f);

        //신규 터렛 획득 패널 비활성화
        GetGameobject((int)GameObjects.newTurretPanel).SetActive(false);
    }
}
