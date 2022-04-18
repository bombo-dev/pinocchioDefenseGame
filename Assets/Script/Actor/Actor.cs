using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Actor : MonoBehaviour
{
    [Header("Stat")]    //�ɷ�ġ

    [SerializeField]
    protected int maxHP;   //�ִ� ü��

    [SerializeField]
    protected int currentHP;   //���� ü��

    [SerializeField]
    protected int power;  // ���ݷ�

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
    protected bool reverse; //������Ʈ�� 3d���� ������ �ݴ�� �� ���

    [SerializeField]
    protected Animator animator; //�ִϸ�����

    [SerializeField]
    protected List<GameObject> attackTargets;    //������ Ÿ��

    //[SerializeField]
    //public GameObject attackOwner;      // ������ �����ϴ� ���

    public Vector3 attackDirVec;   //������ Ÿ���� ���⺤��
    
    public float bulletSpeed = 50f;

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
            //DecreaseHP
            animator.SetBool("meleeAttack", false);
        }

        //������ ����� �������� ȸ��, ���� ���� ������ �ƴ� ��쿡�� �ǽð� ȸ��
        if (attackTargetNum <= 1)
        {
            Quaternion rotation;

            if (reverse)
                rotation = Quaternion.LookRotation((new Vector3(attackDirVec.x, 0, attackDirVec.z)));
            else
                rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
        }
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
            SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, firePos.transform.position, attackTargets[0]);
        }
        //���� Ÿ�� ������ ���
        else
        {
            for (int i = 0; i < attackTargets.Count; i++)
            {
                Actor actor = attackTargets[i].GetComponent<Actor>();
                SystemManager.Instance.BulletManager.EnableBullet(bulletIndex, actor.dropPos.transform.position, attackTargets[i]);
            }
        }
    }
}
