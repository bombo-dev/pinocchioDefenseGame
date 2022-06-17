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
    Sprite[] turretSprite;  //�ͷ� �̹��� ����

    [SerializeField]
    Sprite[] woodSprite;    //���� �̹��� ����

    enum TextMeshProUGUIs
    {
        StageText,  //���� �������� ����
        ResultTimeText,  //�ɸ� �ð�
        TurretDamageText0,  //�ͷ� ������ ���� 0~
        TurretDamageText1,
        TurretDamageText2,
        TurretDamageText3,
        TurretDamageText4,
        TurretDamageText5,
        TurretDamageText6,
        TurretDamageText7,  //~7
        TurretCostText0,    //�ͷ� ��� ���� 0~
        TurretCostText1,
        TurretCostText2,
        TurretCostText3,
        TurretCostText4,
        TurretCostText5,
        TurretCostText6,
        TurretCostText7,    //~7
        TurretNumText0,
        TurretNumText1, //�ͷ� ���� ���� 0~
        TurretNumText2,
        TurretNumText3,
        TurretNumText4,
        TurretNumText5,
        TurretNumText6,
        TurretNumText7, //~7
        RewardNumText0, //���� ���� ���� 0~
        RewardNumText1,
        RewardNumText2,
        RewardNumText3,
        RewardNumText4,
        RewardNumText5,  //~5
    }

    enum GameObjects
    {
        BattleAnalysisItem0,    //�����м� �г� 0~
        BattleAnalysisItem1,
        BattleAnalysisItem2,
        BattleAnalysisItem3,
        BattleAnalysisItem4,
        BattleAnalysisItem5,
        BattleAnalysisItem6,
        BattleAnalysisItem7,    //~7
        RewardPanel, //�����г�
        RewardItem0,    //���� ������ 0~
        RewardItem1,
        RewardItem2,
        RewardItem3,
        RewardItem4,    
        RewardItem5,    //~5
        StageFailText,   //�������� ����
        StageClearText,   //�������� Ŭ����
        StageResultPanel    //�г� UI��ü
    }

    enum Sliders
    {
        DamageSlider0,  //������ �����̴� 0~
        DamageSlider1,
        DamageSlider2,
        DamageSlider3,
        DamageSlider4,
        DamageSlider5,
        DamageSlider6,
        DamageSlider7,  //~7
        CostSlider0,    //�ڽ�Ʈ �����̴� 0~
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
        TurretImage0,   //�ͷ� �̹��� 0~
        TurretImage1,
        TurretImage2,
        TurretImage3,
        TurretImage4,
        TurretImage5,
        TurretImage6,
        TurretImage7,   //~7
        RewardImage0,   //���� ���� �̹��� 0~
        RewardImage1,
        RewardImage2,
        RewardImage3,
        RewardImage4,
        RewardImage5,   //~5
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Slider>(typeof(Sliders));

        //���ӿ���
        if (SystemManager.Instance.GameFlowManager.gameState == GameFlowManager.GameState.StageFail)
        {
            GetGameobject((int)GameObjects.StageClearText).SetActive(false);
        }
        else
        {
            GetGameobject((int)GameObjects.StageFailText).SetActive(false);

        }

        //�г� ��Ȱ��ȭ
        GetGameobject((int)GameObjects.StageResultPanel).SetActive(false);

        //�г� Ȱ��ȭ �ڷ�ƾ ȣ��
        StartCoroutine("OnResultPanel");
    }

    /// <summary>
    /// �г� ���� �ʱ���·� : ������
    /// </summary>
    void ResetStageEndPanel()
    {
        //�̱��� ���� ĳ��
        GameFlowManager gfm = SystemManager.Instance.GameFlowManager;
        ResourceManager rm = SystemManager.Instance.ResourceManager;

        for (int i = 0; i < MAXTURRETPRESETNUM; i++)
        {
            //�г� ��Ȱ��ȭ
            GetGameobject((int)GameObjects.BattleAnalysisItem0 + i).SetActive(false);
        }

        //�����г� ��Ȱ��ȭ
        for (int i = 0; i < MAXREWARDNUM; i++)
        {
            GetGameobject((int)GameObjects.RewardItem0 + i).SetActive(false);
        }
        GetGameobject((int)GameObjects.RewardPanel).SetActive(false);

    }

    /// <summary>
    /// �г� ���� ������Ʈ : ������
    /// </summary>
    void UpdateStageEndPanel()
    {
        //�̱��� ���� ĳ��
        GameFlowManager gfm = SystemManager.Instance.GameFlowManager;
        ResourceManager rm = SystemManager.Instance.ResourceManager;
        TurretManager tm = SystemManager.Instance.TurretManager;
        RewardManager rwm = SystemManager.Instance.RewardManager;

        //����ó��
        if (!gfm || !rm || !tm)
            return;

        //�������� �ؽ�Ʈ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageText).text = "Stage " + gfm.stage.ToString();
        //Ÿ�� �ؽ�Ʈ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ResultTimeText).text = 
            ((int)(gfm.stageTime / 60)).ToString() + " �� " + ((int)(gfm.stageTime % 60)).ToString() + " �� ";


        int maxDamage = 0;  //�ͷ��� �� �������� ���� ū ��
        int maxCost = 0;    //�ͷ� �ڽ�Ʈ �� ���� ū ��

        for (int i = 0; i < rm.selectedTurretPreset.Count; i++)
        {
            //�ͷ� ��ȣ
            int turretNum = rm.selectedTurretPreset[i];

            //maxDamage���ϱ�
            if (gfm.turretBattleAnalysisDic.ContainsKey(turretNum))
            {
                if (maxDamage < gfm.turretBattleAnalysisDic[turretNum])
                    maxDamage = gfm.turretBattleAnalysisDic[turretNum];
            }

            //maxCost���ϱ�
            if (gfm.turretSummonAnalysisDic.ContainsKey(turretNum))
            {
                if (maxCost < (gfm.turretSummonAnalysisDic[turretNum] * tm.turretCostArr[turretNum]))
                    maxCost = (gfm.turretSummonAnalysisDic[turretNum] * tm.turretCostArr[turretNum]);
            }
        }

        //�ͷ� ������ �����м� ���� ����
        for (int i = 0; i < rm.selectedTurretPreset.Count; i++)
        {
            //�ͷ� ��ȣ
            int turretNum = rm.selectedTurretPreset[i];

            //��ųʸ��� ���Ե��ִ� �������� ���
            if (gfm.turretBattleAnalysisDic.ContainsKey(turretNum))
            {
                //�г� Ȱ��ȭ
                GetGameobject((int)GameObjects.BattleAnalysisItem0 + i).SetActive(true);

                //�ͷ� �̹��� ����
                GetImage((int)Images.TurretImage0 + i).sprite =
                    turretSprite[turretNum];

                //������ ����
                GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretDamageText0 + i).text =
                    gfm.turretBattleAnalysisDic[turretNum].ToString();

                //������ �����̴� ����
                GetSlider((int)Sliders.DamageSlider0 + i).value = (float)gfm.turretBattleAnalysisDic[turretNum] / (float)maxDamage;


                //�ͷ� �Ǽ� ���� ����
                GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretNumText0 + i).text =
                    "X" + gfm.turretSummonAnalysisDic[turretNum].ToString();

                //�ͷ� ���� ����
                GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretCostText0 + i).text =
                    SystemManager.Instance.TurretManager.turretCostArr[turretNum] + " X " +
                    gfm.turretSummonAnalysisDic[turretNum] + " = " +
                    (tm.turretCostArr[turretNum] * gfm.turretSummonAnalysisDic[turretNum]);

                //�ͷ� ���� �����̴� ����
                GetSlider((int)Sliders.CostSlider0 + i).value = (tm.turretCostArr[turretNum] * gfm.turretSummonAnalysisDic[turretNum]) / (float)maxCost;

            }

        }

        //----------�������� ������Ʈ----------
        //Ŭ���� ������ ���
        if (gfm.gameState == GameFlowManager.GameState.StageClear)
        {
            //�����г� Ȱ��ȭ
            GetGameobject((int)GameObjects.RewardPanel).SetActive(true);

            for (int i = 0; i < MAXREWARDNUM; i++)
            {
                //���� �����Ҷ� �г� Ȱ��ȭ
                if (rwm.colorWoodReward[i] > 0)
                    GetGameobject((int)GameObjects.RewardItem0 + i).SetActive(true);

                //���� �̹���
                GetImage((int)Images.RewardImage0 + i).sprite = woodSprite[i];
                //���� ����
                GetTextMeshProUGUI((int)TextMeshProUGUIs.RewardNumText0 + i).text =
                    "X" + rwm.colorWoodReward[i];
            }

        }

    }//end of UpdateStageEndPanel

    /// <summary>
    /// �����ð� �� ResultPanel Ȱ��ȭ �� ���� �ʱ�ȭ : ������
    /// </summary>
    IEnumerator OnResultPanel()
    {
        yield return new WaitForSeconds(1.0f);
        //�г� Ȱ��ȭ
        GetGameobject((int)GameObjects.StageResultPanel).SetActive(true);

        //�г� ���� ����
        ResetStageEndPanel();

        //�г� ���� ������Ʈ
        UpdateStageEndPanel();

    }

}
