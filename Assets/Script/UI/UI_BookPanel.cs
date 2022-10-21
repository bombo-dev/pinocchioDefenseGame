using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BookPanel : UI_Controller
{
    public int page = 0;

    //��ư�� Ŭ���ؼ� å�� ������ ��
    public bool isClickBookButton = false;

    // 0 -> Start, 1 -> End
    public int booktype = 0;

    //UI ���ε� �Ϸ� ����
    bool isBinding = false;

    [SerializeField]
    Sprite[] bookSprit1;
    [SerializeField]
    Sprite[] bookSprit2;

    enum TextMeshProUGUIs
    {
        BookText,   //å ���丮 �ؽ�Ʈ
        PageText    //å ������ �ؽ�Ʈ
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

        isBinding = true;

        if (!(SystemManager.Instance.UserInfo.isShowBook))
        {
            //�ʱ�ȭ
            SystemManager.Instance.PanelManager.bookPanel.page = 0;
            SystemManager.Instance.PanelManager.bookPanel.UpdateBook();
        }
        else
        {
            if (!isClickBookButton)
                SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);
            else
            {
                SystemManager.Instance.PanelManager.bookPanel.UpdateBook();
                isClickBookButton = false;
            }
        }


    }

    /// <summary>
    /// ���丮 �����ֱ� : ������
    /// </summary>
    public void UpdateBook()
    {
        if (!isBinding)
        {
            return;
        }

        //�ڷ�ƾ ����
        StopAllCoroutines();

        //UI���� ������
        this.transform.SetAsLastSibling();

        //End Book
        if (booktype == 1)
        {
            //������ �ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookSprit2.Length;

            switch (page)
            {

                case 0:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�ǳ�Ű���� �������� �������� �¸��Ͽ����ϴ�. \n�����ڴ� ��������, \n�ǳ�Ű���� �������� è�Ǿ��� �Ǿ����� \n������ �������� �����ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;

                case 1:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�ǳ�Ű���� �ڶ󳪴� �ڴ� \n�ǳ�Ű���� ������ �ɸ����¿� ���� \n�پ��� ������ ������, \n������ ���� ���ϰ� ���� ���̴� \n��ȫ���� ��¦�̰� �ֽ��ϴ�", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 2:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�ǳ�Ű���� �̰����� \n��� ������ ���� ������ ���� ���� �־�����, \n�������� ������ �̰��� ������� ����߰�\n" +
                        "���� ���� �ӿ��� ���� �����Ͽ����ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 3:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "������ �ǳ�Ű���� �̾߱Ⱑ \n��� �귯���� �ɱ��? \n�׸��� �ǳ�Ű���� �ڴ� \n� ������ ������ �ɱ��?\n" +
                       "����� � ������ ������ �ͳ���?\n-end-", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 4:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "- �÷��� ���ּż� �����մϴ�!!!! -\n����: ������, ������, ������ \n3D��Ʈ: ������ \nUI����: õ�ο�, ������", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
            }
        }
        //Start Book
        else
        {
            //������ �ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookSprit1.Length;

            switch (page)
            {

                case 0:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�б� ģ������ �̲����� ������ �Ա��� \n���ε��� ȯ�� ������ ��������, \nȣ��� ������ �ǳ�Ű���� \n�������� �ռ� \n�װ��� �̲��� �����ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;

                case 1:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "����� ���� �ڸ� �Ű� ������ �ʰ� \n��� �ų��� ��� �ǳ�Ű���� \n���� ���� ���� �� ������� ���ҽ��ϴ�.\n" +
                        " ������ �б��� �п�, ���� ������ \n�¹��� ���� ��Ȱ���� ��� \nó������ �����ο��� ����� ��� \n�ʹ� �ູ�߽��ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 2:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�̰��� ��� �ֱ� ���ؼ� \n���� �ʿ��ϴٴ� ���� ������ �ǳ�Ű���� \n���� �����ڸ� �� ����������\n" +
                        " \"������\"��� �̸��� �����忡�� ���ϸ� \nū���� �� �� �ִٴ� ���� ��� �ʰ� \n���ÿ� ���Ҹ� �����ް� �˴ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 3:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "��� �ؿ��� �ڶ� �ǳ�Ű���� \n�紫���� ��� ��� �Ƿ����� \n��ž�� ����� \n���������� ������ �մϴ�! \n������ �̾߱Ⱑ \n��� �귯���� �ɱ��?", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
            }
        }
       
    }

    /// <summary>
    /// ���������� �̵� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickNextButton(PointerEventData data = null)
    {

        //������ ������

        //Start
        if (booktype == 0)
        {
            //������ �ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 2 + "/" + bookSprit1.Length;

            if (page >= bookSprit1.Length - 1)
            {
                //���� ������ ����
                SystemManager.Instance.UserInfo.isShowBook = true;
                // UserInfo Save
                SaveLoad Save = new SaveLoad();
                Save.SaveUserInfo();

                SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);
            }
        }
        //End
        if (booktype == 1)
        {
            //������ �ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 2 + "/" + bookSprit2.Length;

            if (page >= bookSprit2.Length - 1)
            {
                //���� ������ ����
                SystemManager.Instance.UserInfo.isShowBook = true;
                // UserInfo Save
                SaveLoad Save = new SaveLoad();
                Save.SaveUserInfo();

                SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);
            }
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

        if (booktype == 0)
        {
            //������ �ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookSprit1.Length;
        }
        else if(booktype == 1)
        {
            //������ �ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookSprit2.Length;
        }

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

    /// <summary>
    /// ��� ��� : ������
    /// </summary>
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
