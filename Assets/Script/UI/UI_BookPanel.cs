using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BookPanel : UI_Controller
{
    public int page = 0;

    //버튼을 클릭해서 책을 열었을 때
    public bool isClickBookButton = false;

    // 0 -> Start, 1 -> End
    public int booktype = 0;

    //UI 바인딩 완료 여부
    bool isBinding = false;

    [SerializeField]
    Sprite[] bookSprit1;
    [SerializeField]
    Sprite[] bookSprit2;

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

        isBinding = true;

        if (!(SystemManager.Instance.UserInfo.isShowBook))
        {
            //초기화
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
    /// 스토리 보여주기 : 김현진
    /// </summary>
    public void UpdateBook()
    {
        if (!isBinding)
        {
            return;
        }

        //코루틴 중지
        StopAllCoroutines();

        //UI가장 앞으로
        this.transform.SetAsLastSibling();

        //End Book
        if (booktype == 1)
        {
            //페이지 텍스트 초기화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookSprit2.Length;

            switch (page)
            {

                case 0:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "피노키오는 고깔모자의 전투에서 승리하였습니다. \n고깔모자는 쓰려졌고, \n피노키오는 투기장의 챔피언이 되었으며 \n유유히 투기장을 나갑니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;

                case 1:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "피노키오의 자라나는 코는 \n피노키오가 느끼는 심리상태에 따라 \n다양한 빛깔을 내었고, \n지금은 아주 강하고 굳세 보이는 \n분홍빛이 반짝이고 있습니다", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 2:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "피노키오는 이곳에서 \n평생 방탕한 삶을 보내며 지낼 수도 있었지만, \n스스로의 의지로 이곳을 떠나기로 결심했고\n" +
                        "많은 생각 속에서 한층 성장하였습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 3:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "앞으로 피노키오의 이야기가 \n어떻게 흘러가게 될까요? \n그리고 피노키오의 코는 \n어떤 색으로 빛나게 될까요?\n" +
                       "당신은 어떤 색으로 빛나고 싶나요?\n-end-", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
                case 4:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "- 플레이 해주셔서 감사합니다!!!! -\n개발: 김현진, 하은비, 문종운 \n3D아트: 김현진 \nUI도움: 천민욱, 김정훈", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit2[page];
                    break;
            }
        }
        //Start Book
        else
        {
            //페이지 텍스트 초기화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookSprit1.Length;

            switch (page)
            {

                case 0:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "학교 친구에게 이끌려간 마을의 입구는 \n밤인데도 환한 광경이 펼쳐졌고, \n호기심 가득한 피노키오는 \n생각보다 앞서 \n그곳에 이끌려 들어갔습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;

                case 1:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "길어져 가는 코를 신경 쓰지도 않고 \n밤새 신나게 놀던 피노키오는 \n가진 돈을 전부 다 써버리고 말았습니다.\n" +
                        " 하지만 학교와 학원, 집을 오가는 \n쳇바퀴 같은 생활에서 벗어나 \n처음으로 자유로워진 기분이 들어 \n너무 행복했습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 2:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "이곳에 계속 있기 위해선 \n돈이 필요하다는 것을 깨달은 피노키오는 \n검은 고깔모자를 쓴 누군가에게\n" +
                        " \"케이지\"라는 이름의 투기장에서 일하면 \n큰돈을 벌 수 있다는 말을 듣게 됨과 \n동시에 숙소를 제공받게 됩니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
                    break;
                case 3:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "목수 밑에서 자란 피노키오는 \n곁눈질로 배운 목공 실력으로 \n포탑을 만들어 \n투기장으로 가려고 합니다! \n앞으로 이야기가 \n어떻게 흘러가게 될까요?", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit1[page];
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

        //Start
        if (booktype == 0)
        {
            //페이지 텍스트 초기화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 2 + "/" + bookSprit1.Length;

            if (page >= bookSprit1.Length - 1)
            {
                //유저 정보에 저장
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
            //페이지 텍스트 초기화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 2 + "/" + bookSprit2.Length;

            if (page >= bookSprit2.Length - 1)
            {
                //유저 정보에 저장
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

        if (booktype == 0)
        {
            //페이지 텍스트 초기화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookSprit1.Length;
        }
        else if(booktype == 1)
        {
            //페이지 텍스트 초기화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PageText).text = page + 1 + "/" + bookSprit2.Length;
        }

        UpdateBook();
    }

    /// <summary>
    /// 스토리 스킵 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickSkipButton(PointerEventData data)
    {
        //코루틴 중지
        StopCoroutine("Typing");

        //유저 정보에 저장
        SystemManager.Instance.UserInfo.isShowBook = true;
        // UserInfo Save
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);
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

    /// <summary>
    /// 잠시 대기 : 김현진
    /// </summary>
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
