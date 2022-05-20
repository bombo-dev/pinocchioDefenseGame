using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_TurretMngPanel : UI_Controller
{
    public string filePath;

    const int MAXTURRET = 17;   //�ִ� �ͷ� ��

    int currentSelectedTurretIdx = 0;   //���� ������ �ͷ� ��ȣ

    enum Buttons
    {
        TurretButton0,//0~
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
        TurretButton16,//~16
        TurretSummonButton
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));

        //�ͷ� ���� ��ư �̺�Ʈ �߰�
        for (int i = 0; i< MAXTURRET; i++)
        {
            AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);
        }

        //�ͷ� ��ȯ ��ư �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.TurretSummonButton).gameObject, OnClickTurretSummonButton, Define.UIEvent.Click);

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
        if (currentSelectedTurretIdx >= 0 && currentSelectedTurretIdx < MAXTURRET && SystemManager.Instance.InputManager.currenstSelectNest != null)
        {
            GameObject nestGo = SystemManager.Instance.InputManager.currenstSelectNest;
            
            //����ó��
            if (!nestGo)
                return;

            GameObject turretGo = SystemManager.Instance.TurretManager.EnableTurret(currentSelectedTurretIdx, nestGo.transform.position);

            if (!turretGo)
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
}
