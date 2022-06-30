using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float bulletMaxDistance = 200.0f;

    const float bulletMaxLifeTime = 3.0f; //불릿이 존재할 수 있는 최대 시간 

    public float bulletLifeTime = 0.0f;   //불릿이 존재할 수 있는 최대 시간 측정

    public GameObject attackTarget;    //총알의 최종 목적지  

    [SerializeField]
    public float bulletSpeed;    // 총알의 이동 속도

    public float initSpeed;    // 총알의 초기 속도를 저장할 변수

    [SerializeField]
    float maxforce;

    [SerializeField]
    string filePath; 

    [SerializeField]
    int bulletType; //0: 직선형 총알, 1: 곡선형 총알 

    [SerializeField]
    float reduceHeight;         // 곡선형 공격의 포물선 높이 조절 변수

    float distance = 0.0f;

    [SerializeField]
    public float force;    // 가속도

    public float initForce;    // 초기 가속도를 저장할 변수

    public GameObject attackOwner;

    int i = 0;

    int damage; // 디버프를 적용한 데미지

    int recovery; //디버프를 적용한 회복량

    Vector3 initPos;



    // Update is called once per frame
    void Update()
    {
        UpdateBullet(); 
    }
    private void Start()
    {
        initPos = transform.position;
        initForce = force;
        initSpeed = bulletSpeed;
    }
    /// <summary>
    /// 총알 발사 업데이트 : 하은비
    /// </summary>
    void UpdateBullet()
    {
        //예외처리
        /*
        if (!attackTarget || bulletLifeTime > 200)
        {            
            // 총알 파괴 모션
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
            
            force = initForce;

            bulletSpeed = initSpeed;

            return;
        }*/


            Vector3 bulletPos = transform.position;   // 총알의 위치
        Vector3 targetPos = attackTarget.transform.position;  // 타겟이 총알을 맞는 위치

        // 총알 방향 업데이트
        Vector3 bulletAttackDir = (attackTarget.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(bulletAttackDir);
        transform.rotation = rotation;

        

        if (Time.timeScale != 0)
        {
            // 가속도 붙이기
            bulletSpeed += (bulletSpeed * force);
            force += 0.003f;
        }

        if (float.IsInfinity(targetPos.x))
        {
            targetPos = Vector3.zero;
        }

        if (bulletType == 0) // 직선형
        {
            //transform.position = Vector3.Lerp(bulletPos, targetPos, moveDist*Time.deltaTime*0.2f);  
            Vector3 translation = (targetPos - bulletPos).normalized * Time.deltaTime * bulletSpeed * 1.5f;
            transform.position += translation;

        }
        else if (bulletType == 1) //곡선형
        {
            
            Vector3 center = (bulletPos + targetPos) / 2;
            center -= new Vector3(0, reduceHeight * 1.0f, 0);
            Vector3 startPos = bulletPos - center;
            Vector3 endPos = targetPos - center;

            transform.position = Vector3.Slerp(startPos, endPos, Time.deltaTime * bulletSpeed * 0.025f);
            transform.position += center;

        }


        // bullet과 target의 거리가 10보다 작을 경우 불렛 비활성화
        distance = (targetPos - bulletPos).sqrMagnitude;

        //Debug.Log("distance= " + (Mathf.Round(distance)));
        if ((Mathf.Round(distance)) < bulletMaxDistance)
        {      
            GameObject target;
            target = attackTarget.transform.parent.gameObject;

            //예외처리
            if (target.GetComponent<Actor>().currentHP == 0 || !target)
            {

                SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);

                force = initForce;

                bulletSpeed = initSpeed;

                return;
            }

            //회복 타워 
            if (attackOwner.GetComponent<Actor>().isRecoveryTower)
            {
                if (target.tag == "Enemy")
                {
                    Enemy recoveryTarget = target.GetComponent<Enemy>();
                    Enemy attacker = attackOwner.GetComponent<Enemy>();

                    recovery = attacker.currentPower;

                    // 회복력 버프를 적용한 현재 회복력
                    recovery += (int)((float)recovery * ((float)recoveryTarget.currentRegeneration / (float)100));

                    if (recovery > 0)
                    {
                        // 회복 UI 생성
                        GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, target);

                        if (!damageMngPanelGo)
                            return;

                        DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                        // 회복 UI 화면에 띄우기
                        damageMngPanel.ShowDamage(recovery, 1);

                        recoveryTarget.damageMngPanel = damageMngPanel;
                        damageMngPanel.damageOwner = recoveryTarget.gameObject;
                    }

                    //피 공격자 회복
                    recoveryTarget.IncreaseHP(recovery);

                    //피 공격자의 데미지 이펙트 출력
                    recoveryTarget.EnableHealEffect(attacker);
                }
                else if (target.tag == "Turret")
                {
                    Turret recoveryTarget = target.GetComponent<Turret>();
                    Turret attacker = attackOwner.GetComponent<Turret>();

                    recovery = attacker.currentPower;

                    // 회복력 버프를 적용한 현재 회복력
                    recovery += (int)((float)recovery * ((float)recoveryTarget.currentRegeneration / (float)100));

                    if (recovery > 0)
                    {
                        // 회복 UI 생성
                        GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, target);

                        if (!damageMngPanelGo)
                            return;

                        DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                        // 회복 UI 화면에 띄우기
                        damageMngPanel.ShowDamage(recovery, 1);

                        recoveryTarget.damageMngPanel = damageMngPanel;
                        damageMngPanel.damageOwner = recoveryTarget.gameObject;

                        //전투분석
                        SystemManager.Instance.GameFlowManager.AnalyzeTurretBattle(recovery, attacker.turretNum);
                    }

                    //피 공격자 회복
                    recoveryTarget.IncreaseHP(recovery);
                    //피 공격자의 데미지 이펙트 출력
                    recoveryTarget.EnableHealEffect(attacker);
                }

                SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);

                force = initForce;

                bulletSpeed = initSpeed;
            }
            //공격 타워
            else if (target.tag == "Enemy")
            {
                Enemy enemy = target.GetComponent<Enemy>();

                Turret attacker = attackOwner.GetComponent<Turret>();

                // 방어력 디버프를 적용한 데미지 계산
                damage = attacker.currentPower - (int)(attacker.currentPower * ((float)enemy.currentDefense * 0.01f));

                enemy.DecreaseHP(damage);

                if (damage > 0)
                {                                        
                    // 데미지 UI 생성
                    GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, target);

                    if (!damageMngPanelGo)
                        return;

                    DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();
                    
                    // 활성화된 데미지 패널을 리스트에 저장
                    //SystemManager.Instance.PanelManager.damagePanels.Add(damageMngPanelGo);

                    // 데미지 UI 화면에 띄우기
                    damageMngPanel.ShowDamage(damage, 0);

                    enemy.damageMngPanel = damageMngPanel;
                    damageMngPanel.damageOwner = enemy.gameObject;
                   
                }
                //피 공격자 디버프 걸기
                if (attacker.debuffType > 0)
                {
                    enemy.AddDebuff(attacker.debuffType, attacker.debuffDuration);

                    //피 공격자에게 디버프 이펙트 출력
                    enemy.EnableDebuffEffect(attacker);
                }
                //피 공격자에게 데미지 이펙트 출력
                enemy.EnableDamageEffect(attacker);

                //전투분석
                SystemManager.Instance.GameFlowManager.AnalyzeTurretBattle(damage, attacker.turretNum);
            }
            //공격 타워
            else if(target.tag == "Turret")
            {
                Turret turret = target.GetComponent<Turret>();

                Enemy attacker = attackOwner.GetComponent<Enemy>();

                // 방어력 디버프를 적용한 데미지 계산
                damage = attacker.currentPower - (int)(attacker.currentPower * ((float)turret.currentDefense * 0.01f));

                turret.DecreaseHP(damage);

                if (damage > 0)
                {                    
                    // 데미지 UI 생성
                    GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, target);

                    if (!damageMngPanelGo)
                        return;
                                        
                    DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                    // 활성화된 데미지 패널을 리스트에 저장
                    //SystemManager.Instance.PanelManager.damagePanels.Add(damageMngPanelGo);

                    // 데미지 UI 화면에 띄우기
                    damageMngPanel.ShowDamage(damage, 0);

                    damageMngPanel.damageOwner = turret.gameObject;
                    turret.damageMngPanel = damageMngPanel;
                }
                //피 공격자 디버프 걸기
                if (attacker.debuffType > 0)
                {
                    turret.AddDebuff(attacker.debuffType, attacker.debuffDuration);

                    //피 공격자에게 디버프 이펙트 출력
                    turret.EnableDebuffEffect(attacker);
                }
                //피 공격자에게 데미지 이펙트 출력
                turret.EnableDamageEffect(attacker);
            }

            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);

            // Debug.Log("-----------------------------------------------미사일 " + (i++) + "번째");

            force = initForce;

            bulletSpeed = initSpeed;
        }
    }

}
