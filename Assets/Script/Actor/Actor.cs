using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Actor : MonoBehaviour
{
    [Header("Stat")]    //�ɷ�ġ

    [SerializeField]
    public int maxHP = 30;   //�ִ� ü��

    [SerializeField]
    public int currentHP;   //���� ü��

    [SerializeField]
    public int power = 10;  // ���ݷ�

    [SerializeField]
    public float attackSpeed;    //���ݼӵ�

    [SerializeField]
    protected int range;  //��Ÿ�

    [SerializeField]
    protected int regeneration;   // ȸ����

    [Header("AttackType")]  //����Ÿ�� : ���Ÿ�, �ٰŸ�, ���ϰ���, ���߰��� 

    [SerializeField]
    protected int attackRangeType; // 0:�ٰŸ�Ÿ�� 1:���Ÿ�Ÿ��

    [SerializeField]
    protected int attackTargetNum;    //���� Ÿ�� �� 

    [Header("multipleAttack")]    //���� ���� ���� ���� �ɷ�ġ

    [SerializeField]
    protected int multiAttackRange;  //���߰��� ��Ÿ�

    [Header("Bullet, Effect")]  //���Ÿ� ���� ���� - �Ѿ� ����
    [SerializeField]
    protected int bulletIndex;  //����� �Ѿ� ��ȣ

    [SerializeField]
    protected GameObject firePos;  //���� ���ݽ� �Ѿ��� �߻�Ǵ� ������

    public GameObject hitPos;   //�Ѿ˰� �浹�ϴ� ��ü�� ��ġ

    [SerializeField]
    public GameObject dropPos;  //���� ���ݽ� �Ѿ��� �������� ������    

    protected float attackTimer;  //���ݽð� Ÿ�̸�

    public int damageEffectIndex; //����� ���� ����Ʈ ��ȣ

    public int deadEffectIndex; //Dead�� ����� ����Ʈ ��ȣ

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
    protected virtual void DetectTarget(List<GameObject> target)
    {
        //���� ������ �ƴѰ��
        if (attackTargetNum == 0)
            return;

        //����Ʈ �ʱ�ȭ
        attackTargets.Clear();
        attackTargetsActor.Clear();

        for (int i = 0; i < target.Count; i++)
        {
            //��Ÿ� �ȿ� ���� ���� ������ Ÿ��
            if (target[i].activeSelf && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < range)
            {
                //���� Ÿ�� �����ϰ��
                if (attackTargetNum > 1)
                {
                    //��Ÿ� �ȿ� ���� ���� ������ Ÿ��
                    attackTargets.Add(target[i]);
                    attackTargetsActor.Add(target[i].GetComponent<Actor>());
                    attackDirVec = Vector3.zero;

                    //���� ��Ÿ� �ȿ� ���� �� Ÿ�� �߰�
                    DetectTargets(target, i);
                }
                else
                {              
                    //Ÿ�ٰ� Ÿ�� ���⺤�� �ʱ�ȭ
                    attackTargets.Add(target[i]);
                    attackTargetsActor.Add(target[i].GetComponent<Actor>());
                    attackDirVec = (attackTargets[0].transform.position - transform.position).normalized;

                    //����
                    Attack();
                }   

                return;
            }

        }//end of for
    }

    /// <summary>
    /// ���� Ÿ�� ������ ���, this ��ü�� ���� ��Ÿ� �ȿ��ִ� Ÿ���� ������ ���� ������ Ÿ�ٵ��� ���� : ������
    /// </summary>
    /// <param name="target">Ÿ���� �� ��� �迭</param>
    /// <param name="detectedUnitIndex">���� ���� ������ ���� �ε���</param>
    protected void DetectTargets(List<GameObject> target,int detectedUnitIndex)
    {
        Dictionary<GameObject, float> targetDistances = new Dictionary<GameObject, float>();

        for (int i = 0; i < target.Count; i++)
        {
            //��Ÿ� �ȿ� ���� ���� ������ Ÿ���� ������ ���� ��Ÿ� �ȿ� ������ ���ֵ�
            if ((target[i].activeSelf && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < multiAttackRange) && (i != detectedUnitIndex))
            {
                targetDistances.Add(target[i],Vector3.SqrMagnitude(target[i].transform.position - transform.position)); 
            }
        }

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
        //����ó��
        if (attackDirVec == Vector3.zero)
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
                enemy.DecreseHP(attacker.power);

                //�� �������� ������ ����Ʈ ���
                enemy.EnableDamageEffect(attacker);
            }
            else if (attackTargets[0].tag == "Turret")
            {
                Turret turret = attackTargets[0].GetComponent<Turret>();

                Enemy attacker = gameObject.GetComponent<Enemy>();
                turret.DecreseHP(attacker.power);


                //�� �������� ������ ����Ʈ ���
                turret.EnableDamageEffect(attacker);
            }

            animator.SetBool("meleeAttack", false);
        }

            Quaternion rotation;

            rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);

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
        if (attackTargetNum == 1)
        {            
            SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, firePos.transform.position, attackTargets[0], gameObject);
        }
        //���� Ÿ�� ������ ���
        else
        {

            for (int i = 0; i < attackTargets.Count; i++)
            {
                if (attackTargetsActor[i].currentHP > 0 && attackTargets[i].activeSelf) //Ÿ���� ���°��
                {
                    SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, attackTargetsActor[i].dropPos.transform.position, attackTargets[i], this.gameObject);
                }
            }

        }

    }


    /// <summary>
    /// ������ ���� Ÿ���� HP�� ���� : ������
    /// </summary>
    /// <param name="attackTarget"></param>
    public virtual void DecreseHP(int damage)
    {
        if (currentHP <= 0)
            return;

        if (currentHP > damage)
            currentHP -= damage;
        else
        {
            callFlashCoroutine(ShaderController.RED);

            currentHP = 0;
            animator.SetBool("isDead", true);
            animator.Play("Dead");//Test

            return;
        }

        callFlashCoroutine(ShaderController.WHITE);

    }

    /// <summary>
    /// ����Ʈ ���
    /// </summary>
    /// <param name="attacker">������</param>
    public virtual void EnableDamageEffect(Actor attacker)
    {
        //����Ʈ ��� 

        SystemManager.Instance.EffectManager.EnableEffect(attacker.damageEffectIndex, hitPos.transform.position);   //�ǰ� ����Ʈ ���
 
        if (currentHP <= 0)
            SystemManager.Instance.EffectManager.EnableEffect(deadEffectIndex, hitPos.transform.position);    //Dead ����Ʈ ���
    }

    /// <summary>
    /// Flashȿ���� ��Ÿ���� ���� �ڷ�ƾ�� ȣ�� : ������
    /// </summary>
    /// <param name="color">Flashȿ���� ��</param>
    void callFlashCoroutine(Vector4 color)
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
   
}
