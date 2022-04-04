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
    //����Ʈ ����
    const int GATENUM = 3;
    
    enum GameState
    {
        Start,   //���ӽ���
        Defense, //
        End    //�̵�X, ��Ȱ��ȭ ó��
    }
    [SerializeField]
    GameState gameState = GameState.Start;

    //�迭 ������
    int[] arrPointer = new int[GATENUM];

    //Ÿ�̸�
    float flowTimer;


    //���潺 ������ �帧 ���� Data 
    public DefenseFlowData[] defenseFlowDatas;

    

    // Start is called before the first frame update
    void Start()
    {
        //�迭 ������ �ʱ�ȭ
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
                //Enemy Ȱ��ȭ
                SystemManager.Instance.EnemyManager.EnableEnemy(defenseFlowDatas[i].enemyFlowIndexArr[arrPointer[i]], i, defenseFlowDatas[i].targetTileIndexArr);

                //������ �ε���
                if (arrPointer[i] < defenseFlowDatas[i].enemyFlowIndexArr.Length - 1)
                {

                }
                else
                {
                    //�迭 ������ ����
                    arrPointer[i]++;

                    //Ÿ�̸� �ʱ�ȭ
                    flowTimer = Time.time;
                }
            }

        }
        
            //gate1
            //SystemManager.Instance.EnemyManager.EnableEnemy(,1)
    }
}
