using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_OptionPopUpPanel : UI_Controller
{
    const int MAXTURRETNUM = 23;

    //���� �̹���
    [SerializeField]
    Sprite soundOnSprite;
    [SerializeField]
    Sprite soundOffSprite;

    enum Buttons
    {
        OptionButton,   //�ɼǹ�ư �˾�
        CloseOptionOpUpPanel,   //�ɼ��˾� �г� �ݱ�
        ExitOptionButton,   //��������
        BgSoundOptionPanel,  //����� ��ư
        EfSoundOptionPanel,  //ȿ���� ��ư
        ReStartOptionButton,  //�ٽý��� ��ư
        LobbyOptionButton,   //�κ�� ���ư��� ��ư
        TurretPediaButton,   //�ͷ� ���� ���� ��ư
        WoodPediaButton,    //���� ���� ���� ��ư
        CloseTurretPediaButton, //�ͷ� ���� �ݱ� ��ư
        CloseWoodPediaButton,   //���� ���� �ݱ� ��ư
        TouchSpeedPlus,  //��ġ �ӵ� ���� ��ư
        TouchSpeedMinus  //��ġ �ӵ� ���� ��ư
    }

    enum GameObjects
    {
        touchGuardPanel,    //�ٸ� UI��ġ���� �г�
        OptionPopUpPanel,    //�ɼ� �˾� �г�
        TurretPediaPanel,   //�ͷ� ���� �г�
        WoodPediaPanel,      //���� ���� �г�
        TurretItemPanel1,   //�ͷ� ���� ������ 1~
        TurretItemPanel2,
        TurretItemPanel3,
        TurretItemPanel4,
        TurretItemPanel5,
        TurretItemPanel6,
        TurretItemPanel7,
        TurretItemPanel8,
        TurretItemPanel9,
        TurretItemPanel10,
        TurretItemPanel11,
        TurretItemPanel12,
        TurretItemPanel13,
        TurretItemPanel14,
        TurretItemPanel15,
        TurretItemPanel16,
        TurretItemPanel17,
        TurretItemPanel18,
        TurretItemPanel19,
        TurretItemPanel20,
        TurretItemPanel21,
        TurretItemPanel22,
        TurretItemPanel23,  //~23
    }

    enum TextMeshProUGUIs
    {
        BgSoundOptionText,   //����� ��ư �ؽ�Ʈ
        EfSoundOptionText,   //ȿ���� ��ư �ؽ�Ʈ
        TouchSpeedText  //�ͷ� �ӵ� �ؽ�Ʈ
    }

    enum Images
    {
        BgSoundOptionImage,  //����� �̹���
        EfSoundOptionImage,  //ȿ���� �̹���
    }

    enum Scrollbars
    {
        BgSoundScrollbar,   //������ ���� ����
        EfSoundScrollbar,   //ȿ������ ���� ����
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
        Bind<Scrollbar>(typeof(Scrollbars));

        //�̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.OptionButton).gameObject, OnClickOptionPopUpButton, Define.UIEvent.Click);    //�ɼ��г� Ȱ��ȭ
        AddUIEvent(GetButton((int)Buttons.CloseOptionOpUpPanel).gameObject, OnClickCloseOptionPopUpButton, Define.UIEvent.Click);    //�ɼ��г� ��Ȱ��ȭ
        AddUIEvent(GetButton((int)Buttons.BgSoundOptionPanel).gameObject, OnClickBgSoundButton, Define.UIEvent.Click);    //����� 
        AddUIEvent(GetButton((int)Buttons.EfSoundOptionPanel).gameObject, OnClickEfSoundButton, Define.UIEvent.Click);    //ȿ���� 
        AddUIEvent(GetButton((int)Buttons.ExitOptionButton).gameObject, OnClickExitButton, Define.UIEvent.Click);    //������ 

        AddUIEvent(GetButton((int)Buttons.TurretPediaButton).gameObject, OnClickTurretPedia, Define.UIEvent.Click);    //�ͷ� ���� ����
        AddUIEvent(GetButton((int)Buttons.CloseTurretPediaButton).gameObject, OnClickCloseTurretPedia, Define.UIEvent.Click);    //�ͷ� ���� �ݱ�
        AddUIEvent(GetButton((int)Buttons.WoodPediaButton).gameObject, OnClickWoodPedia, Define.UIEvent.Click);    //���� ���� ����
        AddUIEvent(GetButton((int)Buttons.CloseWoodPediaButton).gameObject, OnClickCloseWoodPedia, Define.UIEvent.Click);    //���� ���� �ݱ�


        //�� ���� ��ư Ȱ��ȭ
        OnButton();

        //�������� �ʱ�ȭ
        InitializeSoundInfo();

        //GameScene
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //���Ӿ������� ���� �ɼ� ��Ȱ��ȭ
            GetButton((int)Buttons.TurretPediaButton).gameObject.SetActive(false);
            GetButton((int)Buttons.WoodPediaButton).gameObject.SetActive(false);

            //��ġ�ӵ� �ɼ�
            AddUIEvent(GetButton((int)Buttons.TouchSpeedPlus).gameObject, OnClickTouchSpeedPlusButton, Define.UIEvent.Click);    //��ġ �ӵ� ����
            AddUIEvent(GetButton((int)Buttons.TouchSpeedMinus).gameObject, OnClickTouchSpeedMinusButton, Define.UIEvent.Click);    //��ġ �ӵ� ����

            //��ġ �ӵ� �ؽ�Ʈ ����
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TouchSpeedText).text = SystemManager.Instance.UserInfo.touchSpeed.ToString();
        }

        //�ɼ��г� �˾� �ݱ�
        GetGameobject((int)GameObjects.OptionPopUpPanel).SetActive(false);

        //���� �г� �ݱ�
        GetGameobject((int)GameObjects.TurretPediaPanel).SetActive(false);
        GetGameobject((int)GameObjects.WoodPediaPanel).SetActive(false);

        //��ġ���� �ݱ�
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(false);

        
    }

    #region �ɼ�

    /// <summary>
    /// �ɼ� �˾� �г� Ȱ��ȭ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickOptionPopUpButton(PointerEventData data)
    {
        //��ġ���� �г� Ȱ��ȭ�Ͽ� �ڿ��ִ� UI�� ��ġ�� ���´�
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(true);

        //�ɼ��г� �˾�
        GetGameobject((int)GameObjects.OptionPopUpPanel).SetActive(true);

        //UI���� ������
        this.transform.SetAsLastSibling();
    }

    /// <summary>
    /// �ɼ� �˾� �г� Ȱ��ȭ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickCloseOptionPopUpButton(PointerEventData data)
    {
        //�ɼ��г� �˾� �ݱ�
        GetGameobject((int)GameObjects.OptionPopUpPanel).SetActive(false);

        //��ġ���� �ݱ�
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(false);
    }

    /// <summary>
    /// �� ���� ��ư ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickExitButton(PointerEventData data)
    {
        //�������� ��� �÷��̸�� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else   //������
            UnityEngine.Application.Quit();
#endif

        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();

    }

    #region ����
    /// <summary>
    /// ����� �ѱ�/���� ��ư ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickBgSoundButton(PointerEventData data)
    {
        AudioSource audioSource = SoundManager.Instance.audioSource;

        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text.Equals("�ѱ�"))
        {
            //�Ҹ��ѱ�
            audioSource.mute = false;

            //�������� ������Ʈ
            SystemManager.Instance.UserInfo.isBgSound = true;

            //UI������Ʈ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "����";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            //�Ҹ�����
            audioSource.mute = true;

            //�������� ������Ʈ
            SystemManager.Instance.UserInfo.isBgSound = false;

            //UI������Ʈ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "�ѱ�";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOnSprite;
        }
    }

    /// <summary>
    /// ȿ���� �ѱ�/���� ��ư ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param> 
    void OnClickEfSoundButton(PointerEventData data)
    {
        List<AudioSource> audioSource = SoundEffectManager.Instance.effectAudioSource;

        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text.Equals("�ѱ�"))
        {
            //�Ҹ��ѱ�
            for (int i = 0; i < audioSource.Count; i++)
            {
                audioSource[i].mute = false;     
            }

            //�������� ������Ʈ
            SystemManager.Instance.UserInfo.isEfSound = true;

            //UI������Ʈ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "����";
            GetImage((int)Images.EfSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            //�Ҹ�����
            for (int i = 0; i < audioSource.Count; i++)
            {
                audioSource[i].mute = true;
            }

            //�������� ������Ʈ
            SystemManager.Instance.UserInfo.isEfSound = false;

            //UI������Ʈ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "�ѱ�";
            GetImage((int)Images.EfSoundOptionImage).sprite = soundOnSprite;
        }
    }

    /// <summary>
    /// ������ ���� ���� : ������
    /// </summary>
    public void OnValueChagneBgSoundSlider()
    {
        float value = GetScrollBar((int)Scrollbars.BgSoundScrollbar).value;
        //�������� ����
        SoundManager.Instance.audioSource.volume = value;

        //�������� ����
        SystemManager.Instance.UserInfo.bgSoundVolume = value;
    }

    /// <summary>
    /// ȿ������ ���� ���� : ������
    /// </summary>
    public void OnValueChagneEfSoundSlider()
    {
        float value = GetScrollBar((int)Scrollbars.EfSoundScrollbar).value;
        List<AudioSource> audioSource = SoundEffectManager.Instance.effectAudioSource;

        //�������� ����
        for (int i = 0; i < audioSource.Count; i++)
        {
            SoundEffectManager.Instance.effectAudioSource[i].volume = value;    
        }

        //�������� ����
        SystemManager.Instance.UserInfo.efSoundVolume = value;
    }

    /// <summary>
    /// ���� ���� ���� �ʱ�ȭ : ������
    /// </summary>
    void InitializeSoundInfo()
    {
        AudioSource bgAudioSource = SoundManager.Instance.audioSource;
        List<AudioSource> efAudioSource = SoundEffectManager.Instance.effectAudioSource;
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //�����
        bgAudioSource.volume = userInfo.bgSoundVolume;
        GetScrollBar((int)Scrollbars.BgSoundScrollbar).value = userInfo.bgSoundVolume;

        if (userInfo.isBgSound)
        {
            bgAudioSource.mute = false;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "����";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            bgAudioSource.mute = true;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "�ѱ�";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOnSprite;
        }
        //ȿ����
        GetScrollBar((int)Scrollbars.EfSoundScrollbar).value = userInfo.efSoundVolume;

        for (int i = 0; i < efAudioSource.Count; i++)
        {
            efAudioSource[i].volume = userInfo.efSoundVolume;

            if (userInfo.isEfSound)
            {
                efAudioSource[i].mute = false;

                if (i == 0)
                {
                    GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "����";
                    GetImage((int)Images.EfSoundOptionImage).sprite = soundOffSprite;
                }
            }
            else
            {
                efAudioSource[i].mute = true;

                if (i == 0)
                {
                    GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "�ѱ�";
                    GetImage((int)Images.EfSoundOptionImage).sprite = soundOnSprite;
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// ��ġ ���ǵ� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickTouchSpeedPlusButton(PointerEventData data)
    {
        if (SystemManager.Instance.UserInfo.touchSpeed >= 10)
            return;

        SystemManager.Instance.UserInfo.touchSpeed++;

        //�ؽ�Ʈ ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TouchSpeedText).text = SystemManager.Instance.UserInfo.touchSpeed.ToString();
    }

    /// <summary>
    /// ��ġ ���ǵ� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickTouchSpeedMinusButton(PointerEventData data)
    {
        if (SystemManager.Instance.UserInfo.touchSpeed <= 1)
            return;

        SystemManager.Instance.UserInfo.touchSpeed--;

        //�ؽ�Ʈ ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TouchSpeedText).text = SystemManager.Instance.UserInfo.touchSpeed.ToString();
    }

    /// <summary>
    /// ���Ӿ��� �ٽ� �ε� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickReStartButton(PointerEventData data)
    {
        SceneController.Instance.LoadScene(SceneController.Instance.gameSceneName);
    }

    /// <summary>
    /// �κ�� ���ư��� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickLobbyOptionButton(PointerEventData data)
    {
        SceneController.Instance.LoadScene(SceneController.Instance.lobbySceneName);
    }
    #endregion

    #region ����

    /// <summary>
    /// ������ư Ȱ��ȭ : ������
    /// </summary>
    public void EnablePediaButton()
    {
        //���� ��ư Ȱ��ȭ
        GetButton((int)Buttons.TurretPediaButton).gameObject.SetActive(true);
        GetButton((int)Buttons.WoodPediaButton).gameObject.SetActive(true);
    }

    /// <summary>
    /// �ͷ� ���� �г� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickTurretPedia(PointerEventData data)
    {
        //��ġ���� Ȱ��ȭ
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(true);

        GetGameobject((int)GameObjects.TurretPediaPanel).SetActive(true);

        //������ Ȱ��ȭ
        for (int i = 0; i < MAXTURRETNUM; i++)
        {
            if(i < SystemManager.Instance.UserInfo.maxTurretNum)
                GetGameobject((int)GameObjects.TurretItemPanel1 + i).SetActive(true);
            else
                GetGameobject((int)GameObjects.TurretItemPanel1 + i).SetActive(false);
        }

        //���Ӿ��϶� �Ͻ�����
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Time.timeScale > 0f && SystemManager.Instance.PanelManager.optionPanel)
                SystemManager.Instance.PanelManager.optionPanel.OnClickPlayOptionButton();
        }
        //UI���� ������
        this.transform.SetAsLastSibling();
    }

    /// <summary>
    /// ���� ���� �г� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickWoodPedia(PointerEventData data)
    {
        //��ġ���� Ȱ��ȭ
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(true);

        GetGameobject((int)GameObjects.WoodPediaPanel).SetActive(true);

        //���Ӿ��϶� �Ͻ�����
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Time.timeScale > 0f && SystemManager.Instance.PanelManager.optionPanel)
                SystemManager.Instance.PanelManager.optionPanel.OnClickPlayOptionButton();
        }
        //UI���� ������
        this.transform.SetAsLastSibling();
    }

    /// <summary>
    /// �ͷ� ���� �г� �ݱ� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickCloseTurretPedia(PointerEventData data)
    {
        GetGameobject((int)GameObjects.TurretPediaPanel).SetActive(false);

        //���Ӿ��϶� �Ͻ����� ����
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Time.timeScale == 0f && SystemManager.Instance.PanelManager.optionPanel)
                SystemManager.Instance.PanelManager.optionPanel.OnClickPlayOptionButton();
        }
        //UI���� ������
        this.transform.SetAsLastSibling();

        //��ġ���� ��Ȱ��ȭ
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(false);
    }

    /// <summary>
    /// ���� ���� �г� �ݱ� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickCloseWoodPedia(PointerEventData data)
    {
        GetGameobject((int)GameObjects.WoodPediaPanel).SetActive(false);

        //���Ӿ��϶� �Ͻ����� ����
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Time.timeScale == 0f && SystemManager.Instance.PanelManager.optionPanel)
                SystemManager.Instance.PanelManager.optionPanel.OnClickPlayOptionButton();
        }
        //UI���� ������
        this.transform.SetAsLastSibling();

        //��ġ���� ��Ȱ��ȭ
        GetGameobject((int)GameObjects.touchGuardPanel).SetActive(false);
    }


    #endregion

    /// <summary>
    /// ���Ӿ��� ��� �ٽý��� ��ư Ȱ��ȭ
    /// </summary>
    public void OnButton()
    {
        //���Ӿ��� ��� �ٽ��ϱ� �ɼ� Ȱ��ȭ
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //�̺�Ʈ�߰� - �ٽý���
            AddUIEvent(GetButton((int)Buttons.ReStartOptionButton).gameObject, OnClickReStartButton, Define.UIEvent.Click);

            //�̺�Ʈ�߰� - �κ��
            AddUIEvent(GetButton((int)Buttons.LobbyOptionButton).gameObject, OnClickLobbyOptionButton, Define.UIEvent.Click);
        }
    }
}
