using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<Enemy> enemises = new List<Enemy>();

    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    [SerializeField]
    PrefabCacheSystem prefabCacheSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            prefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount);          
        }
    }

}
