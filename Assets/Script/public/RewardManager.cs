using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    //강화 나무 보상 리스트
    public int[] colorWoodReward = new int[6];

    //스테이지별 터렛 보상 종류
    [SerializeField]
    Dictionary<int, int> turretReward = new Dictionary<int, int>();  //스테이지, 터렛 종류

    //별 보상
    public int starRewardNum;

    // Start is called before the first frame update
    void Start()
    {
        SetResward();
    }

    /// <summary>
    /// 해당 스테이지 보상 설정 : 김현진
    /// </summary>
    void SetResward()
    {
        //게임플로우 매니저
        GameFlowManager gfm = SystemManager.Instance.GameFlowManager;

        //배열 초기화
        colorWoodReward.Initialize();

        //TurretReward세팅
        SetTurretRewardInfo();

        //랜덤 WoodReward세팅
        for (int i = 0; i < gfm.stage; i++)
        {
            //0~4번 나무는 스테이지 수만큼 랜덤 보상
            colorWoodReward[Random.Range(0, 5)]++;

            //10%확률로 추가보상
            if (Random.Range(0, 10) == 1)
                colorWoodReward[5]++;
        }

        //이미 클리어한 스테이지일 경우 보상 3/1
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        if (!userInfo)
            return;
         
        //이미 클리어한 스테이지일 경우
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
    /// 스테이지별 터렛 보상 정보 : 김현진
    /// </summary>
    void SetTurretRewardInfo()
    {
        turretReward.Add(2, 2);//딱다구리
        turretReward.Add(3, 3);//베이비버드
        turretReward.Add(4, 4);//강화비둘기
        turretReward.Add(5, 5);//펠리컨
    }

    /// <summary>
    /// 스테이지별 별 보상 정보 : 김현진
    /// </summary>
    /// <returns>별 갯수</returns>
    public void SetStarReward()
    {
        Turret turret = SystemManager.Instance.TurretManager.baseTurret;

        //베이스터렛 남은 HP비율 계싼
        int baseHP = (int)(((float)turret.currentHP / (float)turret.maxHP) * 100);

        if (baseHP < 40)
            starRewardNum = 1;
        else if (baseHP < 100)
            starRewardNum = 2;
        else
            starRewardNum = 3;
    }
}
