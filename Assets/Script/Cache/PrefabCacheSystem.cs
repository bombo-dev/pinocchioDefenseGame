using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabCacheData
{
    public string filePath;
    public int cacheCount;
}

public class PrefabCacheSystem : MonoBehaviour
{
    Dictionary<string, Queue<GameObject>> prefabCaChes = new Dictionary<string, Queue<GameObject>>();

    public void GeneratePrefabCache(string filePath, int cacheCount)
    {
        //�̹� ���� filePath�� ĳ�ø� ���� �� ���
        if (prefabCaChes.ContainsKey(filePath))
            return;

        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < cacheCount; i++)
        {
            //queue.Enqueue 
        }
    }
}
