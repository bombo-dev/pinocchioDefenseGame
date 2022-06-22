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

    //�κ���� �÷��̾� ��ũ��Ʈ
    LobbyPlayer lobbyPlayer;

    //�ͷ� �̹���
    [SerializeField]
    Sprite[] turretSprite;

    //�� �̹���
    [SerializeField]
    Sprite[] starSprite;

    //���� �̹���
    [SerializeField]
    Sprite soundOnSprite;
    [SerializeField]
    Sprite soundOffSprite;

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
        StageStartButton,    //���ӽ� ���� ��ư
        StageMapCloseButton,    //�������� ���� ���� ���� ��ư
        StageSelectButton,   //�������� ���� ���� �ѱ� ��ư
        StageLeftArrowButton,   //�������� ���� ���� ���� ��ũ�� ������
        StageRightArrowButton,   //�������� ���� ���� ������ ��ũ�� ������
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
        StageSelectPanel,    //�������� ���� �г�
        StageContent    //�������� ���� ��� ���� ������Ʈ
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
        StarNumText_Menu,   //�޴��г� �� �� ����
        StageNumText,    //�ִ� Ŭ������ ��������
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
        SelectedStageStarImage, //������ �������� �� ����
        SelectedStage   //������ �������� ��� �̹���
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

        //�κ� �޴��г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);

        //�ͷ� �����г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.BackLobbyMenuButton).gameObject, OnClickBackLobbyMenu, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickTurretSelectLeftArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickTurretSelectRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretPresetClearButton).gameObject, OnClickTurretPresetClearButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageStartButton).gameObject, OnClickStageStartButton, Define.UIEvent.Click);

        //�������� �����г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.StageMapCloseButton).gameObject, OnClickStageMapCloseButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageSelectButton).gameObject, OnClickStageMapOpenButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageRightArrowButton).gameObject, OnClickStageRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageLeftArrowButton).gameObject, OnClickStageLeftArrow, Define.UIEvent.Click);

        //�������� ĳ��
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //�κ��г� �ʱ�ȭ
        
        //���� �� ���� 
        int totStarNum = 0;
        for (int i = 0; i <= userInfo.maxStageNum; i++)
        {
            //���� �� ���� ���ϱ�
            totStarNum += userInfo.stageStarList[i].starNum;
        }

        GetTextMeshProUGUI((int)TextMeshProUGUIs.StarNumText).text = "X" + totStarNum.ToString();
        //�޴��г� �� �� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StarNumText_Menu).text = "X" + totStarNum.ToString();

        //�ִ� Ŭ������ ��������
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = 
            "Stage " + userInfo.maxStageNum.ToString();

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
        
        //�������� �г�
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

        //�κ������ hotelPino ���ӿ�����Ʈ ã�Ƽ� ��ü�� LobbyPlayer��ũ��Ʈ ��������
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //�ͷ� ������ �ʱ�ȭ
        ResetTurretPreset();

        //���� �������� ���� ������Ʈ
        UpdateSelectedStageInfo();

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
            GetImage((int)Images.TurretImage0 + i).sprite = turretSprite[SystemManager.Instance.UserInfo.turretPreset[i]];

            //�ؽ�Ʈ����
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretPresetText0 + i).text =
                SystemManager.Instance.TurretJson.GetTurretData()[SystemManager.Instance.UserInfo.turretPreset[i]].turretCost.ToString();
        }
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
        SystemManager.Instance.UserInfo.turretPreset.Clear();

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

        //������ �������� ���� ���� ������ ����
        userInfo.selectedStageNum = int.Parse(Regex.Replace(SelectedStageNum, @"\D", ""));

        //�гο� ������ �������� ���� ����
        GetImage((int)Images.SelectedStage).sprite = SelectedToggleImage;
        GetImage((int)Images.SelectedStageStarImage).sprite = SelectedStarImage;
        GetTextMeshProUGUI((int)TextMeshProUGUIs.SelectedStageText).text = SelectedStageNum;

        if (GetGameobject((int)GameObjects.StageSelectPanel))
            GetGameobject((int)GameObjects.StageSelectPanel).SetActive(false);
    }

    /// <summary>
    /// �������� ���� �ѱ� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickStageMapOpenButton(PointerEventData data)
    {
        if (GetGameobject((int)GameObjects.StageSelectPanel))
            GetGameobject((int)GameObjects.StageSelectPanel).SetActive(true);
    }

    /// <summary>
    /// ���õ� �������� ���� ���� : ������
    /// </summary>
    void UpdateSelectedStageInfo()
    {
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        Toggle toggle = GetGameobject((int)GameObjects.StageContent).transform.GetChild(userInfo.selectedStageNum).GetComponent<Toggle>();
        toggle.isOn = true;

        //�ݰ� �гο� ���� ������Ʈ
        OnClickStageMapCloseButton();
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
