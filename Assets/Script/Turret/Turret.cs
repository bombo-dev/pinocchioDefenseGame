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
    bool straightAttack = false;    // 직선형 공격 플래그

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
                DetectTarget(SystemManager.Instance.EnemyManager.enemies.ToArray());
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

        turretState = TurretState.Battle;

    }

    /// <summary>
    /// 총알 위치 초기화 : 하은비
    /// </summary>
    void InitializeBullet()
    {
        //예외처리
        if (attackTargetNum <= 0)
            return;

        //총알 생성

        // 단일 타겟 유닛일 경우
        if (attackTargetNum == 1) 
        {
            SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, firePos.transform.position, attackTargets[0]);
        }
        //다중 타겟 유닛일 경우
        else
        {
            for (int i = 0; i < attackTargets.Count; i++)
            {
                Enemy enemy = attackTargets[i].GetComponent<Enemy>();
                SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, enemy.dropPos.transform.position, attackTargets[i]);
            }
        }
    }

    /// <summary>
    /// Battle 상태를 업데이트 : 하은비
    /// </summary>
    void UpdateBattle()
    {
        Debug.Log("UpdateBattle");

        //원거리 유닛 전용 총알 생성
        if (attackRangeType == 1 && animator.GetBool("rangedAttack"))
        {
            InitializeBullet();
            animator.SetBool("rangedAttack", false);
        }

        //근거리 유닛 전용 데미지 처리
        if (attackRangeType == 0 && animator.GetBool("meleeAttack"))
        {
            //DecreaseHP
            animator.SetBool("meleeAttack", false);
        }

        //감지 범위를 벗어나면 공격 종료
        if (Vector3.SqrMagnitude(attackTargets[0].transform.position - transform.position) >= range)
        {
            turretState = TurretState.Idle;
            return;
        }

        //attackSpeed초에 1번 공격
        if (Time.time - attackTimer > attackSpeed)
        {
            turretState = TurretState.Idle;
            return;
        }


        UpdateTargetPos();
        rotateTurret();
        
    }

    /// <summary>
    /// 이동하는 Enemy 방향으로 터렛이 계속 회전하도록 타겟 위치 업데이트 : 하은비
    /// </summary>
    void UpdateTargetPos()
    {
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




}
