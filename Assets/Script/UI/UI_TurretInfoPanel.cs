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

    const int MAXCOLORWOOD = 4;   //�ִ� ���� ��

    bool isBind = false;

    [SerializeField]
    Sprite[] turretSprite;  //�ͷ� �̹��� ����
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
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        isBind = true;//���ε� �Ϸ�
        Reset();
    }

    /// <summary>
    /// �ͷ����� UI �ֽ������� ������Ʈ : ������
    /// </summary>
    /// <param name="updateAllState">true�� �ǰ�,��ȭ state�� ����, false�� ������� ����</param>
    public void Reset(bool updateAllState = true)
    {
        Debug.Log("Reset");
        //���ε尡 ���� �ȵ� ����
        if (!isBind)
            return;

        Nest nest = null;
        if (SystemManager.Instance.InputManager.currenstSelectNest)
            nest = SystemManager.Instance.InputManager.currenstSelectNest.GetComponent<Nest>();

        //����ó��
        if (!nest)
            return;

        Turret turret = nest.turret.GetComponent<Turret>();

        if (!turret)
            return;
        if (updateAllState)
        {
            //�̹��� ���� ����
            if (turret.turretNum < turretSprite.Length)
                GetImage((int)Images.TurretInfoImage).sprite = turretSprite[turret.turretNum];
        }
        //HP �ؽ�Ʈ ���� ����
         GetTextMeshProUGUI((int)TextMeshProUGUIs.HpPointText).text= turret.currentHP + "/" + turret.maxHP;
    }
}
