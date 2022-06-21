using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageStar
{
    public int stageNum;    //��������
    public int starNum;  //Ŭ���� ��

    public StageStar()
    {
        stageNum = 0;
        starNum = 0;
    }
}
public class UserInfo : MonoBehaviour
{
    [Header("WoodIResourceInfo")]    //��ȭ ���� �ڿ�
    public int[] colorWoodResource = new int[6]; //0 - red, 1- yellow, 2 - green, 3 - white, 4 - blue, 5 - black

    [Header("StageInfo")]   //�������� ����
    public int maxStageNum; //�ִ� Ŭ������ ��������
    public int selectedStageNum;   //������ ��������

    public List<StageStar> stageStarList;    //�������� Ŭ���� �� ����

    [Header("TurretInfo")]
    public int maxTurretNum;    //�ִ� �ͷ� ����
    public List<int> turretPreset; //���õ� �ͷ� ����Ʈ

    public UserInfo()
    {
        colorWoodResource[0] = 5;
        colorWoodResource[1] = 7;
        colorWoodResource[2] = 10;
        colorWoodResource[3] = 9;
        colorWoodResource[4] = 3;
        colorWoodResource[5] = 2;

        maxStageNum = 3;
        selectedStageNum = 7;

        stageStarList = new List<StageStar>();
        stageStarList.Add(new StageStar());

        maxTurretNum = 4;

        turretPreset = new List<int>();
        turretPreset.Add(3);
        turretPreset.Add(2);
    }

    private void Awake()
    {
        //�� �̵��� �ı� ����, �ߺ� ������Ʈ ���� �����ϰ� ���� �ϵ��� ó��
        var objs = FindObjectsOfType<UserInfo>();
        if (objs.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
