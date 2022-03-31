using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum EnemyState
    {
        Walk,   //필드의 포인터를 향해 이동
        Attack, //터렛을 공격
        Dead    //이동X, 비활성화 처리
    }
    [SerializeField]
    EnemyState enemyState = EnemyState.Walk;

    //Actor State
    [SerializeField]
    int maxHP;   //최대 체력

    [SerializeField]
    int power;  // 공격력

    [SerializeField]
    int attackSpeed;    //공격속도

    [SerializeField]
    int range;  // 사거리

    [SerializeField]
    int regeneration;   // 회복력

    [SerializeField]
    int speed;  //이동속도


    //이동 관련
    [SerializeField]
    GameObject[] targetTile;    //타일맵 위에 있는 이동 타겟

    int targetTileIndex = 0;    //타일맵 타겟 인덱스

    GameObject currentTarget;   //현재 타겟

    Vector3 dirVec; //이동처리할 방향벡터



    //공격관련
    float attackTimer = 0;  //공격시간 타이머

    [SerializeField]
    Animator enemyAnimator; //애니메이터

    [SerializeField]
    GameObject attackTarget;    //공격할 타겟

    [SerializeField]
    Vector3 attackDirVec;   //공격할 타겟의 방향벡터



    //test
    [SerializeField]
    GameObject[] turret;

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

    /// <summary>
    /// this객체의 위치에서 target으로의 방향벡터를 구해 반환 : 김현진
    /// </summary>
    /// <param name="target">도착점이 될 타겟</param>
    /// <returns>연산한 방향벡터</returns>
    Vector3 FindDirVec(GameObject target)
    {
        if (target == null)
            return Vector3.zero;

        Vector3 dirVec = Vector3.zero;
        dirVec = target.transform.position - transform.position;
        dirVec.Normalize();
        return dirVec;
    }

    /// <summary>
    /// 실시간 this객체의 상태별 동작 함수 호출
    /// </summary>
    void UpdateEnemy()
    {
        switch (enemyState)
        {
            case EnemyState.Walk:
                CheckArrive();
                UpdateMove(dirVec);
                DetectTurret();
                break;
            case EnemyState.Attack:
                CheckFinAttack();
                break;

        }
    }

    #region Walk - 이동및 적 감지

    /// <summary>
    /// 목표 도착점에 도착 했는지 확인 : 김현진
    /// </summary>
    void CheckArrive()
    {
        //예외처리
        if (currentTarget == null)
            return;
        if (targetTileIndex >= targetTile.Length - 1)
            return;

            //타겟에 도착하지 않았을 경우
            if (Vector3.Distance(transform.position, currentTarget.transform.position) > 0.5f)
        {
            float rotY = Mathf.Round(transform.localEulerAngles.y);
            //예외처리, 속도가 빨라 distance로 감지하지 못했을 경우 방향별 예외처리
            if (!((rotY == 0f && transform.position.z < currentTarget.transform.position.z)//전진
                || (rotY == 90f && transform.position.x < currentTarget.transform.position.x)//오른쪽
                || (rotY == 180f && transform.position.z > currentTarget.transform.position.z)//후진
                || (rotY == 270f && transform.position.x > currentTarget.transform.position.x)))//왼쪽
            {
                return;
            }
        }

        //이동 타겟 변경
        transform.position = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
        
        currentTarget = targetTile[++targetTileIndex];

        dirVec = FindDirVec(currentTarget);
    }

    /// <summary>
    /// 방향 벡터로 실시간 this객체의 위치및 회전을 변경: 김현진 
    /// </summary>
    /// <param name="dirVec">이동및 회전할 방향벡터</param>
    void UpdateMove(Vector3 dirVec)
    {
        //예외처리
        if (dirVec == Vector3.zero)
            return;

        Vector3 updateVec = new Vector3(dirVec.x * speed * Time.deltaTime, 0 , dirVec.z * speed * Time.deltaTime);
        transform.position += updateVec;
        Quaternion rotation = Quaternion.LookRotation(-dirVec);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
    }

    /// <summary>
    /// this객체의 사거리 안에있는 터렛을 감지해 그중 공격할 타겟을 지정 : 김현진
    /// </summary>
    void DetectTurret()
    {
        for (int i = 0 ; i < turret.Length; i++)
        {
           // Debug.Log(i.ToString() + " : " + Vector3.Distance(transform.position, turret[i].transform.position));
            //사거리 안에 가장 먼저 감지된 터렛
            if (turret[i].activeSelf && Vector3.Distance(transform.position, turret[i].transform.position) < range)
            {
                //타겟과 타겟 방향벡터 초기화
                attackTarget = turret[i];
                attackDirVec = (attackTarget.transform.position - transform.position).normalized;

                //공격
                Attack();

                return;
            }
        }
    }

    #endregion

    #region Attack - 감지한 적 공격

    /// <summary>
    /// this객체의 상태를 공격으로 변경 : 김현진
    /// </summary>
    void Attack()
    {
        //공격 상태로 변경 
        this.enemyState = EnemyState.Attack;

        //공격시간 측정 변수 초기화
        attackTimer = Time.time;

        //공격 
        enemyAnimator.SetBool("attack",true);
        enemyAnimator.SetBool("finAttack", false);
    }

    /// <summary>
    /// 실시간으로 공격이 끝났는지 안끝났는지를 판별하고 끝났을경우 
    /// 다음 공격으로 이행할지 다른 상태로 변경할지를 결정 : 김현진
    /// </summary>
    void CheckFinAttack()
    {
        //공격할 대상의 방향으로 회전
        Quaternion rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);

        //attackSpeed초에 1번 공격
        if (Time.time - attackTimer > attackSpeed)
        {
            //공격할 대상의 존재 유무에 따른 상태 변화
            if (attackTarget == null || !(attackTarget.activeSelf))
            {
                enemyState = EnemyState.Walk;

                //공격 종료
                enemyAnimator.SetBool("finAttack", true);
            }
            else
            {
                //공격시간 측정 변수 초기화
                attackTimer = Time.time;

                //다음 공격
                enemyAnimator.SetBool("attack", true);
            }
        }
    }

    #endregion
}
