using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Load�� Enemy ������ ����
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // Ȱ��ȭ�� enemy�� �޾ƿ� �迭
    public List<GameObject> enemies;

    [SerializeField]
    Transform enemyParents;

    //filePath, cacheCount ����
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();
    }

    /// <summary>
    /// �� �ε� �� Enemy ĳ�� �����͸� �������� ������ �Լ� ȣ�� : ������
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
    /// Enemy ��ü�� ����
    /// </summary>
    /// <param name="enemyIndex">������ Enemy�� ����� �ε���</param>
    /// <param name="gateNum">������ Gate �ε��� 0~2</param>
    /// <param name="targetTile">������ Enemy�� ���� targetPoint��ȣ</param>
    public void EnableEnemy(int enemyIndex, int gateNum, int[] targetPoint)
    {
        //����ó��
        if (enemyIndex >= prefabCacheDatas.Length || prefabCacheDatas[enemyIndex].filePath == null)
            return;

        //������ ������ ���ӿ�����Ʈ ���� �޾ƿ���
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[enemyIndex].filePath);

        if (go == null)
            return;

        //������ �������� ��ȿ�� ���
        Enemy enemy = go.GetComponent<Enemy>();
        enemies.Add(go);

        //������ ������ ��ü ���� �ʱ�ȭ
        enemy.enemyIndex = enemies.FindIndex(x => x == go); //enemise ����Ʈ�� �ε����� ��ġ�ϴ� ��ȣ ����

        enemy.gateNum = gateNum;
        enemy.targetPoint = SystemManager.Instance.TileManager.CreateTileMapArr(targetPoint);

        //���� �ʱ���·�
        enemy.Reset();
    }

    /// <summary>
    /// ����Ʈ���� ������ enemy�� �����ϰ� ����Ʈ�� �籸��
    /// </summary>
    /// <param name="removeEnemyIndex">�籸���� ������ gameObject</param>
    public void ReorganizationEnemiesList(int removeEnemyIndex)
    {
        List<GameObject> tempEnemies = new List<GameObject>();
        int index = 0;

        for (int i = 0; i < enemies.Count; i++)
        {
            //������ gameObject�� ����
            if (i != removeEnemyIndex)
            {
                //enemies[i]�� null�̸� ����
                if (enemies[i])
                {
                    //����Ʈ �籸��
                    tempEnemies.Add(enemies[i]);
                    //enemyIndex��ȣ �ʱ�ȭ
                    enemies[i].GetComponent<Enemy>().enemyIndex = index;

                    index++;
                }
            }
        }//end of for

        enemies = tempEnemies;
    }
}
