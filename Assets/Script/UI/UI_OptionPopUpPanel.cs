using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_OptionPopUpPanel : UI_Controller
{
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
    }

    enum GameObjects
    {
        touchGuardPanel,    //�ٸ� UI��ġ���� �г�
        OptionPopUpPanel    //�ɼ� �˾� �г�
    }

    enum TextMeshProUGUIs
    {
        BgSoundOptionText,   //����� ��ư �ؽ�Ʈ
        EfSoundOptionText,   //ȿ���� ��ư �ؽ�Ʈ
    }

    enum Images
    {
        BgSoundOptionImage,  //����� �̹���
        EfSoundOptionImage,  //ȿ���� �̹���
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

        //�̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.OptionButton).gameObject, OnClickOptionPopUpButton, Define.UIEvent.Click);    //�ɼ��г� Ȱ��ȭ
        AddUIEvent(GetButton((int)Buttons.CloseOptionOpUpPanel).gameObject, OnClickCloseOptionPopUpButton, Define.UIEvent.Click);    //�ɼ��г� ��Ȱ��ȭ
        AddUIEvent(GetButton((int)Buttons.BgSoundOptionPanel).gameObject, OnClickBgSoundButton, Define.UIEvent.Click);    //����� 
        AddUIEvent(GetButton((int)Buttons.EfSoundOptionPanel).gameObject, OnClickEfSoundButton, Define.UIEvent.Click);    //ȿ���� 
        AddUIEvent(GetButton((int)Buttons.ExitOptionButton).gameObject, OnClickExitButton, Define.UIEvent.Click);    //������ 


        //�ɼ��г� �˾� �ݱ�
        GetGameobject((int)GameObjects.OptionPopUpPanel).SetActive(false);

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

    }

    /// <summary>
    /// ����� �ѱ�/���� ��ư ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickBgSoundButton(PointerEventData data)
    {
        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text.Equals("�ѱ�"))
        {
            //�Ҹ��ѱ�
            GetTextMeshProUGUI((int)TextMeshProUGUIs.BgSoundOptionText).text = "����";
            GetImage((int)Images.BgSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            //�Ҹ�����
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
        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text.Equals("�ѱ�"))
        {
            //�Ҹ��ѱ�
            GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "����";
            GetImage((int)Images.EfSoundOptionImage).sprite = soundOffSprite;
        }
        else
        {
            //�Ҹ�����
            GetTextMeshProUGUI((int)TextMeshProUGUIs.EfSoundOptionText).text = "�ѱ�";
            GetImage((int)Images.EfSoundOptionImage).sprite = soundOnSprite;
        }
    }
    #endregion
}
