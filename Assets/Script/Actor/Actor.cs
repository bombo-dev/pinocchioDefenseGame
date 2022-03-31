using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //Actor
    [SerializeField]
    protected int maxHP;   //최대 체력

    [SerializeField]
    protected int currentHP;   //현재 체력

    [SerializeField]
    protected int power;  // 공격력

    [SerializeField]
    protected int attackSpeed;    //공격속도

    [SerializeField]
    protected int range;  // 사거리

    [SerializeField]
    protected int regeneration;   // 회복력

    //공격관련
    protected float attackTimer;  //공격시간 타이머

    [SerializeField]
    protected Animator animator; //애니메이터

    [SerializeField]
    protected GameObject attackTarget;    //공격할 타겟

    [SerializeField]
    protected Vector3 attackDirVec;   //공격할 타겟의 방향벡터

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
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
    /// this객체의 사거리 안에있는 타겟을 감지해 그중 공격할 타겟을 지정 : 김현진
    /// </summary>
    protected virtual void DetectTarget(GameObject[] target)
    {
        for (int i = 0; i < target.Length; i++)
        {
            //Debug.Log(i.ToString() + " : " + Vector3.SqrMagnitude(target[i].transform.position - transform.position));
            //사거리 안에 가장 먼저 감지된 타겟
            if (target[i].activeSelf && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < range)
            {
                //타겟과 타겟 방향벡터 초기화
                attackTarget = target[i];
                attackDirVec = (attackTarget.transform.position - transform.position).normalized;

                //공격
                Attack();

                return;
            }
        }
    }

    /// <summary>
    /// this객체의 상태를 공격으로 변경 : 김현진
    /// </summary>
    protected virtual void Attack()
    {
        //공격시간 측정 변수 초기화
        attackTimer = Time.time;
    }

    /// <summary>
    /// 실시간으로 공격이 끝났는지 안끝났는지를 판별하고 끝났을경우 
    /// 다음 공격으로 이행할지 다른 상태로 변경할지를 결정 : 김현진
    /// </summary>
    protected virtual void UpdateBattle()
    {
        //공격할 대상의 방향으로 회전
        Quaternion rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
    }
}
