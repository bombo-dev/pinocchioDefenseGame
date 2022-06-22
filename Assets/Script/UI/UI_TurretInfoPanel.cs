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

    const int TURRETSMOKEEFFECT = 3;

    const int MAXCOLORWOOD = 6;   //�ִ� ���� ��

    const float BUFFDURATIONTIME = 10f;   //���� ���ӽð�

    bool isBind = false;

    [SerializeField]
    Sprite[] turretSprite;  //�ͷ� �̹��� ����

    [SerializeField]
    Sprite emptySprite;   //�� �̹���

    enum Buttons
    {
        ColorWoodButton0,//0~
        ColorWoodButton1,
        ColorWoodButton2,
        ColorWoodButton3, 
        ColorWoodButton4, 
        ColorWoodButton5, //~5
        CacelTurretButton,
        DestroyTurretButton
    }

    enum Images
    {
        TurretInfoImage
    }

    enum TextMeshProUGUIs
    {
        ColorWoodText0,//0~
        ColorWoodText1,
        ColorWoodText2,
        ColorWoodText3,
        ColorWoodText4,
        ColorWoodText5, //~5
        ColorWoodNumText,    //��ȭ�� �Һ�Ǵ� ColorWoodǥ��
        HpPointText, 
        PowerPointText,
        AttackSpeedPointText,
        RegenerationPointText,
        DefensePointText,
        RangePointText,
        TargetPointText 
    }

    enum Gameobjects
    {
        ColorWoodEmpty,
        TurretStatePanel,
        IsConstructionPanel
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
        Bind<GameObject>(typeof(Gameobjects));

        isBind = true;//���ε� �Ϸ�

        Reset();

        //�ͷ� ��ȭ ��ư �̺�Ʈ �߰�
        for (int i = 0; i < MAXCOLORWOOD; i++)
        {
            AddUIEvent(GetButton(i).gameObject, i, AddBuffTurret, Define.UIEvent.Click);
        }

        //�ͷ� �ı� �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.DestroyTurretButton).gameObject, OnClickDestroyTurretButton, Define.UIEvent.Click);
        //������ �ı� �̺�Ʈ �߰�
        AddUIEvent(GetButton((int)Buttons.CacelTurretButton).gameObject, OnClickCancelButton, Define.UIEvent.Click);
    }

    /// <summary>
    /// �ͷ����� UI �ֽ������� ������Ʈ : ������
    /// </summary>
    /// <param name="updateAllState">false�� �ǰ�,��ȭ state�� ����, true�� ������� ����</param>
    public void Reset(bool updateAllState = true, bool updateBuffTextColor = true)
    {
        //���ε尡 ���� �ȵ� ����
        if (!isBind)
            return;

        GameObject nestGo = SystemManager.Instance.InputManager.currenstSelectNest;

        //�������϶�, ����Ϸ�����϶� UI����
        if (nestGo)
        {
            //������ �ƹ��͵� ���� ���
            if (!nestGo.GetComponent<Nest>().construction && !nestGo.GetComponent<Nest>().haveTurret)
            {
                TurretInfoPanelClear();

                return;
            }
            //�������� ���
            else if (nestGo.GetComponent<Nest>().construction)
            {
                ConstructionTurret constructionTurret = nestGo.GetComponent<Nest>().turret.GetComponent<ConstructionTurret>();

                //�̹��� ���� ����
                GetImage((int)Images.TurretInfoImage).sprite = turretSprite[constructionTurret.currentSelectedTurretIdx];

                if (GetGameobject((int)Gameobjects.ColorWoodEmpty).activeSelf)
                    GetGameobject((int)Gameobjects.ColorWoodEmpty).SetActive(false);
                if (GetGameobject((int)Gameobjects.TurretStatePanel).activeSelf)
                    GetGameobject((int)Gameobjects.TurretStatePanel).SetActive(false);
                if (!GetGameobject((int)Gameobjects.IsConstructionPanel).activeSelf)
                    GetGameobject((int)Gameobjects.IsConstructionPanel).SetActive(true);
                if (!GetButton((int)Buttons.CacelTurretButton).gameObject.activeSelf)
                    GetButton((int)Buttons.CacelTurretButton).gameObject.SetActive(true);
                if (GetButton((int)Buttons.DestroyTurretButton).gameObject.activeSelf)
                    GetButton((int)Buttons.DestroyTurretButton).gameObject.SetActive(false);

                return;
            }
            //���� �Ϸ�� ������ ���
            else
            {
                if (!GetGameobject((int)Gameobjects.ColorWoodEmpty).activeSelf)
                    GetGameobject((int)Gameobjects.ColorWoodEmpty).SetActive(true);
                if (!GetGameobject((int)Gameobjects.TurretStatePanel).activeSelf)
                    GetGameobject((int)Gameobjects.TurretStatePanel).SetActive(true);
                if (GetGameobject((int)Gameobjects.IsConstructionPanel).activeSelf)
                    GetGameobject((int)Gameobjects.IsConstructionPanel).SetActive(false);
                if (GetButton((int)Buttons.CacelTurretButton).gameObject.activeSelf)
                    GetButton((int)Buttons.CacelTurretButton).gameObject.SetActive(false);
                if (!GetButton((int)Buttons.DestroyTurretButton).gameObject.activeSelf)
                    GetButton((int)Buttons.DestroyTurretButton).gameObject.SetActive(true);
            }
        }
        else
        {
            TurretInfoPanelClear();
        }

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

        if (updateBuffTextColor)
        {
            //�ؽ�Ʈ �÷����� ���������� ����
            for (int i = 0; i < MAXCOLORWOOD; i++)
            {
                //�ε����� buff�� ����ȯ
                Turret.buff _buffIndex = (Turret.buff)(i + 1);

                if (turret.buffs.ContainsKey(_buffIndex))
                {
                    if (i == MAXCOLORWOOD - 1)
                    {
                        //�ý��� ���� ������ ���
                        GetTextMeshProUGUI((int)TextMeshProUGUIs.PowerPointText).color = Color.red;
                        GetTextMeshProUGUI((int)TextMeshProUGUIs.DefensePointText).color = Color.red;
                        GetTextMeshProUGUI((int)TextMeshProUGUIs.RegenerationPointText).color = Color.red;
                    }
                    else
                        GetTextMeshProUGUI((int)TextMeshProUGUIs.PowerPointText + i).color = Color.red;
                }
                else
                    GetTextMeshProUGUI((int)TextMeshProUGUIs.PowerPointText + i).color = Color.white;
            }
        }

        //HP �ؽ�Ʈ ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.HpPointText).text= turret.currentHP + "/" + turret.maxHP;
        //���ݷ� �ؽ�Ʈ ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PowerPointText).text = turret.currentPower.ToString();
        //���ݼӵ� �ؽ�Ʈ ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.AttackSpeedPointText).text = (1 / turret.currentAttackSpeed).ToString();
        //���� �ؽ�Ʈ ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.DefensePointText).text = turret.currentDefense.ToString();
        //��Ÿ� �ؽ�Ʈ ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.RangePointText).text = turret.currentRange.ToString();
        //ȸ���� �ؽ�Ʈ ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.RegenerationPointText).text = turret.currentRegeneration.ToString();
        //�ִ�Ÿ�� �ؽ�Ʈ ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TargetPointText).text = turret.attackTargetNum.ToString();

        //Color Wood���� ����
        for (int i = 0; i < MAXCOLORWOOD; i++)
        {
            GetTextMeshProUGUI((int)i).text = (turret.turretNum + 1).ToString() + "/" + SystemManager.Instance.ResourceManager.colorWoodResource[i].ToString();

            if (turret.turretNum + 1 > SystemManager.Instance.ResourceManager.colorWoodResource[i])
                GetTextMeshProUGUI((int)i).color = Color.red;
            else
                GetTextMeshProUGUI((int)i).color = Color.white;
        }

        //��ȭ�� �Һ�Ǵ� ColorWood ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ColorWoodNumText).text = "��ȭ�� " + (turret.turretNum + 1).ToString() + "�� �Һ�";

    }

    /// <summary>
    /// �г� �޴����� �ʱ���·� ������ : ������
    /// </summary>
    void TurretInfoPanelClear()
    {
        //�̹��� ���� ����
        GetImage((int)Images.TurretInfoImage).sprite = emptySprite;

        if (GetGameobject((int)Gameobjects.ColorWoodEmpty).activeSelf)
            GetGameobject((int)Gameobjects.ColorWoodEmpty).SetActive(false);
        if (GetGameobject((int)Gameobjects.TurretStatePanel).activeSelf)
            GetGameobject((int)Gameobjects.TurretStatePanel).SetActive(false);
        if (GetGameobject((int)Gameobjects.IsConstructionPanel).activeSelf)
            GetGameobject((int)Gameobjects.IsConstructionPanel).SetActive(false);
        if (GetButton((int)Buttons.CacelTurretButton).gameObject.activeSelf)
            GetButton((int)Buttons.CacelTurretButton).gameObject.SetActive(false);
        if (GetButton((int)Buttons.DestroyTurretButton).gameObject.activeSelf)
            GetButton((int)Buttons.DestroyTurretButton).gameObject.SetActive(false);
    }

    /// <summary>
    /// �ͷ��� �ش� idx�� �ش��ϴ� ������ �߰��Ѵ� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    /// <param name="idx">�߰��� ������ ���� �ε���</param>
    void AddBuffTurret(PointerEventData data, int idx)
    {
        Turret turret = getTurret();

        //����ó��
        if (!turret)
            return;

        //Ÿ���� Dead���¸� ���
        if (turret.currentHP <= 0)
            return;

        //��ȭ�� �ڿ��� �����ϴ��� �Ǵ�
        if (SystemManager.Instance.ResourceManager.colorWoodResource[idx] < (turret.turretNum + 1))
            return;

        //��ȭ �ڿ� �Һ�
        SystemManager.Instance.ResourceManager.colorWoodResource[idx] -= turret.turretNum + 1;

        turret.AddBebuff(idx + 1, BUFFDURATIONTIME);

        Reset();

        SaveLoad save = new SaveLoad();
        save.SaveUserInfo();
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

    /// <summary>
    /// Ŭ���� ������ ���� ���� ���� ��ȯ �Ǿ��ִ� �ͷ��� ������ �޾ƿ´� : ������
    /// </summary>
    /// <returns></returns>
    ConstructionTurret getConstructionTurret()
    {
        Nest nest = null;
        if (SystemManager.Instance.InputManager.currenstSelectNest)
            nest = SystemManager.Instance.InputManager.currenstSelectNest.GetComponent<Nest>();

        //����ó��
        if (!nest)
            return null;

        if (!nest.turret)
            return null;

        return nest.turret.GetComponent<ConstructionTurret>();
    }

    /// <summary>
    /// �̹� ���簡 �Ϸ�� �ͷ��� �ı� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickDestroyTurretButton(PointerEventData data)
    {
        //�ͷ�����
        Turret turret = getTurret();

        //���� ����
        Nest nest = null;
        if (SystemManager.Instance.InputManager.currenstSelectNest)
            nest = SystemManager.Instance.InputManager.currenstSelectNest.GetComponent<Nest>();

        //����ó��
        if (!turret || !nest)
            return;

        //Ÿ���� Dead���¸� ���
        if (turret.currentHP <= 0)
            return;

        //�ı� ����Ʈ ���
        SystemManager.Instance.EffectManager.EnableEffect(TURRETSMOKEEFFECT, turret.hitPos.transform.position);

        //�ͷ� �ı�
        turret.DecreaseHP(99999);
    }

    /// <summary>
    /// �������� �ͷ��� �ı� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    void OnClickCancelButton(PointerEventData data)
    {
        ConstructionTurret turret = getConstructionTurret();

        //�ı� ����Ʈ ���
        SystemManager.Instance.EffectManager.EnableEffect(TURRETSMOKEEFFECT, turret.transform.position);

        //�ͷ� �ı�
        turret.CancelConstruction();
    }
}
