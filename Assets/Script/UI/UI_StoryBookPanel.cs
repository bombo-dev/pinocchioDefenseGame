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
        BookText,   //책 스토리 텍스트
        PageText    //책 페이지 텍스트
    }

    enum Buttons
    {
        NextButton, //다음장 넘어가기 버튼
        SkipButton,  //스킵버튼
        PrevButton   //이전 버튼
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));

        //버튼 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.NextButton).gameObject, OnClickNextButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.SkipButton).gameObject, OnClickSkipButton, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.PrevButton).gameObject, OnClickPrevButton, Define.UIEvent.Click);

        //리스트 초기화
        bookList = new List<Sprite[]>();
        bookList.Add(bookSprit1);
        bookList.Add(bookSprit2);
        bookList.Add(bookSprit3);
        bookList.Add(bookSprit4);

        UpdateBook();
    }

    /// <summary>
    /// 스토리 보여주기 : 김현진
    /// </summary>
    public void UpdateBook()
    {
        if (!GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText))
            return;

        //코루틴 중지
        StopAllCoroutines();

        //UI가장 앞으로
        this.transform.SetAsLastSibling();

        //페이지 텍스트 초기화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page+1 + "/" + bookList[storyNum - 1].Length;

        //스토리 챕터 1
        if (storyNum == 1)
        {
            switch (page)
            {

                case 0:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "피노키오는 저녁에 \n나무 포탑들과 투기장에서 일하고, \n밤에는 거리로 나와 자유를 만끽합니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;

                case 1:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "거리에 나와 노는 시간만큼은 \n누구도 잔소리하지 않았고, \n하고 싶은 것을 마음껏 즐길 수 있었습니다. ", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 2:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "피노키오는 정말 정말 즐거웠지만, \n숙소로 돌아갈 때쯤에는 \n약간의 찝찝함이 남곤 했습니다. \n딱히 원래 살던 집이 그리운 건 아니었지만\n" +
                        "마음속 깊은 곳에서 \n죄책감이라도 느끼고 있는 탓일 까요? ", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 3:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "피노키오는 언제부턴가 \n매사에 떳떳하지 못했고 \n자신을 쳐다보는 남들의 시선마저 \n부담되기 시작했어요, \n그래서 후드를 뒤집어써 \n자신을 가리기로 했습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 4:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "이곳은 자주 마시고 놀던 곳이며,\n자라난 코가 노란색으로 변할 만큼 \n즐거운 시간을 보냈던 곳이지만, \n피노키오는 이제 이곳에 오는 것을 \n그만두기로 다짐하고,\n" +
                       "정말 이런 게 자유로운 걸까 \n다시 한번 고민하면서 거리를 지나칩니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
            }
        }
        else if (storyNum == 2)
        {
            switch (page)
            {

                case 0:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "피노키오는 투기장에서 \n포탑들과 치열하게 전투하고 \n관객들은 열광하며 돈을 집어던집니다.", 0.03f));
                   

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;

                case 1:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "투기장에서 일한 시간이 지나면 지날수록 \n전투는 점점 치열해져갔지만 \n그만큼 더 많은 돈을 벌 수 있었습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 2:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "\"하고 싶은 것은 다 할 수 있지만 \n마냥 즐겁지만은 않네... 어딘가 불안해\" \n\n어느 날 피노키오는 밤인데도 불구하고 \n밖에 나가지 않고\n" +
                        "생각에 빠져 밤을 지새웁니다", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 3:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "이곳은 스트레스를 풀기 너무 좋은 곳이며,\n자라난 코가 붉은색으로 변할 만큼 \n흥분되는 시간을 보냈던 곳이지만, \n피노키오는 이제 이곳에 오는 것을 \n그만두기로 다짐하고,\n" +
                       "정말 이걸로 괜찮은가 \n다시 한번 고민하면서 거리를 지나칩니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
            }
        }
        else if (storyNum == 3)
        {
            switch (page)
            {

                case 0:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "여느 때와 마찬가지로 \n방탕하게 놀고 있던 피노키오는, \n문뜩 주변 사람들의 표정이 \n무언가 공허하고 행복해 보이지 않는다고 \n생각하였습니다.", 0.03f));


                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;

                case 1:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "무언가에 얽매이지 않고 \n자유롭게 지낼 수 있을 것 같았던 이곳은 \n더 이상 그렇게 보이지 않았습니다. \n자유롭게 날아다니는 새들과 \n" +
                        "지금 자신의 모습을 비추어보며 \n밀려오는 괴리감에 좌절합니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
                case 2:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "\"내가 나무로 만든 새들은 날 수 없어... \n내가 나는 법을 모르기 때문이지\"" +
                        "\n\n자유롭게 나는 새가 되고 싶었던 피노키오, \n하지만 피노키오의 코가 자라났던건 \n새의 부리 같은 게 아니었고 " +
                        "\n아무리 기다려도 \n날개는 생기지 않았습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
                case 3:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "이제 피노키오는 이곳을 떠나려 합니다.\n이런 건 진정한 자유가 아니라 \n그저 자신을 이곳에 잡아두어 \n투기장을 더 즐겁게 만들 \n목적일 뿐이였다는 것을 깨달았죠.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
                case 4:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "\"아 이곳에 처음 왔을 때 본 적이 있었는데...\" \n\n이곳을 나가기 위한 발걸음을 재촉하던 중, \n검은 고깔모자를 쓴 누군가가 \n피노키오의 팔을 잡고\n" +
                       "어디론가 데려갑니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
                case 5:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "이곳에서 돈을 쓰는 것은 매우 재밌었고,\n자라난 코가 새하얘질 만큼 \n자극적인 시간을 보냈던 곳이지만, \n피노키오는 이제 이곳에 오는 것을 \n그만두기로 다짐했습니다.\n" +
                       "피노키오는 어디로 가게되는 걸까요?", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit3[page];
                    break;
            }
        }
        else if (storyNum == 4)
        {
            switch (page)
            {

                case 0:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "고깔모자가 데려간 그곳은 \n입구부터 어지러울 정도로 알록달록했고, \n그곳의 사람들은 이미 \n제정신으로 보이지 않았습니다.", 0.03f));


                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;

                case 1:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "그곳에서 빠져나오려고 \n마음을 먹으려는 순간 \n고깔모자는 동그랗고 작은 알갱이를 \n피노키오의 입속으로 집어넣었고\n" +
                        "그 순간 밀려오는 행복감과 중독성이 \n피노키오를 강하게 유혹했습니다", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 2:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "피노키오는 투기장에서 \n치열하게 전투했습니다.\n무언가에 이 정도로 열중했던 건 처음이었고 \n그저 방탕하게 놀 때 느꼈던 감정과는\n" +
                        "전혀다른 고양감이 느껴졌습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 3:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "단순히 새를 동경했던 피노키오는 \n이제 무언가에 열중하고 \n노력하는 것에 동경했고, \n다시 집으로 돌아간다면 \n무엇이든 이뤄낼 수 있을 것만 같았습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 4:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "피노키오는 더 이상 \n이정도 유혹에 흔들릴 정도로 \n약하지 않습니다. \n하지만 유혹을 뿌리치고 \n이곳을 나서려는 피노키오를 \n고깔모자가 가로막습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 5:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "피노키오는 이곳에서 나가기 위해, \n고깔모자와의 마지막 전투를 기약합니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
                case 6:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "이곳은 위험했습니다.\n자라난 코가 새파랗게 될 만큼 \n피노키오를 중독시켰지만, \n피노키오는 더 이상 \n순간의 쾌락만을 추구하지 않습니다.\n" +
                       "이제 고깔모자를 물리치고 이곳에서 나갑시다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit4[page];
                    break;
            }
        }

    }

    /// <summary>
    /// 다음장으로 이동 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickNextButton(PointerEventData data = null)
    {

        //마지막 페이지
        if (page >= bookList[storyNum-1].Length - 1)
        {
            // UserInfo Save
            SaveLoad Save = new SaveLoad();
            Save.SaveUserInfo();

            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/StoryBookPanel", gameObject);
        }
        page++;

        //페이지 텍스트 초기화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookList[storyNum - 1].Length;

        UpdateBook();
    }


    /// <summary>
    /// 이전장 이동: 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickPrevButton(PointerEventData data = null)
    {
        //첫 페이지
        if (page == 0)
        {
            return;
        }


        page--;
        UpdateBook();

        //페이지 텍스트 초기화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookList[storyNum - 1].Length;
    }

    /// <summary>
    /// 스토리 스킵 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickSkipButton(PointerEventData data)
    {
        //코루틴 중지
        StopCoroutine("Typing");

        // UserInfo Save
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/StoryBookPanel", gameObject);
    }

    /// <summary>
    /// 타이핑 효과 : 김현진
    /// </summary>
    /// <param name="typingText">타이핑 효과를 줄 텍스트</param>
    /// <param name="message">텍스트 문장</param>
    /// <param name="speed">타이핑 속도</param>
    IEnumerator Typing(TextMeshProUGUI typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }
}
