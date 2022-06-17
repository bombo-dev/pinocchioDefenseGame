using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class Debuff
{
    public float durationTime = 0;  //지속시간

    public int stack = 0;  //중첩스택
}

public class Actor : MonoBehaviour
{
    public enum debuff  //디버프 종류
    {
        None,   //초기값 0
        DecreaseAttackSpeed,    //공격 속도 감소 1
        Slow,   //이동속도 감소 2
        DecreaseDefense,    //방어력 감소 3
        DecreasePower,  //공격력 감소 4
        ElectricShock,  //감전 - 공격속도,이동속도 대폭감소 5 
        Burn    //화상 - 방어력 대폭감소 6
    }

    public enum buff  //버프 종류
    {
        None,   //초기값 0
        IncreasePower,    //공격력 증가 1, 빨간나무
        IncreaseAttackSpeed,   //공격속도 증가 2, 노랑나무
        IncreaseRegeneration,    //회복력 증가 3, 초록나무
        IncreaseDefense,  //방어력 증가 4, 하얀나무
        IncreaseRange,  //사거리 증가 5, 파란나무 
        IncreaseAll    //올스텟 증가 6, 검정나무
    }

    [SerializeField]
    public debuff _debuff = debuff.None;    //디버프

    [Header("Stat")]    //능력치

    [SerializeField]
    public int maxHP = 30;   //최대 체력

    [SerializeField]
    public int currentHP;   //현재 체력

    [SerializeField]
    public int power = 10;  //공격력

    [SerializeField]
    public int currentPower;    //현재 공격력

    [SerializeField]
    public int defense = 10;    //방어력

    [SerializeField]
    public int currentDefense;  //현재 방어력

    [SerializeField]
    public float attackSpeed;    //공격속도

    [SerializeField]
    public float currentAttackSpeed;    //현재 공격속도

    [SerializeField]
    protected int range;  //사거리

    [SerializeField]
    public int currentRange;  //현재 사거리

    [SerializeField]
    protected int regeneration;   // 회복력

    [SerializeField]
    public int currentRegeneration;   // 회복력

    [Header("Debuff")]  //디버프
    public Dictionary<debuff, Debuff> debuffs = new Dictionary<debuff, Debuff>();

    [Header("AttackType")]  //공격타입 : 원거리, 근거리, 단일공격, 다중공격 

    [SerializeField]
    protected int attackRangeType; // 0:근거리타입 1:원거리타입

    [SerializeField]
    public bool isRecoveryTower; // false - 공격타워 true - 회복타워

    [SerializeField]
    public int attackTargetNum;    //공격 타겟 수 

    [SerializeField]
    public int debuffType;   //디버프 타입 

    [SerializeField]
    public float debuffDuration; //디버프 지속시간

    [Header("multipleAttack")]    //다중 공격 유닛 전용 능력치

    [SerializeField]
    protected int multiAttackRange;  //다중공격 사거리

    [SerializeField]
    protected int currentMultiAttackRange;  //현재 다중공격 사거리

    [Header("Bullet, Effect")]  //원거리 공격 유닛 - 총알 관련
    [SerializeField]
    protected int bulletIndex;  //사용할 총알 번호

    [SerializeField]
    protected GameObject firePos;  //단일 공격시 총알이 발사되는 시작점

    public GameObject hitPos;   //총알과 충돌하는 객체의 위치

    [SerializeField]
    public GameObject hpPos;

    [SerializeField]
    public GameObject dropPos;  //다중 공격시 총알이 떨어지는 시작점    

    protected float attackTimer;  //공격시간 타이머

    public int damageEffectIndex; //사용할 공격 이펙트 번호

    public int deadEffectIndex; //Dead시 사용할 이펙트 번호

    public int fireEffectIndex; //fire시 사용할 이펙트 번호

    public int healEffectIndex; //heal시 사용할 이펙트 번호

    public int debuffEffectIndex; //debuff시 사용할 이펙트 번호

