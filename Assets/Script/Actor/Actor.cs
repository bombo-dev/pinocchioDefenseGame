using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class Debuff
{
    public float durationTime = 0;  //���ӽð�

    public int stack = 0;  //��ø����
}

public class Actor : MonoBehaviour
{
    public enum debuff  //����� ����
    {
        None,   //�ʱⰪ 0
        DecreaseAttackSpeed,    //���� �ӵ� ���� 1
        Slow,   //�̵��ӵ� ���� 2
        DecreaseDefense,    //���� ���� 3
        DecreasePower,  //���ݷ� ���� 4
        ElectricShock,  //���� - ���ݼӵ�,�̵��ӵ� �������� 5 
        Burn    //ȭ�� - ���� �������� 6
    }

    public enum buff  //���� ����
    {
        None,   //�ʱⰪ 0
        IncreasePower,    //���ݷ� ���� 1, ��������
        IncreaseAttackSpeed,   //���ݼӵ� ���� 2, �������
        IncreaseRegeneration,    //ȸ���� ���� 3, �ʷϳ���
        IncreaseDefense,  //���� ���� 4, �Ͼᳪ��
        IncreaseRange,  //��Ÿ� ���� 5, �Ķ����� 
        IncreaseAll    //�ý��� ���� 6, ��������
    }

    [SerializeField]
    public debuff _debuff = debuff.None;    //�����

    [Header("Stat")]    //�ɷ�ġ

    [SerializeField]
    public int maxHP = 30;   //�ִ� ü��

    [SerializeField]
    public int currentHP;   //���� ü��

    [SerializeField]
    public int power = 10;  //���ݷ�

    [SerializeField]
    public int currentPower;    //���� ���ݷ�

    [SerializeField]
    public int defense = 10;    //����

    [SerializeField]
    public int currentDefense;  //���� ����

    [SerializeField]
    public float attackSpeed;    //���ݼӵ�

    [SerializeField]
    public float currentAttackSpeed;    //���� ���ݼӵ�

    [SerializeField]
    protected int range;  //��Ÿ�

    [SerializeField]
    public int currentRange;  //���� ��Ÿ�

    [SerializeField]
    protected int regeneration;   // ȸ����

    [SerializeField]
    public int currentRegeneration;   // ȸ����

    [Header("Debuff")]  //�����
    public Dictionary<debuff, Debuff> debuffs = new Dictionary<debuff, Debuff>();

    [Header("AttackType")]  //����Ÿ�� : ���Ÿ�, �ٰŸ�, ���ϰ���, ���߰��� 

    [SerializeField]
    protected int attackRangeType; // 0:�ٰŸ�Ÿ�� 1:���Ÿ�Ÿ��

    [SerializeField]
    public bool isRecoveryTower; // false - ����Ÿ�� true - ȸ��Ÿ��

    [SerializeField]
    public int attackTargetNum;    //���� Ÿ�� �� 

    [SerializeField]
    public int debuffType;   //����� Ÿ�� 

    [SerializeField]
    public float debuffDuration; //����� ���ӽð�

    [Header("multipleAttack")]    //���� ���� ���� ���� �ɷ�ġ

    [SerializeField]
    protected int multiAttackRange;  //���߰��� ��Ÿ�

    [SerializeField]
    protected int currentMultiAttackRange;  //���� ���߰��� ��Ÿ�

    [Header("Bullet, Effect")]  //���Ÿ� ���� ���� - �Ѿ� ����
    [SerializeField]
    protected int bulletIndex;  //����� �Ѿ� ��ȣ

    [SerializeField]
    protected GameObject firePos;  //���� ���ݽ� �Ѿ��� �߻�Ǵ� ������

    public GameObject hitPos;   //�Ѿ˰� �浹�ϴ� ��ü�� ��ġ

    [SerializeField]
    public GameObject hpPos;

    [SerializeField]
    public GameObject dropPos;  //���� ���ݽ� �Ѿ��� �������� ������    

    protected float attackTimer;  //���ݽð� Ÿ�̸�

    public int damageEffectIndex; //����� ���� ����Ʈ ��ȣ

