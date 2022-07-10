using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BookPanel : UI_Controller
{
    public int page = 0;

    [SerializeField]
    Sprite[] bookSprit;

    enum TextMeshProUGUIs
    {
        BookText,   //å ���丮 �ؽ�Ʈ
    }

    enum Buttons
    {
        NextButton, //������ �Ѿ�� ��ư
        SkipButton,  //��ŵ��ư
        PrevButton   //���� ��ư
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));

        //��ư �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.NextButton).gameObject, OnClickNextButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.SkipButton).gameObject, OnClickSkipButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.PrevButton).gameObject, OnClickPrevButton, Define.UIEvent.Click);

        if (!(SystemManager.Instance.UserInfo.isShowBook))
        {
            //�ʱ�ȭ
            SystemManager.Instance.PanelManager.bookPanel.page = 0;
            SystemManager.Instance.PanelManager.bookPanel.UpdateBook();
        }
        else
        {
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);
        }
    }

    /// <summary>
    /// ���丮 �����ֱ� : ������
    /// </summary>
    public void UpdateBook()
    {
        //�ڷ�ƾ ����
        StopAllCoroutines();

        //UI���� ������
        this.transform.SetAsLastSibling();

        switch (page)
        {

            case 0:
                //�ؽ�Ʈ ����
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                    "�б� ģ������ �̲����� ������ �Ա��� \n���ε��� ȯ�� ������ ��������, \nȣ��� ������ �ǳ�Ű���� \n�������� �ռ� \n�װ��� �̲��� �����ϴ�.", 0.03f));

                //�̹��� ����
                GetComponent<Image>().sprite = bookSprit[page];
                break;

            case 1:
                //�ؽ�Ʈ ����
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                    "����� ���� �ڸ� �Ű� ������ �ʰ� \n��� �ų��� ��� �ǳ�Ű���� \n���� ���� ���� �� ������� ���ҽ��ϴ�.\n" +
                    " ������ �б��� �п�, ���� ������ \n�¹��� ���� ��Ȱ���� ��� \nó������ �����ο��� ����� ��� \n�ʹ� �ູ�߽��ϴ�.", 0.03f));

                //�̹��� ����
                GetComponent<Image>().sprite = bookSprit[page];
                break;
            case 2:
                //�ؽ�Ʈ ����
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                    "�̰��� ��� �ֱ� ���ؼ� \n���� �ʿ��ϴٴ� ���� ������ �ǳ�Ű���� \n���� �����ڸ� �� ����������\n" +
                    " \"������\"��� �̸��� �����忡�� ���ϸ� \nū���� �� �� �ִٴ� ���� ��� �ʰ� \n���ÿ� ���Ҹ� �����ް� �˴ϴ�.", 0.03f));

                //�̹��� ����
                GetComponent<Image>().sprite = bookSprit[page];
                break;
            case 3:
                //�ؽ�Ʈ ����
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                   "��� �ؿ��� �ڶ� �ǳ�Ű���� \n�紫���� ��� ��� �Ƿ����� \n��ž�� ����� \n���������� ������ �մϴ�! \n������ �̾߱Ⱑ \n��� �귯���� �ɱ��?", 0.03f));

                //�̹��� ����
                GetComponent<Image>().sprite = bookSprit[page];
                break;
        }
    }

    /// <summary>
    /// ���������� �̵� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickNextButton(PointerEventData data = null)
    {

        //������ ������
        if (page >= bookSprit.Length - 1)
        {
            //���� ������ ����
            SystemManager.Instance.UserInfo.isShowBook = true;
            // UserInfo Save
            SaveLoad Save = new SaveLoad();
            Save.SaveUserInfo();

            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);
        }
        page++;
        UpdateBook();
    }


    /// <summary>
    /// ������ �̵�: ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickPrevButton(PointerEventData data = null)
    {
        //ù ������
        if (page == 0)
        {
            return;
        }


        page--;
        UpdateBook();
    }

    /// <summary>
    /// ���丮 ��ŵ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickSkipButton(PointerEventData data)
    {
        //�ڷ�ƾ ����
        StopCoroutine("Typing");

        //���� ������ ����
        SystemManager.Instance.UserInfo.isShowBook = true;
        // UserInfo Save
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);
    }

    /// <summary>
    /// Ÿ���� ȿ�� : ������
    /// </summary>
    /// <param name="typingText">Ÿ���� ȿ���� �� �ؽ�Ʈ</param>
    /// <param name="message">�ؽ�Ʈ ����</param>
    /// <param name="speed">Ÿ���� �ӵ�</param>
    IEnumerator Typing(TextMeshProUGUI typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }
}
