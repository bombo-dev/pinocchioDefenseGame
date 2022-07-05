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
[System.Serializable]
public class UserInfo : MonoBehaviour
{
    [Header("WoodIResourceInfo")]    //강화 나무 자원
    public int[] colorWoodResource = new int[6]; //0 - red, 1- yellow, 2 - green, 3 - white, 4 - blue, 5 - black

    [Header("StageInfo")]   //스테이지 정보
    public int maxStageNum; //최대 클리어한 스테이지
    public int selectedStageNum;   //선택한 스테이지
    public int maxStageNum_hard; //최대 클리어한 스테이지 - 하드
    public int selectedStageNum_hard;   //선택한 스테이지 - 하드
    public int selectMode;  //선택한 모드 0 - 노말, 1 - 하드

    public List<StageStar> stageStarList;    //스테이지 클리어 별 정보
    public List<StageStar> stageStarList_hard;    //스테이지 클리어 별 정보 - 하드

    [Header("TurretInfo")]
    public int maxTurretNum;    //최대 터렛 숫자
    public List<int> turretPreset; //선택된 터렛 리스트

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
    /// 테스트용 생성자
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
