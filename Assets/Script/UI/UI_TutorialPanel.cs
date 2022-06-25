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
        TurretSummonText, //�ͷ� ��ȯ ���̵�
        TurretUpgradeText, //�ͷ� ��ȭ ���̵�
        NestTouchText, //�ͷ� ��ġ ���̵�
        WelcomeText// ȯ�� ��Ʈ
    }

    enum Images
    {
        TutorialPanel   //Ʃ�͸��� �г� ��ü �̹���
    }

    enum Buttons
    {
        WelcomeNestButton   //ȯ�� ��Ʈ �ѱ�� ��ư
    }
    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        //��ư �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.WelcomeNestButton).gameObject, OnClickNext, Define.UIEvent.Click);

        ///��Ȱ��ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretSummonText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretUpgradeText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.SetActive(false);

        //�ؽ�Ʈ �ʱ�ȭ
        StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText), "������  \"������\" �� ���Ű��� ȯ���մϴ�", 0.8f));

    }

    /// <summary>
    /// ȯ����Ʈ �ѱ�� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    public void OnClickNext(PointerEventData data)
    {
        //�ؽ�Ʈ ����
        if (step == Step.welcom1)
        {
            //�ؽ�Ʈ �ʱ�ȭ
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText), 
                "������ ��Ű�� ���� �������� ���� \n��� �����ϸ� �¸��Ͻð� �˴ϴ�!", 0.8f));

            step = Step.welcom2;
        }
    }

    /// <summary>
    /// Ÿ���� ȿ��
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
