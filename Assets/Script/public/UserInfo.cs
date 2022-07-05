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
    public int maxStageNum_hard; //�ִ� Ŭ������ �������� - �ϵ�
    public int selectedStageNum_hard;   //������ �������� - �ϵ�
    public int selectMode;  //������ ��� 0 - �븻, 1 - �ϵ�

    public List<StageStar> stageStarList;    //�������� Ŭ���� �� ����
    public List<StageStar> stageStarList_hard;    //�������� Ŭ���� �� ���� - �ϵ�

    [Header("TurretInfo")]
    public int maxTurretNum;    //�ִ� �ͷ� ����
    public List<int> turretPreset; //���õ� �ͷ� ����Ʈ

    [Header("Option")]
    public float bgSoundVolume;
    public bool isBgSound;
    public float efSoundVolume;
    public bool isEfSound;
    public int touchSpeed;
    public bool isShowRange;
    public bool isShowBook;

    /*
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
        maxStageNum_hard = 0;
        selectedStageNum_hard = 0;
        selectMode = 0;

        stageStarList = new List<StageStar>();
        stageStarList.Add(new StageStar());

        stageStarList_hard = new List<StageStar>();
        stageStarList_hard.Add(new StageStar());

        maxTurretNum = 1;

        turretPreset = new List<int>();
        turretPreset.Add(0);

        bgSoundVolume = 0.5f;
        isBgSound = true;
        efSoundVolume = 0.5f;
        isEfSound = true;
        touchSpeed = 5;
        isShowRange = true;
        isShowBook = false;
    }*/

    /// <summary>
    /// �׽�Ʈ�� ������
    /// </summary>
    public UserInfo()
    {
        colorWoodResource[0] = 10000;
        colorWoodResource[1] = 10000;
        colorWoodResource[2] = 10000;
        colorWoodResource[3] = 10000;
        colorWoodResource[4] = 10000;
        colorWoodResource[5] = 10000;

        maxStageNum = 40;
        selectedStageNum = 40;
        maxStageNum_hard = 35;
        selectedStageNum_hard = 35;
        selectMode = 0;

        stageStarList = new List<StageStar>();
        for (int i = 0; i < 41; i++)
        {
            stageStarList.Add(new StageStar());
        }

        stageStarList_hard = new List<StageStar>();
        for (int i = 0; i < 36; i++)
        {
            stageStarList_hard.Add(new StageStar());
        }

        maxTurretNum = 23;

        turretPreset = new List<int>();
        turretPreset.Add(0);

        bgSoundVolume = 0.5f;
        isBgSound = true;
        efSoundVolume = 0.5f;
        isEfSound = true;
        touchSpeed = 5;
        isShowRange = true;
        isShowBook = false;
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
