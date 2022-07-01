using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_BossPanel : UI_Controller
{
    [SerializeField]
    string[] bossText;

    [SerializeField]
    Sprite[] bossImage;

    enum TextMeshProUGUIs
    {
        bossText
    }

    enum Images
    {
        bossImage
    }



    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    /// 
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));

        //이미지 초기화
        if (SystemManager.Instance.UserInfo.selectedStageNum == 11)
        {
            GetImage((int)Images.bossImage).sprite = bossImage[0];
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 20)
        {
            GetImage((int)Images.bossImage).sprite = bossImage[1];
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 28)
        {
            GetImage((int)Images.bossImage).sprite = bossImage[2];
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 36)
        {
            GetImage((int)Images.bossImage).sprite = bossImage[3];
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 38)
        {
            GetImage((int)Images.bossImage).sprite = bossImage[4];
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 40)
        {
            GetImage((int)Images.bossImage).sprite = bossImage[5];
        }

        StartBossStage();
    }

    /// <summary>
    /// 보스 스테이지 연출 시작 : 김현진
    /// </summary>
    public void StartBossStage()
    {   //타이핑
        if (SystemManager.Instance.UserInfo.selectedStageNum == 11)
        {
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.bossText), bossText[0], 0.1f));
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 20)
        {
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.bossText), bossText[1], 0.1f));
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 28)
        {
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.bossText), bossText[2], 0.1f));
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 36)
        {
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.bossText), bossText[3], 0.1f));
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 38)
        {
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.bossText), bossText[4], 0.1f));
        }
        else if (SystemManager.Instance.UserInfo.selectedStageNum == 40)
        {
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.bossText), bossText[5], 0.1f));
        }
       
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

        yield return new WaitForSeconds(2.0f);
        //패널 비활성화
        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(
            "Panel/BossPanel", gameObject);

    }
}
