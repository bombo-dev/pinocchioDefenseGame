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
    Sprite[] bookSprit;

    enum TextMeshProUGUIs
    {
        BookText,   //책 스토리 텍스트
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

        //스토리 챕터 1
        if (storyNum == 1)
        {
            switch (page)
            {

                case 0:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "학교 친구에게 이끌려간 마을의 입구는 \n밤인데도 환한 광경이 펼쳐졌고, \n호기심 가득한 피노키오는 \n생각보다 앞서 그곳에 이끌려 들어갔습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit[page];
                    break;

                case 1:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "길어져 가는 코를 신경 쓰지도 않고 \n밤새 신나게 놀던 피노키오는 \n가진 돈을 전부 다 써버리고 말았습니다.\n" +
                        " 하지만 학교와 학원, 집을 오가는 쳇바퀴 같은 생활에서 벗어나 \n처음으로 자유로워진 기분이 들어 너무 행복했습니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit[page];
                    break;
                case 2:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                        "이곳에 계속 있기 위해선 \n돈이 필요하다는 것을 깨달은 피노키오는 \n검은 고깔모자를 쓴 누군가에게\n" +
                        " \"케이지\"라는 이름의 투기장에서 일하면 \n큰돈을 벌 수 있다는 말을 듣게 됨과 동시에 \n숙소를 제공받게 됩니다.", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit[page];
                    break;
                case 3:
                    //텍스트 갱신
                    StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                       "목수 밑에서 자란 피노키오는 \n곁눈질로 배운 목공 실력으로 \n포탑을 만들어 투기장으로 가려고 합니다! \n앞으로 이야기가 어떻게 흘러가게 될까요?", 0.03f));

                    //이미지 갱신
                    GetComponent<Image>().sprite = bookSprit[page];
                    break;
            }
        }
        else if (storyNum == 2)
        {
            
        }
       
    }

    /// <summary>
    /// 다음장으로 이동 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickNextButton(PointerEventData data = null)
    {

        //마지막 페이지
        if (page >= bookSprit.Length - 1)
        {
            // UserInfo Save
            SaveLoad Save = new SaveLoad();
            Save.SaveUserInfo();

            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);
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
