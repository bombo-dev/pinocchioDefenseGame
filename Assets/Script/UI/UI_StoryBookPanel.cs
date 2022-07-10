using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StoryBookPanel : UI_Controller
{
    public int page = 0;

    public int storyNum = 0;

    [SerializeField]
    Sprite[] bookSprit1;

    [SerializeField]
    Sprite[] bookSprit2;

    [SerializeField]
    Sprite[] bookSprit3;

    [SerializeField]
    Sprite[] bookSprit4;

    [SerializeField]
    List<Sprite[]> bookList;

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

        //����Ʈ �ʱ�ȭ
        bookList = new List<Sprite[]>();
        bookList.Add(bookSprit1);
        bookList.Add(bookSprit2);
        bookList.Add(bookSprit3);
        bookList.Add(bookSprit4);

        UpdateBook();
    }

    /// <summary>
    /// ���丮 �����ֱ� : ������
    /// </summary>
    public void UpdateBook()
    {
        if (!GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText))
            return;

        //�ڷ�ƾ ����
        StopAllCoroutines();

        //UI���� ������
        this.transform.SetAsLastSibling();

        //������ �ؽ�Ʈ �ʱ�ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page+1 + "/" + bookList[storyNum - 1].Length;

        //���丮 é�� 1
        if (storyNum == 1)
        {
            switch (page)
            {

                case 0:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�ǳ�Ű���� ���ῡ \n���� ��ž��� �����忡�� ���ϰ�, \n�㿡�� �Ÿ��� ���� ������ �����մϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;

                case 1:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�Ÿ��� ���� ��� �ð���ŭ�� \n������ �ܼҸ����� �ʾҰ�, \n�ϰ� ���� ���� ������ ��� �� �־����ϴ�. ", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 2:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�ǳ�Ű���� ���� ���� ��ſ�����, \n���ҷ� ���ư� ���뿡�� \n�ణ�� �������� ���� �߽��ϴ�. \n���� ���� ��� ���� �׸��� �� �ƴϾ�����\n" +
                        "������ ���� ������ \n��å���̶� ������ �ִ� ſ�� ���? ", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 3:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "�ǳ�Ű���� �������ϰ� \n�Ż翡 �������� ���߰� \n�ڽ��� �Ĵٺ��� ������ �ü����� \n�δ�Ǳ� �����߾��, \n�׷��� �ĵ带 ������� \n�ڽ��� ������� �߽��ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 4:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "�̰��� ���� ���ð� ��� ���̸�,\n�ڶ� �ڰ� ��������� ���� ��ŭ \n��ſ� �ð��� ���´� ��������, \n�ǳ�Ű���� ���� �̰��� ���� ���� \n�׸��α�� �����ϰ�,\n" +
                       "���� �̷� �� �����ο� �ɱ� \n�ٽ� �ѹ� ����ϸ鼭 �Ÿ��� ����Ĩ�ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
            }
        }
        else if (storyNum == 2)
        {
            switch (page)
            {

                case 0:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�ǳ�Ű���� �����忡�� \n��ž��� ġ���ϰ� �����ϰ� \n�������� �����ϸ� ���� ��������ϴ�.", 0.03f));
                   

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;

                case 1:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�����忡�� ���� �ð��� ������ �������� \n������ ���� ġ������������ \n�׸�ŭ �� ���� ���� �� �� �־����ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 2:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "\"�ϰ� ���� ���� �� �� �� ������ \n���� ��������� �ʳ�... ��� �Ҿ���\" \n\n��� �� �ǳ�Ű���� ���ε��� �ұ��ϰ� \n�ۿ� ������ �ʰ�\n" +
                        "������ ���� ���� ������ϴ�", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 3:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "�̰��� ��Ʈ������ Ǯ�� �ʹ� ���� ���̸�,\n�ڶ� �ڰ� ���������� ���� ��ŭ \n��еǴ� �ð��� ���´� ��������, \n�ǳ�Ű���� ���� �̰��� ���� ���� \n�׸��α�� �����ϰ�,\n" +
                       "���� �̰ɷ� �������� \n�ٽ� �ѹ� ����ϸ鼭 �Ÿ��� ����Ĩ�ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
            }
        }
        else if (storyNum == 3)
        {
            switch (page)
            {

                case 0:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "���� ���� ���������� \n�����ϰ� ��� �ִ� �ǳ�Ű����, \n���� �ֺ� ������� ǥ���� \n���� �����ϰ� �ູ�� ������ �ʴ´ٰ� \n�����Ͽ����ϴ�.", 0.03f));


                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;

                case 1:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "���𰡿� �������� �ʰ� \n�����Ӱ� ���� �� ���� �� ���Ҵ� �̰��� \n�� �̻� �׷��� ������ �ʾҽ��ϴ�. \n�����Ӱ� ���ƴٴϴ� ����� \n" +
                        "���� �ڽ��� ����� ���߾�� \n�з����� �������� �����մϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
                case 2:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "\"���� ������ ���� ������ �� �� ����... \n���� ���� ���� �𸣱� ��������\"" +
                        "\n\n�����Ӱ� ���� ���� �ǰ� �;��� �ǳ�Ű��, \n������ �ǳ�Ű���� �ڰ� �ڶ󳵴��� \n���� �θ� ���� �� �ƴϾ��� " +
                        "\n�ƹ��� ��ٷ��� \n������ ������ �ʾҽ��ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
                case 3:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "���� �ǳ�Ű���� �̰��� ������ �մϴ�.\n�̷� �� ������ ������ �ƴ϶� \n���� �ڽ��� �̰��� ��Ƶξ� \n�������� �� ��̰� ���� \n������ ���̿��ٴ� ���� ���޾���.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
                case 4:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "\"�� �̰��� ó�� ���� �� �� ���� �־��µ�...\" \n\n�̰��� ������ ���� �߰����� �����ϴ� ��, \n���� �����ڸ� �� �������� \n�ǳ�Ű���� ���� ���\n" +
                       "���а� �������ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
                case 5:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "�̰����� ���� ���� ���� �ſ� ��վ���,\n�ڶ� �ڰ� ���Ͼ��� ��ŭ \n�ڱ����� �ð��� ���´� ��������, \n�ǳ�Ű���� ���� �̰��� ���� ���� \n�׸��α�� �����߽��ϴ�.\n" +
                       "�ǳ�Ű���� ���� ���ԵǴ� �ɱ��?", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
            }
        }
        else if (storyNum == 4)
        {
            switch (page)
            {

                case 0:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�����ڰ� ������ �װ��� \n�Ա����� �������� ������ �˷ϴ޷��߰�, \n�װ��� ������� �̹� \n���������� ������ �ʾҽ��ϴ�.", 0.03f));


                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;

                case 1:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�װ����� ������������ \n������ �������� ���� \n�����ڴ� ���׶��� ���� �˰��̸� \n�ǳ�Ű���� �Լ����� ����־���\n" +
                        "�� ���� �з����� �ູ���� �ߵ����� \n�ǳ�Ű���� ���ϰ� ��Ȥ�߽��ϴ�", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 2:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "�ǳ�Ű���� �����忡�� \nġ���ϰ� �����߽��ϴ�.\n���𰡿� �� ������ �����ߴ� �� ó���̾��� \n���� �����ϰ� �� �� ������ ��������\n" +
                        "�����ٸ� ��簨�� ���������ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 3:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "�ܼ��� ���� �����ߴ� �ǳ�Ű���� \n���� ���𰡿� �����ϰ� \n����ϴ� �Ϳ� �����߰�, \n�ٽ� ������ ���ư��ٸ� \n�����̵� �̷ﳾ �� ���� �͸� ���ҽ��ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 4:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "�ǳ�Ű���� �� �̻� \n������ ��Ȥ�� ��鸱 ������ \n������ �ʽ��ϴ�. \n������ ��Ȥ�� �Ѹ�ġ�� \n�̰��� �������� �ǳ�Ű���� \n�����ڰ� ���θ����ϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 5:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "�ǳ�Ű���� �̰����� ������ ����, \n�����ڿ��� ������ ������ ����մϴ�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 6:
                    //�ؽ�Ʈ ����
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "�̰��� �����߽��ϴ�.\n�ڶ� �ڰ� ���Ķ��� �� ��ŭ \n�ǳ�Ű���� �ߵ���������, \n�ǳ�Ű���� �� �̻� \n������ ������� �߱����� �ʽ��ϴ�.\n" +
                       "���� �����ڸ� ����ġ�� �̰����� �����ô�.", 0.03f));

                    //�̹��� ����
                    GetComponent<Image>().sprite = bookSprit4[page];
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
        if (page >= bookList[storyNum-1].Length - 1)
        {
            // UserInfo Save
            SaveLoad Save = new SaveLoad();
            Save.SaveUserInfo();

            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/StoryBookPanel", gameObject);
        }
        page++;

        //������ �ؽ�Ʈ �ʱ�ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookList[storyNum - 1].Length;

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

        //������ �ؽ�Ʈ �ʱ�ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookList[storyNum - 1].Length;
    }

    /// <summary>
    /// ���丮 ��ŵ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickSkipButton(PointerEventData data)
    {
        //�ڷ�ƾ ����
        StopCoroutine("Typing");

        // UserInfo Save
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/StoryBookPanel", gameObject);
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
