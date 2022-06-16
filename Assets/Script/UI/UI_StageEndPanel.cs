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

    enum TextMeshProUGUIs
    {
        StageText,  //현재 스테이지 정보
        ResultTime,  //걸린 시간
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

        //패널 정보 리셋
        ResetStageEndPanel();

        //패널 정보 업데이트
        UpdateStageEndPanel();
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

        //예외처리
        if (!gfm || !rm || !tm)
            return;

        int maxDamage = 0;  //터렛이 준 데미지중 가장 큰 값
        for (int i = 0; i < rm.selectedTurretPreset.Count; i++)
        {
            //터렛 번호
            int turretNum = rm.selectedTurretPreset[i];

            if (gfm.turretBattleAnalysisDic.ContainsKey(turretNum))
            {
                if (maxDamage < gfm.turretBattleAnalysisDic[turretNum])
                    maxDamage = gfm.turretBattleAnalysisDic[turretNum];
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
                    "총 건설비용: " + SystemManager.Instance.TurretManager.turretCostArr[turretNum] + " X " +
                    gfm.turretSummonAnalysisDic[turretNum] + "기 = " +
                    (SystemManager.Instance.TurretManager.turretCostArr[turretNum] * gfm.turretSummonAnalysisDic[turretNum]);
            }


        }

    }
}