    [SerializeField]
    GameObject currentDamageEffect;   //현재 출력한 피격 이펙트

    [SerializeField]
    GameObject currentDeadEffect;   //현재 출력한 Dead 이펙트

    [SerializeField]
    GameObject currentFireEffect;   //현재 출력한 데미지 이펙트

    [SerializeField]
    GameObject currentHealEffect;   //현재 출력한 회복 이펙트

    [SerializeField]
    GameObject currentDebuffEffect;   //현재 출력한 디버프 이펙트

    [Header("data")]    //기타 데이터
    [SerializeField]
    protected string filePath; //프리팹 저장 파일 경로

    [SerializeField]
    protected Animator animator; //애니메이터

    [SerializeField]
    protected List<GameObject> attackTargets;    //공격을 당하는 오브젝트
    [SerializeField]
    protected List<Actor> attackTargetsActor;  //공격을 당하는 오브젝트 액터 클래스

    public Vector3 attackDirVec;   //공격할 타겟의 방향벡터
    
    public float bulletSpeed = 100f;    // 총알의 이동 속도

    public float maxBulletSpeed;    // 총알의 최대 이동 속도

    public bool finAttack;  // 공격이 끝났는지를 확인하는 플래그

    [Header("Material")]  //적용된 Material
    [SerializeField]
    Renderer[] rendererArr;

    [SerializeField]
    List<Renderer> rendererCaches;

    [SerializeField]
    List<Vector4> emissionCaches;

    public bool showWhiteFlash_coroutine_is_running = false;//코루틴 실행중 여부 플래그

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        //attackOwner = this.gameObject;  
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActor();

