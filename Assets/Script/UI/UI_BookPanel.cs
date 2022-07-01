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
        SkipButton  //��ŵ��ư
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
        StopCoroutine("Typing");

        //UI���� ������
        this.transform.SetAsLastSibling();

        switch (page)
        {
            case 0:
                //�ؽ�Ʈ ����
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText), 
                    "�б� ģ������ �̲����� �װ�����, ���ε��� ȯ�� ������ ��������, \nȣ��� ������ �ǳ�Ű���� �������� �ռ� �װ��� �̲��� �����ϴ�.", 0.03f));

                //�̹��� ����
                GetComponent<Image>().sprite = bookSprit[page];
                break;

            case 1:
                //�ؽ�Ʈ ����
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                    "����� ���� �ڸ� �Ű� ������ �ʰ� ��� �ų��� ��� �ǳ�Ű���� ���� ���� ���� �� ������� ���ҽ��ϴ�.\n" +
                    " ������ �б��� �п�, ���� ������ �¹��� ���� ��Ȱ���� ��� \nó������ �����ο��� ����� ��� �ʹ� �ູ�߽��ϴ�.", 0.03f));

                //�̹��� ����
                GetComponent<Image>().sprite = bookSprit[page];
                break;
            case 2:
                //�ؽ�Ʈ ����
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                    "�̰��� ��� �ֱ� ���ؼ� ���� �ʿ��ϴٴ� ���� ������ �ǳ�Ű���� ���� �����ڸ� �� ����������\n" +
                    " \"������\"��� �̸��� �����忡�� ���ϸ� ū���� �� �� �ִٴ� ���� ��� �˴ϴ�.", 0.03f));

                //�̹��� ����
                GetComponent<Image>().sprite = bookSprit[page];
                break;
            case 3:
                //�ؽ�Ʈ ����
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                   "��� �ؿ��� �ڶ� �ǳ�Ű���� �紫���� ��� ��� �Ƿ����� ��ž�� ����� ���������� ������ �մϴ�! \n������ �̾߱Ⱑ ��� �귯���� �ɱ��?", 0.03f));

                //�̹��� ����
                GetComponent<Image>().sprite = bookSprit[page];
                break;
        }

        //Next��ư ��Ȱ��ȭ
        if (GetButton((int)Buttons.NextButton).gameObject.activeSelf)
            GetButton((int)Buttons.NextButton).gameObject.SetActive(false);
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
            //�ڷ�ƾ ����
            StopCoroutine("Typing");

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

        //Next��ư Ȱ��ȭ
        GetButton((int)Buttons.NextButton).gameObject.SetActive(true);
    }
}
