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

    const int MAXCOLORWOOD = 6;   //�ִ� ���� ��

    const float BuffDurationTime = 10f;   //���� ���ӽð�

    bool isBind = false;

    [SerializeField]
    Sprite[] turretSprite;  //�ͷ� �̹��� ����
    enum Buttons
    {
        ColorWoodButton0,//0~
        ColorWoodButton1,
        ColorWoodButton2,
        ColorWoodButton3, 
        ColorWoodButton4, 
        ColorWoodButton5, //~5
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

        //�ͷ� ��ȭ ��ư �̺�Ʈ �߰�
        for (int i = 0; i < MAXCOLORWOOD; i++)
        {
            AddUIEvent(GetButton(i).gameObject, i, AddBuffTurret, Define.UIEvent.Click);
        }

        //�г� �ݱ� �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.CloseTurretInfoPanelButton).gameObject, ClosePanel, Define.UIEvent.Click);
    }

    /// <summary>
    /// �ͷ����� UI �ֽ������� ������Ʈ : ������
    /// </summary>
    /// <param name="updateAllState">true�� �ǰ�,��ȭ state�� ����, false�� ������� ����</param>
    public void Reset(bool updateAllState = true)
    {
        //���ε尡 ���� �ȵ� ����
        if (!isBind)
            return;

        Turret turret = getTurret();

        //����ó��
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

    /// <summary>
    /// �ͷ��� �ش� idx�� �ش��ϴ� ������ �߰��Ѵ� : ������
    /// </summary>
    /// <param name="datam">�̺�Ʈ ����</param>
    /// <param name="idx">�߰��� ������ ���� �ε���</param>
    void AddBuffTurret(PointerEventData datam, int idx)
    {
        Turret turret = getTurret();

        //����ó��
        if (!turret)
            return;

        turret.AddBebuff(idx + 1, BuffDurationTime);
    }

    /// <summary>
    /// �г��� �ݴ´� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void ClosePanel(PointerEventData data)
    {
        //UI_TurretMngPanel �г��� ������ ���
        if (SystemManager.Instance.PanelManager.turretInfoPanel)
        {
            //�г� ��Ȱ��ȭ
            SystemManager.Instance.PanelManager.DisablePanel<UI_TurretInfoPanel>(SystemManager.Instance.PanelManager.turretInfoPanel.gameObject);
        }
    }

    /// <summary>
    /// Ŭ���� ������ ���� ���� ���� ��ȯ �Ǿ��ִ� �ͷ��� ������ �޾ƿ´� : ������
    /// </summary>
    /// <returns></returns>
    Turret getTurret()
    {
        Nest nest = null;
        if (SystemManager.Instance.InputManager.currenstSelectNest)
            nest = SystemManager.Instance.InputManager.currenstSelectNest.GetComponent<Nest>();

        //����ó��
        if (!nest)
            return null;

        if (!nest.turret)
            return null;

        return nest.turret.GetComponent<Turret>();
    }
}
