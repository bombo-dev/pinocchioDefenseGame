using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TurretInfoPanel : UI_Controller
{
    public string filePath;

    const int MAXCOLORWOOD = 4;   //최대 나무 수

    bool isBind = false;

    [SerializeField]
    Sprite[] turretSprite;  //터렛 이미지 모음
    enum Buttons
    {
        ColorWoodButton0,//0~
        ColorWoodButton1,
        ColorWoodButton2,
        ColorWoodButton3, //~3
        TurretUpgradeButton,
        CloseTurretInfoPanelButton
    }

    enum Images
    {
        TurretInfoImage
    }

    enum TextMeshProUGUIs
    {
        HpPointText
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        isBind = true;//바인드 완료

        Reset();

        //패널 닫기 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.CloseTurretInfoPanelButton).gameObject, ClosePanel, Define.UIEvent.Click);
    }

    /// <summary>
    /// 터렛정보 UI 최신정보로 업데이트 : 김현진
    /// </summary>
    /// <param name="updateAllState">true면 피격,강화 state만 갱신, false면 모든정보 갱신</param>
    public void Reset(bool updateAllState = true)
    {
        Debug.Log("Reset");
        //바인드가 아직 안된 상태
        if (!isBind)
            return;

        Nest nest = null;
        if (SystemManager.Instance.InputManager.currenstSelectNest)
            nest = SystemManager.Instance.InputManager.currenstSelectNest.GetComponent<Nest>();

        //예외처리
        if (!nest)
            return;

        Turret turret = nest.turret.GetComponent<Turret>();

        if (!turret)
            return;
        if (updateAllState)
        {
            //이미지 정보 갱신
            if (turret.turretNum < turretSprite.Length)
                GetImage((int)Images.TurretInfoImage).sprite = turretSprite[turret.turretNum];
        }
        //HP 텍스트 정보 갱신
         GetTextMeshProUGUI((int)TextMeshProUGUIs.HpPointText).text= turret.currentHP + "/" + turret.maxHP;
    }

    /// <summary>
    /// 패널을 닫는다 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    void ClosePanel(PointerEventData data)
    {
        //UI_TurretMngPanel 패널이 존재할 경우
        if (SystemManager.Instance.PanelManager.turretInfoPanel)
        {
            //패널 비활성화
            SystemManager.Instance.PanelManager.DisablePanel<UI_TurretInfoPanel>(SystemManager.Instance.PanelManager.turretInfoPanel.gameObject);
        }
    }
}
