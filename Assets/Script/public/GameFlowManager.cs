using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DefenseFlowData
{
    int gateNum;
    public int[] targetPointIndexArr;
    public int[] timeFlowIndexArr;
    public int[] enemyFlowIndexArr;
}
[System.Serializable]
public class DefenseFlowDataArr
{
    public int stageNum;
    public DefenseFlowData[] defenseFlowDataArr;
}
[System.Serializable]
public class DefenseFlowDataList
{
    public List<DefenseFlowDataArr> datas;
}

public class GameFlowManager : MonoBehaviour
{
    //최대 스테이지
    public int MAXSTAGE;

    //게이트 숫자
    const int GATENUM = 3;

    public GameObject HPBar;

    public enum GameState
    {
        Start,   //게임시작
        Defense, //
        StageClear,    //스테이지 클리어
        StageFail,   //스테이지 실패
        StageEnd    //스테이지 종료
    }
    [SerializeField]
    public GameState gameState = GameState.Start;

    //디펜스 페이지 흐름 관련 Data 배열 리스트
    DefenseFlowDataList defenseFlowDataList;

    //배열 포인터
    int[] arrPointer = new int[GATENUM];

    //타이머
    float[] flowTimer = new float[GATENUM];

    //게임 스테이지 인덱스
    public int stage;

    //게임 맵 블록 인덱스
    public int block;

    //시간 측정용 변수
    public float stageTime;

    //웨이브 종료 여부 
    bool finWave = false;
    
    //전투분석 딕셔너리
    public Dictionary<int,int> turretBattleAnalysisDic = new Dictionary<int,int>(); //터렛번호 / 데미지
    public Dictionary<int, int> turretSummonAnalysisDic = new Dictionary<int, int>(); //터렛번호 / 소환개수

    // Start is called before the first frame update
    void Start()
    {
        //유저정보 불러오기
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //스테이지 설정
        stage = userInfo.selectedStageNum;

        //************** 암호화된 Json데이터 불러와 자료구조와 사상 ****************
        // defenseFlowDataList = SystemManager.Instance.LoadJson.PrepareGameFlowJsonData();
        // ***************************************************************************

        //************** 암호화 되어 있지 않은 Json데이터 불러와 자료구조와 사상 **************
        defenseFlowDataList = SystemManager.Instance.GateJson.PrepareGameFlowDecryptJsonData();
        //***************************************************************************************
        //배열 포인터 초기화
        for (int i = 0; i < GATENUM; i++)
        {
            arrPointer[i] = 0;
            flowTimer[i] = Time.time;
        }

        //시간 초기화
        stageTime = 0;

        //전투분석 초기화
        turretBattleAnalysisDic.Clear();

        //웨이브 초기화
        finWave = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGame();
    }

    /// <summary>
    /// 실시간 게임 진행 상태 별 동작 : 김현진
    /// </summary>
    void UpdateGame()
    {
        switch (gameState)
        {
            case GameState.Start:
                break;
            case GameState.Defense:
                stageTime += Time.deltaTime;
                if (!finWave)
                    UpdateDefense();
                else
                    ChkClear();
                //UpdateTimer();
                break;
            case GameState.StageClear:
                break;
            case GameState.StageFail:
                break;
            case GameState.StageEnd:
                break;
        }
    }

    /// <summary>
    /// 일정 시간마다 Enemy를 활성해준다 : 김현진
    /// </summary>
    void UpdateDefense()
    {
        //Gate 1~3
        for (int i = 0; i < GATENUM; i++)
        {
            if (defenseFlowDataList.datas[stage].defenseFlowDataArr[i].targetPointIndexArr.Length <= 0)
                continue;

            if(Time.time - flowTimer[i] > defenseFlowDataList.datas[stage].defenseFlowDataArr[i].timeFlowIndexArr[arrPointer[i]])
            {
                //Enemy 활성화
                SystemManager.Instance.EnemyManager.EnableEnemy(defenseFlowDataList.datas[stage].defenseFlowDataArr[i].enemyFlowIndexArr[arrPointer[i]]
                                                                         , i, defenseFlowDataList.datas[stage].defenseFlowDataArr[i].targetPointIndexArr);

                //GameObject go = SystemManager.Instance.EnemyManager.enemies[defenseFlowDataList.datas[stage].defenseFlowDataArr[i].enemyFlowIndexArr[arrPointer[i]]];
                //SystemManager.Instance.PanelManager.EnablePanel<StatusMngPanel>(3, go.transform.position);
                
                //HPBar = SystemManager.Instance.PanelManager.statusMngPanel.gameObject;
                //Enemy enemy = go.GetComponent<Enemy>();
                //enemy.UpdateEnemyPos(HPBar);

                //마지막 인덱스
                if (arrPointer[i] >= defenseFlowDataList.datas[stage].defenseFlowDataArr[i].enemyFlowIndexArr.Length - 1)
                {
                    finWave = true;
                }
                else
                {
                    //배열 포인터 증가
                    arrPointer[i]++;

                    //타이머 초기화
                    flowTimer[i] = Time.time;
                }
            }

        }//end of for   

    }

    /// <summary>
    /// 터렛 공격으로 데미지 발생시 정보 기록 : 김현진
    /// </summary>
    /// <param name="damage">터렛이 준 데미지</param>
    /// <param name="turretNum">터렛 번호</param>
    public void AnalyzeTurretBattle(int damage, int turretNum)
    {
        //데미지-터렛 전투분석 딕셔너리에 데이터 갱신
        if (turretBattleAnalysisDic.ContainsKey(turretNum))
            turretBattleAnalysisDic[turretNum] += damage;
        else
            turretBattleAnalysisDic.Add(turretNum, damage);
    }

