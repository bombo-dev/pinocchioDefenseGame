using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Actor : MonoBehaviour
{
    [Header("Stat")]    //�ɷ�ġ

    [SerializeField]
    protected int maxHP = 30;   //�ִ� ü��

    [SerializeField]
    protected int currentHP;   //���� ü��

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

    [Header("Bullet")]  //���Ÿ� ���� ���� - �Ѿ� ����
    [SerializeField]
    protected int bulletIndex;  //����� �Ѿ� ��ȣ

    [SerializeField]
    protected GameObject firePos;  //���� ���ݽ� �Ѿ��� �߻�Ǵ� ������

    [SerializeField]
    public GameObject hitPos;   //�Ѿ˰� �浹�ϴ� ��ü�� ��ġ

    [SerializeField]
    public GameObject dropPos;  //���� ���ݽ� �Ѿ��� �������� ������    

    protected float attackTimer;  //���ݽð� Ÿ�̸�

    [Header("data")]    //��Ÿ ������

    [SerializeField]
    protected Animator animator; //�ִϸ�����

    [SerializeField]
    protected List<GameObject> attackTargets;    //������ ���ϴ� ������Ʈ

    public Vector3 attackDirVec;   //������ Ÿ���� ���⺤��
    
    public float bulletSpeed = 100f;    // �Ѿ��� �̵� �ӵ�

    public float maxBulletSpeed;    // �Ѿ��� �ִ� �̵� �ӵ�

    public bool finAttack;  // ������ ���������� Ȯ���ϴ� �÷���

    public bool isFinDelay = false;    

    float delayTime = 1.5f;   // �״� �ִϸ��̼� ��� �� ��Ȱ��ȭ�� ���� �����ð�

    float flowTime = 0; // �����ð� ������ ���� Ÿ�̸�

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
                    attackDirVec = Vector3.zero;

                    //���� ��Ÿ� �ȿ� ���� �� Ÿ�� �߰�
                    DetectTargets(target, i);
                }
                else
                {
                    //Ÿ�ٰ� Ÿ�� ���⺤�� �ʱ�ȭ
                    attackTargets.Add(target[i]);
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
        if (finAttack == true)
        {
            // DecreseHP(attackOwner);
        }

        //����ó��
        if (attackDirVec == Vector3.zero)
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
            }
            else if (attackTargets[0].tag == "Turret")
            {
                Turret turret = attackTargets[0].GetComponent<Turret>();

                Enemy attacker = gameObject.GetComponent<Enemy>();
                turret.DecreseHP(attacker.power);
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
        if (!attackTargets[0] || !attackTargets[0].activeSelf)
            return;

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
                Actor actor = attackTargets[i].GetComponent<Actor>();
                SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, actor.dropPos.transform.position, attackTargets[i], this.gameObject);
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
            //���̴� ���� �ʱ�ȭ
            SystemManager.Instance.ShaderController.OffWhiteFlash(rendererCaches, emissionCaches);

            currentHP = 0;
            animator.SetBool("isDead", true);
        }

        //WhiteFlash �ǰ�ȿ��

        //�ڷ�ƾ�� �������̸� ������ �� �ٽý���

        if (rendererArr.Length <= 0)
            return;

        if (showWhiteFlash_coroutine_is_running)
        {
            //���̴� ���� �ʱ�ȭ
            SystemManager.Instance.ShaderController.OffWhiteFlash(rendererCaches, emissionCaches);

            showWhiteFlash_coroutine_is_running = false;
            StopCoroutine(SystemManager.Instance.ShaderController.ShowWhiteFlash(rendererCaches, emissionCaches, this));
        }

        StartCoroutine(SystemManager.Instance.ShaderController.ShowWhiteFlash(rendererCaches, emissionCaches, this));

    }

   


    /// <summary>
    /// ���� ��Ȱ��ȭ ó���� ���� Dead ���� ������Ʈ : ������
    /// </summary>
    protected virtual void UpdateDead()
    {
        // �����̰� ������ �ʾ����� ���� ó��
        if (isFinDelay == false)
        {            
            flowTime += Time.deltaTime;

            // �����̰� �������� 
            if (flowTime >= delayTime)
            {
                isFinDelay = true;

                // Ÿ�̸� �ʱ�ȭ
                flowTime = 0;
            }
        }        
    }
    
    /// <summary>
    /// ���� ������ ����
    /// </summary>
    public virtual void Reset()
    {
        //Ÿ�ٹ迭 �ʱ�ȭ
        attackTargets.Clear();
    }
}
