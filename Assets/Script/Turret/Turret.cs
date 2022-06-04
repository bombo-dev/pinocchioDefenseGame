using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turret : Actor
{
    enum TurretState
    {
        Idle = 0,       // 기본 상태. 공격 사거리에 적이 있는지 감지
        Battle = 1,        // 감지한 Enemy를 공격
        Dead,          //  Enemy에 의해 죽은 상태
    }

    public class Buff
    {
        public float durationTime = 0;  //지속시간
    }

    [Header("buff")]  //버프
    public Dictionary<buff, Buff> buffs = new Dictionary<buff, Buff>();

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

        //버프 동작 
        UpdateBuff();

        switch (turretState)
        {
            case TurretState.Idle:
                UpdateHPBarsPos();
                if(!isRecoveryTower)    //공격타워
                    DetectTarget(SystemManager.Instance.EnemyManager.enemies);
                else    //회복타워        
                    DetectTarget(SystemManager.Instance.TurretManager.turrets, gameObject);
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
    protected override void DetectTarget(List<GameObject> target, GameObject mine = null)
    {
        if (mine)
            base.DetectTarget(target, mine);
        else
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
            // 터렛 
            StatusMngPanel statusMngPanel = SystemManager.Instance.PanelManager.turretHPBars[turretIndex].GetComponent<StatusMngPanel>();
            statusMngPanel.StatusReset();
            
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


        //디버프 효과
        switch (debuffIndex)
        {
            case 1: //공격 속도 감소
                currentAttackSpeed *= 1.2f;
                break;
            case 3: //방어력 감소
                currentDefense -= (currentDefense / 5);
                break;
            case 4: //공격력 감소
                currentPower -= (currentPower / 5);
                break;
        }

    }
    /// <summary>
    /// 디버프 제거 : 김현진
    /// </summary>
    /// <param name="debuffIndex">제거할 디버프</param>
    protected override void RemoveDebuff(int debuffIndex)
    {
        base.RemoveDebuff(debuffIndex);

        //디버프 효과 제거
        switch (debuffIndex)
        {
            case 1: //공격 속도 초기화
                currentAttackSpeed = attackSpeed;
                break;
            case 3: //방어력 초기화
                currentDefense = defense;
                break;
            case 4: //공격력 초기화
                currentPower = power;
                break;
        }


        StatusMngPanel statusMngPanel = SystemManager.Instance.PanelManager.enemyHPBars[turretIndex].GetComponent<StatusMngPanel>();
        statusMngPanel.RemoveDebuff(debuffIndex, debuffs);
    }
    #endregion

    #region 버프

    /// <summary>
    /// 실시간 버프 동작 처리 : 김현진
    /// </summary>
    void UpdateBuff()
    {
        if (buffs.Count > 0)
        {
            for (int i = 0; i < Enum.GetValues(typeof(buff)).Length; i++)
            {
                //인덱스를 buff로 형변환
                buff _buffIndex = (buff)i;

                //버프 업데이트
                if (buffs.ContainsKey(_buffIndex))
                {
                    //지속시간 업데이트
                    buffs[_buffIndex].durationTime -= Time.deltaTime;

                    //지속시간 경과시 버프 제거
                    if (buffs[_buffIndex].durationTime < 0)
                        RemoveBuff(i);

                }
            }
        }

    }

    /// <summary>
    /// 버프 추가 : 김현진
    /// </summary>
    /// <param name="debuffIndex">추가할 버프 종류 인덱스</param>
    /// <param name="time">추가할 버프의 지속시간</param>
    public void AddBebuff(int buffIndex, float time)
    {
        //예외처리
        if (buffIndex >= Enum.GetValues(typeof(buff)).Length)
            return;

        //인덱스를 buff로 형변환
        buff _buffIndex = (buff)buffIndex;

        //이미 존재하는 버프가 아닌 경우
        if (!buffs.ContainsKey(_buffIndex))
        {
            Buff buff = new Buff(); //객체 생성

            buffs.Add(_buffIndex, buff);   //자료구조에 추가

            //버프 효과
            switch (buffIndex)
            {
                case 1: //공격력 증가
                    currentPower += (currentPower / 3);
                    break;
                case 2: //공격속도 증가
                    currentAttackSpeed *= 0.7f;
                    break;
                case 3: //회복력 증가 + 즉시 회복
                    currentRegeneration += (currentRegeneration / 3);
                    IncreaseHP(currentHP / 3);
                    break;
                case 4: //방어력 증가
                    currentDefense += (currentDefense / 3);
                    break;
                case 5: //사거리 증가
                    currentRange += (currentRange / 3);
                    currentMultiAttackRange += (currentMultiAttackRange / 3);
                    break;
                case 6: //올스텟 증가
                    currentPower += (currentPower / 3);
                    currentDefense += (currentDefense / 3);
                    currentRegeneration += (currentRegeneration / 3);
                    break;
            }
        }

        buffs[_buffIndex].durationTime = time;   //지속시간 초기화
    }

    /// <summary>
    /// 버프 제거 : 김현진
    /// </summary>
    /// <param name="debuffIndex">제거할 버프</param>
    protected void RemoveBuff(int buffIndex)
    {
        //인덱스를 debuff로 형변환
        buff _buffIndex = (buff)buffIndex;

        //키 값 참조하여 해당 요소 제거
        if (buffs.ContainsKey(_buffIndex))
        {
            buffs.Remove(_buffIndex);

            //버프 효과 제거
            switch (buffIndex)
            {
                case 1: //공격력 초기화
                    currentPower = power;
                    break;
                case 2: //공격속도 초기화
                    currentAttackSpeed = attackSpeed;
                    break;
                case 3: //회복력 초기화
                    currentRegeneration = regeneration;
                    break;
                case 4: //방어력 초기화
                    currentDefense = defense;
                    break;
                case 5: //사거리 초기화
                    currentRange = range;
                    currentMultiAttackRange = multiAttackRange;
                    break;
                case 6: //올스텟 증가
                    currentPower = power;
                    currentDefense = defense;
                    currentRegeneration = regeneration;
                    break;
            }
        }
    }

    #endregion

    protected override void UpdateHPBarsPos()
    {
        base.UpdateHPBarsPos();

        Vector3 screenPos = Camera.main.WorldToScreenPoint(hpPos.transform.position);        
        SystemManager.Instance.PanelManager.turretHPBars[turretIndex].transform.position = screenPos;
    }

}
