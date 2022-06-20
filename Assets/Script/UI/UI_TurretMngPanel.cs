using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TurretMngPanel : UI_Controller
{
    public string filePath;

    const int MAXTURRET = 23;   //�ִ� �ͷ� ��

    int currentSelectedTurretIdx = 0;   //���� ������ �ͷ� ��ȣ

    
    Actor actor; // HPBar ��ġ ������Ʈ�� ����

    Turret baseTurret;  //���̽� �ͷ�

    enum TextMeshProUGUIs
    {
        TurretText0, //0~
        TurretText1,
        TurretText2,
        TurretText3,
        TurretText4,
        TurretText5,
        TurretText6,
        TurretText7,
        TurretText8,
        TurretText9,
        TurretText10,
        TurretText11,
        TurretText12,
        TurretText13,
        TurretText14,
        TurretText15,
        TurretText16,
        TurretText17,
        TurretText18,
        TurretText19,
        TurretText20,
        TurretText21,
        TurretText22, //~22
        ConstructionText0,
        ConstructionText1,
        ConstructionText2,
        ConstructionText3,
        ConstructionText4,
        ConstructionText5,
        ConstructionText6,
        ConstructionText7,
        ConstructionText8,
        ConstructionText9,
        ConstructionText10,
        ConstructionText11,
        ConstructionText12,
        ConstructionText13,
        ConstructionText14,
        ConstructionText15,
        ConstructionText16,
        ConstructionText17,
        ConstructionText18,
        ConstructionText19,
        ConstructionText20,
        ConstructionText21,
        ConstructionText22,
        WoodResourceText   //���� �����ϰ��ִ� �ڿ� ǥ��
    }

    enum Buttons
    {
        TurretButton0, //0~
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
        TurretButton16,
        TurretButton17,
        TurretButton18,
        TurretButton19,
        TurretButton20,
        TurretButton21,
        TurretButton22 //~22
    }

    enum GameObjects
    {
        TurretPanel0,   //0~
        TurretPanel1,
        TurretPanel2,
        TurretPanel3,
        TurretPanel4,
        TurretPanel5,
        TurretPanel6,
        TurretPanel7,
        TurretPanel8,
        TurretPanel9,
        TurretPanel10,
        TurretPanel11,
        TurretPanel12,
        TurretPanel13,
        TurretPanel14,
        TurretPanel15,
        TurretPanel16,
        TurretPanel17,
        TurretPanel18,
        TurretPanel19,
        TurretPanel20,
        TurretPanel21,
        TurretPanel22   //~22
    }

    public enum Sliders
    {
        BaseHP
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Slider>(typeof(Sliders));

        //�ε��� ����
        int idx = 0;
        bool endTurret = false;
        //Ž���� ����Ʈ ����
        SystemManager.Instance.ResourceManager.selectedTurretPreset.Sort();

        //�ͷ� ���� ��ư �̺�Ʈ �߰�
        for (int i = 0; i < MAXTURRET; i++) 
        {
            //���õ� �ͷ��ΰ�� ��ư Ȱ��ȭ
            if (!endTurret)
            {
                if (SystemManager.Instance.ResourceManager.selectedTurretPreset[idx] == i)
                {
                    AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);

                    //�ͷ� �г� ���� �ʱ�ȭ
                    ResetTurretInfo(idx);

                    idx++;

                    if (idx >= SystemManager.Instance.ResourceManager.selectedTurretPreset.Count)
                        endTurret = true;
                }
                else
                {
                    if (GetGameobject((int)GameObjects.TurretPanel0 + i).activeSelf)
                        GetGameobject((int)GameObjects.TurretPanel0 + i).SetActive(false);
                }
            }
            //���õ� �ͷ��� �ƴҰ�� ��ư ��Ȱ��ȭ
            else
            {
                if (GetGameobject((int)GameObjects.TurretPanel0 + i).activeSelf)
                    GetGameobject((int)GameObjects.TurretPanel0 + i).SetActive(false);
            }
        }
    }

    /// <summary>
    /// �ͷ��Ŵ��� UI�� �ͷ����� ���� : ������
    /// </summary>
    private void ResetTurretInfo(int idx)
    {
        //����ó��
        if (SystemManager.Instance.TurretManager.turretCostArr.Length < MAXTURRET)
            SystemManager.Instance.TurretManager.InitializeTurretArrData();

        // �ͷ� �ڽ�Ʈ ���� �ʱ�ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretText0 + idx).text = SystemManager.Instance.TurretManager.turretCostArr[idx].ToString();

        //�ͷ� �Ǽ� �ð� ���� �ʱ�ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ConstructionText0 + idx).text = "����ð�: " + SystemManager.Instance.TurretManager.turretConstructionTimeArr[idx].ToString() + "��";
    }

    /// <summary>
    /// ��ȯ�� �ͷ��� �����ϰų� ���� ��ġ or Ŭ������ ��ȯ
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    /// <param name="idx">��ȯ�� �ͷ��� �ε���</param>
    public void OnClickTurretButton(PointerEventData data, int idx)
    {
        currentSelectedTurretIdx = idx;

        OnClickTurretSummonButton(data);
    }

    /// <summary>
    /// �����س��� �ͷ��� ��ȯ : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    public void OnClickTurretSummonButton(PointerEventData data)
    {
        //�Ǽ� ��� ����
        int cost = int.Parse(GetTextMeshProUGUI(currentSelectedTurretIdx).text);

        if (cost <= 0)
            return;

        //����� �����Ѱ�� �Ǽ��Ұ�
        if (cost > SystemManager.Instance.ResourceManager.woodResource)
        {
            //������ ó��
            return;
        }

        if (currentSelectedTurretIdx >= 0 && currentSelectedTurretIdx < MAXTURRET && SystemManager.Instance.InputManager.currenstSelectNest != null)
        {
            GameObject nestGo = SystemManager.Instance.InputManager.currenstSelectNest;

            //����ó��
            if (!nestGo)
                return;

            Nest nest = nestGo.GetComponent<Nest>();

            //����ó��
            if (!nest)
                return;

            //�̹� �ͷ��� �����ϰų� �������� ���
            if (nest.haveTurret || nest.construction)
                return;

            //����� �ͷ� ��ȯ
            GameObject turretGo = SystemManager.Instance.TurretManager.EnableTurret(SystemManager.Instance.TurretManager.CONSTRUCTIONTURRET_INDEX, nestGo.transform.position);

            //����ó��
            if (!turretGo)
                return;

            ConstructionTurret constructTurret = turretGo.GetComponent<ConstructionTurret>();

            //����ó��
            if (!constructTurret)
                return;

            //UI_ConstructionGaugePanel����
            GameObject constructionGaugePanel = SystemManager.Instance.PanelManager.EnablePanel<UI_ConstructionGauge>(5, turretGo);

            //����� �ͷ��� �ֿ� �������� �Ѱ��ֱ�
            constructTurret.timer = Time.time;  //Ÿ�̸� �ʱ�ȭ
            constructTurret.currentSelectedTurretIdx = currentSelectedTurretIdx;    //��ȯ�� �ͷ� �ε���
            constructTurret.nestGo = nestGo;    //��ȯ�� ����    
            constructTurret.constructionValue = 0;  //�Ǽ� ������ �� �ʱ�ȭ
            constructTurret.constructionTime = SystemManager.Instance.TurretManager.turretConstructionTimeArr[currentSelectedTurretIdx]; //�Ǽ��� �ɸ��� �ð� �ʱ�ȭ
            constructTurret.constructionGaugePanel = constructionGaugePanel;    //�Ǽ� ������ �г� ����

            //���������� ������ ����
            nest.construction = true;
            nest.turret = turretGo;

            //�ͷ� �������
            constructTurret.startConstruction = true;   //�������

            //UI_TurretInfoPanel �г��� ������ ��� ������ ���� ����
            if (SystemManager.Instance.PanelManager.turretInfoPanel)
            {
                //�г� ����
                SystemManager.Instance.PanelManager.turretInfoPanel.Reset();
            }

            //�������
            SystemManager.Instance.ResourceManager.DecreaseWoodResource(cost);

        }
    }

    /// <summary>
    /// �����̵���� ���� �ǽð����� ���� ���ش� : ������
    /// </summary>
    public void UpdateSlideBar()
    {
        if (SystemManager.Instance.TurretManager.baseTurret)
            GetSlider((int)Sliders.BaseHP).value =  (float)SystemManager.Instance.TurretManager.baseTurret.currentHP / (float)SystemManager.Instance.TurretManager.baseTurret.maxHP;
    }

    /// <summary>
    /// ���� ���� �ڿ����� �޾ƿ� ���� �ڿ��� ǥ�����ִ� UI�� ����
    /// </summary>
    public void UpdateWoodResource()
    {
        //���� ���� �ڿ��� �޾ƿ� �ؽ�Ʈ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.WoodResourceText).text = SystemManager.Instance.ResourceManager.woodResource.ToString();
    }

}
