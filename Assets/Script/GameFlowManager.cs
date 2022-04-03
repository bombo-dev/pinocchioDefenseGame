using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    enum GameState
    {
        Start,   //���ӽ���
        Defense, //
        End    //�̵�X, ��Ȱ��ȭ ó��
    }
    [SerializeField]
    GameState gameState = GameState.Start;

    //�迭 ������
    int arrPointer1;
    int arrPointer2;
    int arrPointer3;

    //Ÿ�̸�
    float flowTimer;


    //Enemy�� ���� Ÿ��
    public int[] targetTileIndexArr1;
    public int[] targetTileIndexArr2;
    public int[] targetTileIndexArr3;

    //Enemy�� �����Ǵ� �ð�
    public int[] timeFlowIndexArr1;
    public int[] timeFlowIndexArr2;
    public int[] timeFlowIndexArr3;

    //�����Ǵ� Enemy�� ����
    public int[] enemyFlowIndexArr1;
    public int[] enemyFlowIndexArr2;
    public int[] enemyFlowIndexArr3;

    

    // Start is called before the first frame update
    void Start()
    {
        //�迭 ������ �ʱ�ȭ
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
