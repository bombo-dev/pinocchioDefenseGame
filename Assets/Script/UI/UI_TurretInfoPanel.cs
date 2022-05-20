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
        TurretUpgradeButton
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
    }

    public void Reset()
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

        if (turret.turretNum < turretSprite.Length)
            GetImage((int)Images.TurretInfoImage).sprite = turretSprite[turret.turretNum];
         GetTextMeshProUGUI((int)TextMeshProUGUIs.HpPointText).text= turret.currentHP + "/" + turret.maxHP;
    }
}
