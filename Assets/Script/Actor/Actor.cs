using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Actor : MonoBehaviour
{
    [Header("Stat")]    //능력치

    [SerializeField]
    protected int maxHP;   //최대 체력

    [SerializeField]
    protected int currentHP;   //현재 체력

    [SerializeField]
    protected int power;  // 공격력

    [SerializeField]
    public float attackSpeed;    //공격속도

    [SerializeField]
    protected int range;  //사거리

    [SerializeField]
    protected int regeneration;   // 회복력

    [Header("AttackType")]  //공격타입 : 원거리, 근거리, 단일공격, 다중공격 

    [SerializeField]
    protected int attackRangeType; // 0:근거리타입 1:원거리타입

    [SerializeField]
    protected int attackTargetNum;    //공격 타겟 수 

    [Header("multipleAttack")]    //다중 공격 유닛 전용 능력치

    [SerializeField]
    protected int multiAttackRange;  //다중공격 사거리

    [Header("Bullet")]  //원거리 공격 유닛 - 총알 관련
    [SerializeField]
    protected int bulletIndex;  //사용할 총알 번호

    [SerializeField]
    protected GameObject firePos;  //단일 공격시 총알이 발사되는 시작점

    [SerializeField]
    public GameObject hitPos;   //총알과 충돌하는 객체의 위치

    [SerializeField]
    public GameObject dropPos;  //다중 공격시 총알이 떨어지는 시작점    

    protected float attackTimer;  //공격시간 타이머

    [Header("data")]    //기타 데이터
    [SerializeField]
    protected bool reverse; //오브젝트의 3d모델의 방향이 반대로 된 경우

    [SerializeField]
    protected Animator animator; //애니메이터

    [SerializeField]
    protected List<GameObject> attackTargets;    //공격할 타겟

    //[SerializeField]
    //public GameObject attackOwner;      // 공격을 실행하는 사람

    public Vector3 attackDirVec;   //공격할 타겟의 방향벡터
    
    public float bulletSpeed = 50f;

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
    }

    /// <summary>
    /// 초기화 함수 : 김현진
    /// </summary>
    protected virtual void Initialize()
    {
        //HP초기화
        currentHP = maxHP;
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
    protected virtual void DetectTarget(List<GameObject> target)
    {
        //공격 유닛이 아닌경우
        if (attackTargetNum == 0)
            return;

        //리스트 초기화
        attackTargets.Clear();

        for (int i = 0; i < target.Count; i++)
        {
            //사거리 안에 가장 먼저 감지된 타겟
            if (target[i].activeSelf && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < range)
            {
                //다중 타겟 유닛일경우
                if (attackTargetNum > 1)
                {
                    //사거리 안에 가장 먼저 감지된 타겟
                    attackTargets.Add(target[i]);
                    attackDirVec = Vector3.zero;

                    //공격 사거리 안에 감지 될 타겟 추가
                    DetectTargets(target, i);
                }
                else
                {
                    //타겟과 타겟 방향벡터 초기화
                    attackTargets.Add(target[i]);
                    attackDirVec = (attackTargets[0].transform.position - transform.position).normalized;

                    //공격
                    Attack();
                }   

                return;
            }

        }//end of for
    }

    /// <summary>
    /// 다중 타겟 유닛일 경우, this 객체의 공격 사거리 안에있는 타겟을 감지해 그중 공격할 타겟들을 지정 : 김현진
    /// </summary>
    /// <param name="target">타겟이 될 대상 배열</param>
    /// <param name="detectedUnitIndex">가장 먼저 감지된 유닛 인덱스</param>
    protected void DetectTargets(List<GameObject> target,int detectedUnitIndex)
    {
        Dictionary<GameObject, float> targetDistances = new Dictionary<GameObject, float>();

        for (int i = 0; i < target.Count; i++)
        {
            //사거리 안에 가장 먼저 감지된 타겟을 제외한 공격 사거리 안에 감지된 유닛들
            if ((target[i].activeSelf && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < multiAttackRange) && (i != detectedUnitIndex))
            {
                targetDistances.Add(target[i],Vector3.SqrMagnitude(target[i].transform.position - transform.position)); 
            }
        }

        //거리순으로 오름차순 정렬
        var sortedTargetDistances = targetDistances.OrderBy(x => x.Value);

        //거리순으로 최대 타겟 가능 유닛 이하로 넣어준다
        foreach (KeyValuePair<GameObject, float> item in sortedTargetDistances)
        {
            attackTargets.Add(item.Key);

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
        //예외처리
        if (attackDirVec == Vector3.zero)
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
            //DecreaseHP
            animator.SetBool("meleeAttack", false);
        }

        //공격할 대상의 방향으로 회전, 다중 공격 유닛이 아닐 경우에만 실시간 회전
        if (attackTargetNum <= 1)
        {
            Quaternion rotation;

            if (reverse)
                rotation = Quaternion.LookRotation((new Vector3(attackDirVec.x, 0, attackDirVec.z)));
            else
                rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
        }
    }

    /// <summary>
    /// 총알 위치 초기화 : 하은비
    /// </summary>
    void InitializeBullet()
    {
        //예외처리
        if (!attackTargets[0] || !attackTargets[0].activeSelf)
            return;

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
                Actor actor = attackTargets[i].GetComponent<Actor>();
                SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, actor.dropPos.transform.position, attackTargets[i]);
            }
        }
    }
}
