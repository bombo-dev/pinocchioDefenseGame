using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BookPanel : UI_Controller
{
    public int page;

    [SerializeField]
    Sprite[] bookSprit;

    enum TextMeshProUGUIs
    {
        BookText,   //책 스토리 텍스트
    }

    enum Buttons
    {
        NextButton, //다음장 넘어가기 버튼
        SkipButton  //스킵버튼
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

        //페이지 초기화 
        page = 0;

        //스토리 시작
        UpdateBook();
    }

    /// <summary>
    /// 스토리 보여주기 : 김현진
    /// </summary>
    public void UpdateBook()
    {
        //UI가장 앞으로
        this.transform.SetAsLastSibling();

        switch (page)
        {
            case 0:
                //텍스트 갱신
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText), 
                    "학교 친구에게 이끌려간 그곳에는, 밤인데도 환한 광경이 펼쳐졌고, \n호기심 가득한 피노키오는 생각보다 앞서 그곳에 이끌려 들어갔습니다.", 0.03f));

                //이미지 갱신
                GetComponent<Image>().sprite = bookSprit[page];
                break;

            case 1:
                //텍스트 갱신
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                    "길어져 가는 코를 신경 쓰지도 않고 밤새 신나게 놀던 피노키오는 가진 돈을 전부 다 써버리고 말았습니다.\n" +
                    " 하지만 학교와 학원, 집을 오가는 쳇바퀴 같은 생활에서 벗어나 \n처음으로 자유로워진 기분이 들어 너무 행복했습니다.", 0.03f));

                //이미지 갱신
                GetComponent<Image>().sprite = bookSprit[page];
                break;
            case 2:
                //텍스트 갱신
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                    "이곳에 계속 있기 위해선 돈이 필요하다는 것을 깨달은 피노키오는 검은 고깔모자를 쓴 누군가에게\n" +
                    " \"케이지\"라는 이름의 투기장에서 일하면 큰돈을 벌 수 있다는 말을 듣게 됩니다.", 0.03f));

                //이미지 갱신
                GetComponent<Image>().sprite = bookSprit[page];
                break;
            case 3:
                //텍스트 갱신
                StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.BookText),
                   "목수 밑에서 자란 피노키오는 곁눈질로 배운 목공 실력으로 포탑을 만들어 투기장으로 가려고 합니다! \n앞으로 이야기가 어떻게 흘러가게 될까요?", 0.03f));

                //이미지 갱신
                GetComponent<Image>().sprite = bookSprit[page];
                break;
        }

        //Next버튼 비활성화
        if (GetButton((int)Buttons.NextButton).gameObject.activeSelf)
            GetButton((int)Buttons.NextButton).gameObject.SetActive(false);
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
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);

            //유저 정보에 저장
            SystemManager.Instance.UserInfo.isShowBook = true;
            // UserInfo Save
            SaveLoad Save = new SaveLoad();
            Save.SaveUserInfo();
        }
        page++;
        UpdateBook();
    }

    /// <summary>
    /// 스토리 스킵 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void OnClickSkipButton(PointerEventData data)
    {
        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache("Panel/BookPanel", gameObject);

        //유저 정보에 저장
        SystemManager.Instance.UserInfo.isShowBook = true;
        // UserInfo Save
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();
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

        //Next버튼 활성화
        GetButton((int)Buttons.NextButton).gameObject.SetActive(true);
    }
}
