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
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    /// 
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));

        //�̹��� �ʱ�ȭ
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
    /// ���� �������� ���� ���� : ������
    /// </summary>
    public void StartBossStage()
    {   //Ÿ����
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

        yield return new WaitForSeconds(2.0f);
        //�г� ��Ȱ��ȭ
        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(
            "Panel/BossPanel", gameObject);

    }
}
