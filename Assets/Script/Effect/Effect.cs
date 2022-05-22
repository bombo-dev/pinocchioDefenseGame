using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField]
    string filePath;

    ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!ps.IsAlive())
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);
    }
}
