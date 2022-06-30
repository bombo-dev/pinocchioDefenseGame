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
    public float bulletSpeed;    // �Ѿ��� �̵� �ӵ�

    public float initSpeed;    // �Ѿ��� �ʱ� �ӵ��� ������ ����

    [SerializeField]
    float maxforce;

    [SerializeField]
    string filePath; 

    [SerializeField]
    int bulletType; //0: ������ �Ѿ�, 1: ��� �Ѿ� 

    [SerializeField]
    float reduceHeight;         // ��� ������ ������ ���� ���� ����

    float distance = 0.0f;

    [SerializeField]
    public float force;    // ���ӵ�

    public float initForce;    // �ʱ� ���ӵ��� ������ ����

    public GameObject attackOwner;

    int i = 0;

    int damage; // ������� ������ ������

    int recovery; //������� ������ ȸ����

    Vector3 initPos;



    // Update is called once per frame
    void Update()
    {
        UpdateBullet(); 
    }
    private void Start()
    {
        initPos = transform.position;
        initForce = force;
        initSpeed = bulletSpeed;
    }
    /// <summary>
    /// �Ѿ� �߻� ������Ʈ : ������
    /// </summary>
    void UpdateBullet()
    {
        //����ó��
        /*
        if (!attackTarget || bulletLifeTime > 200)
        {            
            // �Ѿ� �ı� ���
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
            
            force = initForce;

            bulletSpeed = initSpeed;

            return;
        }*/


            Vector3 bulletPos = transform.position;   // �Ѿ��� ��ġ
        Vector3 targetPos = attackTarget.transform.position;  // Ÿ���� �Ѿ��� �´� ��ġ

        // �Ѿ� ���� ������Ʈ
        Vector3 bulletAttackDir = (attackTarget.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(bulletAttackDir);
        transform.rotation = rotation;

        

        if (Time.timeScale != 0)
        {
            // ���ӵ� ���̱�
            bulletSpeed += (bulletSpeed * force);
            force += 0.003f;
        }

        if (float.IsInfinity(targetPos.x))
        {
            targetPos = Vector3.zero;
        }

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

            transform.position = Vector3.Slerp(startPos, endPos, Time.deltaTime * bulletSpeed * 0.025f);
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

                force = initForce;

                bulletSpeed = initSpeed;

                return;
            }

            //ȸ�� Ÿ�� 
            if (attackOwner.GetComponent<Actor>().isRecoveryTower)
            {
                if (target.tag == "Enemy")
                {
                    Enemy recoveryTarget = target.GetComponent<Enemy>();
                    Enemy attacker = attackOwner.GetComponent<Enemy>();

                    recovery = attacker.currentPower;

                    // ȸ���� ������ ������ ���� ȸ����
                    recovery += (int)((float)recovery * ((float)recoveryTarget.currentRegeneration / (float)100));

                    if (recovery > 0)
                    {
                        // ȸ�� UI ����
                        GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, target);

                        if (!damageMngPanelGo)
                            return;

                        DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                        // ȸ�� UI ȭ�鿡 ����
                        damageMngPanel.ShowDamage(recovery, 1);

                        recoveryTarget.damageMngPanel = damageMngPanel;
                        damageMngPanel.damageOwner = recoveryTarget.gameObject;
                    }

                    //�� ������ ȸ��
                    recoveryTarget.IncreaseHP(recovery);

                    //�� �������� ������ ����Ʈ ���
                    recoveryTarget.EnableHealEffect(attacker);
                }
                else if (target.tag == "Turret")
                {
                    Turret recoveryTarget = target.GetComponent<Turret>();
                    Turret attacker = attackOwner.GetComponent<Turret>();

                    recovery = attacker.currentPower;

                    // ȸ���� ������ ������ ���� ȸ����
                    recovery += (int)((float)recovery * ((float)recoveryTarget.currentRegeneration / (float)100));

                    if (recovery > 0)
                    {
                        // ȸ�� UI ����
                        GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, target);

                        if (!damageMngPanelGo)
                            return;

                        DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                        // ȸ�� UI ȭ�鿡 ����
                        damageMngPanel.ShowDamage(recovery, 1);

                        recoveryTarget.damageMngPanel = damageMngPanel;
                        damageMngPanel.damageOwner = recoveryTarget.gameObject;

                        //�����м�
                        SystemManager.Instance.GameFlowManager.AnalyzeTurretBattle(recovery, attacker.turretNum);
                    }

                    //�� ������ ȸ��
                    recoveryTarget.IncreaseHP(recovery);
                    //�� �������� ������ ����Ʈ ���
                    recoveryTarget.EnableHealEffect(attacker);
                }

                SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);

                force = initForce;

                bulletSpeed = initSpeed;
            }
            //���� Ÿ��
            else if (target.tag == "Enemy")
            {
                Enemy enemy = target.GetComponent<Enemy>();

                Turret attacker = attackOwner.GetComponent<Turret>();

                // ���� ������� ������ ������ ���
                damage = attacker.currentPower - (int)(attacker.currentPower * ((float)enemy.currentDefense * 0.01f));

                enemy.DecreaseHP(damage);

                if (damage > 0)
                {                                        
                    // ������ UI ����
                    GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, target);

                    if (!damageMngPanelGo)
                        return;

                    DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();
                    
                    // Ȱ��ȭ�� ������ �г��� ����Ʈ�� ����
                    //SystemManager.Instance.PanelManager.damagePanels.Add(damageMngPanelGo);

                    // ������ UI ȭ�鿡 ����
                    damageMngPanel.ShowDamage(damage, 0);

                    enemy.damageMngPanel = damageMngPanel;
                    damageMngPanel.damageOwner = enemy.gameObject;
                   
                }
                //�� ������ ����� �ɱ�
                if (attacker.debuffType > 0)
                {
                    enemy.AddDebuff(attacker.debuffType, attacker.debuffDuration);

                    //�� �����ڿ��� ����� ����Ʈ ���
                    enemy.EnableDebuffEffect(attacker);
                }
                //�� �����ڿ��� ������ ����Ʈ ���
                enemy.EnableDamageEffect(attacker);

                //�����м�
                SystemManager.Instance.GameFlowManager.AnalyzeTurretBattle(damage, attacker.turretNum);
            }
            //���� Ÿ��
            else if(target.tag == "Turret")
            {
                Turret turret = target.GetComponent<Turret>();

                Enemy attacker = attackOwner.GetComponent<Enemy>();

                // ���� ������� ������ ������ ���
                damage = attacker.currentPower - (int)(attacker.currentPower * ((float)turret.currentDefense * 0.01f));

                turret.DecreaseHP(damage);

                if (damage > 0)
                {                    
                    // ������ UI ����
                    GameObject damageMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<DamageMngPanel>(6, target);

                    if (!damageMngPanelGo)
                        return;
                                        
                    DamageMngPanel damageMngPanel = damageMngPanelGo.GetComponent<DamageMngPanel>();

                    // Ȱ��ȭ�� ������ �г��� ����Ʈ�� ����
                    //SystemManager.Instance.PanelManager.damagePanels.Add(damageMngPanelGo);

                    // ������ UI ȭ�鿡 ����
                    damageMngPanel.ShowDamage(damage, 0);

                    damageMngPanel.damageOwner = turret.gameObject;
                    turret.damageMngPanel = damageMngPanel;
                }
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

            // Debug.Log("-----------------------------------------------�̻��� " + (i++) + "��°");

            force = initForce;

            bulletSpeed = initSpeed;
        }
    }

}
