using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    //��ȭ ���� ���� ����Ʈ
    public int[] colorWoodReward = new int[6];

    //���������� �ͷ� ���� ����
    public Dictionary<int, int> turretReward = new Dictionary<int, int>();  //��������, �ͷ� ����

    //�� ����
    public int starRewardNum;

    //�ߺ����� ���� �ͷ������� ������� true
    public bool getNewTurret = false;

    // Start is called before the first frame update
    void Start()
    {
        SetReward();
    }

    /// <summary>
    /// �ش� �������� ���� ���� : ������
    /// </summary>
    void SetReward()
    {
        //�����÷ο� �Ŵ���
        GameFlowManager gfm = SystemManager.Instance.GameFlowManager;

        //�迭 �ʱ�ȭ
        colorWoodReward.Initialize();

        //TurretReward����
        SetTurretRewardInfo();

        //���� WoodReward����

        //Ʃ�丮�� ù Ŭ���� ����
        if (gfm.stage == 0)
        {
            colorWoodReward[0] = 1;
            colorWoodReward[1] = 1;
            colorWoodReward[2] = 1;
            colorWoodReward[3] = 1;
            colorWoodReward[4] = 1;
        }
        else
        {
            for (int i = 0; i < gfm.stage; i++)
            {
                //0~4�� ������ �������� ����ŭ ���� ����
                colorWoodReward[Random.Range(0, 5)]++;

                //10%Ȯ���� �߰�����
                if (Random.Range(0, 10) == 1)
                    colorWoodReward[5]++;
            }
        }
        

        //�̹� Ŭ������ ���������� ��� ���� 3/1
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        if (!userInfo)
            return;

        //�̹� Ŭ������ ���������� ���
        if (gfm.stage < userInfo.maxStageNum)
        {
            if (gfm.stage == 0) //Ʃ�丮��
            {
                colorWoodReward[0] = 0;
                colorWoodReward[1] = 0;
                colorWoodReward[2] = 0;
                colorWoodReward[3] = 0;
                colorWoodReward[4] = 0;
            }
            else
            {
                for (int i = 0; i < colorWoodReward.Length; i++)
                {
                    if (colorWoodReward[i] > 2)
                        colorWoodReward[i] = colorWoodReward[i] / 2;
                    else if (colorWoodReward[i] == 2)
                        colorWoodReward[i] = 1;
                }
            }

        }

    }
    /// <summary>
    /// ���������� �ͷ� ���� ���� : ������
    /// </summary>
    void SetTurretRewardInfo()
    {
        turretReward.Add(1, 2);//���ٱ���
        turretReward.Add(2, 3);//���̺����
        turretReward.Add(3, 4);//��ȭ��ѱ�
        turretReward.Add(4, 5);//�縮��
        turretReward.Add(5, 6);//�ð�
        turretReward.Add(7, 7);//���
        turretReward.Add(11, 8);//�ξ�
        turretReward.Add(13, 9);//��ȭ ���ٱ���
        turretReward.Add(15, 10);//��
        turretReward.Add(18, 11);//������
        turretReward.Add(21, 12);//��ȭ �Ʊ��
        turretReward.Add(23, 13);//��ȭ ������
        turretReward.Add(25, 14);//�ʰ�ȭ ��ѱ�


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
