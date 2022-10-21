using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TutorialPanel : UI_Controller
{
    Vector3 nestTourchPos = new Vector3(1.6f, 0, 0.18f);

    TurretManager turretManager;

    enum Step
    {
        welcom1,  //0
        welcom2,  //1
        welcom3,  //2
        touchNest, //3
        summonTurret,//4
        upgradeTurret,//5
        upgradeInfo,//6
        startDefense//7
    }
    Step step = Step.welcom1;

    enum TextMeshProUGUIs
    {
        TurretSummonText, //�ͷ� ��ȯ ���̵�
        TurretUpgradeText, //�ͷ� ��ȭ ���̵�
        NestTouchText, //�ͷ� ��ġ ���̵�
        WelcomeText,// ȯ�� ��Ʈ
        ImpossibleTurretRemoveText//�ͷ� ö�� �Ұ� ���
    }

    enum Images
    {
        TutorialPanel   //Ʃ�丮�� �г� ��ü �̹���
    }

    enum Buttons
    {
        WelcomeTextButton,   //ȯ�� ��Ʈ �ѱ�� ��ư
        WelcomeTextButton2
    }

    enum GameObjects
    {
        ColorWoodInfoPanel  //���� ��ȭ ���� �г�
    }


    private void Update()
    {
        if (step == Step.touchNest)
        {
            UpdatePosNestTouchText();
            ChkClickNest();
        }
        if (step == Step.summonTurret)
        {
            ChkSummonTurret();
        }
        if (step == Step.upgradeTurret)
        {
            ChkUpgradeTurret();
        }
    }

    /// <summary>
    /// ������ �������� ��� ���� Ʃ�丮������ �̵� : ������
    /// </summary>
    void ChkClickNest()
    {
        if (SystemManager.Instance.InputManager.currenstSelectNest)
        {
            //Ʃ�丮�� �ؽ�Ʈ ��ü
            GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.SetActive(false);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretSummonText).gameObject.SetActive(true);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.ImpossibleTurretRemoveText).gameObject.SetActive(true);

            //���� �ڿ��� ����
            SystemManager.Instance.ResourceManager.woodResource += 50;

            //UI���� ����
            if (SystemManager.Instance.PanelManager.resoursePanel)
            {
                UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
                resourcePanel.UpdateWoodResource();
            }

            //���� UI �� ������
            GetComponent<Image>().enabled = true;
            SystemManager.Instance.PanelManager.turretMngPanel.gameObject.transform.SetAsLastSibling();

            //���� ����
            step = Step.summonTurret;
        }
    }

    /// <summary>
    /// �ͷ��� ��ȯ ���� ��� ���� Ʃ�丮������ �̵� : ������
    /// </summary>
    void ChkSummonTurret()
    {
        if (SystemManager.Instance.TurretManager.turrets.Count > 1)
        {
            //Ʃ�丮�� �ؽ�Ʈ ��ü
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretSummonText).gameObject.SetActive(false);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretUpgradeText).gameObject.SetActive(true);

            //���� ����
            step = Step.upgradeTurret;
        }
    }

    /// <summary>
    /// �ͷ��� ���׷��̵� ���� ��� ���� Ʃ�丮������ �̵� : ������
    /// </summary>
    void ChkUpgradeTurret()
    {
        if (SystemManager.Instance.TurretManager.turrets[1].GetComponent<Turret>().buffs.Count > 0)
        {
            //Ʃ�丮�� �ؽ�Ʈ ��ü
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretUpgradeText).gameObject.SetActive(false);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.ImpossibleTurretRemoveText).gameObject.SetActive(false);

            GetGameobject((int)GameObjects.ColorWoodInfoPanel).SetActive(true);

            GetButton((int)Buttons.WelcomeTextButton2).gameObject.SetActive(true);

            //���º���
            step = Step.upgradeInfo;
        }
    }

    /// <summary>
    /// �ͷ� Ŭ�� �ؽ�Ʈ ��ġ ������Ʈ : ������
    /// </summary>
    void UpdatePosNestTouchText()
    {
        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.activeSelf)
        {
            Vector3 vec = new Vector3(turretManager.tutorialNest.position.x, turretManager.tutorialNest.position.y + 30f, turretManager.tutorialNest.position.z);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.transform.position =
                Camera.main.WorldToScreenPoint(vec);
        }
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        turretManager = SystemManager.Instance.TurretManager;

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        //��ư �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.WelcomeTextButton).gameObject, OnClickNext, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.WelcomeTextButton2).gameObject, OnClickNext, Define.UIEvent.Click);

        ///��Ȱ��ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretSummonText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretUpgradeText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ImpossibleTurretRemoveText).gameObject.SetActive(false);

        GetButton((int)Buttons.WelcomeTextButton).gameObject.SetActive(false);
        GetButton((int)Buttons.WelcomeTextButton2).gameObject.SetActive(false);

        GetGameobject((int)GameObjects.ColorWoodInfoPanel).SetActive(false);

        //�ؽ�Ʈ �ʱ�ȭ
        StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText), "������  \"������\" �� ���Ű��� ȯ���մϴ�", 0.05f));

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
            StopCoroutine("Typing");
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText),
                "������ ��Ű�� ���� �������� ���� \n��� �����ϸ� �¸��Ͻð� �˴ϴ�!", 0.05f));

            step = Step.welcom2;
        }
        else if (step == Step.welcom2)
        {
            //�ؽ�Ʈ �ʱ�ȭ
            StopCoroutine("Typing");
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText),
                "�ͷ��� �Ǽ��Ͽ� �¸��� �����ϼ���!", 0.05f));

            step = Step.welcom3;
        }
        else if (step == Step.welcom3)
        {
            //�ؽ�Ʈ Ȱ��ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.SetActive(true);

            //�̹���,�ؽ�Ʈ ��Ȱ��ȭ
            GetComponent<Image>().enabled = false;
            GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText).gameObject.SetActive(false);

            //�ͷ� ���� �г� Ȱ��ȭ
            PanelManager pm = SystemManager.Instance.PanelManager;

            //�ͷ� ��ȯ �г�
            pm.EnablePanel<UI_TurretMngPanel>(0);

            //��ȯ�Ǿ��ִ� �ͷ� ���� �г�
            pm.EnablePanel<UI_TurretInfoPanel>(1);
            if (pm.turretInfoPanel)
                pm.turretInfoPanel.Reset();

            step = Step.touchNest;
        }
        else if (step == Step.upgradeInfo)
        {
            //�ؽ�Ʈ �ʱ�ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText).gameObject.SetActive(true);

            //���� �ڿ��� ����
            SystemManager.Instance.ResourceManager.woodResource += 100;

            StopCoroutine("Typing");
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText),
                "��, �׷� ���� �����Դϴ�!", 0.05f));

            GetGameobject((int)GameObjects.ColorWoodInfoPanel).SetActive(false);

            step = Step.startDefense;
        }
        else if (step == Step.startDefense)
        {
            GetComponent<Image>().enabled = false;
            GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText).gameObject.SetActive(false);

            //���� Ȱ��ȭ
            for (int i = 0; i < SystemManager.Instance.BlockManager.tutorialNest.Length; i++)
                SystemManager.Instance.BlockManager.tutorialNest[i].SetActive(true);

            //���潺 ����
            SystemManager.Instance.GameFlowManager.gameState = GameFlowManager.GameState.Defense;
        }


        //Next��ư ��Ȱ��ȭ
        if (GetButton((int)Buttons.WelcomeTextButton).gameObject.activeSelf) 
            GetButton((int)Buttons.WelcomeTextButton).gameObject.SetActive(false);
        if (GetButton((int)Buttons.WelcomeTextButton2).gameObject.activeSelf)
            GetButton((int)Buttons.WelcomeTextButton2).gameObject.SetActive(false);
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

        //Next��ư Ȱ��ȭ
        GetButton((int)Buttons.WelcomeTextButton).gameObject.SetActive(true);
    }
}
