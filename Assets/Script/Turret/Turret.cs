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

    protected override void UpdateActor()
    {
        base.UpdateActor();

        switch (turretState)
        {
            case TurretState.Idle:
                DetectTarget(SystemManager.Instance.EnemyManager.enemies);
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
    protected override void DetectTarget(List<GameObject> target)
    {
        base.DetectTarget(target);
    }
    
    /// <summary>
    /// 감지한 Enemy 공격을 위한 상태 변경 : 하은비
    /// </summary>
    protected override void Attack()
    {
        base.Attack();

        turretState = TurretState.Battle;
    }

    /// <summary>
    /// Battle 상태를 업데이트 : 하은비
    /// </summary>
    protected override void UpdateBattle()
    {
        base.UpdateBattle();

        //공격시간이 종료시 공격 종료
        if (Time.time - attackTimer > attackSpeed)
        {
            turretState = TurretState.Idle;
            return;
        }

        UpdateTargetPos();  
    }

    /// <summary>
    /// 이동하는 Enemy 방향으로 터렛이 계속 회전하도록 타겟 위치 업데이트 : 하은비
    /// </summary>
    void UpdateTargetPos()
    {
        attackDirVec = (attackTargets[0].transform.position - this.transform.position).normalized;
    }

}
