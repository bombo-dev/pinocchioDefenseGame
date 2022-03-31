using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TurretState
{
    Idle = 0,       // �⺻ ����
    Ready = 1,   // Enemy�� �����Ͽ� Enemy �������� ȸ��
    Attack,        // ������ Enemy ����
    Dead,          //  Enemy�� ���� ���� ����. ��Ȱ��ȭ
}

public class TurretAttack : MonoBehaviour
{
    // Actor
    int maxHP;
    int power;
    float attackSpeed;

    Vector3 distance;
    int detectRange = 50;    // Enemy ������ ���� 
    int attackRange;           // Enemy ���ݿ� ����
    Vector3 dirVec;             // Turret ȸ�� ���� ����

    TurretState turretState = TurretState.Idle;     // �ͷ��� �ʱ� ����

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
    /// ������ ���� ���� ����� Enemy�� �ִ��� �����ϴ� ���� : ������
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
    /// ������ Enemy�� ���� ���͸� ���ϰ� �ͷ� ȸ�� ��Ű��.
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
