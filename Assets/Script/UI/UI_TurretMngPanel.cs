using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_TurretMngPanel : UI_Controller
{
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
    /// enum�� ���ŵ� �̸����� UI������ ���ε�
    /// </summary>
    protected override void BindingUI()
    {
        Bind<Button>(typeof(Buttons));

        //�ͷ� ���� ��ư �̺�Ʈ �߰�
        for (int i = 0; i< MAXTURRET; i++)
        {
            AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);
        }

        //�ͷ� ��ȯ ��ư �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.TurretSummonButton).gameObject, OnClickTurretSummonButton, Define.UIEvent.Click);

    }

    public void OnClickTurretButton(PointerEventData data, int idx)
    {
        currentSelectedTurretIdx = idx;
    }

    public void OnClickTurretSummonButton(PointerEventData data)
    {
        SystemManager.Instance.TurretManager.EnableTurret(currentSelectedTurretIdx, SystemManager.Instance.InputManager.currenstSelectNest.transform.position);
    }
}
