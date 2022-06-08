using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class EnemyData
{
    public int enemyNum;
    public int enemyIndex;
    public int maxHP;
    public int power;
    public int defense;
    public int speed;
    public float attackSpeed;
    public int range;
    public int regeneration;
    public int attackRangeType;
    public bool isRecoveryTower;
    public bool selfDestruct;
    public int attackTargetNum;
    public int debuffType;
    public int debuffDuration;
    public int multiAttackRange;
    public int bulletIndex;
    public int damageEffectIndex;
    public int deadEffectIndex;
    public int fireEffectIndex;
    public int healEffectIndex;
    public int debuffEffectIndex;
    public AppearPos[] appearPos;
    public int rewardWoodResource;
    public string filepath;
}

[Serializable]
public class AppearPos
{
    public float X;
    public float Y;
    public float Z;
}

public class EnemyJson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Monster Json 불러오기
    /// </summary>
    void Load()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
        string jsonString = File.ReadAllText(filepath);

        EnemyData[] enemyData = JsonMonsterHelper.FromJson<EnemyData>(jsonString);

        Debug.Log(enemyData[0].appearPos[0].X + " , " + enemyData[0].appearPos[0].Y + " , " + enemyData[0].appearPos[0].Z);
        Debug.Log(enemyData[0].appearPos[1].X + " , " + enemyData[0].appearPos[1].Y + " , " + enemyData[0].appearPos[1].Z);
        Debug.Log(enemyData[0].appearPos[2].X + " , " + enemyData[0].appearPos[2].Y + " , " + enemyData[0].appearPos[2].Z);
    }

    public EnemyData[] GetEnemyData()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
        string jsonString = File.ReadAllText(filepath);

        EnemyData[] enemyData = JsonMonsterHelper.FromJson<EnemyData>(jsonString);

        return enemyData;
    }

}

/// <summary>
/// Monster Json Parsing 도와주는 클래스
/// </summary>
public static class JsonMonsterHelper
{
    [Serializable]
    private class Wrapper<T> 
    {
        public T[] Enemy;
    }

    public static T[] FromJson<T>(string json) 
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Enemy;
    }

    public static string ToJson<T>(T[] array) 
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Enemy = array;
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }
}
