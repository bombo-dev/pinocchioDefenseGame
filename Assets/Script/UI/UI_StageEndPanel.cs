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
    Sprite[] turretSprite;  //�ͷ� �̹��� ����

    [SerializeField]
    Sprite[] woodSprite;    //���� �̹��� ����

    [SerializeField]
    Sprite[] starSprite;    //�� �̹��� ����

    [SerializeField]
    Sprite panelSpriteLight;
    [SerializeField]
    Sprite panelSpriteDark;

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
        RewardNumText5,
        RewardNumText6,//~6
        TurretPresetText0,    //�ͷ� ������ �ؽ�Ʈ 0~
        TurretPresetText1,
        TurretPresetText2,
        TurretPresetText3,
        TurretPresetText4,
        TurretPresetText5,
        TurretPresetText6,
        TurretPresetText7,   //~7
        TurretPresetCountText,   //�ͷ� �������� �ִ� ���Ѽ��� ���� �ͷ��� �ؽ�Ʈ
        TurretText0,    //�ͷ� �����г� �ؽ�Ʈ 0~
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
        newTurretText //���ο� �ͷ� �߰� �˸� �ؽ�Ʈ
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
        RewardItem5,
        RewardItem6,//~6
        StageFailText,   //�������� ����
        StageClearText,   //�������� Ŭ����
        StageResultPanel,    //�г� UI��ü
        TurretPresetPanel0,   //0~
        TurretPresetPanel1,
        TurretPresetPanel2,
        TurretPresetPanel3,
        TurretPresetPanel4,
        TurretPresetPanel5,
        TurretPresetPanel6,
        TurretPresetPanel7,   //~7
        TurretSelectScrollView, //�ͷ� ���ÿ� �ͷ� ����Ʈ ��ũ��
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
        newTurretPanel  //���� �߰��� �ͷ� �˸���
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
        RewardImage5,
        RewardImage6,//~6
        StarRewardImage, //�� ���� �̹���
        StageResultPanel,    //�г� UI��ü
        StageResultBackGround,   //StageResultPanel �� �г�
        TurretPresetImage0,   //�ͷ� ������ �̹��� 0~
        TurretPresetImage1,
        TurretPresetImage2,
        TurretPresetImage3,
        TurretPresetImage4,
        TurretPresetImage5,
        TurretPresetImage6,
        TurretPresetImage7,   //~7
        newTurretImage  //���� �߰��� �ͷ� �̹���
    }

    enum Buttons
    {
        ReStartButton,  //�ٽý��� ��ư
        ExitButton,  //������ ��ư
        NextStageButton,  //���� �������� ��ư
        TurretSelectLeftArrowButton,    //�ͷ� ���� ��ũ�� <��ư
        TurretSelectRightArrowButton,    //�ͷ� ���� ��ũ�� >��ư
        TurretPresetClearButton, //�ͷ� ������ ���� ��ư
        TurretButton0,  //�ͷ�����Ʈ 0~
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
        TurretRemoveButton0, //�����¿� �ִ� �ͷ� ���� ��ư 0~
        TurretRemoveButton1,
        TurretRemoveButton2,
        TurretRemoveButton3,
        TurretRemoveButton4,
        TurretRemoveButton5,
        TurretRemoveButton6,
        TurretRemoveButton7, // ~7
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
        Bind<Button>(typeof(Buttons));

        //�̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.ReStartButton).gameObject, OnClickRestartButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.ExitButton).gameObject, OnClickExitButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.NextStageButton).gameObject, OnClickNextStageButton, Define.UIEvent.Click);

        //�ͷ� �����г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickTurretSelectLeftArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickTurretSelectRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretPresetClearButton).gameObject, OnClickTurretPresetClearButton, Define.UIEvent.Click);

        for (int i = 0; i < MAXTURRETPRESETNUM; i++)
        {
            //�ͷ� ���� ���� �̺�Ʈ �ʱ�ȭ
            AddUIEvent(GetButton((int)Buttons.TurretRemoveButton0 + i).gameObject, i, OnClickTurretRemoveButton, Define.UIEvent.Click);
        }

        for (int i = 0; i < MAXTURRETNUM; i++)
        {
            //�ͷ� ���� �̺�Ʈ �ʱ�ȭ
            AddUIEvent(GetButton((int)Buttons.TurretButton0 + i).gameObject, i, OnClickAddSelectTurret, Define.UIEvent.Click);

            //�ͷ� ���� Cost ���� �ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretText0 + i).text = SystemManager.Instance.TurretJson.GetTurretData()[i].turretCost.ToString();

            //�ͷ� ����Ʈ �ʱ�ȭ
            if (i >= SystemManager.Instance.UserInfo.maxTurretNum)
                GetGameobject((int)GameObjects.TurretPanel0 + i).SetActive(false);

        }


        //���ӿ���
        if (SystemManager.Instance.GameFlowManager.gameState == GameFlowManager.GameState.StageFail)
        {
            GetGameobject((int)GameObjects.StageClearText).SetActive(false);
            GetButton((int)Buttons.NextStageButton).gameObject.SetActive(false);
        }
        else
        {
            GetGameobject((int)GameObjects.StageFailText).SetActive(false);

        }

        //�ͷ� ������ �ʱ�ȭ
        ResetTurretPreset();

        //�г� ��Ȱ��ȭ
        GetGameobject((int)GameObjects.StageResultPanel).SetActive(false);

        //�ű� �ͷ� ȹ�� �г� ��Ȱ��ȭ
        GetGameobject((int)GameObjects.newTurretPanel).SetActive(false);

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
        UserInfo ui = SystemManager.Instance.UserInfo;

        //����ó��
        if (!gfm || !rm || !tm)
            return;

        //�������� �ؽ�Ʈ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageText).text = "Stage " + gfm.stage.ToString();
        //Ÿ�� �ؽ�Ʈ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ResultTimeText).text = 
            "Time: " + ((int)(gfm.stageTime / 60)).ToString() + " �� " + ((int)(gfm.stageTime % 60)).ToString() + " �� ";


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

            //�� ���� �̹��� ��ü
            GetImage((int)Images.StarRewardImage).sprite = starSprite[SystemManager.Instance.RewardManager.starRewardNum];

            for (int i = 0; i < MAXREWARDNUM - 1; i++)
            {
                //���� �����Ҷ� �г� Ȱ��ȭ
                if (rwm.colorWoodReward[i] > 0)
                    GetGameobject((int)GameObjects.RewardItem0 + i).SetActive(true);
                else
                    continue;

                //���� �̹���
                GetImage((int)Images.RewardImage0 + i).sprite = woodSprite[i];
                //���� ����
                GetTextMeshProUGUI((int)TextMeshProUGUIs.RewardNumText0 + i).text =
                    "X" + rwm.colorWoodReward[i];
            }


            //�߰� �ͷ� ���� (������ �ε����� �ͷ� ���� �߰�)
            if (rwm.turretReward.ContainsKey(gfm.stage)) //�ͷ� ���� �����Ұ��
            {
                //�ű� �ͷ� ȹ�� �г� Ȱ��ȭ
                GetGameobject((int)GameObjects.newTurretPanel).SetActive(true);

                //�̹� ������ �ִ� �ͷ��� �ƴѰ��
                if (rwm.getNewTurret)
                {
                    //�ͷ����� ������Ʈ
                    ui.maxTurretNum = rwm.turretReward[gfm.stage];

                    //�гΰ���
                    //�г� Ȱ��ȭ
                    GetGameobject((int)GameObjects.RewardItem6).SetActive(true);
                    //���� �̹���
                    GetImage((int)Images.RewardImage6).sprite = turretSprite[rwm.turretReward[gfm.stage] - 1];
                    //���� ����
                    GetTextMeshProUGUI((int)TextMeshProUGUIs.RewardNumText6).text =
                        "X1";

                    rwm.getNewTurret = false;
                }

                //�ű��ͷ� ȹ�� ����
                GetImage((int)Images.newTurretImage).sprite = turretSprite[rwm.turretReward[gfm.stage] - 1];
                StartCoroutine(newTurretTyping(GetTextMeshProUGUI((int)TextMeshProUGUIs.newTurretText), "�ű� �ͷ��� �÷��ǿ� �߰��Ǿ����ϴ�!", 0.03f));
            }

            //�г��̹��� ��ü
            GetImage((int)Images.StageResultPanel).sprite = panelSpriteLight;
            GetImage((int)Images.StageResultBackGround).sprite = panelSpriteLight;
        }
        //���ӿ��� ������ ���
        else
        {
            //�г��̹��� ��ü
            GetImage((int)Images.StageResultPanel).sprite = panelSpriteDark;
            GetImage((int)Images.StageResultBackGround).sprite = panelSpriteDark;
        }

    }//end of UpdateStageEndPanel

    /// <summary>
    /// �� ��ε��Ͽ� �ٽ� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickRestartButton(PointerEventData data)
    {
        //�������� ����
        SystemManager.Instance.UserInfo.selectedStageNum = SystemManager.Instance.GameFlowManager.stage;

        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();

        SceneController.Instance.LoadScene(SceneController.Instance.gameSceneName);
    }

    /// <summary>
    /// �κ������ ���ư��� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickExitButton(PointerEventData data)
    {
        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();

        //�� �̵�
        SceneController.Instance.LoadScene(SceneController.Instance.lobbySceneName);
    }

    /// <summary>
    /// ���� ���������� �̵� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickNextStageButton(PointerEventData data)
    {
        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();

        //����ó��
        if (SystemManager.Instance.UserInfo.maxStageNum < SystemManager.Instance.GameFlowManager.stage + 1)
            return;

        //�������� ����
        SystemManager.Instance.UserInfo.selectedStageNum = SystemManager.Instance.GameFlowManager.stage + 1;
        save.SaveUserInfo();

        //�� �̵�
        SceneController.Instance.LoadScene(SceneController.Instance.gameSceneName);
    }

    /// <summary>
    /// �����ð� �� ResultPanel Ȱ��ȭ �� ���� �ʱ�ȭ : ������
    /// </summary>
    IEnumerator OnResultPanel()
    {
        yield return new WaitForSeconds(2.5f);
        //�г� Ȱ��ȭ
        GetGameobject((int)GameObjects.StageResultPanel).SetActive(true);

        //�г� ���� ����
        ResetStageEndPanel();

        //�г� ���� ������Ʈ
        UpdateStageEndPanel();
    }


    #region �ͷ� ����
    /// <summary>
    /// �ͷ� ���� ��ũ�� ������ ������ �̵� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickTurretSelectRightArrow(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretSelectScrollView).GetComponent<ScrollRect>().normalizedPosition = new Vector2(1, 0);
    }

    /// <summary>
    /// �ͷ� ���� ��ũ�� ���� ������ �̵� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickTurretSelectLeftArrow(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretSelectScrollView).GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }

    /// <summary>
    /// �ͷ� �������� �ʱ�ȭ : ������
    /// </summary>
    void ResetTurretPreset()
    {
        //����Ʈ �������� ����
        if (SystemManager.Instance.UserInfo.turretPreset.Count > 1)
            SystemManager.Instance.UserInfo.turretPreset.Sort();

        //�ؽ�Ʈ ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretPresetCountText).text = SystemManager.Instance.UserInfo.turretPreset.Count + "/8";

        //�ͷ� ������ �ʱ�ȭ
        for (int i = 0; i < MAXTURRETPRESETNUM; i++)
        {
            //�г� ��Ȱ��ȭ
            GetGameobject((int)GameObjects.TurretPresetPanel0 + i).SetActive(false);
        }

        //�ͷ� ������ ����
        for (int i = 0; i < SystemManager.Instance.UserInfo.turretPreset.Count; i++)
        {
            //�г� Ȱ��ȭ
            GetGameobject((int)GameObjects.TurretPresetPanel0 + i).SetActive(true);

            //�̹�������
            GetImage((int)Images.TurretPresetImage0 + i).sprite = turretSprite[SystemManager.Instance.UserInfo.turretPreset[i]];

            //�ؽ�Ʈ����
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretPresetText0 + i).text =
                SystemManager.Instance.TurretJson.GetTurretData()[SystemManager.Instance.UserInfo.turretPreset[i]].turretCost.ToString();
        }

        // userinfo Save
        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();
    }

    /// <summary>
    /// �ͷ� �����¿� ������ �ͷ��� �߰� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    /// <param name="idx">�߰��� �ͷ� �ε���</param>
    void OnClickAddSelectTurret(PointerEventData data, int idx)
    {
        if (SystemManager.Instance.UserInfo.turretPreset.Count >= 8)
            return;

        if (!SystemManager.Instance.UserInfo.turretPreset.Contains(idx) && idx >= 0)
            SystemManager.Instance.UserInfo.turretPreset.Add(idx);

        //�ͷ� ������ ����
        ResetTurretPreset();
    }

    /// <summary>
    /// �ͷ� �����¿��� �ͷ��� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    /// <param name="idx">������ �ͷ� �ε���</param>
    void OnClickTurretRemoveButton(PointerEventData data, int idx)
    {
        //1���� �������� ���� �Ұ�
        if (SystemManager.Instance.UserInfo.turretPreset.Count <= 1)
            return;

        //�ش� �ε��� �ͷ� ����
        if (idx >= 0)
            SystemManager.Instance.UserInfo.turretPreset.RemoveAt(idx);

        //�ͷ� ������ ����
        ResetTurretPreset();
    }

    /// <summary>
    /// �ͷ� ������ ��� ��� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickTurretPresetClearButton(PointerEventData data)
    {
        if (SystemManager.Instance.UserInfo.turretPreset.Count > 1)
        {
            SystemManager.Instance.UserInfo.turretPreset.RemoveRange(1, SystemManager.Instance.UserInfo.turretPreset.Count - 1);
        }

        //�ͷ� ������ ����
        ResetTurretPreset();
    }
    #endregion


    /// <summary>
    /// Ÿ���� ȿ�� : ������
    /// </summary>
    /// <param name="typingText">Ÿ���� ȿ���� �� �ؽ�Ʈ</param>
    /// <param name="message">�ؽ�Ʈ ����</param>
    /// <param name="speed">Ÿ���� �ӵ�</param>
    IEnumerator newTurretTyping(TextMeshProUGUI typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        //���
        yield return new WaitForSeconds(3.0f);

        //�ű� �ͷ� ȹ�� �г� ��Ȱ��ȭ
        GetGameobject((int)GameObjects.newTurretPanel).SetActive(false);
    }
}
