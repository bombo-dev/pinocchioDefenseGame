using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    //Load�� Enemy ������ ����
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    [SerializeField]
    public Transform enemyParents;

    //filePath, cacheCount ����
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();
    }

    /// <summary>
    /// �� �ε� �� Bullet ĳ�� �����͸� �������� ������ �Լ� ȣ�� : ������
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), enemyParents);
        }
    }

    /// <summary>
    /// ������ ��θ� ���� ���ӿ�����Ʈ�� �����´� : ������
    /// </summary>
    /// <param name="filePath">�������� ������ִ� ���</param>
    /// <returns>��ο��� ������ ���� ������Ʈ</returns>
    GameObject Load(string filePath)
    {
        //�̹� ĳ�ÿ� ���ԵǾ� ���� ���
        if (prefabCaChes.ContainsKey(filePath))
            return prefabCaChes[filePath];
        else
        {
            GameObject go = Resources.Load<GameObject>(filePath);
            prefabCaChes.Add(filePath, go);

            return go;
        }
    }

    /// <summary>
    /// �Ѿ� �Ѱ� ���� : ������
    /// </summary>
    /// <param name="bulletIndex">������ �Ѿ� ��ȣ</param>
    /// <param name="bulletPos">�Ѿ��� ������ ��ġ</param>
    /// <param name="attackTarget">�Ѿ��� ���� ������</param>
    public void EnableBullet(int bulletIndex, Vector3 bulletPos, GameObject attackTarget, GameObject attackOwner)
    {

        //����ó��
        if (bulletIndex >= prefabCacheDatas.Length || prefabCacheDatas[bulletIndex].filePath == null)
            return;

        //������ ������ ���ӿ�����Ʈ ���� �޾ƿ���
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[bulletIndex].filePath);

        if (go == null)
            return;

        // ���� ���� �����϶� bullet ��Ȱ��ȭ
        if (bulletIndex == -1)
            go.SetActive(false);

        go.transform.position = bulletPos;

        Bullet bullet = go.GetComponent<Bullet>();

        // �ҷ��� �ʱ� ���� ���� ������ ��쿡�� �ʱ�ȭ
        if (bullet.initForce != 0 && bullet.initSpeed != 0)
        {
            bullet.force = bullet.initForce;
            bullet.bulletSpeed = bullet.initSpeed;
        }

        //����ó��
        if (bullet == null)
            return;

        Actor actor = attackTarget.GetComponent<Actor>();

        //����ó��
        if (actor == null)
            return;

        bullet.attackTarget = actor.hitPos;
        bullet.bulletLifeTime = 0;
        bullet.attackOwner = attackOwner;
        
    }

}
