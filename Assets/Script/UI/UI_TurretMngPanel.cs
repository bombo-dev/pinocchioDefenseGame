using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_TurretMngPanel : UI_Controller
{
    public string filePath;

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
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));

        //터렛 선택 버튼 이벤트 추가
        for (int i = 0; i< MAXTURRET; i++)
        {
            AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);
        }

        //터렛 소환 버튼 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.TurretSummonButton).gameObject, OnClickTurretSummonButton, Define.UIEvent.Click);

    }

    /// <summary>
    /// 소환할 터렛을 선택하거나 더블 터치 or 클릭으로 소환
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    /// <param name="idx">소환할 터렛의 인덱스</param>
    public void OnClickTurretButton(PointerEventData data, int idx)
    {
        currentSelectedTurretIdx = idx;

        //더블클릭 이벤트
        if (data.clickCount == 2)
            OnClickTurretSummonButton(data);
    }

    /// <summary>
    /// 선택해놓은 터렛을 소환 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    public void OnClickTurretSummonButton(PointerEventData data)
    {
        if (currentSelectedTurretIdx >= 0 && currentSelectedTurretIdx < MAXTURRET && SystemManager.Instance.InputManager.currenstSelectNest != null)
        {
            GameObject nestGo = SystemManager.Instance.InputManager.currenstSelectNest;
            
            //예외처리
            if (!nestGo)
                return;

            GameObject turretGo = SystemManager.Instance.TurretManager.EnableTurret(currentSelectedTurretIdx, nestGo.transform.position);

            if (!turretGo)
                return;
            
            //둥지정보 갱신
            Nest nest = nestGo.GetComponent<Nest>();
            if (nest)
            {
                turretGo.GetComponent<Turret>().nest = nestGo;
                nest.haveTurret = true;
                nest.turret = turretGo;
            }

            //UI_TurretMngPanel 패널이 존재할 경우
            if (SystemManager.Instance.PanelManager.turretMngPanel)
            {
                //패널 비활성화
                SystemManager.Instance.PanelManager.DisablePanel<UI_TurretMngPanel>(SystemManager.Instance.PanelManager.turretMngPanel.gameObject);
            }
            if (!SystemManager.Instance.PanelManager.turretMngPanel)
                SystemManager.Instance.PanelManager.EnablePanel<UI_TurretInfoPanel>(1); //1: UI_TurretInfoPanel

        }
    }
}
