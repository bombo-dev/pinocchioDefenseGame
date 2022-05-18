using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_TurretMngPanel : UI_Controller
{
    enum Buttons
    {
        TurretButton0,
        TurretButton1
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩
    /// </summary>
    protected override void BindingUI()
    {
        Bind<Button>(typeof(Buttons));



        AddUIEvent(GetButton((int)Buttons.TurretButton0).gameObject, onClick, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.TurretButton1).gameObject, (int)Buttons.TurretButton1, OnClickTestButton, Define.UIEvent.Click);
    }

    public void OnClickTestButton(PointerEventData data, int idx)
    {
        Debug.Log(idx);
    }

    public void onClick(PointerEventData data)
    {
        Debug.Log("asd");
    }
}
