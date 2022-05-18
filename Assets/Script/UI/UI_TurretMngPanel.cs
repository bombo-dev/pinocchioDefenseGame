using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_TurretMngPanel : UI_Controller
{
    const int MAXTURRET = 17;   //최대 터렛 수

    int currentSelectedTurretIdx = 0;   //현재 선택한 터렛 번호
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
    /// enum에 열거된 이름으로 UI정보를 바인딩
    /// </summary>
    protected override void BindingUI()
    {
        Bind<Button>(typeof(Buttons));

        //터렛 선택 버튼 이벤트 추가
        for (int i = 0; i< MAXTURRET; i++)
        {
            AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);
        }

        //터렛 소환 버튼 이벤트 추가
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
