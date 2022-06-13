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
    //게이트 숫자
    const int GATENUM = 3;

    public GameObject HPBar;

    enum GameState
    {
        Start,   //게임시작
        Defense, //
        End    //이동X, 비활성화 처리
    }
    [SerializeField]
    GameState gameState = GameState.Start;

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


    // Start is called before the first frame update
    void Start()
    {
        //************** 암호화된 Json데이터 불러와 자료구조와 사상 ****************
        // defenseFlowDataList = SystemManager.Instance.LoadJson.PrepareGameFlowJsonData();
        // ***************************************************************************

        //************** 암호화 되어 있지 않은 Json데이터 불러와 자료구조와 사상 **************
        defenseFlowDataList = SystemManager.Instance.LoadJson.PrepareGameFlowDecryptJsonData();
        //***************************************************************************************
        //배열 포인터 초기화
        for (int i = 0; i < GATENUM; i++)
        {
            arrPointer[i] = 0;
            flowTimer[i] = Time.time;
        }

        //스테이지 제한시간 초기화
        //timer = limitTime;

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
                UpdateDefense();                                    
                //UpdateTimer();
                break;
            case GameState.End:
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
                    gameState = GameState.End;
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
