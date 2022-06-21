using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageStar
{
    public int stageNum;    //��������
    public int starNum;  //Ŭ���� ��
}
public class UserInfo : MonoBehaviour
{
    [Header("WoodIResourceInfo")]    //��ȭ ���� �ڿ�
    public int[] colorWoodResource = new int[6]; //0 - red, 1- yellow, 2 - green, 3 - white, 4 - blue, 5 - black

    [Header("StageInfo")]   //�������� ����
    public int maxStageNum; //�ִ� Ŭ������ ��������
    public int selectedStageNum;   //������ ��������

    public StageStar[] stageStarArr;    //�������� Ŭ���� �� ����

    [Header("TurretInfo")]
    public int maxTurretNum;    //�ִ� �ͷ� ����
    public List<int> turretPreset; //���õ� �ͷ� ����Ʈ

    [Header("Option")]
    public float bgSoundVolume;
    public bool isBgSound;
    public float efSoundVolume;
    public bool isEfSound;

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
