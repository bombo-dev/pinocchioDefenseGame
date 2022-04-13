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



    private void Update()
    {

    }

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


    public void EnableEnemy(int enemyIndex, int gateNum, int[] targetTile)
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
        enemy.targetTile = SystemManager.Instance.TileManager.CreateTileMapArr(targetTile);

        //���� �ʱ���·�
        enemy.Reset();


    }

}
