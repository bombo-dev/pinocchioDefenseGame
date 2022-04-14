using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Actor
{
    enum TurretState
    {
        Idle = 0,       // 기본 상태. 공격 사거리에 적이 있는지 감지
        Battle = 1,        // 감지한 Enemy를 공격
        Dead,          //  Enemy에 의해 죽은 상태
    }



    [SerializeField]
    TurretState turretState = TurretState.Idle;

    [SerializeField]
    GameObject[] bullet;  

    [SerializeField]
    int bulletIdx = 0;

    [SerializeField]
    Enemy enemy;

    [SerializeField]
    bool straightAttack = false;    // 직선형 공격 플래그

    [SerializeField]
    float reduceHeight;         // 곡선형 공격의 포물선 높이 조절 변수

    [SerializeField]
    int checkAttackType = 0; // 0이면 근거리, 1이면 원거리

    [SerializeField]
    public float distance;  // turret과 enemy의 거리

    // Start is called before the first frame update
    void Start()
    {
    }
    void Update()
    {
        
        UpdateTurret();
    }

    void UpdateTurret()
    {
        switch (turretState)
        {
            case TurretState.Idle:
                DetectTarget(SystemManager.Instance.PrefabCacheSystem.activeEnemy.ToArray());
                break;
            case TurretState.Battle:
                UpdateBattle();
                break;
        }
    }
    /// <summary>
    /// Enemy를 거리순으로 감지
    /// </summary>
    /// <param name="target"></param>
    protected override void DetectTarget(GameObject[] target)
    {
        base.DetectTarget(target);
    }
    
    /// <summary>
    /// 감지한 Enemy 공격을 위한 상태 변경 : 하은비
    /// </summary>
    protected override void Attack()
    {
        base.Attack();
         
        animator.SetBool("attack", true);
        animator.SetBool("finAttack", false);

        turretState = TurretState.Battle;

        // 원거리 공격이면
        if (checkAttackType == 1)
        {
            InitializeBullet();
        }
    }

    /// <summary>
    /// 총알 위치 초기화 : 하은비
    /// </summary>
    void InitializeBullet()
    {
        enemy = attackTargets[0].GetComponentInParent<Enemy>();

        for (int i = 0; i < bullet.Length; i++)
        {
            Debug.Log("bullet.Length = " + bullet.Length);
            if (attackTargetNum > 1) // 다중 타겟 유닛일 경우
            {                
                bullet[i].transform.position = enemy.dropPos.transform.position;
            }
            else // 단일 타겟일 경우
            {
                bullet[i].transform.position = firePos.transform.position;
            }
            bullet[i].SetActive(false);
        }
        bullet[bulletIdx].SetActive(true);
    }

    /// <summary>
    /// Battle 상태를 업데이트 : 하은비
    /// </summary>
    void UpdateBattle()
    {
        Debug.Log("UpdateBattle");
        // 타겟이 비활성화 상태이거나 감지 범위를 벗어나면 공격 종료
        if (!attackTargets[0].activeSelf || Vector3.SqrMagnitude(attackTargets[0].transform.position - transform.position) >= range)
        {
            bullet[bulletIdx].SetActive(false);
            bulletIdx++;

            animator.SetBool("finAttack", true);
            turretState = TurretState.Idle;
            return;
        }


        UpdateTargetPos();
        rotateTurret();

        if (checkAttackType == 1)
            UpdateFire();
        
        IsContinue();
    }

    /// <summary>
    /// 이동하는 Enemy 방향으로 터렛이 계속 회전하도록 타겟 위치 업데이트 : 하은비
    /// </summary>
    void UpdateTargetPos()
    {
        // Enemy와 Turret의 거리, 방향 구하기
        distance = (attackTargets[0].transform.position - this.transform.position).sqrMagnitude;        
        attackDirVec = (attackTargets[0].transform.position - this.transform.position).normalized;
    }

    /// <summary>
    /// 터렛 회전 : 하은비
    /// </summary>
    void rotateTurret()
    {
        Quaternion rotation = Quaternion.LookRotation(new Vector3(attackDirVec.x, 0, attackDirVec.z));
        this.transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
    }

    /// <summary>
    /// 총알 발사 업데이트 : 하은비
    /// </summary>
    void UpdateFire()
    {
        Debug.Log("UpdateFire");

        Fire();
    }

    /// <summary>
    /// 총알 발사 : 하은비
    /// </summary>
    void Fire()
    {
        if (!bullet[bulletIdx].activeSelf)
            return;

        Vector3 bulletPos;   // 총알의 위치
        Vector3 targetPos;  // 타겟이 총알을 맞는 위치

        // 다중 타겟인 경우
        if (attackTargetNum > 1)
        {
            bulletPos = bullet[bulletIdx].transform.position;
            targetPos = enemy.bulletDesPos.transform.position;

            bullet[bulletIdx].transform.position = Vector3.Lerp(bulletPos, targetPos, 0.075f);            
        }
         else   // 단일 타겟인 경우 
        {

            bulletPos = bullet[bulletIdx].transform.position;
            targetPos = enemy.hitPos.transform.position;            

            if (distance < 1600f) // 직선형 공격
            {
                bullet[bulletIdx].transform.position = Vector3.Lerp(bulletPos, targetPos, 0.05f);
            }
            else  //곡선형 공격
            {
                Vector3 center = (bulletPos + targetPos) / 2;
                center -= new Vector3(0, reduceHeight * 1.0f, 0);
                Vector3 startPos = bulletPos - center;
                Vector3 endPos = targetPos - center;

                bullet[bulletIdx].transform.position = Vector3.Slerp(startPos, endPos, 0.05f);
                bullet[bulletIdx].transform.position += center;
            }
        }

        // bullet과 target의 거리가 5보다 작을 경우 불렛 비활성화
        float bulletDistance = (targetPos - bulletPos).sqrMagnitude;        

        if (Mathf.Round(bulletDistance) < 5f)
        {
            Debug.Log("bullet active false");
            bullet[bulletIdx].SetActive(false);
            //if (multiTarget)
            // 다중 타겟인 경우, 이펙트 출력
        }

    }

    /// <summary>
    /// Battle 상태를 유지할 것인지 판단 : 하은비
    /// </summary>
    void IsContinue()
    {
        if (Time.time - attackTimer > attackSpeed)
        {
            if (attackTargets[0] == null || !attackTargets[0].activeSelf)
            {
                // 터렛의 상태와 애니메이션 초기화
                turretState = TurretState.Idle;
                animator.SetBool("finAttack", true);
            }
            else
            {
                // 원거리 공격이면
                if (checkAttackType == 1)
                {
                    bulletIdx++;
                    bullet[bulletIdx].SetActive(true);

                    // 다중 공격 유닛일 경우, 불렛의 위치를 업데이트
                    if (attackTargetNum > 1)
                        bullet[bulletIdx].transform.position = enemy.dropPos.transform.position;
                }

                // 공격 시작 타이머와 애니메이션 초기화
                attackTimer = Time.time;
                animator.SetBool("attack", true);

            }
        }
    }


}
