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

    //�κ���� �÷��̾� ��ũ��Ʈ
    LobbyPlayer lobbyPlayer;

    //�ͷ� �̹���
    [SerializeField]
    Sprite[] turretSprite;

    //�������� �̹���
    [SerializeField]
    Sprite[] stageSprite;

    //�� �̹���
    [SerializeField]
    Sprite[] starSprite;

    [SerializeField]
    Sprite[] starSprite_Hard;

    enum Buttons
    {
        GameStartButton,    //���ӽ��۹�ư
        TurretSelectLeftArrowButton,    //�ͷ� ���� ��ũ�� <��ư
        TurretSelectRightArrowButton,    //�ͷ� ���� ��ũ�� >��ư
        BackLobbyMenuButton, //�κ� �޴��� ���ư��� ��ư
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
        TurretPresetClearButton, //�ͷ� ������ ���� ��ư
        StageSelectLeftArrowButton,   //�������� ���� ��ũ�� <��ư
        StageSelectRightArrowButton,    //�������� ���� ��ũ�� >��ư
        StageStartButton,    //���Ӿ� ���� ��ư
        StageMapCloseButton,    //�������� ���� ���� ���� ��ư
        StageSelectButton,   //�������� ���� ���� �ѱ� ��ư
        StageLeftArrowButton,   //�������� ���� ���� ���� ��ũ�� ������
        StageRightArrowButton,   //�������� ���� ���� ������ ��ũ�� ������
        StoryButton, //���丮�� ���� ��ư
        NormalButton,   //�븻��� ��ư
        HardButton      //�ϵ��� ��ư
    }

    enum GameObjects
    {
        LobbyMenuPanel, //LobbyMenu UI ����
        GameSettingPanel,   //GameSettring UI����
        TurretSelectScrollView, //�ͷ� ���ÿ� �ͷ� ����Ʈ ��ũ��
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
        StageItem0, //�������� ���� �г� 
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
        StageSelectPanel,    //�������� ���� �г�
        StageContent,    //�������� ���� ��� ���� ������Ʈ
        NormalSelectButtonImage,    //�븻��� ���� üũ ��� �̹���
        HardSelectButtonImage,    //�ϵ��� ���� üũ ��� �̹���
        NormalText, //�븻 ��� �ؽ�Ʈ
        HardText,   //�ϵ� ��� �ؽ�Ʈ
    }

    enum TextMeshProUGUIs
    {
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
        TurretPresetText0,    //�ͷ� ������ �ؽ�Ʈ 0~
        TurretPresetText1,
        TurretPresetText2,
        TurretPresetText3,
        TurretPresetText4,
        TurretPresetText5,
        TurretPresetText6,
        TurretPresetText7,   //~7
        TurretPresetCountText,   //�ͷ� �������� �ִ� ���Ѽ��� ���� �ͷ��� �ؽ�Ʈ
        ColorWoodText0,
        ColorWoodText1,
        ColorWoodText2,
        ColorWoodText3,
        ColorWoodText4,
        ColorWoodText5,
        StarNumText,    //�� �� ����
        HardStarNumText,    //�� �� ���� - Hard
        StarNumText_Menu,   //�޴��г� �� �� ����
        HardStarNumText_Menu,   //�޴��г� �� �� ���� - Hard
        StageNumText,    //�ִ� Ŭ������ ��������
        HardStageNumText,//�ִ� Ŭ������ �ϵ� ��������
        SelectedStageText   //������ ��������
    }

    enum Images
    {
        TurretImage0,   //�ͷ� ������ �̹��� 0~
        TurretImage1,
        TurretImage2,
        TurretImage3,
        TurretImage4,
        TurretImage5,
        TurretImage6,
        TurretImage7,   //~7
        StageImage, //���������� ��� ����
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
        SelectedStageStarImage, //������ �������� �� ����
        SelectedStage,   //������ �������� ��� �̹���
        MapPanel    //�� ��� �̹���
    }

    enum ToggleGroups
    {
        StageSelectPanel    //�������� ����
    }

    enum Scrollbars
    {
        MapScrollbar    //�������� ���� ��ũ�� ��
    }

	private void Update()
    {
        //UI ������Ʈ
        UpdateUI();
    }
    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
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

        //�κ� �޴��г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StoryButton).gameObject, OnClickStoryButton, Define.UIEvent.Click);


        //�ͷ� �����г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.BackLobbyMenuButton).gameObject, OnClickBackLobbyMenu, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickTurretSelectLeftArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickTurretSelectRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretPresetClearButton).gameObject, OnClickTurretPresetClearButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageStartButton).gameObject, OnClickStageStartButton, Define.UIEvent.Click);

        //�������� �����г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.StageMapCloseButton).gameObject, OnClickStageMapCloseButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageSelectButton).gameObject, OnClickStageSelectButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageRightArrowButton).gameObject, OnClickStageRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageLeftArrowButton).gameObject, OnClickStageLeftArrow, Define.UIEvent.Click);

        AddUIEvent(GetButton((int)Buttons.NormalButton).gameObject, OnClickStageMapOpenButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.HardButton).gameObject, OnClickHardStageMapOpenButton, Define.UIEvent.Click);


        //�������� ĳ��
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //�κ��г� �ʱ�ȭ
        
        //���� �� ���� 
        int totStarNum = 0;
        int totHardStarNum = 0;
        for (int i = 0; i <= userInfo.maxStageNum; i++)
        {
            //���� �� ���� ���ϱ�
            totStarNum += userInfo.stageStarList[i].starNum;
        }
        for (int i = 0; i <= userInfo.maxStageNum_hard; i++)
        {
            //���� �� ���� ���ϱ� - �ϵ�
            totHardStarNum += userInfo.stageStarList_hard[i].starNum;
        }

        //�������� �̹��� �ٲٱ�
        if (totStarNum <= 40)
            GetImage((int)Images.StageImage).sprite = stageSprite[0]; //�����
        else if(totStarNum <= 73)
            GetImage((int)Images.StageImage).sprite = stageSprite[1]; //�ǹ�
        else if (totStarNum <= 85)
            GetImage((int)Images.StageImage).sprite = stageSprite[2]; //���
        else if (totStarNum <= 100)
            GetImage((int)Images.StageImage).sprite = stageSprite[3]; //�÷�Ƽ��
        else if (totStarNum <= 122)
            GetImage((int)Images.StageImage).sprite = stageSprite[4]; //���̾�
        else
            GetImage((int)Images.StageImage).sprite = stageSprite[5]; //������

        GetTextMeshProUGUI((int)TextMeshProUGUIs.StarNumText).text = "X" + totStarNum.ToString();
        GetTextMeshProUGUI((int)TextMeshProUGUIs.HardStarNumText).text = "X" + totHardStarNum.ToString();
        //�޴��г� �� �� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StarNumText_Menu).text = "X" + totStarNum.ToString();
        GetTextMeshProUGUI((int)TextMeshProUGUIs.HardStarNumText_Menu).text = "X" + totHardStarNum.ToString();

        //�ִ� Ŭ������ ��������
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = 
            "max:" + userInfo.maxStageNum.ToString();
        //�ִ� Ŭ������ �ϵ� ��������
        GetTextMeshProUGUI((int)TextMeshProUGUIs.HardStageNumText).text =
            "max:" + userInfo.maxStageNum_hard.ToString();

        for (int i = 0; i < MAXTCOLORWOOD; i++)
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.ColorWoodText0 + i).text = userInfo.colorWoodResource[i].ToString();
        }

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
            if (i >= userInfo.maxTurretNum)
                GetGameobject((int)GameObjects.TurretPanel0 + i).SetActive(false);

        }

        //�κ������ hotelPino ���ӿ�����Ʈ ã�Ƽ� ��ü�� LobbyPlayer��ũ��Ʈ ��������
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //�ͷ� ������ �ʱ�ȭ
        ResetTurretPreset();

        //���� �������� ���� ������Ʈ
        //UpdateSelectedStageInfo();
        OnClickStageSelectButton();

        //�ݰ� �гο� ���� ������Ʈ
        OnClickStageMapCloseButton();

        //GameSetting UI ��Ȱ��ȭ
        GetGameobject((int)GameObjects.GameSettingPanel).SetActive(false);
    }

    /// <summary>
    /// ���ӽ��� ��ư Ŭ���� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickGameStartButton(PointerEventData data)
    {
        //Lobby UI�ݱ�
        GetGameobject((int)GameObjects.LobbyMenuPanel).SetActive(false);

        //�÷��̾� ���� �ִϸ��̼�
        lobbyPlayer.animator.SetBool("gameStart", true);

        //ī�޶� ���� �ִϸ��̼�
        lobbyPlayer.camAnimator.Play("CamAnim");
    }

    /// <summary>
    /// ���ӽ��� ��ư Ŭ���� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickBackLobbyMenu(PointerEventData data)
    {
        //Lobby UI�ݱ�
        GetGameobject((int)GameObjects.GameSettingPanel).SetActive(false);

        //�÷��̾� ���� �ִϸ��̼�
        lobbyPlayer.animator.SetBool("gameStart", false);
        lobbyPlayer.animator.Play("Idle");

        //ī�޶� ���� �ִϸ��̼�
        lobbyPlayer.camAnimator.Play("CamBackAnim");
    }

    /// <summary>
    /// ���Ӿ����� �̵� :������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param> 
    void OnClickStageStartButton(PointerEventData data)
    {
        SceneController.Instance.LoadScene(SceneController.Instance.gameSceneName);
    }

    /// <summary>
    /// ���丮������ �̵� :������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param> 
    void OnClickStoryButton(PointerEventData data)
    {
        SceneController.Instance.LoadScene(SceneController.Instance.storySceneName);
    }

    /// <summary>
    /// UI ������Ʈ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void UpdateUI()
    {
        //ī�޶� ������ ���� �Ǿ��� ��� GameSetting UI���� - LoobyMenu -> GameSetting
        if (lobbyPlayer.camAnimator.GetBool("finCameraWalk"))
        {
            //GameSetting UI �ѱ�
            GetGameobject((int)GameObjects.GameSettingPanel).SetActive(true);

            //�ѹ��� ����ǰ�
            lobbyPlayer.camAnimator.SetBool("finCameraWalk", false);
        }

        //ī�޶� ������ ���� �Ǿ��� ��� LoobyMenu UI���� - GameSetting -> LoobyMenu
        if (lobbyPlayer.camAnimator.GetBool("finCameraBack"))
        {
            //GameSetting UI �ѱ�
            GetGameobject((int)GameObjects.LobbyMenuPanel).SetActive(true);

            //�ѹ��� ����ǰ�
            lobbyPlayer.camAnimator.SetBool("finCameraBack", false);
        }
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
        //---����Ʈ ���ݼ� �������� ����---
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

        //---����Ʈ ���ݼ� �������� ����---

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
            GetImage((int)Images.TurretImage0 + i).sprite = turretSprite[SystemManager.Instance.UserInfo.turretPreset[i]];

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

    #region �������� ����
    /// <summary>
    /// �������� ���� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickStageMapCloseButton(PointerEventData data = null)
    {
        //������ �������� ���� ĳ��
        UserInfo userInfo = SystemManager.Instance.UserInfo;
        GameObject SelectedToggle = GetToggleGroup((int)ToggleGroups.StageSelectPanel).GetFirstActiveToggle().gameObject;
        Sprite SelectedToggleImage = SelectedToggle.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;
        Sprite SelectedStarImage = SelectedToggle.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite;
        string SelectedStageNum = SelectedToggle.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;

        if (userInfo.selectMode == 0)   //�븻
        {
            //������ �������� ���� ���� ������ ����
            userInfo.selectedStageNum = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));

            //�гο� ������ �������� ���� ����
            GetImage((int)Images.SelectedStage).sprite = SelectedToggleImage;
            GetImage((int)Images.SelectedStageStarImage).sprite = SelectedStarImage;
            GetTextMeshProUGUI((int)TextMeshProUGUIs.SelectedStageText).text = SelectedStageNum;

            if (GetGameobject((int)GameObjects.StageSelectPanel))
                GetGameobject((int)GameObjects.StageSelectPanel).SetActive(false);
        }
        else    //�ϵ�
        {
            //������ �������� ���� ���� ������ ����
            userInfo.selectedStageNum_hard = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));

            //�гο� ������ �������� ���� ����
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
    /// �κ񿡼� �������� �����ѱ� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickStageSelectButton(PointerEventData data = null)
    {
        //�븻
        if (SystemManager.Instance.UserInfo.selectMode == 0)
        {
            OnClickStageMapOpenButton();
        }
        //�ϵ�
        else
        {
            OnClickHardStageMapOpenButton();
        }

        EnableStageModeText();
    }

    /// <summary>
    /// �������� ��忡 �´� �븻/�ϵ��� ���� �ؽ�Ʈ Ȱ��ȭ : ������
    /// </summary>
    void EnableStageModeText()
    {
        //�븻
        if (SystemManager.Instance.UserInfo.selectMode == 0)
        {
            if (!GetGameobject((int)GameObjects.NormalText).activeSelf)
                GetGameobject((int)GameObjects.NormalText).SetActive(true);
            if (GetGameobject((int)GameObjects.HardText).activeSelf)
                GetGameobject((int)GameObjects.HardText).SetActive(false);
        }
        //�ϵ�
        else
        {
            if (GetGameobject((int)GameObjects.NormalText).activeSelf)
                GetGameobject((int)GameObjects.NormalText).SetActive(false);
            if (!GetGameobject((int)GameObjects.HardText).activeSelf)
                GetGameobject((int)GameObjects.HardText).SetActive(true);
        }
    }

    /// <summary>
    /// �������� ���� �ѱ� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickStageMapOpenButton(PointerEventData data = null)
    {
        if (GetGameobject((int)GameObjects.StageSelectPanel))
            GetGameobject((int)GameObjects.StageSelectPanel).SetActive(true);

        //������ ������ �������� ���� ����
        //������ �������� ����
        GameObject SelectedToggle = GetToggleGroup((int)ToggleGroups.StageSelectPanel).GetFirstActiveToggle().gameObject;
        string SelectedStageNum = SelectedToggle.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        if (SystemManager.Instance.UserInfo.selectMode == 1)   //�ϵ�
        {
            //������ �������� ���� ���� ������ ����
            SystemManager.Instance.UserInfo.selectedStageNum_hard = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));
        }

        //�� ��� �̹���
        if (GetImage((int)Images.MapPanel).gameObject.activeSelf)
            GetImage((int)Images.MapPanel).color = Color.white;

        //��� �̹���
        if (!GetGameobject((int)GameObjects.NormalSelectButtonImage).activeSelf)
            GetGameobject((int)GameObjects.NormalSelectButtonImage).SetActive(true);
        if (GetGameobject((int)GameObjects.HardSelectButtonImage).activeSelf)
            GetGameobject((int)GameObjects.HardSelectButtonImage).SetActive(false);
        //�������� ���� ����
        SystemManager.Instance.UserInfo.selectMode = 0; //�븻

        //�������� �г�
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

        //������ �������� ��� ����
        UpdateSelectedStageInfo();
    }

    /// <summary>
    /// �������� ���� �ѱ� - �ϵ�: ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickHardStageMapOpenButton(PointerEventData data = null)
    {
        if (GetGameobject((int)GameObjects.StageSelectPanel))
            GetGameobject((int)GameObjects.StageSelectPanel).SetActive(true);

        //������ ������ �������� ���� ����
        //������ �������� ����
        GameObject SelectedToggle = GetToggleGroup((int)ToggleGroups.StageSelectPanel).GetFirstActiveToggle().gameObject;
        string SelectedStageNum = SelectedToggle.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        if (SystemManager.Instance.UserInfo.selectMode == 0)    //�븻
        {
            //������ �������� ���� ���� ������ ����
            SystemManager.Instance.UserInfo.selectedStageNum = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));
        }

        //�� ��� �̹���
        if (GetImage((int)Images.MapPanel).gameObject.activeSelf)
            GetImage((int)Images.MapPanel).color = new Color(1, 0.5f, 0.5f);

        //��� �̹���
        if (GetGameobject((int)GameObjects.NormalSelectButtonImage).activeSelf)
            GetGameobject((int)GameObjects.NormalSelectButtonImage).SetActive(false);
        if (!GetGameobject((int)GameObjects.HardSelectButtonImage).activeSelf)
            GetGameobject((int)GameObjects.HardSelectButtonImage).SetActive(true);
        //�������� ���� ����
        SystemManager.Instance.UserInfo.selectMode = 1; //�ϵ�

        //�������� �г�
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

        //������ ��� ����
        UpdateSelectedStageInfo();
    }

    /// <summary>
    /// ���õ� �������� ���� ���� : ������
    /// </summary>
    void UpdateSelectedStageInfo()
    {
        UserInfo userInfo = SystemManager.Instance.UserInfo;
        Toggle toggle;

        if (userInfo.selectMode == 0)   //�븻
            toggle = GetGameobject((int)GameObjects.StageContent).transform.GetChild(userInfo.selectedStageNum).GetComponent<Toggle>();
        else
            toggle = GetGameobject((int)GameObjects.StageContent).transform.GetChild(userInfo.selectedStageNum_hard).GetComponent<Toggle>();

        toggle.isOn = true;
    }

    /// <summary>
    /// �������� ���� ��ũ�� ������ ������ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickStageRightArrow(PointerEventData data)
    {
        GetScrollBar((int)Scrollbars.MapScrollbar).value = 1;
    }

    /// <summary>
    /// �������� ���� ��ũ�� ���� ������ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickStageLeftArrow(PointerEventData data)
    {
        GetScrollBar((int)Scrollbars.MapScrollbar).value = 0;
    }
    #endregion

}
