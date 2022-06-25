using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TutorialPanel : UI_Controller
{
    enum Step
    {
        welcom1,  //0
        welcom2,  //1
    }
    Step step = Step.welcom1;

    enum TextMeshProUGUIs
    {
        TurretSummonText, //터렛 소환 가이드
        TurretUpgradeText, //터렛 강화 가이드
        NestTouchText, //터렛 터치 가이드
        WelcomeText// 환영 멘트
    }

    enum Images
    {
        TutorialPanel   //튜터리얼 패널 전체 이미지
    }

    enum Buttons
    {
        WelcomeNestButton   //환영 멘트 넘기기 버튼
    }
    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        //버튼 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.WelcomeNestButton).gameObject, OnClickNext, Define.UIEvent.Click);

        ///비활성화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretSummonText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretUpgradeText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.SetActive(false);

        //텍스트 초기화
        StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText), "투기장  \"케이지\" 에 오신것을 환영합니다", 0.8f));

    }

    /// <summary>
    /// 환영멘트 넘기기 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    public void OnClickNext(PointerEventData data)
    {
        //텍스트 변경
        if (step == Step.welcom1)
        {
            //텍스트 초기화
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText), 
                "거점을 지키기 위해 몰려오는 적을 \n모두 섬멸하면 승리하시게 됩니다!", 0.8f));

            step = Step.welcom2;
        }
    }

    /// <summary>
    /// 타이핑 효과
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
