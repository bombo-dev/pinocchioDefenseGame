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

    [SerializeField]
    int currentSpeed; //현재 이동속도

    [Header("EnemyInfo")]   //Enemy 정보

    public int enemyNum;   //enemy 종류 번호 (enemy 종류에 따라 번호 부여)

    [SerializeField]
    public int enemyIndex;  //enemy고유 번호

    [SerializeField]
    public int gateNum;    //생성 게이트 번호

    [SerializeField]
    Vector3[] appearPos;  //생성위치

    [Header("Move")]    //이동관련

    [SerializeField]
    public GameObject[] targetPoint;    //타일맵 위에 있는 이동 타겟

    [SerializeField]
    int targetPointIndex = 0;    //타일맵 타겟 인덱스

    [SerializeField]
    GameObject currentTarget;   //현재 타겟

    Vector3 dirVec; //이동처리할 방향벡터

    bool isEndShow = false; // 코루틴의 종료여부 확인 플래그

    int i = 0;

    bool selfDestruct = false;  //자폭일 경우 true

    [SerializeField]
    int rewardWoodResource; //잡았을때 보상 woodResource

    // 에너미의 hpBar 패널
    public StatusMngPanel statusMngPanel;

    public DamageMngPanel damageMngPanel;

    /// <summary>
    /// 초기화 함수 : 김현진
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        EnemyInitializing();
        Reset();
    }

    public override void Reset()
    {
        base.Reset();

        //이동속도 초기화
        currentSpeed = speed;

        //사거리 초기화 - 게이트를 완전히 빠져나오기 전까진(targetIndex > 0) Walk상태 
        currentRange = 0;

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
                UpdatePanelPos();
                CheckArrive();
                UpdateMove(dirVec);

                if (!isRecoveryTower)    //공격 유닛
                    DetectTarget(SystemManager.Instance.TurretManager.turrets);
                else    //회복 유닛
                    DetectTarget(SystemManager.Instance.EnemyManager.enemies, gameObject);
                break;
            case EnemyState.Battle:
                UpdatePanelPos();
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
        //transform.position = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);

        //사거리초기화
        if (targetPointIndex == 0)
        {
            //사거리 초기화 - 게이트를 완전히 빠져나온 상태
            currentRange = range;
        }

        //타겟 인덱스 증가
        targetPointIndex++;

        //마지막 타겟에 도착했을경우
        if (targetPointIndex >= targetPoint.Length)
        {
            //공격 타겟이 아닌경우 폭발 처리
            if (attackTargetNum <= 0 || isRecoveryTower)
            {
                selfDestruct = true;

                //Dead처리 하기
                DecreaseHP(0);

                //Base터렛 타격
                SystemManager.Instance.TurretManager.turrets[0].GetComponent<Turret>().DecreaseHP(power);
            }
            return;
        }

        //타겟 변경
        currentTarget = targetPoint[targetPointIndex];
        //방향벡터 변경된 타겟으로 갱신 
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

        Vector3 updateVec = new Vector3(dirVec.x * currentSpeed * Time.deltaTime, 0, dirVec.z * currentSpeed * Time.deltaTime);
        transform.position += updateVec;
        Quaternion rotation = Quaternion.LookRotation(-dirVec);

        //회전
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
        //회전 x,z축 고정
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    /// <summary>
    /// this객체의 사거리 안에있는 타겟을 감지해 그중 공격할 타겟을 지정 : 김현진
    /// </summary>
    protected override void DetectTarget(List<GameObject> target, GameObject mine = null)
    {
        if (mine)   //회복타워가 자신을 감지하지 않게한다
            base.DetectTarget(target, mine);
        else
            base.DetectTarget(target);
    }

    public void UpdateEnemyPos(GameObject go)
    {
        go.transform.position = transform.position;
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
        if (Time.time - attackTimer > currentAttackSpeed)
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

            //회복타워일경우
            if (isRecoveryTower)
            {
                animator.SetBool("attack", false);
                animator.SetBool("finAttack", true);

                enemyState = EnemyState.Walk;

                return;
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
                //공격 이펙트 호출
                EnableFireEffect(this);
            }
        }

    }

    /// <summary>
    /// 적의 HP 감소와 사망처리 : 하은비
    /// </summary>
    /// <param name="damage"></param>
    #endregion

    #region Dead - HP 감소와 사망

    public override void DecreaseHP(int damage)
    {
        base.DecreaseHP(damage);

        if (statusMngPanel)
        {            
            statusMngPanel.SetHPBar(currentHP, maxHP);
        }
        else
            Debug.Log("statusMngPanel is null");

 




        //GameObject go = SystemManager.Instance.EnemyManager.enemies[enemyIndex].gameObject;
        //hitPos = go.GetComponent<Enemy>().hitPos;
        //Debug.Log("hitted enemy=" + hitPos.GetComponent<GameObject>().name);

        /*
        if (isEndShow == false)
        {
            StartCoroutine(showDmgCoroutine());
            Debug.Log("2. isEndShow=" + isEndShow);
        }
        else
        {
            StopCoroutine(showDmgCoroutine());
            Debug.Log("3. isEndShow=" + isEndShow);
            if (SystemManager.Instance.PanelManager.damageMngPanel == null)
                Debug.Log("Disable Panel Successed");
            else
                Debug.Log("Disable Pabel UnSuccessed");
            SystemManager.Instance.PanelManager.DisablePanel<DamageMngPanel>(SystemManager.Instance.PanelManager.damageMngPanel.gameObject);
            
            if (SystemManager.Instance.PanelManager.damageMngPanel == null)
                Debug.Log("Disable Panel Successed");
            else
                Debug.Log("Disable Pabel UnSuccessed");
        }
        */

        //HP가 0밑으로 떨어지거나 자폭상태가 될 경우
        if (currentHP <= 0 || selfDestruct)
        {
            //StatusMngPanel 비활성화
            SystemManager.Instance.PanelManager.DisablePanel<StatusMngPanel>(statusMngPanel.gameObject);

            // StatusMngPanel 리셋
            statusMngPanel.StatusReset();
          
            // 에너미 리스트 재구성
            SystemManager.Instance.EnemyManager.ReorganizationEnemiesList(enemyIndex);

            enemyState = EnemyState.Dead;

            //퇴치
            if (!selfDestruct)
                //잡았을때 보상 지급
                SystemManager.Instance.ResourceManager.IncreaseWoodResource(rewardWoodResource);
            //자폭
            else
            {
                //자폭 이펙트 출력
                EnableDamageEffect(this);

                //Flash효과 
                callFlashCoroutine(ShaderController.RED);

                //Dead처리
                currentHP = 0;
                animator.SetBool("isDead", true);
                animator.Play("Dead");

                return;
            }
        }
    }

    IEnumerator showDmgCoroutine()
    {
        yield return new WaitForSeconds(1);
        isEndShow = true;
        Debug.Log("1. idEndShow=" + isEndShow);
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
    
    #region 디버프
    /// <summary>
    /// 디버프 추가 : 김현진
    /// </summary>
    /// <param name="debuffIndex">추가할 디버프 종류 인덱스</param>
    /// <param name="time">추가할 디버프의 지속시간</param>
    public override void AddDebuff(int debuffIndex, float time)
    {
        base.AddDebuff(debuffIndex, time);

        //풀스택일경우 예외처리
        if (debuffs[(debuff)debuffIndex].stack > 5)
            return;
        
        statusMngPanel.SetDebuff(debuffIndex, debuffs, time);


        //디버프 효과
        switch (debuffIndex)
        {
            case 1: //공격 속도 감소
                currentAttackSpeed *= 1.2f;
                break;
            case 2: //이동 속도 감소
                currentSpeed -= (currentSpeed / 5);
                break;
            case 3: //방어력 감소
                currentDefense -= (currentDefense / 5);
                break;
            case 4: //공격력 감소
                currentPower -= (currentPower / 5);
                break;
            case 5: //감전 - 공격속도, 이동속도 대폭감소
                currentPower -= (currentPower / 2);
                currentSpeed -= (currentSpeed / 2);
                break;
            case 6: //화상 - 방어력 대폭감소
                currentDefense -= (currentDefense / 2);
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
            case 2: //이동 속도 초기화
                currentSpeed = speed;
                break;
            case 3: //방어력 초기화
                currentDefense = defense;
                break;
            case 4: //공격력 초기화
                currentPower = power;
                break;
            case 5: //감전 - 공격속도, 이동속도 초기화
                currentPower = power;
                currentSpeed = speed;
                break;
            case 6: //화상 - 방어력 초기화
                currentDefense = defense;
                break;
        }

        
        statusMngPanel.RemoveDebuff(debuffIndex, debuffs);
    }
    #endregion

    protected override void UpdatePanelPos()
    {
        base.UpdatePanelPos();

        if (statusMngPanel)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(hpPos.transform.position);
            statusMngPanel.transform.position = screenPos;
        }

    }



    #endregion

    #region Enemy 스탯 초기화

    void EnemyInitializing()
    {
        enemyDatas = SystemManager.Instance.EnemyJson.GetEnemyData();
        appearPos = new Vector3[3];

        switch (filePath)
        {
            case "Enemy/Larva":
                Debug.Log("Larva 초기화 완료");
                enemyNum = enemyDatas[0].enemyNum;
                enemyIndex = enemyDatas[0].enemyIndex;
                maxHP = enemyDatas[0].maxHP;
                power = enemyDatas[0].power;
                defense = enemyDatas[0].defense;
                speed = enemyDatas[0].speed;
                attackSpeed = enemyDatas[0].attackSpeed;
                range = enemyDatas[0].range;
                regeneration = enemyDatas[0].regeneration;
                attackRangeType = enemyDatas[0].attackRangeType;
                isRecoveryTower = enemyDatas[0].isRecoveryTower;
                selfDestruct = enemyDatas[0].selfDestruct;
                attackTargetNum = enemyDatas[0].attackTargetNum;
                debuffType = enemyDatas[0].debuffType;
                debuffDuration = enemyDatas[0].debuffDuration;
                multiAttackRange = enemyDatas[0].multiAttackRange;
                bulletIndex = enemyDatas[0].bulletIndex;
                damageEffectIndex = enemyDatas[0].damageEffectIndex;
                deadEffectIndex = enemyDatas[0].deadEffectIndex;
                fireEffectIndex = enemyDatas[0].fireEffectIndex;
                healEffectIndex = enemyDatas[0].healEffectIndex;
                debuffEffectIndex = enemyDatas[0].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[0].appearPos[0].X, enemyDatas[0].appearPos[0].Y, enemyDatas[0].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[0].appearPos[1].X, enemyDatas[0].appearPos[1].Y, enemyDatas[0].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[0].appearPos[2].X, enemyDatas[0].appearPos[2].Y, enemyDatas[0].appearPos[2].Z);
                rewardWoodResource = enemyDatas[0].rewardWoodResource;
                break;
            case "Enemy/SwordMan":
                Debug.Log("SwordMan 초기화 완료");
                enemyNum = enemyDatas[1].enemyNum;
                enemyIndex = enemyDatas[1].enemyIndex;
                maxHP = enemyDatas[1].maxHP;
                power = enemyDatas[1].power;
                defense = enemyDatas[1].defense;
                speed = enemyDatas[1].speed;
                attackSpeed = enemyDatas[1].attackSpeed;
                range = enemyDatas[1].range;
                regeneration = enemyDatas[1].regeneration;
                attackRangeType = enemyDatas[1].attackRangeType;
                isRecoveryTower = enemyDatas[1].isRecoveryTower;
                selfDestruct = enemyDatas[1].selfDestruct;
                attackTargetNum = enemyDatas[1].attackTargetNum;
                debuffType = enemyDatas[1].debuffType;
                debuffDuration = enemyDatas[1].debuffDuration;
                multiAttackRange = enemyDatas[1].multiAttackRange;
                bulletIndex = enemyDatas[1].bulletIndex;
                damageEffectIndex = enemyDatas[1].damageEffectIndex;
                deadEffectIndex = enemyDatas[1].deadEffectIndex;
                fireEffectIndex = enemyDatas[1].fireEffectIndex;
                healEffectIndex = enemyDatas[1].healEffectIndex;
                debuffEffectIndex = enemyDatas[1].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[1].appearPos[0].X, enemyDatas[1].appearPos[0].Y, enemyDatas[1].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[1].appearPos[1].X, enemyDatas[1].appearPos[1].Y, enemyDatas[1].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[1].appearPos[2].X, enemyDatas[1].appearPos[2].Y, enemyDatas[1].appearPos[2].Z);
                rewardWoodResource = enemyDatas[1].rewardWoodResource;
                break;
            case "Enemy/CannonLarva":
                Debug.Log("CannonLarva 초기화 완료");
                enemyNum = enemyDatas[2].enemyNum;
                enemyIndex = enemyDatas[2].enemyIndex;
                maxHP = enemyDatas[2].maxHP;
                power = enemyDatas[2].power;
                defense = enemyDatas[2].defense;
                speed = enemyDatas[2].speed;
                attackSpeed = enemyDatas[2].attackSpeed;
                range = enemyDatas[2].range;
                regeneration = enemyDatas[2].regeneration;
                attackRangeType = enemyDatas[2].attackRangeType;
                isRecoveryTower = enemyDatas[2].isRecoveryTower;
                selfDestruct = enemyDatas[2].selfDestruct;
                attackTargetNum = enemyDatas[2].attackTargetNum;
                debuffType = enemyDatas[2].debuffType;
                debuffDuration = enemyDatas[2].debuffDuration;
                multiAttackRange = enemyDatas[2].multiAttackRange;
                bulletIndex = enemyDatas[2].bulletIndex;
                damageEffectIndex = enemyDatas[2].damageEffectIndex;
                deadEffectIndex = enemyDatas[2].deadEffectIndex;
                fireEffectIndex = enemyDatas[2].fireEffectIndex;
                healEffectIndex = enemyDatas[2].healEffectIndex;
                debuffEffectIndex = enemyDatas[2].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[2].appearPos[0].X, enemyDatas[2].appearPos[0].Y, enemyDatas[2].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[2].appearPos[1].X, enemyDatas[2].appearPos[1].Y, enemyDatas[2].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[2].appearPos[2].X, enemyDatas[2].appearPos[2].Y, enemyDatas[2].appearPos[2].Z);
                rewardWoodResource = enemyDatas[0].rewardWoodResource;
                break;
            case "Enemy/Wagon":
                Debug.Log("Wagon 초기화 완료");
                enemyNum = enemyDatas[3].enemyNum;
                enemyIndex = enemyDatas[3].enemyIndex;
                maxHP = enemyDatas[3].maxHP;
                power = enemyDatas[3].power;
                defense = enemyDatas[3].defense;
                speed = enemyDatas[3].speed;
                attackSpeed = enemyDatas[3].attackSpeed;
                range = enemyDatas[3].range;
                regeneration = enemyDatas[3].regeneration;
                attackRangeType = enemyDatas[3].attackRangeType;
                isRecoveryTower = enemyDatas[3].isRecoveryTower;
                selfDestruct = enemyDatas[3].selfDestruct;
                attackTargetNum = enemyDatas[3].attackTargetNum;
                debuffType = enemyDatas[3].debuffType;
                debuffDuration = enemyDatas[3].debuffDuration;
                multiAttackRange = enemyDatas[3].multiAttackRange;
                bulletIndex = enemyDatas[3].bulletIndex;
                damageEffectIndex = enemyDatas[3].damageEffectIndex;
                deadEffectIndex = enemyDatas[3].deadEffectIndex;
                fireEffectIndex = enemyDatas[3].fireEffectIndex;
                healEffectIndex = enemyDatas[3].healEffectIndex;
                debuffEffectIndex = enemyDatas[3].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[3].appearPos[0].X, enemyDatas[3].appearPos[0].Y, enemyDatas[3].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[3].appearPos[1].X, enemyDatas[3].appearPos[1].Y, enemyDatas[3].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[3].appearPos[2].X, enemyDatas[3].appearPos[2].Y, enemyDatas[3].appearPos[2].Z);
                rewardWoodResource = enemyDatas[3].rewardWoodResource;
                break;
            case "Enemy/StagBeetle":
                Debug.Log("StagBeetle 초기화 완료");
                enemyNum = enemyDatas[4].enemyNum;
                enemyIndex = enemyDatas[4].enemyIndex;
                maxHP = enemyDatas[4].maxHP;
                power = enemyDatas[4].power;
                defense = enemyDatas[4].defense;
                speed = enemyDatas[4].speed;
                attackSpeed = enemyDatas[4].attackSpeed;
                range = enemyDatas[4].range;
                regeneration = enemyDatas[4].regeneration;
                attackRangeType = enemyDatas[4].attackRangeType;
                isRecoveryTower = enemyDatas[4].isRecoveryTower;
                selfDestruct = enemyDatas[4].selfDestruct;
                attackTargetNum = enemyDatas[4].attackTargetNum;
                debuffType = enemyDatas[4].debuffType;
                debuffDuration = enemyDatas[4].debuffDuration;
                multiAttackRange = enemyDatas[4].multiAttackRange;
                bulletIndex = enemyDatas[4].bulletIndex;
                damageEffectIndex = enemyDatas[4].damageEffectIndex;
                deadEffectIndex = enemyDatas[4].deadEffectIndex;
                fireEffectIndex = enemyDatas[4].fireEffectIndex;
                healEffectIndex = enemyDatas[4].healEffectIndex;
                debuffEffectIndex = enemyDatas[4].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[4].appearPos[0].X, enemyDatas[4].appearPos[0].Y, enemyDatas[4].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[4].appearPos[1].X, enemyDatas[4].appearPos[1].Y, enemyDatas[4].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[4].appearPos[2].X, enemyDatas[4].appearPos[2].Y, enemyDatas[4].appearPos[2].Z);
                rewardWoodResource = enemyDatas[4].rewardWoodResource;
                break;
            case "Enemy/ShieldMan":
                Debug.Log("ShieldMan 초기화 완료");
                enemyNum = enemyDatas[5].enemyNum;
                enemyIndex = enemyDatas[5].enemyIndex;
                maxHP = enemyDatas[5].maxHP;
                power = enemyDatas[5].power;
                defense = enemyDatas[5].defense;
                speed = enemyDatas[5].speed;
                attackSpeed = enemyDatas[5].attackSpeed;
                range = enemyDatas[5].range;
                regeneration = enemyDatas[5].regeneration;
                attackRangeType = enemyDatas[5].attackRangeType;
                isRecoveryTower = enemyDatas[5].isRecoveryTower;
                selfDestruct = enemyDatas[5].selfDestruct;
                attackTargetNum = enemyDatas[5].attackTargetNum;
                debuffType = enemyDatas[5].debuffType;
                debuffDuration = enemyDatas[5].debuffDuration;
                multiAttackRange = enemyDatas[5].multiAttackRange;
                bulletIndex = enemyDatas[5].bulletIndex;
                damageEffectIndex = enemyDatas[5].damageEffectIndex;
                deadEffectIndex = enemyDatas[5].deadEffectIndex;
                fireEffectIndex = enemyDatas[5].fireEffectIndex;
                healEffectIndex = enemyDatas[5].healEffectIndex;
                debuffEffectIndex = enemyDatas[5].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[5].appearPos[0].X, enemyDatas[5].appearPos[0].Y, enemyDatas[5].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[5].appearPos[1].X, enemyDatas[5].appearPos[1].Y, enemyDatas[5].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[5].appearPos[2].X, enemyDatas[5].appearPos[2].Y, enemyDatas[5].appearPos[2].Z);
                rewardWoodResource = enemyDatas[5].rewardWoodResource;
                break;
            case "Enemy/SpearMan":
                Debug.Log("SpearMan 초기화 완료");
                enemyNum = enemyDatas[6].enemyNum;
                enemyIndex = enemyDatas[6].enemyIndex;
                maxHP = enemyDatas[6].maxHP;
                power = enemyDatas[6].power;
                defense = enemyDatas[6].defense;
                speed = enemyDatas[6].speed;
                attackSpeed = enemyDatas[6].attackSpeed;
                range = enemyDatas[6].range;
                regeneration = enemyDatas[6].regeneration;
                attackRangeType = enemyDatas[6].attackRangeType;
                isRecoveryTower = enemyDatas[6].isRecoveryTower;
                selfDestruct = enemyDatas[6].selfDestruct;
                attackTargetNum = enemyDatas[6].attackTargetNum;
                debuffType = enemyDatas[6].debuffType;
                debuffDuration = enemyDatas[6].debuffDuration;
                multiAttackRange = enemyDatas[6].multiAttackRange;
                bulletIndex = enemyDatas[6].bulletIndex;
                damageEffectIndex = enemyDatas[6].damageEffectIndex;
                deadEffectIndex = enemyDatas[6].deadEffectIndex;
                fireEffectIndex = enemyDatas[6].fireEffectIndex;
                healEffectIndex = enemyDatas[6].healEffectIndex;
                debuffEffectIndex = enemyDatas[6].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[6].appearPos[0].X, enemyDatas[6].appearPos[0].Y, enemyDatas[6].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[6].appearPos[1].X, enemyDatas[6].appearPos[1].Y, enemyDatas[6].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[6].appearPos[2].X, enemyDatas[6].appearPos[2].Y, enemyDatas[6].appearPos[2].Z);
                rewardWoodResource = enemyDatas[6].rewardWoodResource;
                break;
            case "Enemy/HorseMan":
                Debug.Log("HorseMan 초기화 완료");
                enemyNum = enemyDatas[7].enemyNum;
                enemyIndex = enemyDatas[7].enemyIndex;
                maxHP = enemyDatas[7].maxHP;
                power = enemyDatas[7].power;
                defense = enemyDatas[7].defense;
                speed = enemyDatas[7].speed;
                attackSpeed = enemyDatas[7].attackSpeed;
                range = enemyDatas[7].range;
                regeneration = enemyDatas[7].regeneration;
                attackRangeType = enemyDatas[7].attackRangeType;
                isRecoveryTower = enemyDatas[7].isRecoveryTower;
                selfDestruct = enemyDatas[7].selfDestruct;
                attackTargetNum = enemyDatas[7].attackTargetNum;
                debuffType = enemyDatas[7].debuffType;
                debuffDuration = enemyDatas[7].debuffDuration;
                multiAttackRange = enemyDatas[7].multiAttackRange;
                bulletIndex = enemyDatas[7].bulletIndex;
                damageEffectIndex = enemyDatas[7].damageEffectIndex;
                deadEffectIndex = enemyDatas[7].deadEffectIndex;
                fireEffectIndex = enemyDatas[7].fireEffectIndex;
                healEffectIndex = enemyDatas[7].healEffectIndex;
                debuffEffectIndex = enemyDatas[7].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[7].appearPos[0].X, enemyDatas[7].appearPos[0].Y, enemyDatas[7].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[7].appearPos[1].X, enemyDatas[7].appearPos[1].Y, enemyDatas[7].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[7].appearPos[2].X, enemyDatas[7].appearPos[2].Y, enemyDatas[7].appearPos[2].Z);
                rewardWoodResource = enemyDatas[7].rewardWoodResource;
                break;
            case "Enemy/Bull":
                Debug.Log("Bull 초기화 완료");
                enemyNum = enemyDatas[8].enemyNum;
                enemyIndex = enemyDatas[8].enemyIndex;
                maxHP = enemyDatas[8].maxHP;
                power = enemyDatas[8].power;
                defense = enemyDatas[8].defense;
                speed = enemyDatas[8].speed;
                attackSpeed = enemyDatas[8].attackSpeed;
                range = enemyDatas[8].range;
                regeneration = enemyDatas[8].regeneration;
                attackRangeType = enemyDatas[8].attackRangeType;
                isRecoveryTower = enemyDatas[8].isRecoveryTower;
                selfDestruct = enemyDatas[8].selfDestruct;
                attackTargetNum = enemyDatas[8].attackTargetNum;
                debuffType = enemyDatas[8].debuffType;
                debuffDuration = enemyDatas[8].debuffDuration;
                multiAttackRange = enemyDatas[8].multiAttackRange;
                bulletIndex = enemyDatas[8].bulletIndex;
                damageEffectIndex = enemyDatas[8].damageEffectIndex;
                deadEffectIndex = enemyDatas[8].deadEffectIndex;
                fireEffectIndex = enemyDatas[8].fireEffectIndex;
                healEffectIndex = enemyDatas[8].healEffectIndex;
                debuffEffectIndex = enemyDatas[8].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[8].appearPos[0].X, enemyDatas[8].appearPos[0].Y, enemyDatas[8].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[8].appearPos[1].X, enemyDatas[8].appearPos[1].Y, enemyDatas[8].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[8].appearPos[2].X, enemyDatas[8].appearPos[2].Y, enemyDatas[8].appearPos[2].Z);
                rewardWoodResource = enemyDatas[8].rewardWoodResource;
                break;
            case "Enemy/BowMan":
                Debug.Log("BowMan 초기화 완료");
                enemyNum = enemyDatas[9].enemyNum;
                enemyIndex = enemyDatas[9].enemyIndex;
                maxHP = enemyDatas[9].maxHP;
                power = enemyDatas[9].power;
                defense = enemyDatas[9].defense;
                speed = enemyDatas[9].speed;
                attackSpeed = enemyDatas[9].attackSpeed;
                range = enemyDatas[9].range;
                regeneration = enemyDatas[9].regeneration;
                attackRangeType = enemyDatas[9].attackRangeType;
                isRecoveryTower = enemyDatas[9].isRecoveryTower;
                selfDestruct = enemyDatas[9].selfDestruct;
                attackTargetNum = enemyDatas[9].attackTargetNum;
                debuffType = enemyDatas[9].debuffType;
                debuffDuration = enemyDatas[9].debuffDuration;
                multiAttackRange = enemyDatas[9].multiAttackRange;
                bulletIndex = enemyDatas[9].bulletIndex;
                damageEffectIndex = enemyDatas[9].damageEffectIndex;
                deadEffectIndex = enemyDatas[9].deadEffectIndex;
                fireEffectIndex = enemyDatas[9].fireEffectIndex;
                healEffectIndex = enemyDatas[9].healEffectIndex;
                debuffEffectIndex = enemyDatas[9].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[9].appearPos[0].X, enemyDatas[9].appearPos[0].Y, enemyDatas[9].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[9].appearPos[1].X, enemyDatas[9].appearPos[1].Y, enemyDatas[9].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[9].appearPos[2].X, enemyDatas[9].appearPos[2].Y, enemyDatas[9].appearPos[2].Z);
                rewardWoodResource = enemyDatas[9].rewardWoodResource;
                break;
            case "Enemy/ShacklesMan":
                Debug.Log("ShacklesMan 초기화 완료");
                enemyNum = enemyDatas[10].enemyNum;
                enemyIndex = enemyDatas[10].enemyIndex;
                maxHP = enemyDatas[10].maxHP;
                power = enemyDatas[10].power;
                defense = enemyDatas[10].defense;
                speed = enemyDatas[10].speed;
                attackSpeed = enemyDatas[10].attackSpeed;
                range = enemyDatas[10].range;
                regeneration = enemyDatas[10].regeneration;
                attackRangeType = enemyDatas[10].attackRangeType;
                isRecoveryTower = enemyDatas[10].isRecoveryTower;
                selfDestruct = enemyDatas[10].selfDestruct;
                attackTargetNum = enemyDatas[10].attackTargetNum;
                debuffType = enemyDatas[10].debuffType;
                debuffDuration = enemyDatas[10].debuffDuration;
                multiAttackRange = enemyDatas[10].multiAttackRange;
                bulletIndex = enemyDatas[10].bulletIndex;
                damageEffectIndex = enemyDatas[10].damageEffectIndex;
                deadEffectIndex = enemyDatas[10].deadEffectIndex;
                fireEffectIndex = enemyDatas[10].fireEffectIndex;
                healEffectIndex = enemyDatas[10].healEffectIndex;
                debuffEffectIndex = enemyDatas[10].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[10].appearPos[0].X, enemyDatas[10].appearPos[0].Y, enemyDatas[10].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[10].appearPos[1].X, enemyDatas[10].appearPos[1].Y, enemyDatas[10].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[10].appearPos[2].X, enemyDatas[10].appearPos[2].Y, enemyDatas[10].appearPos[2].Z);
                rewardWoodResource = enemyDatas[10].rewardWoodResource;
                break;
            case "Enemy/Wagon2":
                Debug.Log("Wagon2 초기화 완료");
                enemyNum = enemyDatas[11].enemyNum;
                enemyIndex = enemyDatas[11].enemyIndex;
                maxHP = enemyDatas[11].maxHP;
                power = enemyDatas[11].power;
                defense = enemyDatas[11].defense;
                speed = enemyDatas[11].speed;
                attackSpeed = enemyDatas[11].attackSpeed;
                range = enemyDatas[11].range;
                regeneration = enemyDatas[11].regeneration;
                attackRangeType = enemyDatas[11].attackRangeType;
                isRecoveryTower = enemyDatas[11].isRecoveryTower;
                selfDestruct = enemyDatas[11].selfDestruct;
                attackTargetNum = enemyDatas[11].attackTargetNum;
                debuffType = enemyDatas[11].debuffType;
                debuffDuration = enemyDatas[11].debuffDuration;
                multiAttackRange = enemyDatas[11].multiAttackRange;
                bulletIndex = enemyDatas[11].bulletIndex;
                damageEffectIndex = enemyDatas[11].damageEffectIndex;
                deadEffectIndex = enemyDatas[11].deadEffectIndex;
                fireEffectIndex = enemyDatas[11].fireEffectIndex;
                healEffectIndex = enemyDatas[11].healEffectIndex;
                debuffEffectIndex = enemyDatas[11].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[11].appearPos[0].X, enemyDatas[11].appearPos[0].Y, enemyDatas[11].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[11].appearPos[1].X, enemyDatas[11].appearPos[1].Y, enemyDatas[11].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[11].appearPos[2].X, enemyDatas[11].appearPos[2].Y, enemyDatas[11].appearPos[2].Z);
                rewardWoodResource = enemyDatas[11].rewardWoodResource;
                break;
            case "Enemy/Elephant":
                Debug.Log("Elephant 초기화 완료");
                enemyNum = enemyDatas[12].enemyNum;
                enemyIndex = enemyDatas[12].enemyIndex;
                maxHP = enemyDatas[12].maxHP;
                power = enemyDatas[12].power;
                defense = enemyDatas[12].defense;
                speed = enemyDatas[12].speed;
                attackSpeed = enemyDatas[12].attackSpeed;
                range = enemyDatas[12].range;
                regeneration = enemyDatas[12].regeneration;
                attackRangeType = enemyDatas[12].attackRangeType;
                isRecoveryTower = enemyDatas[12].isRecoveryTower;
                selfDestruct = enemyDatas[12].selfDestruct;
                attackTargetNum = enemyDatas[12].attackTargetNum;
                debuffType = enemyDatas[12].debuffType;
                debuffDuration = enemyDatas[12].debuffDuration;
                multiAttackRange = enemyDatas[12].multiAttackRange;
                bulletIndex = enemyDatas[12].bulletIndex;
                damageEffectIndex = enemyDatas[12].damageEffectIndex;
                deadEffectIndex = enemyDatas[12].deadEffectIndex;
                fireEffectIndex = enemyDatas[12].fireEffectIndex;
                healEffectIndex = enemyDatas[12].healEffectIndex;
                debuffEffectIndex = enemyDatas[12].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[12].appearPos[0].X, enemyDatas[12].appearPos[0].Y, enemyDatas[12].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[12].appearPos[1].X, enemyDatas[12].appearPos[1].Y, enemyDatas[12].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[12].appearPos[2].X, enemyDatas[12].appearPos[2].Y, enemyDatas[12].appearPos[2].Z);
                rewardWoodResource = enemyDatas[12].rewardWoodResource;
                break;
            case "Enemy/Wizard":
                Debug.Log("Wizard 초기화 완료");
                enemyNum = enemyDatas[13].enemyNum;
                enemyIndex = enemyDatas[13].enemyIndex;
                maxHP = enemyDatas[13].maxHP;
                power = enemyDatas[13].power;
                defense = enemyDatas[13].defense;
                speed = enemyDatas[13].speed;
                attackSpeed = enemyDatas[13].attackSpeed;
                range = enemyDatas[13].range;
                regeneration = enemyDatas[13].regeneration;
                attackRangeType = enemyDatas[13].attackRangeType;
                isRecoveryTower = enemyDatas[13].isRecoveryTower;
                selfDestruct = enemyDatas[13].selfDestruct;
                attackTargetNum = enemyDatas[13].attackTargetNum;
                debuffType = enemyDatas[13].debuffType;
                debuffDuration = enemyDatas[13].debuffDuration;
                multiAttackRange = enemyDatas[13].multiAttackRange;
                bulletIndex = enemyDatas[13].bulletIndex;
                damageEffectIndex = enemyDatas[13].damageEffectIndex;
                deadEffectIndex = enemyDatas[13].deadEffectIndex;
                fireEffectIndex = enemyDatas[13].fireEffectIndex;
                healEffectIndex = enemyDatas[13].healEffectIndex;
                debuffEffectIndex = enemyDatas[13].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[13].appearPos[0].X, enemyDatas[13].appearPos[0].Y, enemyDatas[13].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[13].appearPos[1].X, enemyDatas[13].appearPos[1].Y, enemyDatas[13].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[13].appearPos[2].X, enemyDatas[13].appearPos[2].Y, enemyDatas[13].appearPos[2].Z);
                rewardWoodResource = enemyDatas[13].rewardWoodResource;
                break;
            case "Enemy/Scorpion":
                Debug.Log("Scorpion 초기화 완료");
                enemyNum = enemyDatas[14].enemyNum;
                enemyIndex = enemyDatas[14].enemyIndex;
                maxHP = enemyDatas[14].maxHP;
                power = enemyDatas[14].power;
                defense = enemyDatas[14].defense;
                speed = enemyDatas[14].speed;
                attackSpeed = enemyDatas[14].attackSpeed;
                range = enemyDatas[14].range;
                regeneration = enemyDatas[14].regeneration;
                attackRangeType = enemyDatas[14].attackRangeType;
                isRecoveryTower = enemyDatas[14].isRecoveryTower;
                selfDestruct = enemyDatas[14].selfDestruct;
                attackTargetNum = enemyDatas[14].attackTargetNum;
                debuffType = enemyDatas[14].debuffType;
                debuffDuration = enemyDatas[14].debuffDuration;
                multiAttackRange = enemyDatas[14].multiAttackRange;
                bulletIndex = enemyDatas[14].bulletIndex;
                damageEffectIndex = enemyDatas[14].damageEffectIndex;
                deadEffectIndex = enemyDatas[14].deadEffectIndex;
                fireEffectIndex = enemyDatas[14].fireEffectIndex;
                healEffectIndex = enemyDatas[14].healEffectIndex;
                debuffEffectIndex = enemyDatas[14].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[14].appearPos[0].X, enemyDatas[14].appearPos[0].Y, enemyDatas[14].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[14].appearPos[1].X, enemyDatas[14].appearPos[1].Y, enemyDatas[14].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[14].appearPos[2].X, enemyDatas[14].appearPos[2].Y, enemyDatas[14].appearPos[2].Z);
                rewardWoodResource = enemyDatas[14].rewardWoodResource;
                break;
            case "Enemy/DarkSwordMan":
                Debug.Log("DarkSwordMan 초기화 완료");
                enemyNum = enemyDatas[15].enemyNum;
                enemyIndex = enemyDatas[15].enemyIndex;
                maxHP = enemyDatas[15].maxHP;
                power = enemyDatas[15].power;
                defense = enemyDatas[15].defense;
                speed = enemyDatas[15].speed;
                attackSpeed = enemyDatas[15].attackSpeed;
                range = enemyDatas[15].range;
                regeneration = enemyDatas[15].regeneration;
                attackRangeType = enemyDatas[15].attackRangeType;
                isRecoveryTower = enemyDatas[15].isRecoveryTower;
                selfDestruct = enemyDatas[15].selfDestruct;
                attackTargetNum = enemyDatas[15].attackTargetNum;
                debuffType = enemyDatas[15].debuffType;
                debuffDuration = enemyDatas[15].debuffDuration;
                multiAttackRange = enemyDatas[15].multiAttackRange;
                bulletIndex = enemyDatas[15].bulletIndex;
                damageEffectIndex = enemyDatas[15].damageEffectIndex;
                deadEffectIndex = enemyDatas[15].deadEffectIndex;
                fireEffectIndex = enemyDatas[15].fireEffectIndex;
                healEffectIndex = enemyDatas[15].healEffectIndex;
                debuffEffectIndex = enemyDatas[15].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[15].appearPos[0].X, enemyDatas[15].appearPos[0].Y, enemyDatas[15].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[15].appearPos[1].X, enemyDatas[15].appearPos[1].Y, enemyDatas[15].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[15].appearPos[2].X, enemyDatas[15].appearPos[2].Y, enemyDatas[15].appearPos[2].Z);
                rewardWoodResource = enemyDatas[15].rewardWoodResource;
                break;
            case "Enemy/DarkLarva":
                Debug.Log("DarkLarva 초기화 완료");
                enemyNum = enemyDatas[16].enemyNum;
                enemyIndex = enemyDatas[16].enemyIndex;
                maxHP = enemyDatas[16].maxHP;
                power = enemyDatas[16].power;
                defense = enemyDatas[16].defense;
                speed = enemyDatas[16].speed;
                attackSpeed = enemyDatas[16].attackSpeed;
                range = enemyDatas[16].range;
                regeneration = enemyDatas[16].regeneration;
                attackRangeType = enemyDatas[16].attackRangeType;
                isRecoveryTower = enemyDatas[16].isRecoveryTower;
                selfDestruct = enemyDatas[16].selfDestruct;
                attackTargetNum = enemyDatas[16].attackTargetNum;
                debuffType = enemyDatas[16].debuffType;
                debuffDuration = enemyDatas[16].debuffDuration;
                multiAttackRange = enemyDatas[16].multiAttackRange;
                bulletIndex = enemyDatas[16].bulletIndex;
                damageEffectIndex = enemyDatas[16].damageEffectIndex;
                deadEffectIndex = enemyDatas[16].deadEffectIndex;
                fireEffectIndex = enemyDatas[16].fireEffectIndex;
                healEffectIndex = enemyDatas[16].healEffectIndex;
                debuffEffectIndex = enemyDatas[16].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[16].appearPos[0].X, enemyDatas[16].appearPos[0].Y, enemyDatas[16].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[16].appearPos[1].X, enemyDatas[16].appearPos[1].Y, enemyDatas[16].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[16].appearPos[2].X, enemyDatas[16].appearPos[2].Y, enemyDatas[16].appearPos[2].Z);
                rewardWoodResource = enemyDatas[16].rewardWoodResource;
                break;
            case "Enemy/GateKeeper":
                Debug.Log("GateKeeper 초기화 완료");
                enemyNum = enemyDatas[17].enemyNum;
                enemyIndex = enemyDatas[17].enemyIndex;
                maxHP = enemyDatas[17].maxHP;
                power = enemyDatas[17].power;
                defense = enemyDatas[17].defense;
                speed = enemyDatas[17].speed;
                attackSpeed = enemyDatas[17].attackSpeed;
                range = enemyDatas[17].range;
                regeneration = enemyDatas[17].regeneration;
                attackRangeType = enemyDatas[17].attackRangeType;
                isRecoveryTower = enemyDatas[17].isRecoveryTower;
                selfDestruct = enemyDatas[17].selfDestruct;
                attackTargetNum = enemyDatas[17].attackTargetNum;
                debuffType = enemyDatas[17].debuffType;
                debuffDuration = enemyDatas[17].debuffDuration;
                multiAttackRange = enemyDatas[17].multiAttackRange;
                bulletIndex = enemyDatas[17].bulletIndex;
                damageEffectIndex = enemyDatas[17].damageEffectIndex;
                deadEffectIndex = enemyDatas[17].deadEffectIndex;
                fireEffectIndex = enemyDatas[17].fireEffectIndex;
                healEffectIndex = enemyDatas[17].healEffectIndex;
                debuffEffectIndex = enemyDatas[17].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[17].appearPos[0].X, enemyDatas[17].appearPos[0].Y, enemyDatas[17].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[17].appearPos[1].X, enemyDatas[17].appearPos[1].Y, enemyDatas[17].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[17].appearPos[2].X, enemyDatas[17].appearPos[2].Y, enemyDatas[17].appearPos[2].Z);
                rewardWoodResource = enemyDatas[17].rewardWoodResource;
                break;
            case "Enemy/Wagon3":
                Debug.Log("Wagon3 초기화 완료");
                enemyNum = enemyDatas[18].enemyNum;
                enemyIndex = enemyDatas[18].enemyIndex;
                maxHP = enemyDatas[18].maxHP;
                power = enemyDatas[18].power;
                defense = enemyDatas[18].defense;
                speed = enemyDatas[18].speed;
                attackSpeed = enemyDatas[18].attackSpeed;
                range = enemyDatas[18].range;
                regeneration = enemyDatas[18].regeneration;
                attackRangeType = enemyDatas[18].attackRangeType;
                isRecoveryTower = enemyDatas[18].isRecoveryTower;
                selfDestruct = enemyDatas[18].selfDestruct;
                attackTargetNum = enemyDatas[18].attackTargetNum;
                debuffType = enemyDatas[18].debuffType;
                debuffDuration = enemyDatas[18].debuffDuration;
                multiAttackRange = enemyDatas[18].multiAttackRange;
                bulletIndex = enemyDatas[18].bulletIndex;
                damageEffectIndex = enemyDatas[18].damageEffectIndex;
                deadEffectIndex = enemyDatas[18].deadEffectIndex;
                fireEffectIndex = enemyDatas[18].fireEffectIndex;
                healEffectIndex = enemyDatas[18].healEffectIndex;
                debuffEffectIndex = enemyDatas[18].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[18].appearPos[0].X, enemyDatas[18].appearPos[0].Y, enemyDatas[18].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[18].appearPos[1].X, enemyDatas[18].appearPos[1].Y, enemyDatas[18].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[18].appearPos[2].X, enemyDatas[18].appearPos[2].Y, enemyDatas[18].appearPos[2].Z);
                rewardWoodResource = enemyDatas[18].rewardWoodResource;
                break;
            case "Enemy/DarkStagBeetle":
                Debug.Log("DarkStagBeetle 초기화 완료");
                enemyNum = enemyDatas[19].enemyNum;
                enemyIndex = enemyDatas[19].enemyIndex;
                maxHP = enemyDatas[19].maxHP;
                power = enemyDatas[19].power;
                defense = enemyDatas[19].defense;
                speed = enemyDatas[19].speed;
                attackSpeed = enemyDatas[19].attackSpeed;
                range = enemyDatas[19].range;
                regeneration = enemyDatas[19].regeneration;
                attackRangeType = enemyDatas[19].attackRangeType;
                isRecoveryTower = enemyDatas[19].isRecoveryTower;
                selfDestruct = enemyDatas[19].selfDestruct;
                attackTargetNum = enemyDatas[19].attackTargetNum;
                debuffType = enemyDatas[19].debuffType;
                debuffDuration = enemyDatas[19].debuffDuration;
                multiAttackRange = enemyDatas[19].multiAttackRange;
                bulletIndex = enemyDatas[19].bulletIndex;
                damageEffectIndex = enemyDatas[19].damageEffectIndex;
                deadEffectIndex = enemyDatas[19].deadEffectIndex;
                fireEffectIndex = enemyDatas[19].fireEffectIndex;
                healEffectIndex = enemyDatas[19].healEffectIndex;
                debuffEffectIndex = enemyDatas[19].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[19].appearPos[0].X, enemyDatas[19].appearPos[0].Y, enemyDatas[19].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[19].appearPos[1].X, enemyDatas[19].appearPos[1].Y, enemyDatas[19].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[19].appearPos[2].X, enemyDatas[19].appearPos[2].Y, enemyDatas[19].appearPos[2].Z);
                rewardWoodResource = enemyDatas[19].rewardWoodResource;
                break;
            case "Enemy/DarkBowMan":
                Debug.Log("DarkBowMan 초기화 완료");
                enemyNum = enemyDatas[20].enemyNum;
                enemyIndex = enemyDatas[20].enemyIndex;
                maxHP = enemyDatas[20].maxHP;
                power = enemyDatas[20].power;
                defense = enemyDatas[20].defense;
                speed = enemyDatas[20].speed;
                attackSpeed = enemyDatas[20].attackSpeed;
                range = enemyDatas[20].range;
                regeneration = enemyDatas[20].regeneration;
                attackRangeType = enemyDatas[20].attackRangeType;
                isRecoveryTower = enemyDatas[20].isRecoveryTower;
                selfDestruct = enemyDatas[20].selfDestruct;
                attackTargetNum = enemyDatas[20].attackTargetNum;
                debuffType = enemyDatas[20].debuffType;
                debuffDuration = enemyDatas[20].debuffDuration;
                multiAttackRange = enemyDatas[20].multiAttackRange;
                bulletIndex = enemyDatas[20].bulletIndex;
                damageEffectIndex = enemyDatas[20].damageEffectIndex;
                deadEffectIndex = enemyDatas[20].deadEffectIndex;
                fireEffectIndex = enemyDatas[20].fireEffectIndex;
                healEffectIndex = enemyDatas[20].healEffectIndex;
                debuffEffectIndex = enemyDatas[20].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[20].appearPos[0].X, enemyDatas[20].appearPos[0].Y, enemyDatas[20].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[20].appearPos[1].X, enemyDatas[20].appearPos[1].Y, enemyDatas[20].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[20].appearPos[2].X, enemyDatas[20].appearPos[2].Y, enemyDatas[20].appearPos[2].Z);
                rewardWoodResource = enemyDatas[20].rewardWoodResource;
                break;
            case "Enemy/DarkKnight":
                Debug.Log("DarkKnight 초기화 완료");
                enemyNum = enemyDatas[21].enemyNum;
                enemyIndex = enemyDatas[21].enemyIndex;
                maxHP = enemyDatas[21].maxHP;
                power = enemyDatas[21].power;
                defense = enemyDatas[21].defense;
                speed = enemyDatas[21].speed;
                attackSpeed = enemyDatas[21].attackSpeed;
                range = enemyDatas[21].range;
                regeneration = enemyDatas[21].regeneration;
                attackRangeType = enemyDatas[21].attackRangeType;
                isRecoveryTower = enemyDatas[21].isRecoveryTower;
                selfDestruct = enemyDatas[21].selfDestruct;
                attackTargetNum = enemyDatas[21].attackTargetNum;
                debuffType = enemyDatas[21].debuffType;
                debuffDuration = enemyDatas[21].debuffDuration;
                multiAttackRange = enemyDatas[21].multiAttackRange;
                bulletIndex = enemyDatas[21].bulletIndex;
                damageEffectIndex = enemyDatas[21].damageEffectIndex;
                deadEffectIndex = enemyDatas[21].deadEffectIndex;
                fireEffectIndex = enemyDatas[21].fireEffectIndex;
                healEffectIndex = enemyDatas[21].healEffectIndex;
                debuffEffectIndex = enemyDatas[21].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[21].appearPos[0].X, enemyDatas[21].appearPos[0].Y, enemyDatas[21].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[21].appearPos[1].X, enemyDatas[21].appearPos[1].Y, enemyDatas[21].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[21].appearPos[2].X, enemyDatas[21].appearPos[2].Y, enemyDatas[21].appearPos[2].Z);
                rewardWoodResource = enemyDatas[21].rewardWoodResource;
                break;
            case "Enemy/DarkElephant":
                Debug.Log("DarkElephant 초기화 완료");
                enemyNum = enemyDatas[22].enemyNum;
                enemyIndex = enemyDatas[22].enemyIndex;
                maxHP = enemyDatas[22].maxHP;
                power = enemyDatas[22].power;
                defense = enemyDatas[22].defense;
                speed = enemyDatas[22].speed;
                attackSpeed = enemyDatas[22].attackSpeed;
                range = enemyDatas[22].range;
                regeneration = enemyDatas[22].regeneration;
                attackRangeType = enemyDatas[22].attackRangeType;
                isRecoveryTower = enemyDatas[22].isRecoveryTower;
                selfDestruct = enemyDatas[22].selfDestruct;
                attackTargetNum = enemyDatas[22].attackTargetNum;
                debuffType = enemyDatas[22].debuffType;
                debuffDuration = enemyDatas[22].debuffDuration;
                multiAttackRange = enemyDatas[22].multiAttackRange;
                bulletIndex = enemyDatas[22].bulletIndex;
                damageEffectIndex = enemyDatas[22].damageEffectIndex;
                deadEffectIndex = enemyDatas[22].deadEffectIndex;
                fireEffectIndex = enemyDatas[22].fireEffectIndex;
                healEffectIndex = enemyDatas[22].healEffectIndex;
                debuffEffectIndex = enemyDatas[22].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[22].appearPos[0].X, enemyDatas[22].appearPos[0].Y, enemyDatas[22].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[22].appearPos[1].X, enemyDatas[22].appearPos[1].Y, enemyDatas[22].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[22].appearPos[2].X, enemyDatas[22].appearPos[2].Y, enemyDatas[22].appearPos[2].Z);
                rewardWoodResource = enemyDatas[22].rewardWoodResource;
                break;
            case "Enemy/DarkWizard":
                Debug.Log("DarkWizard 초기화 완료");
                enemyNum = enemyDatas[23].enemyNum;
                enemyIndex = enemyDatas[23].enemyIndex;
                maxHP = enemyDatas[23].maxHP;
                power = enemyDatas[23].power;
                defense = enemyDatas[23].defense;
                speed = enemyDatas[23].speed;
                attackSpeed = enemyDatas[23].attackSpeed;
                range = enemyDatas[23].range;
                regeneration = enemyDatas[23].regeneration;
                attackRangeType = enemyDatas[23].attackRangeType;
                isRecoveryTower = enemyDatas[23].isRecoveryTower;
                selfDestruct = enemyDatas[23].selfDestruct;
                attackTargetNum = enemyDatas[23].attackTargetNum;
                debuffType = enemyDatas[23].debuffType;
                debuffDuration = enemyDatas[23].debuffDuration;
                multiAttackRange = enemyDatas[23].multiAttackRange;
                bulletIndex = enemyDatas[23].bulletIndex;
                damageEffectIndex = enemyDatas[23].damageEffectIndex;
                deadEffectIndex = enemyDatas[23].deadEffectIndex;
                fireEffectIndex = enemyDatas[23].fireEffectIndex;
                healEffectIndex = enemyDatas[23].healEffectIndex;
                debuffEffectIndex = enemyDatas[23].debuffEffectIndex;
                appearPos[0] = new Vector3(enemyDatas[23].appearPos[0].X, enemyDatas[23].appearPos[0].Y, enemyDatas[23].appearPos[0].Z);
                appearPos[1] = new Vector3(enemyDatas[23].appearPos[1].X, enemyDatas[23].appearPos[1].Y, enemyDatas[23].appearPos[1].Z);
                appearPos[2] = new Vector3(enemyDatas[23].appearPos[2].X, enemyDatas[23].appearPos[2].Y, enemyDatas[23].appearPos[2].Z);
                rewardWoodResource = enemyDatas[23].rewardWoodResource;
                break;


            default:
                break;
        }
    }

    #endregion
}