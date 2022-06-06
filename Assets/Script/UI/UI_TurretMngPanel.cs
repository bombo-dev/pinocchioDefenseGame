using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TurretMngPanel : UI_Controller
{
    public string filePath;

    const int MAXTURRET = 23;   //�ִ� �ͷ� ��

    int currentSelectedTurretIdx = 0;   //���� ������ �ͷ� ��ȣ

    
    Actor actor; // HPBar ��ġ ������Ʈ�� ����

    enum TextMeshProUGUIs
    {
        TurretText0, //0~
        TurretText1,
        TurretText2,
        TurretText3,
        TurretText4,
        TurretText5,
        TurretText6,
        TurretText7,
        TurretText8,
        TurretText9,
        TurretText10,
        TurretText11,
        TurretText12,
        TurretText13,
        TurretText14,
        TurretText15,
        TurretText16,
        TurretText17,
        TurretText18,
        TurretText19,
        TurretText20,
        TurretText21,
        TurretText22 //~22
    }

    enum Buttons
    {
        TurretButton0, //0~
        TurretButton1,
        TurretButton2,
        TurretButton3,
        TurretButton4,
        TurretButton5,
        TurretButton6,
        TurretButton7,
        TurretButton8,
        TurretButton9,
        TurretButton10,
        TurretButton11,
        TurretButton12,
        TurretButton13,
        TurretButton14,
        TurretButton15,
        TurretButton16,
        TurretButton17,
        TurretButton18,
        TurretButton19,
        TurretButton20,
        TurretButton21,
        TurretButton22, //~22
        TurretSummonButton,
        CloseTurretMngPanelButton
    }
    

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));

        //�ͷ� ���� ��ư �̺�Ʈ �߰�
        for (int i = 0; i < MAXTURRET; i++)
        {
            AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);
        }

        //�ͷ� ��ȯ ��ư �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.TurretSummonButton).gameObject, OnClickTurretSummonButton, Define.UIEvent.Click);

        //�г� �ݱ� �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.CloseTurretMngPanelButton).gameObject, ClosePanel, Define.UIEvent.Click);
        //Debug.Log("UI_TurretMngPanel.go =" + GetButton((int)Buttons.CloseTurretMngPanelButton).gameObject);

        // ��ũ�� �� �巡�� ��, ȭ�� ��ũ�� ����
        
    }

    /// <summary>
    /// ��ȯ�� �ͷ��� �����ϰų� ���� ��ġ or Ŭ������ ��ȯ
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    /// <param name="idx">��ȯ�� �ͷ��� �ε���</param>
    public void OnClickTurretButton(PointerEventData data, int idx)
    {
        currentSelectedTurretIdx = idx;

        //����Ŭ�� �̺�Ʈ
        if (data.clickCount == 2)
            OnClickTurretSummonButton(data);
    }

    /// <summary>
    /// �����س��� �ͷ��� ��ȯ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    public void OnClickTurretSummonButton(PointerEventData data)
    {
        //�Ǽ� ��� ����
        int cost = int.Parse(GetTextMeshProUGUI(currentSelectedTurretIdx).text);

        if (cost <= 0)
            return;

        //����� �����Ѱ�� �Ǽ��Ұ�
        if (cost > SystemManager.Instance.ResourceManager.woodResource)
        {
            //������ ó��
            return;
        }
        else
        {
            //�������
            SystemManager.Instance.ResourceManager.DecreaseWoodResource(cost);
        }
        

        if (currentSelectedTurretIdx >= 0 && currentSelectedTurretIdx < MAXTURRET && SystemManager.Instance.InputManager.currenstSelectNest != null)
        {
            GameObject nestGo = SystemManager.Instance.InputManager.currenstSelectNest;
            
            //����ó��
            if (!nestGo)
                return;

            GameObject turretGo = SystemManager.Instance.TurretManager.EnableTurret(currentSelectedTurretIdx, nestGo.transform.position);
                      
            if (!turretGo)
                return;

            Turret turret = turretGo.GetComponent<Turret>();

            // �ͷ� ���� ���� �г� ����
            SystemManager.Instance.PanelManager.EnablePanel<StatusMngPanel>(3, turret.hpPos.transform.position, turret.turretIndex, turret.GetType());
            //Debug.Log("turret.type=" + turret.GetType().Name);
            if (!SystemManager.Instance.PanelManager.statusMngPanel)
                return;

            

            //�������� ����
            Nest nest = nestGo.GetComponent<Nest>();
            if (nest)
            {
                turretGo.GetComponent<Turret>().nest = nestGo;
                nest.haveTurret = true;
                nest.turret = turretGo;
            }

            //UI_TurretMngPanel �г��� ������ ���
            if (SystemManager.Instance.PanelManager.turretMngPanel)
            {
                //�г� ��Ȱ��ȭ
                SystemManager.Instance.PanelManager.DisablePanel<UI_TurretMngPanel>(SystemManager.Instance.PanelManager.turretMngPanel.gameObject);
            }
            if (!SystemManager.Instance.PanelManager.turretMngPanel)
                SystemManager.Instance.PanelManager.EnablePanel<UI_TurretInfoPanel>(1); //1: UI_TurretInfoPanel

        }
    }

    /// <summary>
    /// �г��� �ݴ´� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void ClosePanel(PointerEventData data)
    {
        //UI_TurretMngPanel �г��� ������ ���
        if (SystemManager.Instance.PanelManager.turretMngPanel)
        {
            //�г� ��Ȱ��ȭ
            SystemManager.Instance.PanelManager.DisablePanel<UI_TurretMngPanel>(SystemManager.Instance.PanelManager.turretMngPanel.gameObject);
        }
    }

}
