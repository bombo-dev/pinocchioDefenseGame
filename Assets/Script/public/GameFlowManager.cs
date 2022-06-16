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
    //����Ʈ ����
    const int GATENUM = 3;

    public GameObject HPBar;

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
    public int stage;

    //���� �� ��� �ε���
    public int block;


    // Start is called before the first frame update
    void Start()
    {
        //************** ��ȣȭ�� Json������ �ҷ��� �ڷᱸ���� ��� ****************
        // defenseFlowDataList = SystemManager.Instance.LoadJson.PrepareGameFlowJsonData();
        // ***************************************************************************

        //************** ��ȣȭ �Ǿ� ���� ���� Json������ �ҷ��� �ڷᱸ���� ��� **************
        defenseFlowDataList = SystemManager.Instance.GateJson.PrepareGameFlowDecryptJsonData();
        //***************************************************************************************
        //�迭 ������ �ʱ�ȭ
        for (int i = 0; i < GATENUM; i++)
        {
            arrPointer[i] = 0;
            flowTimer[i] = Time.time;
        }

        //�������� ���ѽð� �ʱ�ȭ
        //timer = limitTime;

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
                //UpdateTimer();
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
            if (defenseFlowDataList.datas[stage].defenseFlowDataArr[i].targetPointIndexArr.Length <= 0)
                continue;

            if(Time.time - flowTimer[i] > defenseFlowDataList.datas[stage].defenseFlowDataArr[i].timeFlowIndexArr[arrPointer[i]])
            {
                //Enemy Ȱ��ȭ
                SystemManager.Instance.EnemyManager.EnableEnemy(defenseFlowDataList.datas[stage].defenseFlowDataArr[i].enemyFlowIndexArr[arrPointer[i]]
                                                                         , i, defenseFlowDataList.datas[stage].defenseFlowDataArr[i].targetPointIndexArr);

                //GameObject go = SystemManager.Instance.EnemyManager.enemies[defenseFlowDataList.datas[stage].defenseFlowDataArr[i].enemyFlowIndexArr[arrPointer[i]]];
                //SystemManager.Instance.PanelManager.EnablePanel<StatusMngPanel>(3, go.transform.position);
                
                //HPBar = SystemManager.Instance.PanelManager.statusMngPanel.gameObject;
                //Enemy enemy = go.GetComponent<Enemy>();
                //enemy.UpdateEnemyPos(HPBar);

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

        }//end of for   

    }

    
    /*
    float timer;
    float limitTime = 30;
    bool needFlowTimer = true;

    /// <summary>
    ///  ���������� ���� �ð� �߰�
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
            Debug.Log("���� Ŭ����!");
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
        Debug.Log("���� �Ͻ�����");
    }
    */
}
