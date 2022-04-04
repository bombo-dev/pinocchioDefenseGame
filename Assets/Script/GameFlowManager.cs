using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenseFlowData
{
    public int stageNum;
    public int[] targetTileIndexArr;
    public int[] timeFlowIndexArr;
    public int[] enemyFlowIndexArr;
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

    //배열 포인터
    int[] arrPointer = new int[GATENUM];

    //타이머
    float flowTimer;


    //디펜스 페이지 흐름 관련 Data 
    public DefenseFlowData[] defenseFlowDatas;

    

    // Start is called before the first frame update
    void Start()
    {
        //배열 포인터 초기화
        for (int i = 0; i < GATENUM; i++)
            arrPointer[i] = 0;

        //test
        flowTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGame();
    }

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

    void UpdateDefense()
    {
        //Gate 1~3
        for (int i = 0; i < GATENUM; i++)
        {
            if (Time.time - flowTimer > defenseFlowDatas[i].timeFlowIndexArr[arrPointer[i]])
            {
                //Enemy 활성화
                SystemManager.Instance.EnemyManager.EnableEnemy(defenseFlowDatas[i].enemyFlowIndexArr[arrPointer[i]], i, defenseFlowDatas[i].targetTileIndexArr);

                //마지막 인덱스
                if (arrPointer[i] < defenseFlowDatas[i].enemyFlowIndexArr.Length - 1)
                {

                }
                else
                {
                    //배열 포인터 증가
                    arrPointer[i]++;

                    //타이머 초기화
                    flowTimer = Time.time;
                }
            }

        }
        
            //gate1
            //SystemManager.Instance.EnemyManager.EnableEnemy(,1)
    }
}
