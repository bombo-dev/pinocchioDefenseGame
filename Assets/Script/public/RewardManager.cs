using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    //강화 나무 보상 리스트
    public int[] colorWoodReward = new int[6];

    //스테이지별 터렛 보상 종류
    public Dictionary<int, int> turretReward = new Dictionary<int, int>();  //스테이지, 터렛 종류

    //별 보상
    public int starRewardNum;

    //중복되지 않은 터렛보상을 얻었을때 true
    public bool getNewTurret = false;

    // Start is called before the first frame update
    void Start()
    {
        SetReward();
    }

    /// <summary>
    /// 해당 스테이지 보상 설정 : 김현진
    /// </summary>
    void SetReward()
    {
        //게임플로우 매니저
        GameFlowManager gfm = SystemManager.Instance.GameFlowManager;

        //배열 초기화
        colorWoodReward.Initialize();

        //TurretReward세팅
        SetTurretRewardInfo();

        //랜덤 WoodReward세팅

        //튜토리얼 첫 클리어 보상
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
                //0~4번 나무는 스테이지 수만큼 랜덤 보상
                colorWoodReward[Random.Range(0, 5)]++;

                //10%확률로 추가보상
                if (Random.Range(0, 10) == 1)
                    colorWoodReward[5]++;
            }
        }
        

        //이미 클리어한 스테이지일 경우 보상 3/1
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        if (!userInfo)
            return;

        //이미 클리어한 스테이지일 경우
        if (gfm.stage < userInfo.maxStageNum)
        {
            if (gfm.stage == 0) //튜토리얼
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
    /// 스테이지별 터렛 보상 정보 : 김현진
    /// </summary>
    void SetTurretRewardInfo()
    {
        turretReward.Add(1, 2);//딱다구리
        turretReward.Add(2, 3);//베이비버드
        turretReward.Add(3, 4);//강화비둘기
        turretReward.Add(4, 5);//펠리컨
        turretReward.Add(5, 6);//시계
        turretReward.Add(7, 7);//펭귄
        turretReward.Add(11, 8);//부엉
        turretReward.Add(13, 9);//강화 딱다구리
        turretReward.Add(15, 10);//매
        turretReward.Add(18, 11);//독수리
        turretReward.Add(21, 12);//강화 아기새
        turretReward.Add(23, 13);//강화 독수리
        turretReward.Add(25, 14);//초강화 비둘기


    }

    /// <summary>
    /// 스테이지별 별 보상 정보 : 김현진
    /// </summary>
    /// <returns>별 갯수</returns>
    public void SetStarReward()
    {
        Turret turret = SystemManager.Instance.TurretManager.baseTurret;

        //베이스터렛 남은 HP비율 계산
        int baseHP = (int)(((float)turret.currentHP / (float)turret.maxHP) * 100);

        if (baseHP < 40)
            starRewardNum = 1;
        else if (baseHP < 100)
            starRewardNum = 2;
        else
            starRewardNum = 3;
    }
}
