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
    float woodResourceIncreaseCycle = 2.0f;

    //���� �ڿ� ���� �ֱ⸶�� �����Ǵ� ���� �ڿ� ��
    [SerializeField]
    int woodResourceIncreaseValue = 10;

    //�ð� ������ ����
    float timer;

    private void Start()
    {
        //���� �ڿ� �ʱ�ȭ
        woodResource = startWoodResource;

        //�ð� ������ ���� �ʱ�ȭ
        timer = Time.time;
    }

    private void Update()
    {
        //���� �ð� ���� �ڵ����� ���̴� �����ڿ�
        IncreaseWoodResoruce_Auto();
    }

    /// <summary>
    /// ���� �ð� ���� �ڵ����� �����ڿ� ������Ű�� UI �������� ���� : ������
    /// </summary>
    void IncreaseWoodResoruce_Auto()
    {
        //woodResourceIncreaseCycle�ʸ��� �����ڿ� ����
        if (Time.time - timer > woodResourceIncreaseCycle)
        {
            //���� �ڿ��� ����
            woodResource += woodResourceIncreaseValue;

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
    void IncreaseWoodResource(int increaseValue)
    {
        //���� �ڿ��� ����
        woodResource += increaseValue;

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

        //UI���� ����
        if (SystemManager.Instance.PanelManager.resoursePanel)
        {
            UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
            resourcePanel.UpdateWoodResource();
        }
    }
}
