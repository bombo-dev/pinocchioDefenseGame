using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_StageEndPanel : UI_Controller
{
    const int MAXTURRETPRESETNUM = 8;
    const int MAXREWARDNUM = 6;

    [SerializeField]
    Sprite[] turretSprite;  //터렛 이미지 모음

    [SerializeField]
    Sprite[] woodSprite;    //나무 이미지 모음

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
        RewardNumText5,  //~5
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
        RewardItem5,    //~5
        StageFailText,   //스테이지 실패
        StageClearText,   //스테이지 클리어
        StageResultPanel    //패널 UI전체
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
        RewardImage5,   //~5
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

        //게임오버
        if (SystemManager.Instance.GameFlowManager.gameState == GameFlowManager.GameState.StageFail)
        {
            GetGameobject((int)GameObjects.StageClearText).SetActive(false);
        }
        else
        {
            GetGameobject((int)GameObjects.StageFailText).SetActive(false);

        }

        //패널 비활성화
        GetGameobject((int)GameObjects.StageResultPanel).SetActive(false);

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

        //예외처리
        if (!gfm || !rm || !tm)
            return;

        //스테이지 텍스트
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageText).text = "Stage " + gfm.stage.ToString();
        //타임 텍스트
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ResultTimeText).text = 
            ((int)(gfm.stageTime / 60)).ToString() + " 분 " + ((int)(gfm.stageTime % 60)).ToString() + " 초 ";


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

            for (int i = 0; i < MAXREWARDNUM; i++)
            {
                //보상 존재할때 패널 활성화
                if (rwm.colorWoodReward[i] > 0)
                    GetGameobject((int)GameObjects.RewardItem0 + i).SetActive(true);

                //보상 이미지
                GetImage((int)Images.RewardImage0 + i).sprite = woodSprite[i];
                //보상 개수
                GetTextMeshProUGUI((int)TextMeshProUGUIs.RewardNumText0 + i).text =
                    "X" + rwm.colorWoodReward[i];
            }

        }

    }//end of UpdateStageEndPanel

    /// <summary>
    /// 일정시간 뒤 ResultPanel 활성화 후 정보 초기화 : 김현진
    /// </summary>
    IEnumerator OnResultPanel()
    {
        yield return new WaitForSeconds(1.0f);
        //패널 활성화
        GetGameobject((int)GameObjects.StageResultPanel).SetActive(true);

        //패널 정보 리셋
        ResetStageEndPanel();

        //패널 정보 업데이트
        UpdateStageEndPanel();

    }

}
