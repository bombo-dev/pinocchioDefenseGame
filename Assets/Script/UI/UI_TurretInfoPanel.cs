using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_TurretInfoPanel : UI_Controller
{
    public string filePath;

    const int MAXCOLORWOOD = 4;   //최대 나무 수

    enum Buttons
    {
        ColorWoodButton0,//0~
        ColorWoodButton1,
        ColorWoodButton2,
        ColorWoodButton3, //~3
        TurretUpgradeButton
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        Bind<Button>(typeof(Buttons));

        //터렛 선택 버튼 이벤트 추가
        for (int i = 0; i < MAXCOLORWOOD; i++)
        {
            //AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);
        }

    }
}
