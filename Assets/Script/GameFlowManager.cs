using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    enum GameState
    {
        Start,   //게임시작
        Defense, //
        End    //이동X, 비활성화 처리
    }
    [SerializeField]
    GameState gameState = GameState.Start;

    //배열 포인터
    int arrPointer1;
    int arrPointer2;
    int arrPointer3;

    //타이머
    float flowTimer;


    //Enemy가 따라갈 타일
    public int[] targetTileIndexArr1;
    public int[] targetTileIndexArr2;
    public int[] targetTileIndexArr3;

    //Enemy가 생성되는 시간
    public int[] timeFlowIndexArr1;
    public int[] timeFlowIndexArr2;
    public int[] timeFlowIndexArr3;

    //생성되는 Enemy의 종류
    public int[] enemyFlowIndexArr1;
    public int[] enemyFlowIndexArr2;
    public int[] enemyFlowIndexArr3;

    

    // Start is called before the first frame update
    void Start()
    {
        //배열 포인터 초기화
        arrPointer1 = 0;
        arrPointer2 = 0;
        arrPointer3 = 0;
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
        if (Time.time - flowTimer > timeFlowIndexArr1[arrPointer1])
        {
            
        }
            //gate1
            //SystemManager.Instance.EnemyManager.EnableEnemy(,1)
    }
}
