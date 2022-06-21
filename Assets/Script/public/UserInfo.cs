using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageStar
{
    public int stageNum;    //스테이지
    public int starNum;  //클리어 별

    public StageStar()
    {
        stageNum = 0;
        starNum = 0;
    }
}
public class UserInfo : MonoBehaviour
{
    [Header("WoodIResourceInfo")]    //강화 나무 자원
    public int[] colorWoodResource = new int[6]; //0 - red, 1- yellow, 2 - green, 3 - white, 4 - blue, 5 - black

    [Header("StageInfo")]   //스테이지 정보
    public int maxStageNum; //최대 클리어한 스테이지
    public int selectedStageNum;   //선택한 스테이지

    public List<StageStar> stageStarList;    //스테이지 클리어 별 정보

    [Header("TurretInfo")]
    public int maxTurretNum;    //최대 터렛 숫자
    public List<int> turretPreset; //선택된 터렛 리스트

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
        //씬 이동시 파괴 방지, 중복 오브젝트 없이 유일하게 존재 하도록 처리
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
