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

    public int enemyNum;   //enemy ���� ��ȣ (enemy ������ ���� ��ȣ �ο�)

    [SerializeField]
    public int enemyIndex;  //enemy���� ��ȣ

    [SerializeField]
    public int gateNum;    //���� ����Ʈ ��ȣ

    [SerializeField]
    Vector3[] appearPos;  //������ġ

    [Header("Move")]    //�̵�����

    [SerializeField]
    public GameObject[] targetPoint;    //Ÿ�ϸ� ���� �ִ� �̵� Ÿ��

    [SerializeField]
    int targetPointIndex = 0;    //Ÿ�ϸ� Ÿ�� �ε���

    [SerializeField]
    GameObject currentTarget;   //���� Ÿ��

    Vector3 dirVec; //�̵�ó���� ���⺤��

    bool isEndShow = false; // �ڷ�ƾ�� ���Ῡ�� Ȯ�� �÷���

    int i = 0;

    bool selfDestruct = false;  //������ ��� true

    [SerializeField]
    int rewardWoodResource; //������� ���� woodResource

    // ���ʹ��� hpBar �г�
    public StatusMngPanel statusMngPanel;

    public DamageMngPanel damageMngPanel;

    //JsonData
    [SerializeField]
    protected EnemyData[] enemyDatas;

    /// <summary>
    /// �ʱ�ȭ �Լ� : ������
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

        //�̵��ӵ� �ʱ�ȭ
        currentSpeed = speed;

        //��Ÿ� �ʱ�ȭ - ����Ʈ�� ������ ���������� ������(targetIndex > 0) Walk���� 
        currentRange = 0;

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
                UpdatePanelPos();
                CheckArrive();
                UpdateMove(dirVec);

                if (!isRecoveryTower)    //���� ����
                    DetectTarget(SystemManager.Instance.TurretManager.turrets);
                else    //ȸ�� ����
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

    #region Walk - �̵��� �� ����

    /// <summary>
    /// ��ǥ �������� ���� �ߴ��� Ȯ�� : ������
    /// </summary>
    void CheckArrive()
    {
        //����ó��
        if (currentTarget == null)
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
        //transform.position = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);

        //��Ÿ��ʱ�ȭ
        if (targetPointIndex == 0)
        {
            //��Ÿ� �ʱ�ȭ - ����Ʈ�� ������ �������� ����
            currentRange = range;
        }

        //Ÿ�� �ε��� ����
        targetPointIndex++;

        //������ Ÿ�ٿ� �����������
        if (targetPointIndex >= targetPoint.Length)
        {
            //���� Ÿ���� �ƴѰ�� ���� ó��
            if (attackTargetNum <= 0 || isRecoveryTower)
            {
                selfDestruct = true;

                //Deadó�� �ϱ�
                DecreaseHP(0);

                //Base�ͷ� Ÿ��
                SystemManager.Instance.TurretManager.turrets[0].GetComponent<Turret>().DecreaseHP(power);
            }
            return;
        }

        //Ÿ�� ����
        currentTarget = targetPoint[targetPointIndex];
        //���⺤�� ����� Ÿ������ ���� 
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

        Vector3 updateVec = new Vector3(dirVec.x * currentSpeed * Time.deltaTime, 0, dirVec.z * currentSpeed * Time.deltaTime);
        transform.position += updateVec;
        Quaternion rotation = Quaternion.LookRotation(-dirVec);

        //ȸ��
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
        //ȸ�� x,z�� ����
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    /// <summary>
    /// this��ü�� ��Ÿ� �ȿ��ִ� Ÿ���� ������ ���� ������ Ÿ���� ���� : ������
    /// </summary>
    protected override void DetectTarget(List<GameObject> target, GameObject mine = null)
    {
        if (mine)   //ȸ��Ÿ���� �ڽ��� �������� �ʰ��Ѵ�
            base.DetectTarget(target, mine);
        else
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

            //ȸ��Ÿ���ϰ��
            if (isRecoveryTower)
            {
                animator.SetBool("attack", false);
                animator.SetBool("finAttack", true);

                enemyState = EnemyState.Walk;

                return;
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
                //���� ����Ʈ ȣ��
                EnableFireEffect(this);
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

        //HP�� 0������ �������ų� �������°� �� ���
        if (currentHP <= 0 || selfDestruct)
        {
            //StatusMngPanel ��Ȱ��ȭ
            SystemManager.Instance.PanelManager.DisablePanel<StatusMngPanel>(statusMngPanel.gameObject);

            // StatusMngPanel ����
            statusMngPanel.StatusReset();
          
            // ���ʹ� ����Ʈ �籸��
            SystemManager.Instance.EnemyManager.ReorganizationEnemiesList(enemyIndex);

            enemyState = EnemyState.Dead;

            //��ġ
            if (!selfDestruct)
                //������� ���� ����
                SystemManager.Instance.ResourceManager.IncreaseWoodResource(rewardWoodResource);
            //����
            else
            {
                //���� ����Ʈ ���
                EnableDamageEffect(this);

                //Flashȿ�� 
                callFlashCoroutine(ShaderController.RED);

                //Deadó��
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
    /// �ͷ� HP ���� : ������
    /// </summary>
    /// <param name="recoveryPower">������</param>
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
        
        statusMngPanel.SetDebuff(debuffIndex, debuffs, time);


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

        //����� ȿ�� ����
        switch (debuffIndex)
        {
            case 1: //���� �ӵ� �ʱ�ȭ
                currentAttackSpeed = attackSpeed;
                break;
            case 2: //�̵� �ӵ� �ʱ�ȭ
                currentSpeed = speed;
                break;
            case 3: //���� �ʱ�ȭ
                currentDefense = defense;
                break;
            case 4: //���ݷ� �ʱ�ȭ
                currentPower = power;
                break;
            case 5: //���� - ���ݼӵ�, �̵��ӵ� �ʱ�ȭ
                currentPower = power;
                currentSpeed = speed;
                break;
            case 6: //ȭ�� - ���� �ʱ�ȭ
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

    #region Enemy ���� �ʱ�ȭ

    void EnemyInitializing()
    {
        enemyDatas = SystemManager.Instance.EnemyJson.GetEnemyData();
        appearPos = new Vector3[3];

        switch (filePath)
        {
            case "Enemy/Larva":
                Debug.Log("Larva �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 0);
                break;
            case "Enemy/SwordMan":
                Debug.Log("SwordMan �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 1);
                break;
            case "Enemy/CannonLarva":
                Debug.Log("CannonLarva �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 2);
                break;
            case "Enemy/Wagon":
                Debug.Log("Wagon �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 3);
                break;
            case "Enemy/StagBeetle":
                Debug.Log("StagBeetle �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 4);
                break;
            case "Enemy/ShieldMan":
                Debug.Log("ShieldMan �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 5);
                break;
            case "Enemy/SpearMan":
                Debug.Log("SpearMan �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 6);
                break;
            case "Enemy/HorseMan":
                Debug.Log("HorseMan �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 7);
                break;
            case "Enemy/Bull":
                Debug.Log("Bull �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 8);
                break;
            case "Enemy/BowMan":
                Debug.Log("BowMan �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 9);
                break;
            case "Enemy/ShacklesMan":
                Debug.Log("ShacklesMan �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 10);
                break;
            case "Enemy/Wagon2":
                Debug.Log("Wagon2 �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 11);
                break;
            case "Enemy/Elephant":
                Debug.Log("Elephant �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 12);
                break;
            case "Enemy/Wizard":
                Debug.Log("Wizard �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 13);
                break;
            case "Enemy/Scorpion":
                Debug.Log("Scorpion �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 14);
                break;
            case "Enemy/DarkSwordMan":
                Debug.Log("DarkSwordMan �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 15);
                break;
            case "Enemy/DarkLarva":
                Debug.Log("DarkLarva �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 16);
                break;
            case "Enemy/GateKeeper":
                Debug.Log("GateKeeper �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 17);
                break;
            case "Enemy/Wagon3":
                Debug.Log("Wagon3 �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 18);
                break;
            case "Enemy/DarkStagBeetle":
                Debug.Log("DarkStagBeetle �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 19);
                break;
            case "Enemy/DarkBowMan":
                Debug.Log("DarkBowMan �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 20);
                break;
            case "Enemy/DarkKnight":
                Debug.Log("DarkKnight �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 21);
                break;
            case "Enemy/DarkElephant":
                Debug.Log("DarkElephant �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 22);
                break;
            case "Enemy/DarkWizard":
                Debug.Log("DarkWizard �ʱ�ȭ �Ϸ�");
                EnemyVariableInitialize(enemyDatas, 23);
                break;

            default:
                break;
        }
    }

    // ���� �ʱ�ȭ �Լ�
    
    void EnemyVariableInitialize(EnemyData[] data, int num)
    {
        enemyNum = data[num].enemyNum;
        maxHP = data[num].maxHP;
        power = data[num].power;
        defense = data[num].defense;
        speed = data[num].speed;
        attackSpeed = data[num].attackSpeed;
        range = data[num].range;
        regeneration = data[num].regeneration;
        attackRangeType = data[num].attackRangeType;
        isRecoveryTower = data[num].isRecoveryTower;
        selfDestruct = data[num].selfDestruct;
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
        appearPos[0] = new Vector3(data[num].appearPos[0].X, data[num].appearPos[0].Y, data[num].appearPos[0].Z);
        appearPos[1] = new Vector3(data[num].appearPos[1].X, data[num].appearPos[1].Y, data[num].appearPos[1].Z);
        appearPos[2] = new Vector3(data[num].appearPos[2].X, data[num].appearPos[2].Y, data[num].appearPos[2].Z);
        rewardWoodResource = data[num].rewardWoodResource;
    }
    
    #endregion
}