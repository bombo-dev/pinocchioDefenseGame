using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_TurretInfoPanel : UI_Controller
{
    public string filePath;

    const int MAXCOLORWOOD = 4;   //�ִ� ���� ��

    enum Buttons
    {
        ColorWoodButton0,//0~
        ColorWoodButton1,
        ColorWoodButton2,
        ColorWoodButton3, //~3
        TurretUpgradeButton
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        Bind<Button>(typeof(Buttons));

        //�ͷ� ���� ��ư �̺�Ʈ �߰�
        for (int i = 0; i < MAXCOLORWOOD; i++)
        {
            //AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);
        }

    }
}
