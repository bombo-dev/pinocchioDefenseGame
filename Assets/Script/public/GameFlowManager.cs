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

    //���潺 ������ �帧 ���� Data �迭 ����Ʈ
    DefenseFlowDataList defenseFlowDataList;

    //�迭 ������
    int[] arrPointer = new int[GATENUM];

    //Ÿ�̸�
    float[] flowTimer = new float[GATENUM];

    //���� �������� �ε���
    int stage;


    // Start is called before the first frame update
    void Start()
    {
        //Json������ �ҷ��� �ڷᱸ���� ���
        defenseFlowDataList = SystemManager.Instance.LoadJson.PrepareGameFlowJsonData();

        //�迭 ������ �ʱ�ȭ
        for (int i = 0; i < GATENUM; i++)
        {
            arrPointer[i] = 0;
            flowTimer[i] = Time.time;
        }

        //�������� ����
        stage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGame();
    }

    /// <summary>
    /// �ǽð� ���� ���� ���� �� ���� : ������
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
    /// ���� �ð����� Enemy�� Ȱ�����ش� : ������
    /// </summary>
    void UpdateDefense()
    {
        //Gate 1~3
        for (int i = 0; i < GATENUM; i++)
        {
            if(Time.time - flowTimer[i] > defenseFlowDataList.datas[stage].defenseFlowDataArr[i].timeFlowIndexArr[arrPointer[i]])
            {
                //Enemy Ȱ��ȭ
                SystemManager.Instance.EnemyManager.EnableEnemy(defenseFlowDataList.datas[stage].defenseFlowDataArr[i].enemyFlowIndexArr[arrPointer[i]]
                                                                         , i, defenseFlowDataList.datas[stage].defenseFlowDataArr[i].targetTileIndexArr);

                //������ �ε���
                if (arrPointer[i] >= defenseFlowDataList.datas[stage].defenseFlowDataArr[i].enemyFlowIndexArr.Length - 1)
                {
                    gameState = GameState.End;
                }
                else
                {
                    //�迭 ������ ����
                    arrPointer[i]++;

                    //Ÿ�̸� �ʱ�ȭ
                    flowTimer[i] = Time.time;
                }
            }

        }   

    }
}
