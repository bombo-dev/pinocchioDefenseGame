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

    enum TextMeshProUGUIs
    {
        StageText,  //���� �������� ����
        ResultTime,  //�ɸ� �ð�
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

        //�г� ���� ����
        ResetStageEndPanel();

        //�г� ���� ������Ʈ
        UpdateStageEndPanel();
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

        //����ó��
        if (!gfm || !rm || !tm)
            return;

        int maxDamage = 0;  //�ͷ��� �� �������� ���� ū ��
        for (int i = 0; i < rm.selectedTurretPreset.Count; i++)
        {
            //�ͷ� ��ȣ
            int turretNum = rm.selectedTurretPreset[i];

            if (gfm.turretBattleAnalysisDic.ContainsKey(turretNum))
            {
                if (maxDamage < gfm.turretBattleAnalysisDic[turretNum])
                    maxDamage = gfm.turretBattleAnalysisDic[turretNum];
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
                    "�� �Ǽ����: " + SystemManager.Instance.TurretManager.turretCostArr[turretNum] + " X " +
                    gfm.turretSummonAnalysisDic[turretNum] + "�� = " +
                    (SystemManager.Instance.TurretManager.turretCostArr[turretNum] * gfm.turretSummonAnalysisDic[turretNum]);
            }


        }

    }
}
