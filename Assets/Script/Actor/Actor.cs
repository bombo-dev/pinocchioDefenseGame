using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected int attackSpeed;    //���ݼӵ�

    [SerializeField]
    protected int range;  // ��Ÿ�

    [SerializeField]
    protected int regeneration;   // ȸ����

    //���ݰ���
    protected float attackTimer;  //���ݽð� Ÿ�̸�

    [SerializeField]
    protected Animator animator; //�ִϸ�����

    [SerializeField]
    protected GameObject attackTarget;    //������ Ÿ��

    [SerializeField]
    protected Vector3 attackDirVec;   //������ Ÿ���� ���⺤��

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
    /// this��ü�� ��Ÿ� �ȿ��ִ� Ÿ���� ������ ���� ������ Ÿ���� ���� : ������
    /// </summary>
    protected virtual void DetectTarget(GameObject[] target)
    {
        for (int i = 0; i < target.Length; i++)
        {
            //Debug.Log(i.ToString() + " : " + Vector3.SqrMagnitude(target[i].transform.position - transform.position));
            //��Ÿ� �ȿ� ���� ���� ������ Ÿ��
            if (target[i].activeSelf && Vector3.SqrMagnitude(target[i].transform.position - transform.position) < range)
            {
                //Ÿ�ٰ� Ÿ�� ���⺤�� �ʱ�ȭ
                attackTarget = target[i];
                attackDirVec = (attackTarget.transform.position - transform.position).normalized;

                //����
                Attack();

                return;
            }
        }
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
        //������ ����� �������� ȸ��
        Quaternion rotation = Quaternion.LookRotation(-(new Vector3(attackDirVec.x, 0, attackDirVec.z)));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.3f);
    }
}