        //디버프 동작 
        UpdateDebuff();
    }

    /// <summary>
    /// 초기화 함수 : 김현진
    /// </summary>
    protected virtual void Initialize()
    {
        rendererCaches = new List<Renderer>();
        emissionCaches = new List<Vector4>();
        //변경할 쉐이더캐시 초기화
        if (rendererArr.Length > 0)
        {
            SystemManager.Instance.ShaderController.InitializeShaderCaches(rendererArr, rendererCaches, emissionCaches);
        }
    }

    /// <summary>
    /// 유닛 정보들 리셋
    /// </summary>
    public virtual void Reset()
    {
        //쉐이더 정보 초기화
        SystemManager.Instance.ShaderController.OffFlash(rendererCaches, emissionCaches);

        //타겟배열 초기화
        attackTargets.Clear();

        //HP초기화
        currentHP = maxHP;

        //공격력 초기화
        currentPower = power;

        //방어력 초기화
        currentDefense = defense;

        //공격속도 초기화
        currentAttackSpeed = attackSpeed;

        //사거리 초기화
        currentMultiAttackRange = multiAttackRange;

        //회복력 초기화
        currentRegeneration = regeneration;

        //디버프 초기화
        ClearDebuff();
    }

    /// <summary>
    /// 실시간 상태별 액터의 동작 : 김현진
    /// </summary>
    protected virtual void UpdateActor()
    {
        
    }

    /// <summary>
    /// this 객체의 사거리 안에있는 타겟을 감지해 그중 공격할 타겟을 지정 : 김현진
    /// </summary>
    /// <param name="target">타겟이 될 대상 배열</param>
    /// <param name="mine">호출한 오브젝트</param>
    protected virtual void DetectTarget(List<GameObject> target , GameObject mine = null)
    {
        //공격 유닛이 아닌경우
        if (attackTargetNum == 0)
            return;

        //리스트 초기화
        attackTargets.Clear();
        attackTargetsActor.Clear();

        //타겟과 타겟과의 거리를 저장할 딕셔너리
        Dictionary<GameObject, float> targetDistances = new Dictionary<GameObject, float>();

        // --------------- 타겟별 거리 측정 ---------------

        //다중타겟 유닛이 range 안에 공격할 적이 존재하는지 판단, * 다중타겟 공격은 range범위 안쪽의 타겟이 존재할 경우 나머지 target은 multiAttackRange범위로 찾는다 *
        bool isTargetInRange = false;

        //타겟 리스트의 모든 요소를 검사하여 사거리안에 들어온 타겟인 경우 딕셔너리에 저장
        for (int i = 0; i < target.Count; i++)
        {
            //회복 타워일 경우
            if (mine && isRecoveryTower)
            {
                // 감지된 타겟이 자신이거나 또다른 회복타워일 경우 다음 유닛 감지
                if ((System.Object.ReferenceEquals(target[i], mine)) || (target[i].GetComponent<Actor>().isRecoveryTower))
                {
                    if (i >= target.Count - 1)
                        break;
                    else
                        continue;
                }
            }

            //타겟이 존재하는경우
            if (target[i].activeSelf)
            {
                //단일타겟
                if (attackTargetNum <= 1)
                {
                    if (Vector3.SqrMagnitude(target[i].transform.position - transform.position) < currentRange)
                        targetDistances.Add(target[i], Vector3.SqrMagnitude(target[i].transform.position - transform.position));
                }
                //다중타겟
                else
                {
                    if (!isTargetInRange && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < currentRange)
                        isTargetInRange = true;
                    if (Vector3.SqrMagnitude(target[i].transform.position - transform.position) < currentMultiAttackRange)
                        targetDistances.Add(target[i], Vector3.SqrMagnitude(target[i].transform.position - transform.position));
                }
            }
        }//end of for

        // --------------- 공격 가능한 타겟이 존재하는지 판단 ---------------

        //타겟 없으면 종료
        if (targetDistances.Count <= 0)
            return;

        //다중 타겟유닛 - range내 타겟 없으면 종료
        if (attackTargetNum > 1 && !isTargetInRange)
            return;

        // --------------- 타겟이 존재할 경우 최종 타겟 선정 ---------------

        //거리순으로 오름차순 정렬
        var sortedTargetDistances = targetDistances.OrderBy(x => x.Value);

        //거리순으로 최대 타겟 가능 유닛 이하로 넣어준다
        foreach (KeyValuePair<GameObject, float> item in sortedTargetDistances)
        {
            attackTargets.Add(item.Key);
            attackTargetsActor.Add(item.Key.GetComponent<Actor>());

            if (attackTargets.Count >= attackTargetNum)
            {
                break;
            }
        }

        //공격
        Attack();

        return;
    }

    /// <summary>
    /// this객체의 상태를 공격으로 변경 : 김현진
    /// </summary>
    protected virtual void Attack()
    {
        //공격 이펙트 호출
        EnableFireEffect(this);

        //공격
        animator.SetBool("attack", true);

        //공격시간 측정 변수 초기화
        attackTimer = Time.time;      
    }


    /// <summary>
    /// 실시간으로 공격이 끝났는지 안끝났는지를 판별하고 끝났을경우 
    /// 다음 공격으로 이행할지 다른 상태로 변경할지를 결정 : 김현진
    /// </summary>
    protected virtual void UpdateBattle()
    {
        // 이동하는 Enemy 방향으로 터렛이 계속 회전하도록 타겟 위치 업데이트
        attackDirVec = (attackTargets[0].transform.position - this.transform.position).normalized;

        //예외처리
        if (attackDirVec == Vector3.zero && tag == "Turret" && isRecoveryTower == false)
            return;

        //단일타겟 예외처리
        if ((attackTargetNum == 1) && (attackTargetsActor[0].currentHP <= 0 || !attackTargets[0].activeSelf)) //타겟이 없는경우
        {
            animator.SetBool("attackCancel", true);
        }
        //다중타겟 예외처리
        else if (attackTargetNum > 1)
        {
            for (int i = 0; i < attackTargets.Count; i++)
            {
                if (attackTargetsActor[i].currentHP > 0 && attackTargets[i].activeSelf) // //타겟이 없는경우
                    break;
                if(i == attackTargets.Count-1)
                    animator.SetBool("attackCancel", true);
            }
        }

        if (animator.GetBool("attackCancel"))
            return;
        
        //원거리 유닛 전용 총알 생성
        if (attackRangeType == 1 && animator.GetBool("rangedAttack"))
        {
            InitializeBullet();
            animator.SetBool("rangedAttack", false);
        }
        //근거리 유닛 전용 데미지 처리
        else if (attackRangeType == 0 && animator.GetBool("meleeAttack"))
        {
            if (attackTargets[0].tag == "Enemy")
            {
                Enemy enemy = attackTargets[0].GetComponent<Enemy>();

                Turret attacker = gameObject.GetComponent<Turret>();

                // 방어력 디버프를 적용한 데미지 계산
                int damage = attacker.currentPower - (int)(attacker.currentPower * ((float)enemy.currentDefense * 0.01f));

                // 에너미의 hp를 감소
                enemy.DecreaseHP(damage);

                // 데미지 UI 생성
                if (damage > 0)
                {
                    GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, attackTargets[0]);

                    if (!damageMngPanelGo)
                        return;

                    DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                    // 데미지 UI 화면에 띄우기
                    damageMngPanel.ShowDamage(damage, 0);

                    enemy.damageMngPanel = damageMngPanel;
                    damageMngPanel.damageOwner = enemy.gameObject;

                }
                //피 공격자 디버프 걸기
                if (debuffType > 0)
                {
                    Debug.Log("******디버프*******");
                    enemy.AddDebuff(debuffType, debuffDuration);
                    //피 공격자에게 디버프 이펙트 출력
                    enemy.EnableDebuffEffect(attacker);
                }
                //피 공격자에게 데미지 이펙트 출력
                enemy.EnableDamageEffect(attacker);
            }
            else if (attackTargets[0].tag == "Turret")
            {
                Turret turret = attackTargets[0].GetComponent<Turret>();

                Enemy attacker = gameObject.GetComponent<Enemy>();

                // 방어력 디버프를 적용한 데미지 계산
                int damage = attacker.currentPower - (int)(attacker.currentPower * ((float)turret.currentDefense * 0.01f));

                // 터렛의 hp를 감소
                turret.DecreaseHP(damage);

                // 데미지 UI 생성
                if (damage > 0)
                {
                    GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, attackTargets[0]);

                    if (!damageMngPanelGo)
                        return;

                    DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                    // 데미지 UI 화면에 띄우기
                    damageMngPanel.ShowDamage(damage, 0);

                    turret.damageMngPanel = damageMngPanel;
                    damageMngPanel.damageOwner = turret.gameObject;

                }

                //피 공격자 디버프 걸기
                if (debuffType > 0)
                {
                    turret.AddDebuff(debuffType, debuffDuration);

                    //피 공격자에게 디버프 이펙트 출력
                    turret.EnableDebuffEffect(attacker);
                }
                //피 공격자에게 데미지 이펙트 출력
                turret.EnableDamageEffect(attacker);
            }

            animator.SetBool("meleeAttack", false);
        }

        //위치 업데이트
        if (!isRecoveryTower)
        {
            Quaternion rotation;

            rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
        }

        //이펙트 위치 업데이트
        if(currentFireEffect)
            if(currentFireEffect.activeSelf)
                currentFireEffect.transform.position = firePos.transform.position;
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
        if (attackTargetNum == 1 && !isRecoveryTower)
        {            
            SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, firePos.transform.position, attackTargets[0], gameObject);
        }
        //다중 타겟 유닛, 회복 유닛일 경우
        else
        {
            for (int i = 0; i < attackTargets.Count; i++)
            {
                if (attackTargetsActor[i].currentHP > 0 && attackTargets[i].activeSelf) //타겟이 존재하는경우
                {
                    SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, attackTargetsActor[i].dropPos.transform.position, attackTargets[i], this.gameObject);
                }
            }

        }

    }


    /// <summary>
    /// 공격을 당한 타겟의 HP를 감소 : 하은비
    /// </summary>
    /// <param name="damage">타겟이 받을 데미지</param>
    public virtual void DecreaseHP(int damage)
    {
        if (damage <= 0 || currentHP < 0)
            return;

        //데미지 계산 공식 -> 방어력은 100미만
        if (currentDefense >= 100)
            currentDefense = 99;

        damage = (int)(damage - (damage * ((float)currentDefense * 0.01f)));

        Debug.Log("damage -> " + damage);

        //예외처리
        if (damage <= 0)
            damage = 1;

        if (currentHP > damage)
        {
            currentHP -= damage;
        }
        else
        {
            callFlashCoroutine(ShaderController.RED);

            currentHP = 0;
            animator.SetBool("isDead", true);
            animator.Play("Dead");

            OnDead();
            return;
        }        

        callFlashCoroutine(ShaderController.WHITE);
    }

    protected virtual void OnDead()
    {

    }
    /// <summary>
    /// 회복이 들어간 타겟의 HP를 증가 : 김현진
    /// </summary>
    /// <param name="recoveryPower">데미지를 증가시킬 타겟</param>
    public virtual void IncreaseHP(int recoveryPower)
    {
        //데미지 계산 공식
        //recoveryPower += recoveryPower * (currentRegeneration / 100);

        if (currentHP + recoveryPower >= maxHP)
            currentHP = maxHP;
        else
            currentHP += recoveryPower;
    }

    #region 이펙트
    /// <summary>
    /// 피격 이펙트 출력 : 김현진
    /// </summary>
    /// <param name="attacker">공격자</param>
    public virtual void EnableDamageEffect(Actor attacker)
    {
        //이펙트 출력 
        if (hitPos)
            currentDamageEffect = SystemManager.Instance.EffectManager.EnableEffect(attacker.damageEffectIndex, hitPos.transform.position);   //피격 이펙트 출력

        if (currentHP <= 0)
        {
            if (hitPos)
                currentDeadEffect = SystemManager.Instance.EffectManager.EnableEffect(deadEffectIndex, hitPos.transform.position);    //Dead 이펙트 출력
        }
    }

    /// <summary>
    /// 공격 이펙트 출력 : 김현진
    /// </summary>
    /// <param name="attacker">공격자</param>
    public virtual void EnableFireEffect(Actor attacker)
    {
        //이펙트 출력 
        if(firePos && attacker.fireEffectIndex != -1)
            currentFireEffect = SystemManager.Instance.EffectManager.EnableEffect(attacker.fireEffectIndex, firePos.transform.position);   //피격 이펙트 출력
    }

    /// <summary>
    /// 회복 이펙트 출력 : 김현진
    /// </summary>
    /// <param name="attacker">회복시전자</param>
    public virtual void EnableHealEffect(Actor attacker)
    {
        //이펙트 출력 
        if (hitPos && attacker.healEffectIndex != -1)
            currentHealEffect = SystemManager.Instance.EffectManager.EnableEffect(attacker.healEffectIndex, hitPos.transform.position);   //회복 이펙트 출력
    }

    /// <summary>
    /// 디버프 이펙트 출력 : 김현진
    /// </summary>
    /// <param name="attacker">공격자</param>
    public virtual void EnableDebuffEffect(Actor attacker)
    {
        //이펙트 출력 
        if (hpPos || debuffEffectIndex != -1)
            currentDebuffEffect = SystemManager.Instance.EffectManager.EnableEffect(attacker.debuffEffectIndex, hpPos.transform.position);   //디버프 이펙트 출력
    }


    #endregion

    /// <summary>
    /// Flash효과를 나타내기 위한 코루틴을 호출 : 김현진
    /// </summary>
    /// <param name="color">Flash효과의 색</param>
    protected void callFlashCoroutine(Vector4 color)
    {
        //WhiteFlash 피격효과

        //예외처리
        if (rendererArr.Length <= 0)
            return;

        //코루틴이 실행중이면 종료한 뒤 다시실행
        if (showWhiteFlash_coroutine_is_running)
        {
            //코루틴 종료
            StopCoroutine(SystemManager.Instance.ShaderController.ShowFlash(rendererCaches, emissionCaches, this, color));
            showWhiteFlash_coroutine_is_running = false;

            //쉐이더 정보 초기화
            SystemManager.Instance.ShaderController.OffFlash(rendererCaches, emissionCaches);
        }

        StartCoroutine(SystemManager.Instance.ShaderController.ShowFlash(rendererCaches, emissionCaches, this, color));
    }
   


    /// <summary>
    /// 유닛 비활성화 처리를 위한 Dead 상태 업데이트 : 하은비
    /// </summary>
    protected virtual void UpdateDead()
    {
        
    }

    #region 디버프

    /// <summary>
    /// 실시간 디버프 동작 처리 : 김현진
    /// </summary>
    void UpdateDebuff()
    {
        if (debuffs.Count > 0)
        {
            for (int i = 0; i < Enum.GetValues(typeof(debuff)).Length; i++)
            {
                //인덱스를 debuff로 형변환
                debuff _debuffIndex = (debuff)i;

                //디버프 업데이트
                if (debuffs.ContainsKey(_debuffIndex))
                {
                    //지속시간 업데이트
                    debuffs[_debuffIndex].durationTime -= Time.deltaTime;

                    //지속시간 경과시 디버프 제거
                    if (debuffs[_debuffIndex].durationTime < 0)
                    {
                        RemoveDebuff(i);                        
                    }

                }
            }
        }

    }

    /// <summary>
    /// 디버프 추가 : 김현진
    /// </summary>
    /// <param name="debuffIndex">추가할 디버프 종류 인덱스</param>
    /// <param name="time">추가할 디버프의 지속시간</param>
    public virtual void AddDebuff(int debuffIndex, float time)
    {
        //예외처리
        if (debuffIndex >= Enum.GetValues(typeof(debuff)).Length)
            return;

        //인덱스를 debuff로 형변환
        debuff _debuffIndex = (debuff)debuffIndex;

        //이미 존재하는 디버프인경우
        if (debuffs.ContainsKey(_debuffIndex))
        {
            if (debuffs[_debuffIndex].stack < 6)//최대 5스택 -> 효과적용을 위해 6까지
                debuffs[_debuffIndex].stack++;   //중첩 스택 추가
        }
        //새로 추가될 디버프인경우
        else 
        {
            Debuff debuff = new Debuff(); //객체 생성

            debuff.stack = 1;   //중첩 스택 초기화
            debuffs.Add(_debuffIndex, debuff);   //자료구조에 추가
        }

        debuffs[_debuffIndex].durationTime = time;   //지속시간 초기화
    }

    /// <summary>
    /// 디버프 제거 : 김현진
    /// </summary>
    /// <param name="debuffIndex">제거할 디버프</param>
    protected virtual void RemoveDebuff(int debuffIndex)
    {
        //인덱스를 debuff로 형변환
        debuff _debuffIndex = (debuff)debuffIndex;

        //키 값 참조하여 해당 요소 제거
        if(debuffs.ContainsKey(_debuffIndex))
            debuffs.Remove(_debuffIndex);
    }

    /// <summary>
    /// 모든 디버프를 제거
    /// </summary>
    void ClearDebuff()
    {
        if (debuffs.Count > 0)
        {
            for (int i = 0; i < Enum.GetValues(typeof(debuff)).Length; i++)
            {
                //디버프 제거
                RemoveDebuff(i);
            }
        }

        //딕셔너리 초기화
        debuffs.Clear();
    }

    #endregion
    protected virtual void UpdatePanelPos()
    {
        if (!SystemManager.Instance.PanelManager.statusMngPanel)
            return;       
    }
}
