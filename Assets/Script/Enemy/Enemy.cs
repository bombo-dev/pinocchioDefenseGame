using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Actor
    [SerializeField]
    int maxHP;   //최대 체력
    [SerializeField]
    int power;  // 공격력
    [SerializeField]
    int range;  // 사거리
    [SerializeField]
    int regeneration;   // 회복력

    [SerializeField]
    int speed;  //이동속도

    [SerializeField]
    GameObject[] targetTile;    //타일맵 위에 있는 이동 타겟
    int targetTileIndex = 0;    //타일맵 타겟 인덱스
    GameObject currentTarget;   //현재 타겟
    Vector3 dirVec; //이동처리할 방향벡터

    enum EnemyState
    { 
        Walk,   //필드의 포인터를 향해 이동
        Ready,  //터렛을 감지하여 적을 향해 이동
        Attack, //터렛을 공격
        Dead    //이동X, 비활성화 처리
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
    /// 목표 위치에 도착 했는지 확인 : 김현진
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
