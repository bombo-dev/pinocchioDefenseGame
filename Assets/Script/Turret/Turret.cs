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
    [SerializeField]
    int turretCost;
    //터렛 건설에 걸리는 시간
    [SerializeField]
    int turretConstructionTime;

    public StatusMngPanel statusMngPanel;

    public DamageMngPanel damageMngPanel;

    //JsonData
    [SerializeField]
    protected TurretDatas[] turretDatas;

    private void Start()
    {
        Initialize();
    }
    protected override void UpdateActor()
    {
        base.UpdateActor();

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
       // TurretInitializing();
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

        //베이스 터렛일경우 베이스터렛 HP Panel갱신
        if (turretNum == SystemManager.Instance.TurretManager.BASETURRET_INDEX)
        {
            if (SystemManager.Instance.PanelManager.turretMngPanel)
                SystemManager.Instance.PanelManager.turretMngPanel.UpdateSlideBar();
        }

        //StatusMngPanel 갱신
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
            if (turretNum == SystemManager.Instance.TurretManager.BASETURRET_INDEX)
            {
                //싱글톤 캐싱
                PanelManager pm = SystemManager.Instance.PanelManager;

                //게임오버 상태
                SystemManager.Instance.GameFlowManager.gameState = GameFlowManager.GameState.StageFail;

                //스테이지 비활성화
                SystemManager.Instance.GameFlowManager.DisableStage();

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
                _nest.haveTurret = false;
                _nest.turret = null;

                //현재 선택하고 있는 둥지위에있는 터렛이 Dead상태로 변하고 Turret정보 UI가 켜져있을 경우 UI off
                //UI_TurretInfoPanel 패널이 존재할 경우
                if (SystemManager.Instance.PanelManager.turretInfoPanel && System.Object.ReferenceEquals(SystemManager.Instance.InputManager.currenstSelectNest, _nest.gameObject))
                {
                    //패널 정보 갱신
                    SystemManager.Instance.PanelManager.turretInfoPanel.Reset();
                }
                if(!SystemManager.Instance.PanelManager.turretInfoPanel)
                    SystemManager.Instance.PanelManager.EnablePanel<UI_TurretMngPanel>(0); //0: UI_TurretMngPanel

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


        statusMngPanel.RemoveDebuff(null, 0, debuffIndex);
    }
    #endregion

    #region 버프

    /*
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
    */

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
            //기존에 있던 버프 삭제
            for (int i = 0; i < Enum.GetValues(typeof(buff)).Length; i++)
            {
                //버프 삭제
                if (buffs.ContainsKey((buff)i))
                    RemoveBuff(i);
            }

            //버프추가

            buffs.Add(_buffIndex, time);   //딕셔너리 자료구조에 추가

            //버프 효과
            switch (buffIndex)
            {
                case 1: //공격력 증가
                    currentPower += (currentPower / 3);
                    break;
                case 2: //공격속도 증가
                    currentAttackSpeed *= 0.8f;
                    break;
                case 3: //회복력 증가 + 즉시 회복
                    currentRegeneration += (currentRegeneration / 3);
                    break;
                case 4: //방어력 증가
                    currentDefense += (currentDefense / 2);
                    break;
                case 5: //사거리 증가
                    if (currentRange < 9000)
                    {
                        currentRange += (currentRange / 3);
                        currentMultiAttackRange += (currentMultiAttackRange / 3);
                    }
                    else //일정사거리 이상 유닛은 고정값 증가
                    {
                        currentRange += 2500;
                        currentMultiAttackRange += 2500;
                    }
                    break;
                case 6: //올스텟 증가
                    currentPower += (currentPower / 3);
                    currentDefense += (currentDefense / 3);
                    currentRegeneration += (currentRegeneration / 3);
                    break;
            }

            //이펙트 생성
            EnableBuffEffect(buffIndex - 1);

            //Upgrade 사운드 출력
            if (upgradeSoundClip)
                SoundEffectManager.Instance.ChangeEffectAudioClip(upgradeSoundClip);
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
    /// <param name="debuffIndex">제거할 버프거</param>
    protected void RemoveBuff(int buffIndex = 0)
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
                Debug.Log("PigeonTurret 초기화");
                TurretVariableInitialize(turretDatas, 0);
                break;

            case "Turret/WoodpeckerTurret":
                Debug.Log("WoodpeckerTurret 초기화");
                TurretVariableInitialize(turretDatas, 1);
                break;

            case "Turret/BabyBirdTurret":
                Debug.Log("BabyBirdTurret 초기화");
                TurretVariableInitialize(turretDatas, 2);
                break;

            case "Turret/UpgradedPigeonTurret":
                Debug.Log("UpgradedPigeonTurret 초기화");
                TurretVariableInitialize(turretDatas, 3);
                break;

            case "Turret/PelicanTurret":
                Debug.Log("PelicanTurret 초기화");
                TurretVariableInitialize(turretDatas, 4);
                break;

            case "Turret/CuckooTurret":
                Debug.Log("CuckooTurret 초기화");
                TurretVariableInitialize(turretDatas, 5);
                break;

            case "Turret/PenguinTurret":
                Debug.Log("PenguinTurret 초기화");
                TurretVariableInitialize(turretDatas, 6);
                break;

            case "Turret/OwlTurret":
                Debug.Log("OwlTurret 초기화");
                TurretVariableInitialize(turretDatas, 7);
                break;

            case "Turret/UpgradedWoodPecker":
                Debug.Log("UpgradedWoodPecker 초기화");
                TurretVariableInitialize(turretDatas, 8);
                break;

            case "Turret/HawkTurret":
                Debug.Log("HawkTurret 초기화");
                TurretVariableInitialize(turretDatas, 9);
                break;

            case "Turret/EagleTurret":
                Debug.Log("EagleTurret 초기화");
                TurretVariableInitialize(turretDatas, 10);
                break;

            case "Turret/UpgradedBabyBirdTurret":
                Debug.Log("UpgradedBabyBirdTurret 초기화");
                TurretVariableInitialize(turretDatas, 11);
                break;

            case "Turret/UpgradedEagleTurret":
                Debug.Log("UpgradedEagleTurret 초기화");
                TurretVariableInitialize(turretDatas, 12);
                break;

            case "Turret/ArmamentPigeonTurret":
                Debug.Log("ArmamentPigeonTurret 초기화");
                TurretVariableInitialize(turretDatas, 13);
                break;

            case "Turret/ArmamentWoodPeckerTurret":
                Debug.Log("ArmamentWoodPeckerTurret 초기화");
                TurretVariableInitialize(turretDatas, 14);
                break;

            case "Turret/ArmamentBabyBirdTurret":
                Debug.Log("ArmamentBabyBirdTurret 초기화");
                TurretVariableInitialize(turretDatas, 15);
                break;

            case "Turret/ArmamentPenguinTurret":
                Debug.Log("ArmamentPenguinTurret 초기화");
                TurretVariableInitialize(turretDatas, 16);
                break;

            case "Turret/ArmamentHawkTurret":
                Debug.Log("ArmamentHawkTurret 초기화");
                TurretVariableInitialize(turretDatas, 17);
                break;

            case "Turret/RailGunTurret":
                Debug.Log("RailGunTurret 초기화");
                TurretVariableInitialize(turretDatas, 18);
                break;

            case "Turret/RosasioTurret":
                Debug.Log("RosasioTurret 초기화");
                TurretVariableInitialize(turretDatas, 19);
                break;

            case "Turret/SwordBirdTurret":
                Debug.Log("SwordBirdTurret 초기화");
                TurretVariableInitialize(turretDatas, 20);
                break;

            case "Turret/KingTurret":
                Debug.Log("KingTurret 초기화");
                TurretVariableInitialize(turretDatas, 21);
                break;

            case "Turret/WhiteKingTurret":
                Debug.Log("WhiteKingTurret 초기화");
                TurretVariableInitialize(turretDatas, 22);
                break;

            case "Turret/Base":
                Debug.Log("Base 초기화");
                TurretVariableInitialize(turretDatas, 23);
                break;

            default:
                break;
        }
    }

    void TurretVariableInitialize(TurretDatas[] data, int num)
    {
        turretNum = data[num].turretNum;
        maxHP = data[num].maxHP;
        power = data[num].power;
        defense = data[num].defense;
        attackSpeed = data[num].attackSpeed;
        range = data[num].range;
        regeneration = data[num].regeneration;
        attackRangeType = data[num].attackRangeType;
        isRecoveryTower = data[num].isRecoveryTower;
        attackTargetNum = data[num].attackTargetNum;
        debuffType = data[num].debuffType;
        debuffDuration = data[num].debuffDuration;
        multiAttackRange = data[num].multiAttackRange;
        bulletIndex = data[num].bulletIndex;
        damageEffectIndex = data[num].damageEffectIndex;
        deadEffectIndex = data[num].deadEffectIndex;
        fireEffectIndex = data[num].fireEffectIndex;
        healEffectIndex = data[num].healEffectIndex;
        debuffEffectIndex = data[num].debuffEffectIndex;
        turretAppearPosY = data[num].turretAppearPosY;
        turretCost = data[num].turretCost;
        turretConstructionTime = data[num].turretConstructionTime;
    }
	#endregion
}
