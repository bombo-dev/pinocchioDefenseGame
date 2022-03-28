using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Actor
    [SerializeField]
    int maxHP;   //�ִ� ü��
    [SerializeField]
    int power;  // ���ݷ�
    [SerializeField]
    int range;  // ��Ÿ�
    [SerializeField]
    int regeneration;   // ȸ����

    [SerializeField]
    int speed;  //�̵��ӵ�

    [SerializeField]
    GameObject[] targetTile;    //Ÿ�ϸ� ���� �ִ� �̵� Ÿ��
    int targetTileIndex = 0;    //Ÿ�ϸ� Ÿ�� �ε���
    GameObject currentTarget;   //���� Ÿ��
    Vector3 dirVec; //�̵�ó���� ���⺤��

    enum EnemyState
    { 
        Walk,   //�ʵ��� �����͸� ���� �̵�
        Ready,  //�ͷ��� �����Ͽ� ���� ���� �̵�
        Attack, //�ͷ��� ����
        Dead    //�̵�X, ��Ȱ��ȭ ó��
    }
    [SerializeField]
    EnemyState enemyState = EnemyState.Walk;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = targetTile[targetTileIndex];
        dirVec = FindDirVec(currentTarget);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemy();
    }

    void UpdateEnemy()
    {
        switch (enemyState)
        {
            case EnemyState.Walk:
                CheckArrive();
                UpdateMove(dirVec);
                break;
            case EnemyState.Ready:
                break;

        }
    }

    /// <summary>
    /// ��ǥ ��ġ�� ���� �ߴ��� Ȯ�� : ������
    /// </summary>
    void CheckArrive()
    {
        if (Vector3.Distance(transform.position, currentTarget.transform.position) > 0.5f)
            return;
        if (enemyState == EnemyState.Walk)
        {
            currentTarget = targetTile[++targetTileIndex];
            dirVec = FindDirVec(currentTarget);

        }
        else if (enemyState == EnemyState.Ready)
        {

        }
        
    }

    Vector3 FindDirVec(GameObject target)
    {
        if (target == null)
            return Vector3.zero;

        Vector3 dirVec = Vector3.zero;
        dirVec = target.transform.position - transform.position;
        dirVec.Normalize();
        return dirVec;
    }

    void UpdateMove(Vector3 dirVec)
    {
        this.transform.position += dirVec.normalized * speed * Time.deltaTime;
        Quaternion rotation = Quaternion.LookRotation(-dirVec);
        transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, 0.3f);
    }
}