    public int deadEffectIndex; //Dead�� ����� ����Ʈ ��ȣ

    public int fireEffectIndex; //fire�� ����� ����Ʈ ��ȣ

    public int healEffectIndex; //heal�� ����� ����Ʈ ��ȣ

    public int debuffEffectIndex; //debuff�� ����� ����Ʈ ��ȣ

    [SerializeField]
    GameObject currentDamageEffect;   //���� ����� �ǰ� ����Ʈ

    [SerializeField]
    GameObject currentDeadEffect;   //���� ����� Dead ����Ʈ

    [SerializeField]
    GameObject currentFireEffect;   //���� ����� ������ ����Ʈ

    [SerializeField]
    GameObject currentHealEffect;   //���� ����� ȸ�� ����Ʈ

    [SerializeField]
    GameObject currentDebuffEffect;   //���� ����� ����� ����Ʈ

    [Header("data")]    //��Ÿ ������
    [SerializeField]
    protected string filePath; //������ ���� ���� ���

    [SerializeField]
    protected Animator animator; //�ִϸ�����

    [SerializeField]
    protected List<GameObject> attackTargets;    //������ ���ϴ� ������Ʈ
    [SerializeField]
    protected List<Actor> attackTargetsActor;  //������ ���ϴ� ������Ʈ ���� Ŭ����

    public Vector3 attackDirVec;   //������ Ÿ���� ���⺤��
    
    public float bulletSpeed = 100f;    // �Ѿ��� �̵� �ӵ�

    public float maxBulletSpeed;    // �Ѿ��� �ִ� �̵� �ӵ�

    public bool finAttack;  // ������ ���������� Ȯ���ϴ� �÷���

    [Header("Material")]  //����� Material
    [SerializeField]
    Renderer[] rendererArr;

    [SerializeField]
    List<Renderer> rendererCaches;

    [SerializeField]
    List<Vector4> emissionCaches;

    public bool showWhiteFlash_coroutine_is_running = false;//�ڷ�ƾ ������ ���� �÷���

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

