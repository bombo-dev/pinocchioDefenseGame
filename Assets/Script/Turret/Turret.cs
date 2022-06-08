using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Turret : Actor
{
    enum TurretState
    {
        Idle = 0,       // 기본 상태. 공격 사거리에 적이 있는지 감지
        Battle = 1,        // 감지한 Enemy를 공격
        Dead,          //  Enemy에 의해 죽은 상태
    }

    [Header("buff")]  //버프
    //버프 정보 담을 자료구조 , <buff, durationTime>
    public Dictionary<buff, float> buffs = new Dictionary<buff, float>();

    [SerializeField]
    int[] buffEffectIndex;  //활성화할 버프 이펙트 번호
    [SerializeField]
    GameObject[] CurrentBuffEffect; //현재 활성화된 버프 이펙트
    [SerializeField]
    string[] CurrentBuffEffectFilePath; //현재 활성화된 버프 이펙트 파일 주소

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

    //터렛 건설 코스트
    //건설비용
    int turretCost;
    //터렛 건설에 걸리는 시간
    int turretConstructionTime;

    public StatusMngPanel statusMngPanel;

    public DamageMngPanel damageMngPanel;

    private void Start()
    {
        Initialize();
    }
    protected override void UpdateActor()
    {
        base.UpdateActor();

        //버프 동작 
        UpdateBuff();

        switch (turretState)
        {
            case TurretState.Idle:
                UpdatePanelPos();
                if(!isRecoveryTower)    //공격타워
                    DetectTarget(SystemManager.Instance.EnemyManager.enemies);
                else    //회복타워        
                    DetectTarget(SystemManager.Instance.TurretManager.turrets, gameObject);
                break;
            case TurretState.Battle:
                UpdatePanelPos();
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
    protected override void Initialize()
    {
        base.Initialize();
        //TurretInitializing();
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

        //사거리 초기화
        currentRange = range;

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

        //버프초기화
        ClearBuff();
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


        if (statusMngPanel)
        {
            statusMngPanel.SetHPBar(currentHP, maxHP);
        }
        else
            return;


        //TurretInfo UI 갱신
        if (SystemManager.Instance.PanelManager.turretInfoPanel)
        {
            UI_TurretInfoPanel panel = SystemManager.Instance.PanelManager.turretInfoPanel.GetComponent<UI_TurretInfoPanel>();

            //TurretInfo UI 최신정보로 업데이트
            panel.Reset(false, false);
        }

        if (currentHP == 0)
        {
            //BaseTurret
            if (turretNum == 23)
            {
                //게임오버
                return;
            }

            //패널 비활성화
            SystemManager.Instance.PanelManager.DisablePanel<StatusMngPanel>(statusMngPanel.gameObject);
            
            //패널 정보 리셋
            statusMngPanel.StatusReset();

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

        if (statusMngPanel)
        {
            statusMngPanel.SetHPBar(currentHP, maxHP);
        }
        else
            return;

        //TurretInfo UI 갱신
        if (SystemManager.Instance.PanelManager.turretInfoPanel)
        {
            UI_TurretInfoPanel panel = SystemManager.Instance.PanelManager.turretInfoPanel.GetComponent<UI_TurretInfoPanel>();

            //TurretInfo UI 최신정보로 업데이트
            panel.Reset(false, false);
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

            //버프 초기화
            ClearBuff();

            // 터렛 비활성화
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);

            return;
        }
    }

    #region 이펙트
    /// <summary>
    /// 버프 이펙트 출력 : 김현진
    /// </summary>
    /// <param name="attacker">공격자</param>
    public void EnableBuffEffect(int buffIndex)
    {
        //예외처리
        if (!nest)
            return;

        Nest _nest = nest.GetComponent<Nest>();
        //이펙트 출력 
        if (_nest || buffs.Count > 0 || buffs.Count <= Enum.GetValues(typeof(buff)).Length - 2)
        {
            CurrentBuffEffect[buffIndex] = SystemManager.Instance.EffectManager.
                EnableEffect(buffEffectIndex[buffIndex], _nest.buffEffectPos[buffs.Count-1].transform.position);   //피격 이펙트 출력
        }
    }
    #endregion 이펙트

    #region 디버프
    /// <summary>
    /// 디버프 추가 : 김현진
    /// </summary>
    /// <param name="debuffIndex">추가할 디버프 종류 인덱스</param>
    /// <param name="time">추가할 디버프의 지속시간</param>
    public override void AddDebuff(int debuffIndex, float time)
    {
        base.AddDebuff(debuffIndex, time);
        
        if (statusMngPanel)
        {            
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
                    buffs[_buffIndex] -= Time.deltaTime;

                    //지속시간 경과시 버프 제거
                    if (buffs[_buffIndex] < 0)
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
            buffs.Add(_buffIndex, time);   //딕셔너리 자료구조에 추가

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

            //이펙트 생성
            EnableBuffEffect(buffIndex - 1);
        }

        buffs[_buffIndex] = time;   //지속시간 초기화

        //HP즉시회복은 중첩 적용, HP가 0이 아닐경우
        if(buffIndex == 3 && currentHP != 0)
            IncreaseHP(maxHP / 5);

        //버프 이펙트 위치 재배치
        RePositionBuffEffect();
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
            buffs.Remove(_buffIndex);   //딕셔너리 자료구조에서 제거

            //버프 효과 제거
            switch (buffIndex)
            {
                case 1: //공격력 초기화
                    if (buffs.ContainsKey(buff.IncreaseAll))//올스텟 버프가 존재할 경우
                        currentPower -= ((power + (power / 3)) / 3);
                    else
                        currentPower = power;
                    break;
                case 2: //공격속도 초기화
                    currentAttackSpeed = attackSpeed;
                    break;
                case 3: //회복력 초기화
                    if (buffs.ContainsKey(buff.IncreaseAll))//올스텟 버프가 존재할 경우
                        currentRegeneration -= ((regeneration + (regeneration / 3)) / 3);
                    else
                        currentRegeneration = regeneration;
                    break;
                case 4: //방어력 초기화
                    if (buffs.ContainsKey(buff.IncreaseAll))//올스텟 버프가 존재할 경우
                        currentDefense -= ((defense + (defense / 3)) / 3);
                    else
                        currentDefense = defense;
                    break;
                case 5: //사거리 초기화
                    currentRange = range;
                    currentMultiAttackRange = multiAttackRange;
                    break;
                case 6: //올스텟 증가
                    if (buffs.ContainsKey(buff.IncreasePower))//공격력 버프가 존재할 경우
                        currentPower -= ((power + (power / 3)) / 3);
                    else
                        currentPower = power;

                    if(buffs.ContainsKey(buff.IncreaseDefense))//방어력 버프가 존재할 경우
                        currentDefense -= ((defense + (defense / 3)) / 3);
                    else
                        currentDefense = defense;

                    if (buffs.ContainsKey(buff.IncreaseRegeneration))//회복력 버프가 존재할 경우
                        currentRegeneration -= ((regeneration + (regeneration / 3)) / 3);
                    else
                        currentRegeneration = regeneration;
                    break;
            }

            //버프 이펙트 제거
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(CurrentBuffEffectFilePath[buffIndex - 1], CurrentBuffEffect[buffIndex - 1]);

            //UI업데이트
            if (SystemManager.Instance.PanelManager.turretInfoPanel)
                if (SystemManager.Instance.PanelManager.turretInfoPanel.gameObject.activeSelf)
                    SystemManager.Instance.PanelManager.turretInfoPanel.GetComponent<UI_TurretInfoPanel>().Reset();
        }
    }

    /// <summary>
    /// 남은 시간 기준 버프 이펙트 위치를 다시배열
    /// </summary>
    void RePositionBuffEffect()
    {
        //남은 시간 기준 내림차순 정렬
        if (buffs.Count > 0)
        {
            //딕셔너리 내림차순 정렬
            var sortedbuffs = buffs.OrderByDescending(x => x.Value);

            //예외처리
            if (!nest)
                return;
 
            Nest _nest = nest.GetComponent<Nest>();

            //예외처리
            if (!_nest)
                return;

            //버프 이펙트 위치 재배열
            int i = 0;
            foreach (KeyValuePair<buff, float> item in sortedbuffs)
            {
                //버프가 존재할 경우
                if (CurrentBuffEffect[(int)item.Key - 1])
                {
                    //위치 재설정 -> 지속시간이 가장 많이남은 이펙트가 맨 아래로 위치
                    CurrentBuffEffect[(int)item.Key - 1].transform.position
                        = _nest.buffEffectPos[i].transform.position;
                    i++;
                }
            }

        }

    }

    /// <summary>
    /// 모든 버프를 제거
    /// </summary>
    void ClearBuff()
    {
        if (buffs.Count > 0)
        {
            for (int i = 0; i < Enum.GetValues(typeof(buff)).Length; i++)
            {
                //디버프 제거
                RemoveBuff(i);
            }
        }

        //딕셔너리 초기화
        buffs.Clear();
    }

    #endregion

    protected override void UpdatePanelPos()
    {
        base.UpdatePanelPos();

   
        if (statusMngPanel)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(hpPos.transform.position);
            //Debug.Log("Enemy.screenPos=" + screenPos);
            statusMngPanel.gameObject.transform.position = screenPos;
        }
        

    }

    #region TurretInitilizing 터렛 초기화
    void TurretInitializing()
    {
        turretDatas = SystemManager.Instance.TurretJson.GetTurretData();

        switch (filePath)
        {
            case "Turret/PigeonTurret":
                turretNum = turretDatas[0].turretNum;
                maxHP = turretDatas[0].maxHP;
                power = turretDatas[0].power;
                defense = turretDatas[0].defense;
                attackSpeed = turretDatas[0].attackSpeed;
                range = turretDatas[0].range;
                regeneration = turretDatas[0].regeneration;
                attackRangeType = turretDatas[0].attackRangeType;
                isRecoveryTower = turretDatas[0].isRecoveryTower;
                attackTargetNum = turretDatas[0].attackTargetNum;
                debuffType = turretDatas[0].debuffType;
                debuffDuration = turretDatas[0].debuffDuration;
                multiAttackRange = turretDatas[0].multiAttackRange;
                bulletIndex = turretDatas[0].bulletIndex;
                damageEffectIndex = turretDatas[0].damageEffectIndex;
                deadEffectIndex = turretDatas[0].deadEffectIndex;
                fireEffectIndex = turretDatas[0].fireEffectIndex;
                healEffectIndex = turretDatas[0].healEffectIndex;
                debuffEffectIndex = turretDatas[0].debuffEffectIndex;
                turretAppearPosY = turretDatas[0].turretAppearPosY;
                turretCost = turretDatas[0].turretCost;
                turretConstructionTime = turretDatas[0].turretConstructionTime;
                break;

            case "Turret/WoodpeckerTurret":
                Debug.Log("WoodpeckerTurret 초기화");
                turretNum = turretDatas[1].turretNum;
                maxHP = turretDatas[1].maxHP;
                power = turretDatas[1].power;
                defense = turretDatas[1].defense;
                attackSpeed = turretDatas[1].attackSpeed;
                range = turretDatas[1].range;
                regeneration = turretDatas[1].regeneration;
                attackRangeType = turretDatas[1].attackRangeType;
                isRecoveryTower = turretDatas[1].isRecoveryTower;
                attackTargetNum = turretDatas[1].attackTargetNum;
                debuffType = turretDatas[1].debuffType;
                debuffDuration = turretDatas[1].debuffDuration;
                multiAttackRange = turretDatas[1].multiAttackRange;
                bulletIndex = turretDatas[1].bulletIndex;
                damageEffectIndex = turretDatas[1].damageEffectIndex;
                deadEffectIndex = turretDatas[1].deadEffectIndex;
                fireEffectIndex = turretDatas[1].fireEffectIndex;
                healEffectIndex = turretDatas[1].healEffectIndex;
                debuffEffectIndex = turretDatas[1].debuffEffectIndex;
                turretAppearPosY = turretDatas[1].turretAppearPosY;
                turretCost = turretDatas[1].turretCost;
                turretConstructionTime = turretDatas[1].turretConstructionTime;
                break;

            case "Turret/BabyBirdTurret":
                Debug.Log("BabyBirdTurret 초기화");
                turretNum = turretDatas[2].turretNum;
                maxHP = turretDatas[2].maxHP;
                power = turretDatas[2].power;
                defense = turretDatas[2].defense;
                attackSpeed = turretDatas[2].attackSpeed;
                range = turretDatas[2].range;
                regeneration = turretDatas[2].regeneration;
                attackRangeType = turretDatas[2].attackRangeType;
                isRecoveryTower = turretDatas[2].isRecoveryTower;
                attackTargetNum = turretDatas[2].attackTargetNum;
                debuffType = turretDatas[2].debuffType;
                debuffDuration = turretDatas[2].debuffDuration;
                multiAttackRange = turretDatas[2].multiAttackRange;
                bulletIndex = turretDatas[2].bulletIndex;
                damageEffectIndex = turretDatas[2].damageEffectIndex;
                deadEffectIndex = turretDatas[2].deadEffectIndex;
                fireEffectIndex = turretDatas[2].fireEffectIndex;
                healEffectIndex = turretDatas[2].healEffectIndex;
                debuffEffectIndex = turretDatas[2].debuffEffectIndex;
                turretAppearPosY = turretDatas[2].turretAppearPosY;
                turretCost = turretDatas[2].turretCost;
                turretConstructionTime = turretDatas[2].turretConstructionTime;
                break;

            case "Turret/UpgradedPigeonTurret":
                Debug.Log("UpgradedPigeonTurret 초기화");
                turretNum = turretDatas[3].turretNum;
                maxHP = turretDatas[3].maxHP;
                power = turretDatas[3].power;
                defense = turretDatas[3].defense;
                attackSpeed = turretDatas[3].attackSpeed;
                range = turretDatas[3].range;
                regeneration = turretDatas[3].regeneration;
                attackRangeType = turretDatas[3].attackRangeType;
                isRecoveryTower = turretDatas[3].isRecoveryTower;
                attackTargetNum = turretDatas[3].attackTargetNum;
                debuffType = turretDatas[3].debuffType;
                debuffDuration = turretDatas[3].debuffDuration;
                multiAttackRange = turretDatas[3].multiAttackRange;
                bulletIndex = turretDatas[3].bulletIndex;
                damageEffectIndex = turretDatas[3].damageEffectIndex;
                deadEffectIndex = turretDatas[3].deadEffectIndex;
                fireEffectIndex = turretDatas[3].fireEffectIndex;
                healEffectIndex = turretDatas[3].healEffectIndex;
                debuffEffectIndex = turretDatas[3].debuffEffectIndex;
                turretAppearPosY = turretDatas[3].turretAppearPosY;
                turretCost = turretDatas[3].turretCost;
                turretConstructionTime = turretDatas[3].turretConstructionTime;
                break;

            case "Turret/PelicanTurret":
                Debug.Log("PelicanTurret 초기화");
                turretNum = turretDatas[4].turretNum;
                maxHP = turretDatas[4].maxHP;
                power = turretDatas[4].power;
                defense = turretDatas[4].defense;
                attackSpeed = turretDatas[4].attackSpeed;
                range = turretDatas[4].range;
                regeneration = turretDatas[4].regeneration;
                attackRangeType = turretDatas[4].attackRangeType;
                isRecoveryTower = turretDatas[4].isRecoveryTower;
                attackTargetNum = turretDatas[4].attackTargetNum;
                debuffType = turretDatas[4].debuffType;
                debuffDuration = turretDatas[4].debuffDuration;
                multiAttackRange = turretDatas[4].multiAttackRange;
                bulletIndex = turretDatas[4].bulletIndex;
                damageEffectIndex = turretDatas[4].damageEffectIndex;
                deadEffectIndex = turretDatas[4].deadEffectIndex;
                fireEffectIndex = turretDatas[4].fireEffectIndex;
                healEffectIndex = turretDatas[4].healEffectIndex;
                debuffEffectIndex = turretDatas[4].debuffEffectIndex;
                turretAppearPosY = turretDatas[4].turretAppearPosY;
                turretCost = turretDatas[4].turretCost;
                turretConstructionTime = turretDatas[4].turretConstructionTime;
                break;

            case "Turret/CuckooTurret":
                Debug.Log("CuckooTurret 초기화");
                turretNum = turretDatas[5].turretNum;
                maxHP = turretDatas[5].maxHP;
                power = turretDatas[5].power;
                defense = turretDatas[5].defense;
                attackSpeed = turretDatas[5].attackSpeed;
                range = turretDatas[5].range;
                regeneration = turretDatas[5].regeneration;
                attackRangeType = turretDatas[5].attackRangeType;
                isRecoveryTower = turretDatas[5].isRecoveryTower;
                attackTargetNum = turretDatas[5].attackTargetNum;
                debuffType = turretDatas[5].debuffType;
                debuffDuration = turretDatas[5].debuffDuration;
                multiAttackRange = turretDatas[5].multiAttackRange;
                bulletIndex = turretDatas[5].bulletIndex;
                damageEffectIndex = turretDatas[5].damageEffectIndex;
                deadEffectIndex = turretDatas[5].deadEffectIndex;
                fireEffectIndex = turretDatas[5].fireEffectIndex;
                healEffectIndex = turretDatas[5].healEffectIndex;
                debuffEffectIndex = turretDatas[5].debuffEffectIndex;
                turretAppearPosY = turretDatas[5].turretAppearPosY;
                turretCost = turretDatas[5].turretCost;
                turretConstructionTime = turretDatas[5].turretConstructionTime;
                break;

            case "Turret/PenguinTurret":
                Debug.Log("PenguinTurret 초기화");
                turretNum = turretDatas[6].turretNum;
                maxHP = turretDatas[6].maxHP;
                power = turretDatas[6].power;
                defense = turretDatas[6].defense;
                attackSpeed = turretDatas[6].attackSpeed;
                range = turretDatas[6].range;
                regeneration = turretDatas[6].regeneration;
                attackRangeType = turretDatas[6].attackRangeType;
                isRecoveryTower = turretDatas[6].isRecoveryTower;
                attackTargetNum = turretDatas[6].attackTargetNum;
                debuffType = turretDatas[6].debuffType;
                debuffDuration = turretDatas[6].debuffDuration;
                multiAttackRange = turretDatas[6].multiAttackRange;
                bulletIndex = turretDatas[6].bulletIndex;
                damageEffectIndex = turretDatas[6].damageEffectIndex;
                deadEffectIndex = turretDatas[6].deadEffectIndex;
                fireEffectIndex = turretDatas[6].fireEffectIndex;
                healEffectIndex = turretDatas[6].healEffectIndex;
                debuffEffectIndex = turretDatas[6].debuffEffectIndex;
                turretAppearPosY = turretDatas[6].turretAppearPosY;
                turretCost = turretDatas[6].turretCost;
                turretConstructionTime = turretDatas[6].turretConstructionTime;
                break;

            case "Turret/OwlTurret":
                Debug.Log("OwlTurret 초기화");
                turretNum = turretDatas[7].turretNum;
                maxHP = turretDatas[7].maxHP;
                power = turretDatas[7].power;
                defense = turretDatas[7].defense;
                attackSpeed = turretDatas[7].attackSpeed;
                range = turretDatas[7].range;
                regeneration = turretDatas[7].regeneration;
                attackRangeType = turretDatas[7].attackRangeType;
                isRecoveryTower = turretDatas[7].isRecoveryTower;
                attackTargetNum = turretDatas[7].attackTargetNum;
                debuffType = turretDatas[7].debuffType;
                debuffDuration = turretDatas[7].debuffDuration;
                multiAttackRange = turretDatas[7].multiAttackRange;
                bulletIndex = turretDatas[7].bulletIndex;
                damageEffectIndex = turretDatas[7].damageEffectIndex;
                deadEffectIndex = turretDatas[7].deadEffectIndex;
                fireEffectIndex = turretDatas[7].fireEffectIndex;
                healEffectIndex = turretDatas[7].healEffectIndex;
                debuffEffectIndex = turretDatas[7].debuffEffectIndex;
                turretAppearPosY = turretDatas[7].turretAppearPosY;
                turretCost = turretDatas[7].turretCost;
                turretConstructionTime = turretDatas[7].turretConstructionTime;
                break;

            case "Turret/UpgradedWoodPecker":
                Debug.Log("UpgradedWoodPecker 초기화");
                turretNum = turretDatas[8].turretNum;
                maxHP = turretDatas[8].maxHP;
                power = turretDatas[8].power;
                defense = turretDatas[8].defense;
                attackSpeed = turretDatas[8].attackSpeed;
                range = turretDatas[8].range;
                regeneration = turretDatas[8].regeneration;
                attackRangeType = turretDatas[8].attackRangeType;
                isRecoveryTower = turretDatas[8].isRecoveryTower;
                attackTargetNum = turretDatas[8].attackTargetNum;
                debuffType = turretDatas[8].debuffType;
                debuffDuration = turretDatas[8].debuffDuration;
                multiAttackRange = turretDatas[8].multiAttackRange;
                bulletIndex = turretDatas[8].bulletIndex;
                damageEffectIndex = turretDatas[8].damageEffectIndex;
                deadEffectIndex = turretDatas[8].deadEffectIndex;
                fireEffectIndex = turretDatas[8].fireEffectIndex;
                healEffectIndex = turretDatas[8].healEffectIndex;
                debuffEffectIndex = turretDatas[8].debuffEffectIndex;
                turretAppearPosY = turretDatas[8].turretAppearPosY;
                turretCost = turretDatas[8].turretCost;
                turretConstructionTime = turretDatas[8].turretConstructionTime;
                break;

            case "Turret/HawkTurret":
                Debug.Log("HawkTurret 초기화");
                turretNum = turretDatas[9].turretNum;
                maxHP = turretDatas[9].maxHP;
                power = turretDatas[9].power;
                defense = turretDatas[9].defense;
                attackSpeed = turretDatas[9].attackSpeed;
                range = turretDatas[9].range;
                regeneration = turretDatas[9].regeneration;
                attackRangeType = turretDatas[9].attackRangeType;
                isRecoveryTower = turretDatas[9].isRecoveryTower;
                attackTargetNum = turretDatas[9].attackTargetNum;
                debuffType = turretDatas[9].debuffType;
                debuffDuration = turretDatas[9].debuffDuration;
                multiAttackRange = turretDatas[9].multiAttackRange;
                bulletIndex = turretDatas[9].bulletIndex;
                damageEffectIndex = turretDatas[9].damageEffectIndex;
                deadEffectIndex = turretDatas[9].deadEffectIndex;
                fireEffectIndex = turretDatas[9].fireEffectIndex;
                healEffectIndex = turretDatas[9].healEffectIndex;
                debuffEffectIndex = turretDatas[9].debuffEffectIndex;
                turretAppearPosY = turretDatas[9].turretAppearPosY;
                turretCost = turretDatas[9].turretCost;
                turretConstructionTime = turretDatas[9].turretConstructionTime;
                break;

            case "Turret/EagleTurret":
                Debug.Log("EagleTurret 초기화");
                turretNum = turretDatas[10].turretNum;
                maxHP = turretDatas[10].maxHP;
                power = turretDatas[10].power;
                defense = turretDatas[10].defense;
                attackSpeed = turretDatas[10].attackSpeed;
                range = turretDatas[10].range;
                regeneration = turretDatas[10].regeneration;
                attackRangeType = turretDatas[10].attackRangeType;
                isRecoveryTower = turretDatas[10].isRecoveryTower;
                attackTargetNum = turretDatas[10].attackTargetNum;
                debuffType = turretDatas[10].debuffType;
                debuffDuration = turretDatas[10].debuffDuration;
                multiAttackRange = turretDatas[10].multiAttackRange;
                bulletIndex = turretDatas[10].bulletIndex;
                damageEffectIndex = turretDatas[10].damageEffectIndex;
                deadEffectIndex = turretDatas[10].deadEffectIndex;
                fireEffectIndex = turretDatas[10].fireEffectIndex;
                healEffectIndex = turretDatas[10].healEffectIndex;
                debuffEffectIndex = turretDatas[10].debuffEffectIndex;
                turretAppearPosY = turretDatas[10].turretAppearPosY;
                turretCost = turretDatas[10].turretCost;
                turretConstructionTime = turretDatas[10].turretConstructionTime;
                break;

            case "Turret/UpgradedBabyBirdTurret":
                Debug.Log("UpgradedBabyBirdTurret 초기화");
                turretNum = turretDatas[11].turretNum;
                maxHP = turretDatas[11].maxHP;
                power = turretDatas[11].power;
                defense = turretDatas[11].defense;
                attackSpeed = turretDatas[11].attackSpeed;
                range = turretDatas[11].range;
                regeneration = turretDatas[11].regeneration;
                attackRangeType = turretDatas[11].attackRangeType;
                isRecoveryTower = turretDatas[11].isRecoveryTower;
                attackTargetNum = turretDatas[11].attackTargetNum;
                debuffType = turretDatas[11].debuffType;
                debuffDuration = turretDatas[11].debuffDuration;
                multiAttackRange = turretDatas[11].multiAttackRange;
                bulletIndex = turretDatas[11].bulletIndex;
                damageEffectIndex = turretDatas[11].damageEffectIndex;
                deadEffectIndex = turretDatas[11].deadEffectIndex;
                fireEffectIndex = turretDatas[11].fireEffectIndex;
                healEffectIndex = turretDatas[11].healEffectIndex;
                debuffEffectIndex = turretDatas[11].debuffEffectIndex;
                turretAppearPosY = turretDatas[11].turretAppearPosY;
                turretCost = turretDatas[11].turretCost;
                turretConstructionTime = turretDatas[11].turretConstructionTime;
                break;

            case "Turret/UpgradedEagleTurret":
                Debug.Log("UpgradedEagleTurret 초기화");
                turretNum = turretDatas[12].turretNum;
                maxHP = turretDatas[12].maxHP;
                power = turretDatas[12].power;
                defense = turretDatas[12].defense;
                attackSpeed = turretDatas[12].attackSpeed;
                range = turretDatas[12].range;
                regeneration = turretDatas[12].regeneration;
                attackRangeType = turretDatas[12].attackRangeType;
                isRecoveryTower = turretDatas[12].isRecoveryTower;
                attackTargetNum = turretDatas[12].attackTargetNum;
                debuffType = turretDatas[12].debuffType;
                debuffDuration = turretDatas[12].debuffDuration;
                multiAttackRange = turretDatas[12].multiAttackRange;
                bulletIndex = turretDatas[12].bulletIndex;
                damageEffectIndex = turretDatas[12].damageEffectIndex;
                deadEffectIndex = turretDatas[12].deadEffectIndex;
                fireEffectIndex = turretDatas[12].fireEffectIndex;
                healEffectIndex = turretDatas[12].healEffectIndex;
                debuffEffectIndex = turretDatas[12].debuffEffectIndex;
                turretAppearPosY = turretDatas[12].turretAppearPosY;
                turretCost = turretDatas[12].turretCost;
                turretConstructionTime = turretDatas[12].turretConstructionTime;
                break;

            case "Turret/ArmamentPigeonTurret":
                Debug.Log("ArmamentPigeonTurret 초기화");
                turretNum = turretDatas[13].turretNum;
                maxHP = turretDatas[13].maxHP;
                power = turretDatas[13].power;
                defense = turretDatas[13].defense;
                attackSpeed = turretDatas[13].attackSpeed;
                range = turretDatas[13].range;
                regeneration = turretDatas[13].regeneration;
                attackRangeType = turretDatas[13].attackRangeType;
                isRecoveryTower = turretDatas[13].isRecoveryTower;
                attackTargetNum = turretDatas[13].attackTargetNum;
                debuffType = turretDatas[13].debuffType;
                debuffDuration = turretDatas[13].debuffDuration;
                multiAttackRange = turretDatas[13].multiAttackRange;
                bulletIndex = turretDatas[13].bulletIndex;
                damageEffectIndex = turretDatas[13].damageEffectIndex;
                deadEffectIndex = turretDatas[13].deadEffectIndex;
                fireEffectIndex = turretDatas[13].fireEffectIndex;
                healEffectIndex = turretDatas[13].healEffectIndex;
                debuffEffectIndex = turretDatas[13].debuffEffectIndex;
                turretAppearPosY = turretDatas[13].turretAppearPosY;
                turretCost = turretDatas[13].turretCost;
                turretConstructionTime = turretDatas[13].turretConstructionTime;
                break;

            case "Turret/ArmamentWoodPeckerTurret":
                Debug.Log("ArmamentWoodPeckerTurret 초기화");
                turretNum = turretDatas[14].turretNum;
                maxHP = turretDatas[14].maxHP;
                power = turretDatas[14].power;
                defense = turretDatas[14].defense;
                attackSpeed = turretDatas[14].attackSpeed;
                range = turretDatas[14].range;
                regeneration = turretDatas[14].regeneration;
                attackRangeType = turretDatas[14].attackRangeType;
                isRecoveryTower = turretDatas[14].isRecoveryTower;
                attackTargetNum = turretDatas[14].attackTargetNum;
                debuffType = turretDatas[14].debuffType;
                debuffDuration = turretDatas[14].debuffDuration;
                multiAttackRange = turretDatas[14].multiAttackRange;
                bulletIndex = turretDatas[14].bulletIndex;
                damageEffectIndex = turretDatas[14].damageEffectIndex;
                deadEffectIndex = turretDatas[14].deadEffectIndex;
                fireEffectIndex = turretDatas[14].fireEffectIndex;
                healEffectIndex = turretDatas[14].healEffectIndex;
                debuffEffectIndex = turretDatas[14].debuffEffectIndex;
                turretAppearPosY = turretDatas[14].turretAppearPosY;
                turretCost = turretDatas[14].turretCost;
                turretConstructionTime = turretDatas[14].turretConstructionTime;
                break;

            case "Turret/ArmamentBabyBirdTurret":
                Debug.Log("ArmamentBabyBirdTurret 초기화");
                turretNum = turretDatas[15].turretNum;
                maxHP = turretDatas[15].maxHP;
                power = turretDatas[15].power;
                defense = turretDatas[15].defense;
                attackSpeed = turretDatas[15].attackSpeed;
                range = turretDatas[15].range;
                regeneration = turretDatas[15].regeneration;
                attackRangeType = turretDatas[15].attackRangeType;
                isRecoveryTower = turretDatas[15].isRecoveryTower;
                attackTargetNum = turretDatas[15].attackTargetNum;
                debuffType = turretDatas[15].debuffType;
                debuffDuration = turretDatas[15].debuffDuration;
                multiAttackRange = turretDatas[15].multiAttackRange;
                bulletIndex = turretDatas[15].bulletIndex;
                damageEffectIndex = turretDatas[15].damageEffectIndex;
                deadEffectIndex = turretDatas[15].deadEffectIndex;
                fireEffectIndex = turretDatas[15].fireEffectIndex;
                healEffectIndex = turretDatas[15].healEffectIndex;
                debuffEffectIndex = turretDatas[15].debuffEffectIndex;
                turretAppearPosY = turretDatas[15].turretAppearPosY;
                turretCost = turretDatas[15].turretCost;
                turretConstructionTime = turretDatas[15].turretConstructionTime;
                break;

            case "Turret/ArmamentPenguinTurret":
                Debug.Log("ArmamentPenguinTurret 초기화");
                turretNum = turretDatas[16].turretNum;
                maxHP = turretDatas[16].maxHP;
                power = turretDatas[16].power;
                defense = turretDatas[16].defense;
                attackSpeed = turretDatas[16].attackSpeed;
                range = turretDatas[16].range;
                regeneration = turretDatas[16].regeneration;
                attackRangeType = turretDatas[16].attackRangeType;
                isRecoveryTower = turretDatas[16].isRecoveryTower;
                attackTargetNum = turretDatas[16].attackTargetNum;
                debuffType = turretDatas[16].debuffType;
                debuffDuration = turretDatas[16].debuffDuration;
                multiAttackRange = turretDatas[16].multiAttackRange;
                bulletIndex = turretDatas[16].bulletIndex;
                damageEffectIndex = turretDatas[16].damageEffectIndex;
                deadEffectIndex = turretDatas[16].deadEffectIndex;
                fireEffectIndex = turretDatas[16].fireEffectIndex;
                healEffectIndex = turretDatas[16].healEffectIndex;
                debuffEffectIndex = turretDatas[16].debuffEffectIndex;
                turretAppearPosY = turretDatas[16].turretAppearPosY;
                turretCost = turretDatas[16].turretCost;
                turretConstructionTime = turretDatas[16].turretConstructionTime;
                break;

            case "Turret/ArmamentHawkTurret":
                Debug.Log("ArmamentHawkTurret 초기화");
                turretNum = turretDatas[17].turretNum;
                maxHP = turretDatas[17].maxHP;
                power = turretDatas[17].power;
                defense = turretDatas[17].defense;
                attackSpeed = turretDatas[17].attackSpeed;
                range = turretDatas[17].range;
                regeneration = turretDatas[17].regeneration;
                attackRangeType = turretDatas[17].attackRangeType;
                isRecoveryTower = turretDatas[17].isRecoveryTower;
                attackTargetNum = turretDatas[17].attackTargetNum;
                debuffType = turretDatas[17].debuffType;
                debuffDuration = turretDatas[17].debuffDuration;
                multiAttackRange = turretDatas[17].multiAttackRange;
                bulletIndex = turretDatas[17].bulletIndex;
                damageEffectIndex = turretDatas[17].damageEffectIndex;
                deadEffectIndex = turretDatas[17].deadEffectIndex;
                fireEffectIndex = turretDatas[17].fireEffectIndex;
                healEffectIndex = turretDatas[17].healEffectIndex;
                debuffEffectIndex = turretDatas[17].debuffEffectIndex;
                turretAppearPosY = turretDatas[17].turretAppearPosY;
                turretCost = turretDatas[17].turretCost;
                turretConstructionTime = turretDatas[17].turretConstructionTime;
                break;

            case "Turret/RailGunTurret":
                Debug.Log("RailGunTurret 초기화");
                turretNum = turretDatas[18].turretNum;
                maxHP = turretDatas[18].maxHP;
                power = turretDatas[18].power;
                defense = turretDatas[18].defense;
                attackSpeed = turretDatas[18].attackSpeed;
                range = turretDatas[18].range;
                regeneration = turretDatas[18].regeneration;
                attackRangeType = turretDatas[18].attackRangeType;
                isRecoveryTower = turretDatas[18].isRecoveryTower;
                attackTargetNum = turretDatas[18].attackTargetNum;
                debuffType = turretDatas[18].debuffType;
                debuffDuration = turretDatas[18].debuffDuration;
                multiAttackRange = turretDatas[18].multiAttackRange;
                bulletIndex = turretDatas[18].bulletIndex;
                damageEffectIndex = turretDatas[18].damageEffectIndex;
                deadEffectIndex = turretDatas[18].deadEffectIndex;
                fireEffectIndex = turretDatas[18].fireEffectIndex;
                healEffectIndex = turretDatas[18].healEffectIndex;
                debuffEffectIndex = turretDatas[18].debuffEffectIndex;
                turretAppearPosY = turretDatas[18].turretAppearPosY;
                turretCost = turretDatas[18].turretCost;
                turretConstructionTime = turretDatas[18].turretConstructionTime;
                break;

            case "Turret/RosasioTurret":
                Debug.Log("RosasioTurret 초기화");
                turretNum = turretDatas[19].turretNum;
                maxHP = turretDatas[19].maxHP;
                power = turretDatas[19].power;
                defense = turretDatas[19].defense;
                attackSpeed = turretDatas[19].attackSpeed;
                range = turretDatas[19].range;
                regeneration = turretDatas[19].regeneration;
                attackRangeType = turretDatas[19].attackRangeType;
                isRecoveryTower = turretDatas[19].isRecoveryTower;
                attackTargetNum = turretDatas[19].attackTargetNum;
                debuffType = turretDatas[19].debuffType;
                debuffDuration = turretDatas[19].debuffDuration;
                multiAttackRange = turretDatas[19].multiAttackRange;
                bulletIndex = turretDatas[19].bulletIndex;
                damageEffectIndex = turretDatas[19].damageEffectIndex;
                deadEffectIndex = turretDatas[19].deadEffectIndex;
                fireEffectIndex = turretDatas[19].fireEffectIndex;
                healEffectIndex = turretDatas[19].healEffectIndex;
                debuffEffectIndex = turretDatas[19].debuffEffectIndex;
                turretAppearPosY = turretDatas[19].turretAppearPosY;
                turretCost = turretDatas[19].turretCost;
                turretConstructionTime = turretDatas[19].turretConstructionTime;
                break;

            case "Turret/SwordBirdTurret":
                Debug.Log("SwordBirdTurret 초기화");
                turretNum = turretDatas[20].turretNum;
                maxHP = turretDatas[20].maxHP;
                power = turretDatas[20].power;
                defense = turretDatas[20].defense;
                attackSpeed = turretDatas[20].attackSpeed;
                range = turretDatas[20].range;
                regeneration = turretDatas[20].regeneration;
                attackRangeType = turretDatas[20].attackRangeType;
                isRecoveryTower = turretDatas[20].isRecoveryTower;
                attackTargetNum = turretDatas[20].attackTargetNum;
                debuffType = turretDatas[20].debuffType;
                debuffDuration = turretDatas[20].debuffDuration;
                multiAttackRange = turretDatas[20].multiAttackRange;
                bulletIndex = turretDatas[20].bulletIndex;
                damageEffectIndex = turretDatas[20].damageEffectIndex;
                deadEffectIndex = turretDatas[20].deadEffectIndex;
                fireEffectIndex = turretDatas[20].fireEffectIndex;
                healEffectIndex = turretDatas[20].healEffectIndex;
                debuffEffectIndex = turretDatas[20].debuffEffectIndex;
                turretAppearPosY = turretDatas[20].turretAppearPosY;
                turretCost = turretDatas[20].turretCost;
                turretConstructionTime = turretDatas[20].turretConstructionTime;
                break;

            case "Turret/KingTurret":
                Debug.Log("KingTurret 초기화");
                turretNum = turretDatas[21].turretNum;
                maxHP = turretDatas[21].maxHP;
                power = turretDatas[21].power;
                defense = turretDatas[21].defense;
                attackSpeed = turretDatas[21].attackSpeed;
                range = turretDatas[21].range;
                regeneration = turretDatas[21].regeneration;
                attackRangeType = turretDatas[21].attackRangeType;
                isRecoveryTower = turretDatas[21].isRecoveryTower;
                attackTargetNum = turretDatas[21].attackTargetNum;
                debuffType = turretDatas[21].debuffType;
                debuffDuration = turretDatas[21].debuffDuration;
                multiAttackRange = turretDatas[21].multiAttackRange;
                bulletIndex = turretDatas[21].bulletIndex;
                damageEffectIndex = turretDatas[21].damageEffectIndex;
                deadEffectIndex = turretDatas[21].deadEffectIndex;
                fireEffectIndex = turretDatas[21].fireEffectIndex;
                healEffectIndex = turretDatas[21].healEffectIndex;
                debuffEffectIndex = turretDatas[21].debuffEffectIndex;
                turretAppearPosY = turretDatas[21].turretAppearPosY;
                turretCost = turretDatas[21].turretCost;
                turretConstructionTime = turretDatas[21].turretConstructionTime;
                break;

            case "Turret/WhiteKingTurret":
                Debug.Log("WhiteKingTurret 초기화");
                turretNum = turretDatas[22].turretNum;
                maxHP = turretDatas[22].maxHP;
                power = turretDatas[22].power;
                defense = turretDatas[22].defense;
                attackSpeed = turretDatas[22].attackSpeed;
                range = turretDatas[22].range;
                regeneration = turretDatas[22].regeneration;
                attackRangeType = turretDatas[22].attackRangeType;
                isRecoveryTower = turretDatas[22].isRecoveryTower;
                attackTargetNum = turretDatas[22].attackTargetNum;
                debuffType = turretDatas[22].debuffType;
                debuffDuration = turretDatas[22].debuffDuration;
                multiAttackRange = turretDatas[22].multiAttackRange;
                bulletIndex = turretDatas[22].bulletIndex;
                damageEffectIndex = turretDatas[22].damageEffectIndex;
                deadEffectIndex = turretDatas[22].deadEffectIndex;
                fireEffectIndex = turretDatas[22].fireEffectIndex;
                healEffectIndex = turretDatas[22].healEffectIndex;
                debuffEffectIndex = turretDatas[22].debuffEffectIndex;
                turretAppearPosY = turretDatas[22].turretAppearPosY;
                turretCost = turretDatas[22].turretCost;
                turretConstructionTime = turretDatas[22].turretConstructionTime;
                break;

            case "Turret/Base":
                Debug.Log("Base 초기화");
                turretNum = turretDatas[23].turretNum;
                maxHP = turretDatas[23].maxHP;
                power = turretDatas[23].power;
                defense = turretDatas[23].defense;
                attackSpeed = turretDatas[23].attackSpeed;
                range = turretDatas[23].range;
                regeneration = turretDatas[23].regeneration;
                attackRangeType = turretDatas[23].attackRangeType;
                isRecoveryTower = turretDatas[23].isRecoveryTower;
                attackTargetNum = turretDatas[23].attackTargetNum;
                debuffType = turretDatas[23].debuffType;
                debuffDuration = turretDatas[23].debuffDuration;
                multiAttackRange = turretDatas[23].multiAttackRange;
                bulletIndex = turretDatas[23].bulletIndex;
                damageEffectIndex = turretDatas[23].damageEffectIndex;
                deadEffectIndex = turretDatas[23].deadEffectIndex;
                fireEffectIndex = turretDatas[23].fireEffectIndex;
                healEffectIndex = turretDatas[23].healEffectIndex;
                debuffEffectIndex = turretDatas[23].debuffEffectIndex;
                turretAppearPosY = turretDatas[23].turretAppearPosY;
                turretCost = turretDatas[23].turretCost;
                turretConstructionTime = turretDatas[23].turretConstructionTime;
                break;

            default:
                break;
        }
    }
	#endregion
}
