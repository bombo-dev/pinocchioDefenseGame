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
    //�ִ� ��������
    public int MAXSTAGE;

    //����Ʈ ����
    const int GATENUM = 3;

    public GameObject HPBar;

    public enum GameState
    {
        Start,   //���ӽ���
        Defense, //
        StageClear,    //�������� Ŭ����
        StageFail,   //�������� ����
        StageEnd    //�������� ����
    }
    [SerializeField]
    public GameState gameState = GameState.Start;

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

    //�ð� ������ ����
    public float stageTime;

    //���̺� ���� ���� 
    bool finWave = false;
    
    //�����м� ��ųʸ�
    public Dictionary<int,int> turretBattleAnalysisDic = new Dictionary<int,int>(); //�ͷ���ȣ / ������
    public Dictionary<int, int> turretSummonAnalysisDic = new Dictionary<int, int>(); //�ͷ���ȣ / ��ȯ����

    // Start is called before the first frame update
    void Start()
    {
        //�������� �ҷ�����
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //�������� ����
        stage = userInfo.selectedStageNum;

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

        //�ð� �ʱ�ȭ
        stageTime = 0;

        //�����м� �ʱ�ȭ
        turretBattleAnalysisDic.Clear();

        //���̺� �ʱ�ȭ
        finWave = false;
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
                    finWave = true;
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

    /// <summary>
    /// �ͷ� �������� ������ �߻��� ���� ��� : ������
    /// </summary>
    /// <param name="damage">�ͷ��� �� ������</param>
    /// <param name="turretNum">�ͷ� ��ȣ</param>
    public void AnalyzeTurretBattle(int damage, int turretNum)
    {
        //������-�ͷ� �����м� ��ųʸ��� ������ ����
        if (turretBattleAnalysisDic.ContainsKey(turretNum))
            turretBattleAnalysisDic[turretNum] += damage;
        else
            turretBattleAnalysisDic.Add(turretNum, damage);
    }

    /// <summary>
    /// �ͷ� �Ǽ� ���� ��� : ������
    /// </summary>
    /// <param name="turretNum">�ͷ� ��ȣ</param>
    public void AnalyzeTurretSummon(int turretNum)
    {
        //������-�ͷ� �����м� ��ųʸ��� ������ ����
        if (turretSummonAnalysisDic.ContainsKey(turretNum))
            turretSummonAnalysisDic[turretNum] += 1;
        else
            turretSummonAnalysisDic.Add(turretNum, 1);
    }

    /// <summary>
    /// ��� ���� �������� ��� �������� Ŭ���� ó�� : ������
    /// </summary>
    void ChkClear()
    {
        if (SystemManager.Instance.EnemyManager.enemies.Count <= 0)
        {
            //ĳ��
            UserInfo userInfo = SystemManager.Instance.UserInfo;
            RewardManager rewardManager = SystemManager.Instance.RewardManager;

            //�� ���� ����
            rewardManager.SetStarReward();

            //Ŭ���� ���·� ����
            gameState = GameState.StageClear;

            //-------UserInfo�� �������� �ѱ��

            //�ͷ� ���� ���� �����ϸ� �߰�
            if (rewardManager.turretReward.ContainsKey(stage) &&
                userInfo.maxTurretNum < rewardManager.turretReward[stage])
            {
                userInfo.maxTurretNum = rewardManager.turretReward[stage];
                rewardManager.getNewTurret = true;

                //������ �������� �־��ֱ�
                if(userInfo.turretPreset.Count < 8)
                    userInfo.turretPreset.Add(rewardManager.turretReward[stage] - 1);
            }

            //��ȭ ���� ���� �߰�
            for (int i = 0; i < userInfo.colorWoodResource.Length; i++)
            {
                userInfo.colorWoodResource[i] += rewardManager.colorWoodReward[i];
            }

            //�� ���� �߰�
            if (userInfo.stageStarList[stage].starNum < rewardManager.starRewardNum)
            {
                userInfo.stageStarList[stage].starNum = rewardManager.starRewardNum;
            }

            //�ִ� ���������� �������� ������ ��
            if (stage < MAXSTAGE)
            {
                //�������� ���� ����
                if (userInfo.maxStageNum <= stage + 1)
                    userInfo.selectedStageNum = stage + 1;

                //�������� Ŭ���� ����
                if (userInfo.maxStageNum <= stage)
                    userInfo.maxStageNum = stage + 1;

                //���� �������� �� ���� �߰�
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

            //�������� ��Ȱ��ȭ
            DisableStage();
        }
    }

    //�������� ���� ���� ������Ʈ,�г� ��Ȱ��ȭ : ������
    public void DisableStage()
    {
        //�̱��� ĳ��
        PanelManager pm = SystemManager.Instance.PanelManager;

        //���Ӱ�� �г� ����
        pm.EnablePanel<UI_StageEndPanel>(10);

        //Ÿ�ӽ����� �ʱ�ȭ
        Time.timeScale = 1.0f;  //Ÿ�ӽ����� ����

        //FixedDeltaTime����
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        //��ġ ���Ƴ���
        SystemManager.Instance.InputManager.enabled = false;

        //�г� ��Ȱ��ȭ
        pm.DisablePanel<UI_TurretMngPanel>(pm.turretMngPanel.gameObject);
        pm.DisablePanel<UI_TurretInfoPanel>(pm.turretInfoPanel.gameObject);
        pm.DisablePanel<UI_ResourcePanel>(pm.resoursePanel.gameObject);
        if(pm.optionPanel)  //Ʃ�丮���� ��� �ɼ��г� �������� �ʴ´�
            pm.optionPanel.DisablePanelFinStage();

        //��� Enemy����
        for (int i = 0; i < SystemManager.Instance.EnemyManager.enemies.Count; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(
                SystemManager.Instance.EnemyManager.enemies[i].GetComponent<Enemy>().filePath,
                SystemManager.Instance.EnemyManager.enemies[i]);
        }

        //��� Bullet����
        SystemManager.Instance.BulletManager.enemyParents.gameObject.SetActive(false);
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
