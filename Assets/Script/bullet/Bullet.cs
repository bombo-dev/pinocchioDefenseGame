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
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
            // ���� Ÿ���� ���, ����Ʈ ���
        }


    }

}