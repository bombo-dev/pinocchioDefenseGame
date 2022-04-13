using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject attackTarget;    //�Ѿ��� ���� ������  

    [SerializeField]
    string filePath; 

    [SerializeField]
    int bulletType; //0: ������ �Ѿ�, 1: ��� �Ѿ� 

    [SerializeField]
    float reduceHeight;         // ��� ������ ������ ���� ���� ����

    [SerializeField]
    float journeyTime;      // bullet�� ���������� �������� �����ϴ� �ð�

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
        Vector3 bulletPos = transform.position;   // �Ѿ��� ��ġ
        Vector3 targetPos = attackTarget.transform.position;  // Ÿ���� �Ѿ��� �´� ��ġ

        if (bulletType == 0)//������
            transform.position = Vector3.Lerp(bulletPos, targetPos, 0.05f);
        else if (bulletType == 1)//���
        {
            /*
            Vector3 center = (bulletPos + targetPos) / 2;
            center -= new Vector3(0, reduceHeight * 1.0f, 0);
            Vector3 startPos = bulletPos - center;
            Vector3 endPos = targetPos - center;
            float fracCmplete = (Time.time - attackTimer) / journeyTime;
            bullet[bulletIdx].transform.position = Vector3.Slerp(startPos, endPos, fracCmplete);
            bullet[bulletIdx].transform.position += center;
            */
        }

        // bullet�� target�� �Ÿ��� 1���� ���� ��� �ҷ� ��Ȱ��ȭ
        float distance = (targetPos - bulletPos).magnitude;

        if (Mathf.Round(distance * 10) / 10 < 1.0f)
        {
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);

            // ���� Ÿ���� ���, ����Ʈ ���
        }
    }

}
