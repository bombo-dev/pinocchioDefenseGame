using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum EnemyState
    {
        Walk,   //�ʵ��� �����͸� ���� �̵�
        Attack, //�ͷ��� ����
        Dead    //�̵�X, ��Ȱ��ȭ ó��
    }
    [SerializeField]
    EnemyState enemyState = EnemyState.Walk;

    //Actor State
    [SerializeField]
    int maxHP;   //�ִ� ü��

    [SerializeField]
    int power;  // ���ݷ�

    [SerializeField]
    int attackSpeed;    //���ݼӵ�

    [SerializeField]
    int range;  // ��Ÿ�

    [SerializeField]
    int regeneration;   // ȸ����

    [SerializeField]
    int speed;  //�̵��ӵ�


    //�̵� ����
    [SerializeField]
    GameObject[] targetTile;    //Ÿ�ϸ� ���� �ִ� �̵� Ÿ��

    int targetTileIndex = 0;    //Ÿ�ϸ� Ÿ�� �ε���

    GameObject currentTarget;   //���� Ÿ��

    Vector3 dirVec; //�̵�ó���� ���⺤��



    //���ݰ���
    float attackTimer = 0;  //���ݽð� Ÿ�̸�

    [SerializeField]
    Animator enemyAnimator; //�ִϸ�����

    [SerializeField]
    GameObject attackTarget;    //������ Ÿ��

    [SerializeField]
    Vector3 attackDirVec;   //������ Ÿ���� ���⺤��



    //test
    [SerializeField]
    GameObject[] turret;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = targetTile[targetTileIndex];
        dirVec = FindDirVec(currentTarget);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemy();
    }

    /// <summary>
    /// this��ü�� ��ġ���� target������ ���⺤�͸� ���� ��ȯ : ������
    /// </summary>
    /// <param name="target">�������� �� Ÿ��</param>
    /// <returns>������ ���⺤��</returns>
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
    /// �ǽð� this��ü�� ���º� ���� �Լ� ȣ��
    /// </summary>
    void UpdateEnemy()
    {
        switch (enemyState)
        {
            case EnemyState.Walk:
                CheckArrive();
                UpdateMove(dirVec);
                DetectTurret();
                break;
            case EnemyState.Attack:
                CheckFinAttack();
                break;

        }
    }

    #region Walk - �̵��� �� ����

    /// <summary>
    /// ��ǥ �������� ���� �ߴ��� Ȯ�� : ������
    /// </summary>
    void CheckArrive()
    {
        //����ó��
        if (currentTarget == null)
            return;
        if (targetTileIndex >= targetTile.Length - 1)
            return;

            //Ÿ�ٿ� �������� �ʾ��� ���
            if (Vector3.Distance(transform.position, currentTarget.transform.position) > 0.5f)
        {
            float rotY = Mathf.Round(transform.localEulerAngles.y);
            //����ó��, �ӵ��� ���� distance�� �������� ������ ��� ���⺰ ����ó��
            if (!((rotY == 0f && transform.position.z < currentTarget.transform.position.z)//����
                || (rotY == 90f && transform.position.x < currentTarget.transform.position.x)//������
                || (rotY == 180f && transform.position.z > currentTarget.transform.position.z)//����
                || (rotY == 270f && transform.position.x > currentTarget.transform.position.x)))//����
            {
                return;
            }
        }

        //�̵� Ÿ�� ����
        transform.position = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
        
        currentTarget = targetTile[++targetTileIndex];

        dirVec = FindDirVec(currentTarget);
    }

    /// <summary>
    /// ���� ���ͷ� �ǽð� this��ü�� ��ġ�� ȸ���� ����: ������ 
    /// </summary>
    /// <param name="dirVec">�̵��� ȸ���� ���⺤��</param>
    void UpdateMove(Vector3 dirVec)
    {
        //����ó��
        if (dirVec == Vector3.zero)
            return;

        Vector3 updateVec = new Vector3(dirVec.x * speed * Time.deltaTime, 0 , dirVec.z * speed * Time.deltaTime);
        transform.position += updateVec;
        Quaternion rotation = Quaternion.LookRotation(-dirVec);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
    }

    /// <summary>
    /// this��ü�� ��Ÿ� �ȿ��ִ� �ͷ��� ������ ���� ������ Ÿ���� ���� : ������
    /// </summary>
    void DetectTurret()
    {
        for (int i = 0 ; i < turret.Length; i++)
        {
           // Debug.Log(i.ToString() + " : " + Vector3.Distance(transform.position, turret[i].transform.position));
            //��Ÿ� �ȿ� ���� ���� ������ �ͷ�
            if (turret[i].activeSelf && Vector3.Distance(transform.position, turret[i].transform.position) < range)
            {
                //Ÿ�ٰ� Ÿ�� ���⺤�� �ʱ�ȭ
                attackTarget = turret[i];
                attackDirVec = (attackTarget.transform.position - transform.position).normalized;

                //����
                Attack();

                return;
            }
        }
    }

    #endregion

    #region Attack - ������ �� ����

    /// <summary>
    /// this��ü�� ���¸� �������� ���� : ������
    /// </summary>
    void Attack()
    {
        //���� ���·� ���� 
        this.enemyState = EnemyState.Attack;

        //���ݽð� ���� ���� �ʱ�ȭ
        attackTimer = Time.time;

        //���� 
        enemyAnimator.SetBool("attack",true);
        enemyAnimator.SetBool("finAttack", false);
    }

    /// <summary>
    /// �ǽð����� ������ �������� �ȳ��������� �Ǻ��ϰ� ��������� 
    /// ���� �������� �������� �ٸ� ���·� ���������� ���� : ������
    /// </summary>
    void CheckFinAttack()
    {
        //������ ����� �������� ȸ��
        Quaternion rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);

        //attackSpeed�ʿ� 1�� ����
        if (Time.time - attackTimer > attackSpeed)
        {
            //������ ����� ���� ������ ���� ���� ��ȭ
            if (attackTarget == null || !(attackTarget.activeSelf))
            {
                enemyState = EnemyState.Walk;

                //���� ����
                enemyAnimator.SetBool("finAttack", true);
            }
            else
            {
                //���ݽð� ���� ���� �ʱ�ȭ
                attackTimer = Time.time;

                //���� ����
                enemyAnimator.SetBool("attack", true);
            }
        }
    }

    #endregion
}
