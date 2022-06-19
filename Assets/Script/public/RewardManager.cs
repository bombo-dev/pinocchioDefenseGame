using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    //��ȭ ���� ���� ����Ʈ
    public int[] colorWoodReward = new int[6];

    //���������� �ͷ� ���� ����
    [SerializeField]
    Dictionary<int, int> turretReward = new Dictionary<int, int>();  //��������, �ͷ� ����

    //�� ����
    public int starRewardNum;

    // Start is called before the first frame update
    void Start()
    {
        SetResward();
    }

    /// <summary>
    /// �ش� �������� ���� ���� : ������
    /// </summary>
    void SetResward()
    {
        //�����÷ο� �Ŵ���
        GameFlowManager gfm = SystemManager.Instance.GameFlowManager;

        //�迭 �ʱ�ȭ
        colorWoodReward.Initialize();

        //TurretReward����
        SetTurretRewardInfo();

        //���� WoodReward����
        for (int i = 0; i < gfm.stage; i++)
        {
            //0~4�� ������ �������� ����ŭ ���� ����
            colorWoodReward[Random.Range(0, 5)]++;

            //10%Ȯ���� �߰�����
            if (Random.Range(0, 10) == 1)
                colorWoodReward[5]++;
        }

        //�̹� Ŭ������ ���������� ��� ���� 3/1
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        if (!userInfo)
            return;
         
        //�̹� Ŭ������ ���������� ���
        if (gfm.stage > userInfo.maxStageNum)
        {
            for (int i = 0; i < colorWoodReward.Length; i++)
            {
                if (colorWoodReward[i] > 2)
                    colorWoodReward[i] = colorWoodReward[i] / 3;
                else if (colorWoodReward[i] == 2)
                    colorWoodReward[i] = 1;
            }
        }

    }
    /// <summary>
    /// ���������� �ͷ� ���� ���� : ������
    /// </summary>
    void SetTurretRewardInfo()
    {
        turretReward.Add(2, 2);//���ٱ���
        turretReward.Add(3, 3);//���̺����
        turretReward.Add(4, 4);//��ȭ��ѱ�
        turretReward.Add(5, 5);//�縮��
    }

    /// <summary>
    /// ���������� �� ���� ���� : ������
    /// </summary>
    /// <returns>�� ����</returns>
    public void SetStarReward()
    {
        Turret turret = SystemManager.Instance.TurretManager.baseTurret;

        //���̽��ͷ� ���� HP���� ���
        int baseHP = (int)(((float)turret.currentHP / (float)turret.maxHP) * 100);

        if (baseHP < 40)
            starRewardNum = 1;
        else if (baseHP < 100)
            starRewardNum = 2;
        else
            starRewardNum = 3;
    }
}