        //����� ���� 
        UpdateDebuff();
    }

    /// <summary>
    /// �ʱ�ȭ �Լ� : ������
    /// </summary>
    protected virtual void Initialize()
    {
        rendererCaches = new List<Renderer>();
        emissionCaches = new List<Vector4>();
        //������ ���̴�ĳ�� �ʱ�ȭ
        if (rendererArr.Length > 0)
        {
            SystemManager.Instance.ShaderController.InitializeShaderCaches(rendererArr, rendererCaches, emissionCaches);
        }
    }

    /// <summary>
    /// ���� ������ ����
    /// </summary>
    public virtual void Reset()
    {
        //���̴� ���� �ʱ�ȭ
        SystemManager.Instance.ShaderController.OffFlash(rendererCaches, emissionCaches);

        //Ÿ�ٹ迭 �ʱ�ȭ
        attackTargets.Clear();

        //HP�ʱ�ȭ
        currentHP = maxHP;

        //���ݷ� �ʱ�ȭ
        currentPower = power;

        //���� �ʱ�ȭ
        currentDefense = defense;

        //���ݼӵ� �ʱ�ȭ
        currentAttackSpeed = attackSpeed;

        //��Ÿ� �ʱ�ȭ
        currentMultiAttackRange = multiAttackRange;

        //ȸ���� �ʱ�ȭ
        currentRegeneration = regeneration;

        //����� �ʱ�ȭ
        ClearDebuff();
    }

    /// <summary>
    /// �ǽð� ���º� ������ ���� : ������
    /// </summary>
    protected virtual void UpdateActor()
    {
        
    }

    /// <summary>
    /// this ��ü�� ��Ÿ� �ȿ��ִ� Ÿ���� ������ ���� ������ Ÿ���� ���� : ������
    /// </summary>
    /// <param name="target">Ÿ���� �� ��� �迭</param>
    /// <param name="mine">ȣ���� ������Ʈ</param>
    protected virtual void DetectTarget(List<GameObject> target , GameObject mine = null)
    {
        //���� ������ �ƴѰ��
        if (attackTargetNum == 0)
            return;

        //����Ʈ �ʱ�ȭ
        attackTargets.Clear();
        attackTargetsActor.Clear();

        //Ÿ�ٰ� Ÿ�ٰ��� �Ÿ��� ������ ��ųʸ�
        Dictionary<GameObject, float> targetDistances = new Dictionary<GameObject, float>();

        // --------------- Ÿ�ٺ� �Ÿ� ���� ---------------

        //����Ÿ�� ������ range �ȿ� ������ ���� �����ϴ��� �Ǵ�, * ����Ÿ�� ������ range���� ������ Ÿ���� ������ ��� ������ target�� multiAttackRange������ ã�´� *
        bool isTargetInRange = false;

        //Ÿ�� ����Ʈ�� ��� ��Ҹ� �˻��Ͽ� ��Ÿ��ȿ� ���� Ÿ���� ��� ��ųʸ��� ����
        for (int i = 0; i < target.Count; i++)
        {
            //ȸ�� Ÿ���� ���
            if (mine && isRecoveryTower)
            {
                // ������ Ÿ���� �ڽ��̰ų� �Ǵٸ� ȸ��Ÿ���� ��� ���� ���� ����
                if ((System.Object.ReferenceEquals(target[i], mine)) || (target[i].GetComponent<Actor>().isRecoveryTower))
                {
                    if (i >= target.Count - 1)
                        break;
                    else
                        continue;
                }
            }

            //Ÿ���� �����ϴ°��
            if (target[i].activeSelf)
            {
                //����Ÿ��
                if (attackTargetNum <= 1)
                {
                    if (Vector3.SqrMagnitude(target[i].transform.position - transform.position) < currentRange)
                        targetDistances.Add(target[i], Vector3.SqrMagnitude(target[i].transform.position - transform.position));
                }
                //����Ÿ��
                else
                {
                    if (!isTargetInRange && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < currentRange)
                        isTargetInRange = true;
                    if (Vector3.SqrMagnitude(target[i].transform.position - transform.position) < currentMultiAttackRange)
                        targetDistances.Add(target[i], Vector3.SqrMagnitude(target[i].transform.position - transform.position));
                }
            }
        }//end of for

        // --------------- ���� ������ Ÿ���� �����ϴ��� �Ǵ� ---------------

        //Ÿ�� ������ ����
        if (targetDistances.Count <= 0)
            return;

        //���� Ÿ������ - range�� Ÿ�� ������ ����
        if (attackTargetNum > 1 && !isTargetInRange)
            return;

        // --------------- Ÿ���� ������ ��� ���� Ÿ�� ���� ---------------

        //�Ÿ������� �������� ����
        var sortedTargetDistances = targetDistances.OrderBy(x => x.Value);

        //�Ÿ������� �ִ� Ÿ�� ���� ���� ���Ϸ� �־��ش�
        foreach (KeyValuePair<GameObject, float> item in sortedTargetDistances)
        {
            attackTargets.Add(item.Key);
            attackTargetsActor.Add(item.Key.GetComponent<Actor>());

            if (attackTargets.Count >= attackTargetNum)
            {
                break;
            }
        }

        //����
        Attack();

        return;
    }

    /// <summary>
    /// this��ü�� ���¸� �������� ���� : ������
    /// </summary>
    protected virtual void Attack()
    {
        //���� ����Ʈ ȣ��
        EnableFireEffect(this);

        //����
        animator.SetBool("attack", true);

        //���ݽð� ���� ���� �ʱ�ȭ
        attackTimer = Time.time;      
    }


    /// <summary>
    /// �ǽð����� ������ �������� �ȳ��������� �Ǻ��ϰ� ��������� 
    /// ���� �������� �������� �ٸ� ���·� ���������� ���� : ������
    /// </summary>
    protected virtual void UpdateBattle()
    {
        // �̵��ϴ� Enemy �������� �ͷ��� ��� ȸ���ϵ��� Ÿ�� ��ġ ������Ʈ
        attackDirVec = (attackTargets[0].transform.position - this.transform.position).normalized;

        //����ó��
        if (attackDirVec == Vector3.zero && tag == "Turret" && isRecoveryTower == false)
            return;

        //����Ÿ�� ����ó��
        if ((attackTargetNum == 1) && (attackTargetsActor[0].currentHP <= 0 || !attackTargets[0].activeSelf)) //Ÿ���� ���°��
        {
            animator.SetBool("attackCancel", true);
        }
        //����Ÿ�� ����ó��
        else if (attackTargetNum > 1)
        {
            for (int i = 0; i < attackTargets.Count; i++)
            {
                if (attackTargetsActor[i].currentHP > 0 && attackTargets[i].activeSelf) // //Ÿ���� ���°��
                    break;
                if(i == attackTargets.Count-1)
                    animator.SetBool("attackCancel", true);
            }
        }

        if (animator.GetBool("attackCancel"))
            return;
        
        //���Ÿ� ���� ���� �Ѿ� ����
        if (attackRangeType == 1 && animator.GetBool("rangedAttack"))
        {
            InitializeBullet();
            animator.SetBool("rangedAttack", false);
        }
        //�ٰŸ� ���� ���� ������ ó��
        else if (attackRangeType == 0 && animator.GetBool("meleeAttack"))
        {
            if (attackTargets[0].tag == "Enemy")
            {
                Enemy enemy = attackTargets[0].GetComponent<Enemy>();

                Turret attacker = gameObject.GetComponent<Turret>();

                // ���� ������� ������ ������ ���
                int damage = attacker.currentPower - (int)(attacker.currentPower * ((float)enemy.currentDefense * 0.01f));

                // ���ʹ��� hp�� ����
                enemy.DecreaseHP(damage);

                // ������ UI ����
                if (damage > 0)
                {
                    GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, attackTargets[0]);

                    if (!damageMngPanelGo)
                        return;

                    DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                    // ������ UI ȭ�鿡 ����
                    damageMngPanel.ShowDamage(damage, 0);

                    enemy.damageMngPanel = damageMngPanel;
                    damageMngPanel.damageOwner = enemy.gameObject;

                }
                //�� ������ ����� �ɱ�
                if (debuffType > 0)
                {
                    Debug.Log("******�����*******");
                    enemy.AddDebuff(debuffType, debuffDuration);
                    //�� �����ڿ��� ����� ����Ʈ ���
                    enemy.EnableDebuffEffect(attacker);
                }
                //�� �����ڿ��� ������ ����Ʈ ���
                enemy.EnableDamageEffect(attacker);
            }
            else if (attackTargets[0].tag == "Turret")
            {
                Turret turret = attackTargets[0].GetComponent<Turret>();

                Enemy attacker = gameObject.GetComponent<Enemy>();

                // ���� ������� ������ ������ ���
                int damage = attacker.currentPower - (int)(attacker.currentPower * ((float)turret.currentDefense * 0.01f));

                // �ͷ��� hp�� ����
                turret.DecreaseHP(damage);

                // ������ UI ����
                if (damage > 0)
                {
                    GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, attackTargets[0]);

                    if (!damageMngPanelGo)
                        return;

                    DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                    // ������ UI ȭ�鿡 ����
                    damageMngPanel.ShowDamage(damage, 0);

                    turret.damageMngPanel = damageMngPanel;
                    damageMngPanel.damageOwner = turret.gameObject;

                }

                //�� ������ ����� �ɱ�
                if (debuffType > 0)
                {
                    turret.AddDebuff(debuffType, debuffDuration);

                    //�� �����ڿ��� ����� ����Ʈ ���
                    turret.EnableDebuffEffect(attacker);
                }
                //�� �����ڿ��� ������ ����Ʈ ���
                turret.EnableDamageEffect(attacker);
            }

            animator.SetBool("meleeAttack", false);
        }

        //��ġ ������Ʈ
        if (!isRecoveryTower)
        {
            Quaternion rotation;

            rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
        }

        //����Ʈ ��ġ ������Ʈ
        if(currentFireEffect)
            if(currentFireEffect.activeSelf)
                currentFireEffect.transform.position = firePos.transform.position;
    }

    /// <summary>
    /// �Ѿ� ��ġ �ʱ�ȭ : ������
    /// </summary>
    void InitializeBullet()
    {
        //����ó��
        if (attackTargetNum <= 0)
            return;

        //�Ѿ� ����

        // ���� Ÿ�� ������ ���
        if (attackTargetNum == 1 && !isRecoveryTower)
        {            
            SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, firePos.transform.position, attackTargets[0], gameObject);
        }
        //���� Ÿ�� ����, ȸ�� ������ ���
        else
        {
            for (int i = 0; i < attackTargets.Count; i++)
            {
                if (attackTargetsActor[i].currentHP > 0 && attackTargets[i].activeSelf) //Ÿ���� �����ϴ°��
                {
                    SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, attackTargetsActor[i].dropPos.transform.position, attackTargets[i], this.gameObject);
                }
            }

        }

    }


    /// <summary>
    /// ������ ���� Ÿ���� HP�� ���� : ������
    /// </summary>
    /// <param name="damage">Ÿ���� ���� ������</param>
    public virtual void DecreaseHP(int damage)
    {
        if (damage <= 0 || currentHP < 0)
            return;

        //������ ��� ���� -> ������ 100�̸�
        if (currentDefense >= 100)
            currentDefense = 99;

        damage = (int)(damage - (damage * ((float)currentDefense * 0.01f)));

        Debug.Log("damage -> " + damage);

        //����ó��
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
    /// ȸ���� �� Ÿ���� HP�� ���� : ������
    /// </summary>
    /// <param name="recoveryPower">�������� ������ų Ÿ��</param>
    public virtual void IncreaseHP(int recoveryPower)
    {
        //������ ��� ����
        //recoveryPower += recoveryPower * (currentRegeneration / 100);

        if (currentHP + recoveryPower >= maxHP)
            currentHP = maxHP;
        else
            currentHP += recoveryPower;
    }

    #region ����Ʈ
    /// <summary>
    /// �ǰ� ����Ʈ ��� : ������
    /// </summary>
    /// <param name="attacker">������</param>
    public virtual void EnableDamageEffect(Actor attacker)
    {
        //����Ʈ ��� 
        if (hitPos)
            currentDamageEffect = SystemManager.Instance.EffectManager.EnableEffect(attacker.damageEffectIndex, hitPos.transform.position);   //�ǰ� ����Ʈ ���

        if (currentHP <= 0)
        {
            if (hitPos)
                currentDeadEffect = SystemManager.Instance.EffectManager.EnableEffect(deadEffectIndex, hitPos.transform.position);    //Dead ����Ʈ ���
        }
    }

    /// <summary>
    /// ���� ����Ʈ ��� : ������
    /// </summary>
    /// <param name="attacker">������</param>
    public virtual void EnableFireEffect(Actor attacker)
    {
        //����Ʈ ��� 
        if(firePos && attacker.fireEffectIndex != -1)
            currentFireEffect = SystemManager.Instance.EffectManager.EnableEffect(attacker.fireEffectIndex, firePos.transform.position);   //�ǰ� ����Ʈ ���
    }

    /// <summary>
    /// ȸ�� ����Ʈ ��� : ������
    /// </summary>
    /// <param name="attacker">ȸ��������</param>
    public virtual void EnableHealEffect(Actor attacker)
    {
        //����Ʈ ��� 
        if (hitPos && attacker.healEffectIndex != -1)
            currentHealEffect = SystemManager.Instance.EffectManager.EnableEffect(attacker.healEffectIndex, hitPos.transform.position);   //ȸ�� ����Ʈ ���
    }

    /// <summary>
    /// ����� ����Ʈ ��� : ������
    /// </summary>
    /// <param name="attacker">������</param>
    public virtual void EnableDebuffEffect(Actor attacker)
    {
        //����Ʈ ��� 
        if (hpPos || debuffEffectIndex != -1)
            currentDebuffEffect = SystemManager.Instance.EffectManager.EnableEffect(attacker.debuffEffectIndex, hpPos.transform.position);   //����� ����Ʈ ���
    }


    #endregion

    /// <summary>
    /// Flashȿ���� ��Ÿ���� ���� �ڷ�ƾ�� ȣ�� : ������
    /// </summary>
    /// <param name="color">Flashȿ���� ��</param>
    protected void callFlashCoroutine(Vector4 color)
    {
        //WhiteFlash �ǰ�ȿ��

        //����ó��
        if (rendererArr.Length <= 0)
            return;

        //�ڷ�ƾ�� �������̸� ������ �� �ٽý���
        if (showWhiteFlash_coroutine_is_running)
        {
            //�ڷ�ƾ ����
            StopCoroutine(SystemManager.Instance.ShaderController.ShowFlash(rendererCaches, emissionCaches, this, color));
            showWhiteFlash_coroutine_is_running = false;

            //���̴� ���� �ʱ�ȭ
            SystemManager.Instance.ShaderController.OffFlash(rendererCaches, emissionCaches);
        }

        StartCoroutine(SystemManager.Instance.ShaderController.ShowFlash(rendererCaches, emissionCaches, this, color));
    }
   


    /// <summary>
    /// ���� ��Ȱ��ȭ ó���� ���� Dead ���� ������Ʈ : ������
    /// </summary>
    protected virtual void UpdateDead()
    {
        
    }

    #region �����

    /// <summary>
    /// �ǽð� ����� ���� ó�� : ������
    /// </summary>
    void UpdateDebuff()
    {
        if (debuffs.Count > 0)
        {
            for (int i = 0; i < Enum.GetValues(typeof(debuff)).Length; i++)
            {
                //�ε����� debuff�� ����ȯ
                debuff _debuffIndex = (debuff)i;

                //����� ������Ʈ
                if (debuffs.ContainsKey(_debuffIndex))
                {
                    //���ӽð� ������Ʈ
                    debuffs[_debuffIndex].durationTime -= Time.deltaTime;

                    //���ӽð� ����� ����� ����
                    if (debuffs[_debuffIndex].durationTime < 0)
                    {
                        RemoveDebuff(i);                        
                    }

                }
            }
        }

    }

    /// <summary>
    /// ����� �߰� : ������
    /// </summary>
    /// <param name="debuffIndex">�߰��� ����� ���� �ε���</param>
    /// <param name="time">�߰��� ������� ���ӽð�</param>
    public virtual void AddDebuff(int debuffIndex, float time)
    {
        //����ó��
        if (debuffIndex >= Enum.GetValues(typeof(debuff)).Length)
            return;

        //�ε����� debuff�� ����ȯ
        debuff _debuffIndex = (debuff)debuffIndex;

        //�̹� �����ϴ� ������ΰ��
        if (debuffs.ContainsKey(_debuffIndex))
        {
            if (debuffs[_debuffIndex].stack < 6)//�ִ� 5���� -> ȿ�������� ���� 6����
                debuffs[_debuffIndex].stack++;   //��ø ���� �߰�
        }
        //���� �߰��� ������ΰ��
        else 
        {
            Debuff debuff = new Debuff(); //��ü ����

            debuff.stack = 1;   //��ø ���� �ʱ�ȭ
            debuffs.Add(_debuffIndex, debuff);   //�ڷᱸ���� �߰�
        }

        debuffs[_debuffIndex].durationTime = time;   //���ӽð� �ʱ�ȭ
    }

    /// <summary>
    /// ����� ���� : ������
    /// </summary>
    /// <param name="debuffIndex">������ �����</param>
    protected virtual void RemoveDebuff(int debuffIndex)
    {
        //�ε����� debuff�� ����ȯ
        debuff _debuffIndex = (debuff)debuffIndex;

        //Ű �� �����Ͽ� �ش� ��� ����
        if(debuffs.ContainsKey(_debuffIndex))
            debuffs.Remove(_debuffIndex);
    }

    /// <summary>
    /// ��� ������� ����
    /// </summary>
    void ClearDebuff()
    {
        if (debuffs.Count > 0)
        {
            for (int i = 0; i < Enum.GetValues(typeof(debuff)).Length; i++)
            {
                //����� ����
                RemoveDebuff(i);
            }
        }

        //��ųʸ� �ʱ�ȭ
        debuffs.Clear();
    }

    #endregion
    protected virtual void UpdatePanelPos()
    {
        if (!SystemManager.Instance.PanelManager.statusMngPanel)
            return;       
    }
}
