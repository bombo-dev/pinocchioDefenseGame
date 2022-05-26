using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    enum EnemyState
    {
        Walk,   //�ʵ��� �����͸� ���� �̵�
        Battle, //�ͷ��� ����
        Dead    //�̵�X, ��Ȱ��ȭ ó��
    }
    [SerializeField]
    EnemyState enemyState = EnemyState.Walk;

    [Header("EnemyStat")]   //Enemy �ɷ�ġ

    [SerializeField]
    int speed;  //�̵��ӵ�

    [SerializeField]
    int currentSpeed; //���� �̵��ӵ�

    [Header("EnemyInfo")]   //Enemy ����

    [SerializeField]
    public int enemyIndex;  //enemy���� ��ȣ

    [SerializeField]
    public int gateNum;    //���� ����Ʈ ��ȣ

    [SerializeField]
    Vector3[] appearPos;  //������ġ

    [Header("Move")]    //�̵�����

    [SerializeField]
    public GameObject[] targetPoint;    //Ÿ�ϸ� ���� �ִ� �̵� Ÿ��

    int targetPointIndex = 0;    //Ÿ�ϸ� Ÿ�� �ε���

    GameObject currentTarget;   //���� Ÿ��

    Vector3 dirVec; //�̵�ó���� ���⺤��

    

    /// <summary>
    /// �ʱ�ȭ �Լ� : ������
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        Reset();
    }

    public override void Reset()
    {
        base.Reset();

        //�̵��ӵ� �ʱ�ȭ
        currentSpeed = speed;

        //��ġ�ʱ�ȭ
        transform.position = appearPos[gateNum];

        //�����ʱ�ȭ
        enemyState = EnemyState.Walk;

        //�ִϸ��̼� �÷��� �ʱ�ȭ
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

        //Enemy�ִϸ��̼� State �ʱ����
        animator.Play("Walk");

        //�̵� Ÿ�� Ÿ�� �迭 �ʱ�ȭ
        targetPointIndex = 0;
        currentTarget = targetPoint[targetPointIndex];

        //�̵� Ÿ�� �迭�� ù��° Ÿ�Ϸ� ���⺤�� �ʱ�ȭ
        dirVec = FindDirVec(currentTarget);

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
    /// �ǽð� this��ü�� ���º� ���� : ������
    /// </summary>
    protected override void UpdateActor()
    {
        base.UpdateActor();

        switch (enemyState)
        {
            case EnemyState.Walk:
                UpdateHPBarsPos();
                CheckArrive();
                UpdateMove(dirVec);
                DetectTarget(SystemManager.Instance.TurretManager.turrets);                
                break;
            case EnemyState.Battle:
                UpdateHPBarsPos();
                UpdateBattle();                
                break;
            case EnemyState.Dead:
                UpdateDead();
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
        if (targetPointIndex >= targetPoint.Length - 1)
            return;

        //Ÿ�ٿ� �������� �ʾ��� ���
        if (Vector2.SqrMagnitude(new Vector2(transform.position.x, transform.position.z) - new Vector2(currentTarget.transform.position.x, currentTarget.transform.position.z)) > 2f)
        {
            float rotY = Mathf.Round(transform.localEulerAngles.y);

            //����ó��, �ӵ��� ���� distance�� �������� ������ ��� ���⺰ ����ó��
            if (rotY == 360f)
                rotY = 0f;
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
        
        currentTarget = targetPoint[++targetPointIndex];

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

        Vector3 updateVec = new Vector3(dirVec.x * currentSpeed * Time.deltaTime, 0 , dirVec.z * currentSpeed * Time.deltaTime);
        transform.position += updateVec;
        Quaternion rotation = Quaternion.LookRotation(-dirVec);

        //ȸ��
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
        //ȸ�� x,z�� ����
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y,0);
    }

    /// <summary>
    /// this��ü�� ��Ÿ� �ȿ��ִ� Ÿ���� ������ ���� ������ Ÿ���� ���� : ������
    /// </summary>
    protected override void DetectTarget(List<GameObject> target)
    {
        base.DetectTarget(target);
    }

    public void UpdateEnemyPos(GameObject go)
    {        
        go.transform.position = transform.position;
    }

    #endregion

    #region Attack - ������ �� ����

    /// <summary>
    /// this��ü�� ���¸� �������� ���� : ������
    /// </summary>
    protected override void Attack()
    {
        base.Attack();

        //���� ���·� ���� 
        this.enemyState = EnemyState.Battle;

        //����
        animator.SetBool("finAttack", false);
    }

    /// <summary>
    /// �ǽð����� ������ �������� �ȳ��������� �Ǻ��ϰ� ��������� 
    /// ���� �������� �������� �ٸ� ���·� ���������� ���� : ������
    /// </summary>
    protected override void UpdateBattle()
    {
        base.UpdateBattle();

        //attackSpeed�ʿ� 1�� ����
        if (Time.time - attackTimer > currentAttackSpeed)
        {
            //����,���� Ÿ�� �ִϸ��̼� �Ķ���� �ʱ�ȭ
            if (attackTargetNum >= 1)
            {
                animator.SetBool("attackCancel", false);

                if (attackRangeType == 0)
                    animator.SetBool("meleeAttack", false);
                else
                    animator.SetBool("rangedAttack", false);
            }

            //������ ����� ���� ������ ���� ���� ��ȭ
            if (attackTargetsActor[0].currentHP <= 0 || !(attackTargets[0].activeSelf))
            {
                animator.SetBool("attack", false);
                animator.SetBool("finAttack", true);

                enemyState = EnemyState.Walk;     
            }
            else
            {
                //���ݽð� ���� ���� �ʱ�ȭ
                attackTimer = Time.time;

                //���� Ÿ�� ������ ��� Ÿ�� �迭 �缳��
                if (attackTargetNum > 1)
                {
                    //���� ��Ÿ� �ȿ� ���� �� Ÿ�� �߰�
                    DetectTarget(SystemManager.Instance.TurretManager.turrets);
                }

                //���� ����
                animator.SetBool("attack", true);
                animator.SetBool("finAttack", false);
            }
        }
    }

    /// <summary>
    /// ���� HP ���ҿ� ���ó�� : ������
    /// </summary>
    /// <param name="damage"></param>
    #endregion

    #region Dead - HP ���ҿ� ���

    public override void DecreaseHP(int damage)
    {
        base.DecreaseHP(damage);

        if (SystemManager.Instance.PanelManager.enemyHPBars[enemyIndex])
        {
            StatusMngPanel statusMngPanel = SystemManager.Instance.PanelManager.enemyHPBars[enemyIndex].GetComponent<StatusMngPanel>();
            statusMngPanel.SetHPBar(currentHP, maxHP);
        }
        else
            return;

        if (currentHP == 0)
        {            
            SystemManager.Instance.PanelManager.DisablePanel<StatusMngPanel>(SystemManager.Instance.PanelManager.enemyHPBars[enemyIndex].gameObject);
            SystemManager.Instance.PanelManager.ReorganizationPanelList(enemyIndex, GetType());
            SystemManager.Instance.EnemyManager.ReorganizationEnemiesList(enemyIndex);
            
            enemyState = EnemyState.Dead;
        }
    }

    /// <summary>
    /// �ͷ� HP ���� : ������
    /// </summary>
    /// <param name="recoveryPower">������</param>
    public override void IncreaseHP(int recoveryPower)
    {
        base.IncreaseHP(recoveryPower);
    }

    /// <summary>
    /// ����Ʈ ���
    /// </summary>
    /// <param name="attacker">������</param>
    public override void EnableDamageEffect(Actor attacker)
    {
        base.EnableDamageEffect(attacker);
    }

    /// <summary>
    /// ���� Dead �ִϸ��̼� ���� ó�� : ������
    /// </summary>
    protected override void UpdateDead()
    {
        base.UpdateDead();

        //Dead���� ����
        if (!animator.GetBool("isDead"))
        {
            // ���ʹ� ��Ȱ��ȭ
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
            return;
        }         
    }

    #region �����
    /// <summary>
    /// ����� �߰� : ������
    /// </summary>
    /// <param name="debuffIndex">�߰��� ����� ���� �ε���</param>
    /// <param name="time">�߰��� ������� ���ӽð�</param>
    public override void AddDebuff(int debuffIndex, float time)
    {
        base.AddDebuff(debuffIndex, time);

        //Ǯ�����ϰ�� ����ó��
        if (debuffs[(debuff)debuffIndex].stack > 5)
            return;

        //����� ȿ��
        switch (debuffIndex)
        {
            case 1: //���� �ӵ� ����
                currentAttackSpeed *= 1.2f;
                break;
            case 2: //�̵� �ӵ� ����
                currentSpeed -= (currentSpeed / 5);
                break;
            case 3: //���� ����
                currentDefense -= (currentDefense / 5);
                break;
            case 4: //���ݷ� ����
                currentPower -= (currentPower / 5);
                break;
            case 5: //���� - ���ݼӵ�, �̵��ӵ� ��������
                currentPower -= (currentPower / 2);
                currentSpeed -= (currentSpeed / 2);
                break;
            case 6: //ȭ�� - ���� ��������
                currentDefense -= (currentDefense / 2);
                break;
        }
    }

    /// <summary>
    /// ����� ���� : ������
    /// </summary>
    /// <param name="debuffIndex">������ �����</param>
    protected override void RemoveDebuff(int debuffIndex)
    {
        base.RemoveDebuff(debuffIndex);

        //����� ȿ��
        switch (debuffIndex)
        {
            case 1: //���� �ӵ� ����
                currentAttackSpeed = attackSpeed;
                break;
            case 2: //�̵� �ӵ� ����
                currentSpeed = speed;
                break;
            case 3: //���� ����
                currentDefense = defense;
                break;
            case 4: //���ݷ� ����
                currentPower = power;
                break;
            case 5: //���� - ���ݼӵ�, �̵��ӵ� ��������
                currentPower = power;
                currentSpeed = speed;
                break;
            case 6: //ȭ�� - ���� ��������
                currentDefense = defense;
                break;
        }
    }
    #endregion

    protected override void UpdateHPBarsPos()
    {
        base.UpdateHPBarsPos();

        Vector3 screenPos = Camera.main.WorldToScreenPoint(hpPos.transform.position);
        //Debug.Log("Enemy.screenPos=" + screenPos);
        SystemManager.Instance.PanelManager.enemyHPBars[enemyIndex].transform.position = screenPos;
    }
}



#endregion
