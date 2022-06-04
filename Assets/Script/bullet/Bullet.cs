using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float bulletMaxDistance = 200.0f;

    const float bulletMaxLifeTime = 3.0f; //�Ҹ��� ������ �� �ִ� �ִ� �ð� 

    public float bulletLifeTime = 0.0f;   //�Ҹ��� ������ �� �ִ� �ִ� �ð� ����

    public GameObject attackTarget;    //�Ѿ��� ���� ������  

    [SerializeField]
    float bulletSpeed;    // �Ѿ��� �̵� �ӵ�

    [SerializeField]
    string filePath; 

    [SerializeField]
    int bulletType; //0: ������ �Ѿ�, 1: ��� �Ѿ� 

    [SerializeField]
    float reduceHeight;         // ��� ������ ������ ���� ���� ����

    float distance = 0.0f;

    [SerializeField]
    float force;

    public GameObject attackOwner;

    // Update is called once per frame
    void Update()
    {
        UpdateBullet(); 
    }

    /// <summary>
    /// �Ѿ� �߻� ������Ʈ : ������
    /// </summary>
    void UpdateBullet()
    {
        //����ó��
        if (!attackTarget)
        {
            // �Ѿ� �ı� ���
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
            return;
        }

        Vector3 bulletPos = transform.position;   // �Ѿ��� ��ġ
        Vector3 targetPos = attackTarget.transform.position;  // Ÿ���� �Ѿ��� �´� ��ġ

        // �Ѿ� ���� ������Ʈ
        Vector3 bulletAttackDir = (attackTarget.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(bulletAttackDir);
        transform.rotation = rotation;

        // ���ӵ� ���̱�
        bulletSpeed += bulletSpeed * force;
        //Debug.Log("bulletSpeed= " + bulletSpeed);

        if (bulletType == 0) // ������
        {
            //transform.position = Vector3.Lerp(bulletPos, targetPos, moveDist*Time.deltaTime*0.2f);            
            Vector3 translation = (targetPos - bulletPos).normalized * Time.deltaTime * bulletSpeed * 1.5f;
            transform.position += translation;

        }
        else if (bulletType == 1) //���
        {
            Vector3 center = (bulletPos + targetPos) / 2;
            center -= new Vector3(0, reduceHeight * 1.0f, 0);
            Vector3 startPos = bulletPos - center;
            Vector3 endPos = targetPos - center;

            transform.position = Vector3.Slerp(startPos, endPos, Time.deltaTime * bulletSpeed * 0.05f);
            transform.position += center;
        }


        // bullet�� target�� �Ÿ��� 10���� ���� ��� �ҷ� ��Ȱ��ȭ
        distance = (targetPos - bulletPos).sqrMagnitude;

        //Debug.Log("distance= " + (Mathf.Round(distance)));
        if ((Mathf.Round(distance)) < bulletMaxDistance)
        {      
            GameObject target;
            target = attackTarget.transform.parent.gameObject;

            //����ó��
            if (target.GetComponent<Actor>().currentHP == 0 || !target)
            {
                SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
                return;
            }

            //ȸ�� Ÿ�� 
            if (attackOwner.GetComponent<Actor>().isRecoveryTower)
            {
                if (target.tag == "Enemy")
                {
                    Enemy recoveryTarget = target.GetComponent<Enemy>();
                    Enemy attacker = attackOwner.GetComponent<Enemy>();
                     
                    //�� ������ ȸ��
                    recoveryTarget.IncreaseHP(recoveryTarget.power);
                    //�� �������� ������ ����Ʈ ���
                    recoveryTarget.EnableHealEffect(attacker);
                }
                else if (target.tag == "Turret")
                {
                    Turret recoveryTarget = target.GetComponent<Turret>();
                    Turret attacker = attackOwner.GetComponent<Turret>();

                    //�� ������ ȸ��
                    recoveryTarget.IncreaseHP(recoveryTarget.power);
                    //�� �������� ������ ����Ʈ ���
                    recoveryTarget.EnableHealEffect(attacker);
                }

                SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
            }
            //���� Ÿ��
            else if (target.tag == "Enemy")
            {
                Enemy enemy = target.GetComponent<Enemy>();

                Turret attacker = attackOwner.GetComponent<Turret>();

                /*
                SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(4, enemy.hitPos.transform.position, 0, GetType());
                if (SystemManager.Instance.PanelManager.damageMngPanel)
                    SystemManager.Instance.PanelManager.damageMngPanel.ShowDamage(attacker.power);
                else
                    Debug.Log("damageMngPanel is null");
                */

                enemy.DecreaseHP(attacker.power);

                //�� ������ ����� �ɱ�
                if (attacker.debuffType > 0)
                {
                    enemy.AddDebuff(attacker.debuffType, attacker.debuffDuration);

                    //�� �����ڿ��� ����� ����Ʈ ���
                    enemy.EnableDebuffEffect(attacker);
                }
                //�� �����ڿ��� ������ ����Ʈ ���
                enemy.EnableDamageEffect(attacker);
            }
            //���� Ÿ��
            else if(target.tag == "Turret")
            {
                Turret turret = target.GetComponent<Turret>();

                Enemy attacker = attackOwner.GetComponent<Enemy>();
                 /*
                SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(4, turret.hitPos.transform.position, 0, GetType());
                

                if (SystemManager.Instance.PanelManager.damageMngPanel)
                    SystemManager.Instance.PanelManager.damageMngPanel.ShowDamage(attacker.power);
                else
                    Debug.Log("damageMngPanel is null");
                */

                turret.DecreaseHP(attacker.power);

                //�� ������ ����� �ɱ�
                if (attacker.debuffType > 0)
                {
                    turret.AddDebuff(attacker.debuffType, attacker.debuffDuration);

                    //�� �����ڿ��� ����� ����Ʈ ���
                    turret.EnableDebuffEffect(attacker);
                }
                //�� �����ڿ��� ������ ����Ʈ ���
                turret.EnableDamageEffect(attacker);
            }

            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
        }
    }
}
