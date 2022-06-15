using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyPanel : UI_Controller
{
    const int MAXTURRETNUM = 23;
    const int MAXTURRETPRESETNUM = 8;

    //�κ���� �÷��̾� ��ũ��Ʈ
    LobbyPlayer lobbyPlayer;

    //�ͷ� �̹���
    [SerializeField]
    Sprite[] turretSprite;

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
        StageSelectRightArrowButton    //�������� ���� ��ũ�� >��ư
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
        TurretPresetPanel7   //~7
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
        TurretPresetCountText   //�ͷ� �������� �ִ� ���Ѽ��� ���� �ͷ��� �ؽ�Ʈ
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
    }


    enum TMP_DropDowns
    {
        StageDropDown   //�������� ���� ��Ӵٿ�
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
        Bind<TMP_Dropdown>(typeof(TMP_DropDowns));

        //�κ� �޴��г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.GameStartButton).gameObject, OnClickGameStartButton, Define.UIEvent.Click);

        //�ͷ� �����г� UI �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.BackLobbyMenuButton).gameObject, OnClickBackLobbyMenu, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectLeftArrowButton).gameObject, OnClickTurretSelectLeftArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretSelectRightArrowButton).gameObject, OnClickTurretSelectRightArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretPresetClearButton).gameObject, OnClickTurretPresetClearButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageSelectLeftArrowButton).gameObject, OnClickStageSelectLeftArrow, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.StageSelectRightArrowButton).gameObject, OnClickStageSelectRightArrow, Define.UIEvent.Click);

        for (int i = 0; i < MAXTURRETPRESETNUM; i++)
        {
            AddUIEvent(GetButton((int)Buttons.TurretRemoveButton0 + i).gameObject, i, OnClickTurretRemoveButton, Define.UIEvent.Click);
        }

        for (int i = 0; i < MAXTURRETNUM; i++)
        {
            //�ͷ� ���� �̺�Ʈ �ʱ�ȭ
            AddUIEvent(GetButton((int)Buttons.TurretButton0 + i).gameObject, i, OnClickAddSelectTurret, Define.UIEvent.Click);

            //�ͷ� ���� Cost ���� �ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretText0 + i).text = SystemManager.Instance.TurretJson.GetTurretData()[i].turretCost.ToString();
        }

        //�������� ��Ӵٿ� �ʱ�ȭ
        for (int i = 1; i <= SystemManager.Instance.UserInfo.maxStageNum; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = "Stage" + i;
            GetDropDown((int)TMP_DropDowns.StageDropDown).options.Add(option);
        }

        //�κ������ hotelPino ���ӿ�����Ʈ ã�Ƽ� ��ü�� LobbyPlayer��ũ��Ʈ ��������
        lobbyPlayer = GameObject.FindObjectOfType<LobbyPlayer>();

        //�ͷ� ������ �ʱ�ȭ
        ResetTurretPreset();

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
    /// �������� 1���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickStageSelectRightArrow(PointerEventData data)
    {
        if(GetDropDown((int)TMP_DropDowns.StageDropDown).value < SystemManager.Instance.UserInfo.maxStageNum)
            GetDropDown((int)TMP_DropDowns.StageDropDown).value ++;
    }

    /// <summary>
    /// �������� 1���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickStageSelectLeftArrow(PointerEventData data)
    {
        if (GetDropDown((int)TMP_DropDowns.StageDropDown).value > 0)
            GetDropDown((int)TMP_DropDowns.StageDropDown).value --;
    }
    #endregion
}