    /// <summary>
    /// 터렛 건설 정보 기록 : 김현진
    /// </summary>
    /// <param name="turretNum">터렛 번호</param>
    public void AnalyzeTurretSummon(int turretNum)
    {
        //데미지-터렛 전투분석 딕셔너리에 데이터 갱신
        if (turretSummonAnalysisDic.ContainsKey(turretNum))
            turretSummonAnalysisDic[turretNum] += 1;
        else
            turretSummonAnalysisDic.Add(turretNum, 1);
    }

    /// <summary>
    /// 모든 적을 섬멸했을 경우 스테이지 클리어 처리 : 김현진
    /// </summary>
    void ChkClear()
    {
        if (SystemManager.Instance.EnemyManager.enemies.Count <= 0)
        {
            //캐싱
            UserInfo userInfo = SystemManager.Instance.UserInfo;
            RewardManager rewardManager = SystemManager.Instance.RewardManager;

            //별 보상 설정
            rewardManager.SetStarReward();

            //클리어 상태로 변경
            gameState = GameState.StageClear;

            //-------UserInfo에 보상정보 넘기기

            //터렛 보상 정보 존재하면 추가
            if (rewardManager.turretReward.ContainsKey(stage) &&
                userInfo.maxTurretNum < rewardManager.turretReward[stage])
            {
                userInfo.maxTurretNum = rewardManager.turretReward[stage];
                rewardManager.getNewTurret = true;

                //프리셋 비었을경우 넣어주기
                if(userInfo.turretPreset.Count < 8)
                    userInfo.turretPreset.Add(rewardManager.turretReward[stage] - 1);
            }

            //강화 나무 보상 추가
            for (int i = 0; i < userInfo.colorWoodResource.Length; i++)
            {
                userInfo.colorWoodResource[i] += rewardManager.colorWoodReward[i];
            }

            //별 보상 추가
            if (userInfo.stageStarList[stage].starNum < rewardManager.starRewardNum)
            {
                userInfo.stageStarList[stage].starNum = rewardManager.starRewardNum;
            }

            //최대 스테이지에 도달하지 못했을 때
            if (stage < MAXSTAGE)
            {
                //스테이지 선택 정보
                if (userInfo.maxStageNum <= stage + 1)
                    userInfo.selectedStageNum = stage + 1;

                //스테이지 클리어 정보
                if (userInfo.maxStageNum <= stage)
                    userInfo.maxStageNum = stage + 1;

                //다음 스테이지 별 보상 추가
                if (userInfo.stageStarList.Count <= stage + 1)
                {
                    StageStar stageStar = new StageStar();
                    stageStar.stageNum = stage + 1;
                    stageStar.starNum = 0;

                    userInfo.stageStarList.Add(stageStar);
                }
            }

            // UserInfo Save
            SaveLoad save = new SaveLoad();
            save.SaveUserInfo();

            //스테이지 비활성화
            DisableStage();
        }
    }

    //스테이지 종료 관련 오브젝트,패널 비활성화 : 김현진
    public void DisableStage()
    {
        //싱글톤 캐싱
        PanelManager pm = SystemManager.Instance.PanelManager;

        //게임결과 패널 생성
        pm.EnablePanel<UI_StageEndPanel>(10);

        //타임스케일 초기화
        Time.timeScale = 1.0f;  //타임스케일 변경

        //FixedDeltaTime변경
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        //터치 막아놓기
        SystemManager.Instance.InputManager.enabled = false;

        //패널 비활성화
        pm.DisablePanel<UI_TurretMngPanel>(pm.turretMngPanel.gameObject);
        pm.DisablePanel<UI_TurretInfoPanel>(pm.turretInfoPanel.gameObject);
        pm.DisablePanel<UI_ResourcePanel>(pm.resoursePanel.gameObject);
        if(pm.optionPanel)  //튜토리얼일 경우 옵션패널 존재하지 않는다
            pm.optionPanel.DisablePanelFinStage();

        //모든 Enemy제거
        for (int i = 0; i < SystemManager.Instance.EnemyManager.enemies.Count; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(
                SystemManager.Instance.EnemyManager.enemies[i].GetComponent<Enemy>().filePath,
                SystemManager.Instance.EnemyManager.enemies[i]);
        }

        //모든 Bullet제거
        SystemManager.Instance.BulletManager.enemyParents.gameObject.SetActive(false);
    }
    
    /*
    float timer;
    float limitTime = 30;
    bool needFlowTimer = true;

    /// <summary>
    ///  스테이지에 제한 시간 추가
    /// </summary>
    void UpdateTimer()
    {
        if (!needFlowTimer)
        {
            PauseTimer();
            return;
        }

        GameObject go = PanelManager.Instance.stageMngPanel.GetTimerText();
        TextMesh timerText = go.GetComponent<TextMesh>();

        if (!timerText)
        {
            Debug.Log("timerText is null");
            return;
        }

        if (timer > 0)
        {
            FlowTimer(timerText);
        }
        else
        {
            timerText.text = "0";
            Debug.Log("게임 클리어!");
            gameState = GameState.End;
        }
    }
    void FlowTimer(TextMesh timerText)
    {
        timer -= Time.deltaTime;        
        timerText.text = timer.ToString();
        Debug.Log("timer= " + timer);
    }

    void PauseTimer()
    {
        Debug.Log("게임 일시정지");
    }
    */
}
