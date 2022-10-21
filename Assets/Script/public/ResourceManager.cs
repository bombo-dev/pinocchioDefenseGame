using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    //�ͷ� ���� � ���̴� ���� �ڿ�
    public int woodResource = 0;

    //���� �ڿ� �ʱⰪ
    [SerializeField]
    int startWoodResource = 500;

    //���� �ڿ� ���� �ֱ�
    [SerializeField]
    float woodResourceIncreaseCycle = 1.0f;

    //���� �ڿ� ���� �ֱ⸶�� �����Ǵ� ���� �ڿ� ��
    [SerializeField]
    int woodResourceIncreaseValue = 10;

    float timer;

    //User Info
    [Header("UserInfo")]
    //��ȭ ���� �ڿ�
    public int[] colorWoodResource; //0 - red, 1- yellow, 2 - green, 3 - white, 4 - blue, 5 - black

    //������ �ͷ� ����Ʈ
    public List<int> selectedTurretPreset;    //�κ񿡼� �Ѱܿ� �ͷ� ������

    private void Start()
    {
        //���� �ڿ� �ʱ�ȭ
        if (SystemManager.Instance.GameFlowManager.stage > 0)
            woodResource = startWoodResource + ((SystemManager.Instance.GameFlowManager.stage - 1) * 50);
        else
            woodResource = 9999;

       //�ð� ������ ���� �ʱ�ȭ
       timer = Time.time;


        //-------------------���� �ڿ� ���� �ҷ�����-------------------
        UserInfo userInfo = SystemManager.Instance.UserInfo;
        //��ȭ �����ڿ�
        colorWoodResource = new int[6];
        for (int i = 0; i < colorWoodResource.Length; i++)
            colorWoodResource[i] = userInfo.colorWoodResource[i];

        //�ͷ� ������ ����
        selectedTurretPreset.Clear();
        for (int i = 0; i < userInfo.turretPreset.Count; i++)
        {
            selectedTurretPreset.Add(userInfo.turretPreset[i]);
        }
    }

    private void Update()
    {
        if ((int)SystemManager.Instance.GameFlowManager.gameState == (int)GameFlowManager.GameState.Defense)
        {
            //���� �ð� ���� �ڵ����� ���̴� �����ڿ�, 
            IncreaseWoodResoruce_Auto();
        }
    }

    /// <summary>
    /// ���� �ð� ���� �ڵ����� �����ڿ� ������Ű�� UI �������� ���� : ������
    /// </summary>
    void IncreaseWoodResoruce_Auto()
    {
        //woodResourceIncreaseCycle�ʸ��� �����ڿ� ����
        if (Time.time - timer > woodResourceIncreaseCycle)
        {
            int increaseWoodResource;

            if (SystemManager.Instance.UserInfo.selectMode == 0) // - �븻
                increaseWoodResource = woodResourceIncreaseValue + (SystemManager.Instance.UserInfo.selectedStageNum);
            else
                increaseWoodResource = woodResourceIncreaseValue + (SystemManager.Instance.UserInfo.selectedStageNum_hard);

            //���� �ڿ��� ����
            woodResource += increaseWoodResource;

            // GoodsMngPanel ����
            CreateGoodsPanel(increaseWoodResource, 1);

            //UI���� ����
            if (SystemManager.Instance.PanelManager.resoursePanel)
            {
                UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
                resourcePanel.UpdateWoodResource();
            }

            //�ð� ������ ���� �ʱ�ȭ
            timer = Time.time;
        }
    }

    /// <summary>
    /// ���� �ڿ� ������Ű�� UI�������� ���� : ������
    /// </summary>
    /// <param name="increaseValue">������ų ��</param>
    public void IncreaseWoodResource(int increaseValue)
    {
        //���� �ڿ��� ����
        woodResource += increaseValue;

        CreateGoodsPanel(increaseValue, 1);

        //UI���� ����
        if (SystemManager.Instance.PanelManager.resoursePanel)
        {
            UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
            resourcePanel.UpdateWoodResource();
        }
    }

    /// <summary>
    /// ���� �ڿ� ���ҽ�Ű�� UI�������� ���� : ������
    /// </summary>
    /// <param name="decreaseValue">���ҽ�ų ��</param>
    public void DecreaseWoodResource(int decreaseValue)
    {
        //���� �ڿ��� ����
        if (woodResource <= decreaseValue)
            woodResource = 0;
        else
            woodResource -= decreaseValue;

        CreateGoodsPanel(decreaseValue, -1);

        //UI���� ����
        if (SystemManager.Instance.PanelManager.resoursePanel)
        {
            UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
            resourcePanel.UpdateWoodResource();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">����, ���ҽ�ų �ڿ���</param>
    /// <param name="identity">���� �ĺ� ����: ����� �ڿ� ����, ������ �ڿ� ����</param>
    void CreateGoodsPanel(int value, int identity)
    {
        GameObject go = SystemManager.Instance.PanelManager.EnablePanel<RewardsMngPanel>(8);

        if (!go)
        {
            Debug.Log("CreateGoodsPanel Error!");
            return;
        }

        go.GetComponent<RewardsMngPanel>().ShowGold(value, identity);

    }
}
