using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenseFlowData
{
    int gateNum;
    public int[] targetTileIndexArr;
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
    int stage;


    // Start is called before the first frame update
    void Start()
    {
        //Json데이터 불러와 자료구조와 사상
        defenseFlowDataList = SystemManager.Instance.LoadJson.PrepareGameFlowJsonData();

        //배열 포인터 초기화
        for (int i = 0; i < GATENUM; i++)
        {
            arrPointer[i] = 0;
            flowTimer[i] = Time.time;
        }

        //스테이지 정보
        stage = 0;
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
            if(Time.time - flowTimer[i] > defenseFlowDataList.datas[stage].defenseFlowDataArr[i].timeFlowIndexArr[arrPointer[i]])
            {
                //Enemy 활성화
                SystemManager.Instance.EnemyManager.EnableEnemy(defenseFlowDataList.datas[stage].defenseFlowDataArr[i].enemyFlowIndexArr[arrPointer[i]]
                                                                         , i, defenseFlowDataList.datas[stage].defenseFlowDataArr[i].targetTileIndexArr);

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

        }   

    }
}
