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
[System.Serializable]
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

    [Header("Option")]
    public float bgSoundVolume;
    public bool isBgSound;
    public float efSoundVolume;
    public bool isEfSound;

    public UserInfo()
    {
        colorWoodResource[0] = 0;
        colorWoodResource[1] = 0;
        colorWoodResource[2] = 0;
        colorWoodResource[3] = 0;
        colorWoodResource[4] = 0;
        colorWoodResource[5] = 0;

        maxStageNum = 0;
        selectedStageNum = 0;

        stageStarList = new List<StageStar>();
        stageStarList.Add(new StageStar());

        maxTurretNum = 0;

        turretPreset = new List<int>();

        bgSoundVolume = 50;
        isBgSound = true;
        efSoundVolume = 50;
        isEfSound = true;
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
