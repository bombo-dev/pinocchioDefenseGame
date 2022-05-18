using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    enum EnemyState
    {
        Walk,   //필드의 포인터를 향해 이동
        Battle, //터렛을 공격
        Dead    //이동X, 비활성화 처리
    }
    [SerializeField]
    EnemyState enemyState = EnemyState.Walk;

    [Header("EnemyStat")]   //Enemy 능력치

    [SerializeField]
    int speed;  //이동속도

    [Header("EnemyInfo")]   //Enemy 정보

    [SerializeField]
    public int enemyIndex;  //enemy고유 번호

    [SerializeField]
    string filePath; //프리팹 저장 파일 경로

    [SerializeField]
    public int gateNum;    //생성 게이트 번호

    [SerializeField]
    Vector3[] appearPos;  //생성위치

    [Header("Move")]    //이동관련

    [SerializeField]
    public GameObject[] targetPoint;    //타일맵 위에 있는 이동 타겟

    int targetPointIndex = 0;    //타일맵 타겟 인덱스

    GameObject currentTarget;   //현재 타겟

    Vector3 dirVec; //이동처리할 방향벡터

    

    /// <summary>
    /// 초기화 함수 : 김현진
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        Reset();
    }

    public override void Reset()
    {
        base.Reset();

        //HP초기화
        currentHP = maxHP;

        //위치초기화
        transform.position = appearPos[gateNum];

        //상태초기화
        enemyState = EnemyState.Walk;

        //애니메이션 플래그 초기화
        if (attackTargetNum > 0)
        {
            animator.SetBool("attack", false);
            animator.SetBool("attackCancel", false);
            animator.SetBool("finAttack", false);

            if (attackRangeType == 0)
                animator.SetBool("meleeAttack", false);
            else
                animator.SetBool("rangedAttack", false);
        }

        animator.SetBool("isDead", false);

        //Enemy애니메이션 State 초기상태
        animator.Play("Walk");

        //이동 타겟 타일 배열 초기화
        targetPointIndex = 0;
        currentTarget = targetPoint[targetPointIndex];

        //이동 타겟 배열의 첫번째 타일로 방향벡터 초기화
        dirVec = FindDirVec(currentTarget);

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
    /// 실시간 this객체의 상태별 동작 : 김현진
    /// </summary>
    protected override void UpdateActor()
    {
        base.UpdateActor();

        switch (enemyState)
        {
            case EnemyState.Walk:
                CheckArrive();
                UpdateMove(dirVec);
                DetectTarget(SystemManager.Instance.TurretManager.turrets);
                break;
            case EnemyState.Battle:
                UpdateBattle();
                break;
            case EnemyState.Dead:
                UpdateDead();
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
        if (targetPointIndex >= targetPoint.Length - 1)
            return;

        //타겟에 도착하지 않았을 경우
        if (Vector2.SqrMagnitude(new Vector2(transform.position.x, transform.position.z) - new Vector2(currentTarget.transform.position.x, currentTarget.transform.position.z)) > 2f)
        {
            float rotY = Mathf.Round(transform.localEulerAngles.y);

            //예외처리, 속도가 빨라 distance로 감지하지 못했을 경우 방향별 예외처리
            if (rotY == 360f)
                rotY = 0f;
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
        
        currentTarget = targetPoint[++targetPointIndex];

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

        //회전
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
        //회전 x,z축 고정
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
    }

    /// <summary>
    /// this객체의 사거리 안에있는 타겟을 감지해 그중 공격할 타겟을 지정 : 김현진
    /// </summary>
    protected override void DetectTarget(List<GameObject> target)
    {
        base.DetectTarget(target);
    }

    #endregion

    #region Attack - 감지한 적 공격

    /// <summary>
    /// this객체의 상태를 공격으로 변경 : 김현진
    /// </summary>
    protected override void Attack()
    {
        base.Attack();

        //공격 상태로 변경 
        this.enemyState = EnemyState.Battle;

        //공격
        animator.SetBool("finAttack", false);
    }

    /// <summary>
    /// 실시간으로 공격이 끝났는지 안끝났는지를 판별하고 끝났을경우 
    /// 다음 공격으로 이행할지 다른 상태로 변경할지를 결정 : 김현진
    /// </summary>
    protected override void UpdateBattle()
    {
        base.UpdateBattle();

        //attackSpeed초에 1번 공격
        if (Time.time - attackTimer > attackSpeed)
        {
            //단일,다중 타겟 애니메이션 파라미터 초기화
            if (attackTargetNum >= 1)
            {
                animator.SetBool("attackCancel", false);

                if (attackRangeType == 0)
                    animator.SetBool("meleeAttack", false);
                else
                    animator.SetBool("rangedAttack", false);
            }

            //공격할 대상의 존재 유무에 따른 상태 변화
            if (attackTargetsActor[0].currentHP <= 0 || !(attackTargets[0].activeSelf))
            {
                animator.SetBool("attack", false);
                animator.SetBool("finAttack", true);

                enemyState = EnemyState.Walk;     
            }
            else
            {
                //공격시간 측정 변수 초기화
                attackTimer = Time.time;

                //다중 타겟 유닛일 경우 타겟 배열 재설정
                if (attackTargetNum > 1)
                {
                    //공격 사거리 안에 감지 될 타겟 추가
                    DetectTarget(SystemManager.Instance.TurretManager.turrets);
                }

                //다음 공격
                animator.SetBool("attack", true);
                animator.SetBool("finAttack", false);
            }
        }
    }

    /// <summary>
    /// 적의 HP 감소와 사망처리 : 하은비
    /// </summary>
    /// <param name="damage"></param>
    #endregion

    #region Dead - HP 감소와 사망

    public override void DecreseHP(int damage)
    {
        base.DecreseHP(damage);

        if (currentHP == 0)
        {
            SystemManager.Instance.EnemyManager.ReorganizationEnemiesList(enemyIndex);
            enemyState = EnemyState.Dead;
        }
    }

    /// <summary>
    /// 적의 Dead 애니메이션 지연 처리 : 하은비
    /// </summary>
    protected override void UpdateDead()
    {
        base.UpdateDead();

        //Dead상태 종료
        if (!animator.GetBool("isDead"))
        {
            // 에너미 비활성화
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
            return;
        }         
    }
}

#endregion
