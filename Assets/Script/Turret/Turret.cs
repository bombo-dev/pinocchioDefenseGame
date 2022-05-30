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

    //소환해있는 둥지
    public GameObject nest;

    public int turretNum;   //터렛 종류 번호 (터렛 종류에 따라 번호 부여)

    [SerializeField]
    string gateNum; 

    [SerializeField]
    public int turretIndex;  //turret고유 번호 (생성되있는 터렛기준 번호 부여)

    [SerializeField]
    TurretState turretState = TurretState.Idle;

    [SerializeField]
    float turretAppearPosY; //터렛이 생성될때 고정되는 Y축 위치

    private void Start()
    {
        InitializeTurret();
    }
    protected override void UpdateActor()
    {

        base.UpdateActor();

        switch (turretState)
        {
            case TurretState.Idle:
                UpdateHPBarsPos();
                if(!isRecoveryTower)    //공격타워
                    DetectTarget(SystemManager.Instance.EnemyManager.enemies);
                else    //회복타워        
                    DetectTarget(SystemManager.Instance.TurretManager.turrets);
                break;
            case TurretState.Battle:
                UpdateHPBarsPos();
                UpdateBattle();
                break;
            case TurretState.Dead:
                UpdateDead();
                break;

        }
    }

    /// <summary>
    /// 터렛 초기화 : 하은비
    /// </summary>
    void InitializeTurret()
    {
        base.Initialize();

        Reset();
       
    }


    /// <summary>
    /// 터렛의 정보 리셋 : 김현진
    /// </summary>
    public override void Reset()
    {
        base.Reset();

        //위치 초기화
        this.transform.position = new Vector3(this.transform.position.x, turretAppearPosY, this.transform.position.z);

        //상태초기화
        turretState = TurretState.Idle;

        //애니메이션 플래그 초기화
        if (attackTargetNum > 0)
        {
            animator.SetBool("attack", false);
            animator.SetBool("attackCancel", false);

            if (attackRangeType == 0)
                animator.SetBool("meleeAttack", false);
            else
                animator.SetBool("rangedAttack", false);
        }

        animator.SetBool("isDead", false);

        //Enemy애니메이션 State 초기상태
        animator.Play("Idle");

        //상태초기화
        turretState = TurretState.Idle;
    }

    /// <summary>
    /// Enemy를 거리순으로 감지 : 하은비
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
        if (Time.time - attackTimer > currentAttackSpeed)
        {
            turretState = TurretState.Idle;

            //단일,다중 타겟 애니메이션 파라미터 초기화
            if (attackTargetNum >= 1)
            {
                animator.SetBool("attackCancel", false);
                animator.SetBool("attack", false);

                if (attackRangeType == 0)
                    animator.SetBool("meleeAttack", false);
                else
                    animator.SetBool("rangedAttack", false);
            }

            return;
        }

        // 이동하는 Enemy 방향으로 터렛이 계속 회전하도록 타겟 위치 업데이트
        attackDirVec = (attackTargets[0].transform.position - this.transform.position).normalized;
    }

    /// <summary>
    /// 터렛 HP 감소와 사망처리 : 하은비
    /// </summary>
    /// <param name="damage"></param>
    public override void DecreaseHP(int damage)
    {
        base.DecreaseHP(damage);


        if (SystemManager.Instance.PanelManager.turretHPBars[turretIndex])
        {
            StatusMngPanel statusMngPanel = SystemManager.Instance.PanelManager.turretHPBars[turretIndex].GetComponent<StatusMngPanel>();
            statusMngPanel.SetHPBar(currentHP, maxHP);
        }
        else
            return;

        //TurretInfo UI 갱신
        if (SystemManager.Instance.PanelManager.turretInfoPanel)
        {
            UI_TurretInfoPanel panel = SystemManager.Instance.PanelManager.turretInfoPanel.GetComponent<UI_TurretInfoPanel>();

            //TurretInfo UI 최신정보로 업데이트
            panel.Reset(false);
        }

        if (currentHP == 0)
        {
            //int panelIndex = SystemManager.Instance.PanelManager.statusMngPanel.turretHPBarIndex;
            SystemManager.Instance.PanelManager.DisablePanel<StatusMngPanel>(SystemManager.Instance.PanelManager.turretHPBars[turretIndex].gameObject);
            SystemManager.Instance.PanelManager.ReorganizationPanelList(turretIndex, GetType());
            SystemManager.Instance.TurretManager.ReorganizationEnemiesList(turretIndex);            
            
            turretState = TurretState.Dead;
        }

    }

    /// <summary>
    /// 터렛 HP 증가 : 김현진
    /// </summary>
    /// <param name="recoveryPower">증가량</param>
    public override void IncreaseHP(int recoveryPower)
    {
        base.IncreaseHP(recoveryPower);

        if (SystemManager.Instance.PanelManager.turretHPBars[turretIndex])
        {
            StatusMngPanel statusMngPanel = SystemManager.Instance.PanelManager.turretHPBars[turretIndex].GetComponent<StatusMngPanel>();
            statusMngPanel.SetHPBar(currentHP, maxHP);
        }
        else
            return;

        //TurretInfo UI 갱신
        if (SystemManager.Instance.PanelManager.turretInfoPanel)
        {
            UI_TurretInfoPanel panel = SystemManager.Instance.PanelManager.turretInfoPanel.GetComponent<UI_TurretInfoPanel>();

            //TurretInfo UI 최신정보로 업데이트
            panel.Reset(false);
        }
    }

    /// <summary>
    /// 이펙트 출력
    /// </summary>
    /// <param name="attacker">공격자</param>
    public override void EnableDamageEffect(Actor attacker)
    {
        base.EnableDamageEffect(attacker);
    }

    /// <summary>
    /// 터렛의 Dead 애니메이션 지연 처리 : 하은비
    /// </summary>
    protected override void UpdateDead()
    {
        base.UpdateDead();

        //Dead상태 종료
        if (!animator.GetBool("isDead"))
        {
            // 터렛 비활성화
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);

            // 터렛 소환 정보 비활성화 상태로 변경
            Nest _nest = nest.GetComponent<Nest>();
            if (_nest)
            {
                //현재 선택하고 있는 둥지위에있는 터렛이 Dead상태로 변하고 Turret정보 UI가 켜져있을 경우 UI off
                //UI_TurretInfoPanel 패널이 존재할 경우
                if (SystemManager.Instance.PanelManager.turretInfoPanel && System.Object.ReferenceEquals(SystemManager.Instance.InputManager.currenstSelectNest, _nest.gameObject))
                {
                    //패널 비활성화
                    SystemManager.Instance.PanelManager.DisablePanel<UI_TurretInfoPanel>(SystemManager.Instance.PanelManager.turretInfoPanel.gameObject);
                }
                if(!SystemManager.Instance.PanelManager.turretInfoPanel)
                    SystemManager.Instance.PanelManager.EnablePanel<UI_TurretMngPanel>(0); //0: UI_TurretMngPanel


                _nest.haveTurret = false;
                _nest.turret = null;
            }
            nest = null;

            return;
        }
    }

    #region 디버프
    /// <summary>
    /// 디버프 추가 : 김현진
    /// </summary>
    /// <param name="debuffIndex">추가할 디버프 종류 인덱스</param>
    /// <param name="time">추가할 디버프의 지속시간</param>
    public override void AddDebuff(int debuffIndex, float time)
    {
        base.AddDebuff(debuffIndex, time);
        
        if (SystemManager.Instance.PanelManager.turretHPBars[turretIndex])
        {
            StatusMngPanel statusMngPanel = SystemManager.Instance.PanelManager.turretHPBars[turretIndex].GetComponent<StatusMngPanel>();
            statusMngPanel.SetDebuff(debuffIndex, debuffs, time);
        }
        else
            return;
        
    }

    #endregion

    protected override void UpdateHPBarsPos()
    {
        base.UpdateHPBarsPos();

        Vector3 screenPos = Camera.main.WorldToScreenPoint(hpPos.transform.position);        
        SystemManager.Instance.PanelManager.turretHPBars[turretIndex].transform.position = screenPos;
    }

}
