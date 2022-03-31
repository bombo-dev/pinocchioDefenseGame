using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TurretState
{
    Idle = 0,       // 기본 상태
    Ready = 1,   // Enemy를 감지하여 Enemy 방향으로 회전
    Attack,        // 감지한 Enemy 공격
    Dead,          //  Enemy에 의해 죽은 상태. 비활성화
}

public class TurretAttack : MonoBehaviour
{
    // Actor
    int maxHP;
    int power;
    float attackSpeed;

    Vector3 distance;
    int detectRange = 50;    // Enemy 감지용 범위 
    int attackRange;           // Enemy 공격용 범위
    Vector3 dirVec;             // Turret 회전 방향 벡터

    TurretState turretState = TurretState.Idle;     // 터렛의 초기 상태

    GameObject enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("TestSwordMan");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurret();
    }

    void UpdateTurret()
    {        
        switch(turretState)
        {
            case TurretState.Idle:
                FindEnemy();                
                break;
            case TurretState.Ready:
                DetectEnemy(enemy);
                GetDirVector();
                break;
        }
        
    }
    void FindEnemy()
    {
        turretState = TurretState.Ready;
    }
    /// <summary>
    /// 공격을 위한 가장 가까운 Enemy가 있는지 감지하는 상태 : 하은비
    /// </summary>
    /// <param name="enemy"></param>
    void DetectEnemy(GameObject enemy)
    {
        distance = (enemy.transform.position - this.transform.position);

        if (distance.magnitude <= detectRange)
        {            
            Debug.Log("detect Enemy!");
        }
    }

    /// <summary>
    /// 감지한 Enemy의 방향 벡터를 구하고 터렛 회전 시키기.
    /// </summary>
    void GetDirVector()
    {
        dirVec = distance.normalized;
        RotateTurret(dirVec);
    }

    void RotateTurret(Vector3 dirVec)
    {
        Quaternion rotation = Quaternion.LookRotation(dirVec);
        this.transform.rotation = rotation;
    }
    void Attack()
    {

    }
}
