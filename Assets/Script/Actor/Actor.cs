using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Actor : MonoBehaviour
{
    //Actor
    [SerializeField]
    protected int maxHP;   //�ִ� ü��

    [SerializeField]
    protected int currentHP;   //���� ü��

    [SerializeField]
    protected int power;  // ���ݷ�

    [SerializeField]
    protected float attackSpeed;    //���ݼӵ�

    [SerializeField]
    protected int attackTargetNum;    //���� Ÿ�� �� 

    [SerializeField]
    protected int range;  //��Ÿ�

    [SerializeField]
    protected int attackRange;  //���Ÿ� ��Ÿ�

    [SerializeField]
    protected int regeneration;   // ȸ����

    //���ݰ���
    protected float attackTimer;  //���ݽð� Ÿ�̸�

    [SerializeField]
    protected Animator animator; //�ִϸ�����

    [SerializeField]
    protected List<GameObject> attackTargets;    //������ Ÿ��

    [SerializeField]
    protected Vector3 attackDirVec;   //������ Ÿ���� ���⺤��

    [SerializeField]
    protected GameObject firePos;  //���� ���ݽ� �Ѿ��� �߻�Ǵ� ������

    [SerializeField]
    protected int attackRangeType; // 0:�ٰŸ�Ÿ�� 1:���Ÿ�Ÿ��


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
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
    protected virtual void DetectTarget(GameObject[] target)
    {
        //���� ������ �ƴѰ��
        if (attackTargetNum == 0)
            return;

        //����Ʈ �ʱ�ȭ
        attackTargets.Clear();

        for (int i = 0; i < target.Length; i++)
        {
            //Debug.Log(i.ToString() + " : " + Vector3.SqrMagnitude(target[i].transform.position - transform.position));
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
    void DetectTargets(GameObject[] target,int detectedUnitIndex)
    {
        Dictionary<GameObject, float> targetDistances = new Dictionary<GameObject, float>();

        for (int i = 0; i < target.Length; i++)
        {
            //��Ÿ� �ȿ� ���� ���� ������ Ÿ���� ������ ���� ��Ÿ� �ȿ� ������ ���ֵ�
            if ((target[i].activeSelf && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < attackRange) && (i != detectedUnitIndex))
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

        //������ ����� �������� ȸ��
        Quaternion rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
    }
}
