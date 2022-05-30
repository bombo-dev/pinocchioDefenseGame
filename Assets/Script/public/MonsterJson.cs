using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class MonsterData
{
    public int enemyIndex;
    public int maxHP;
    public int power;
    public int defense;
    public int speed;
    public int attackSpeed;
    public int range;
    public int regeneration;
    public int attackRangeType;
    public bool isRecoveryTower;
    public int attackTargetNum;
    public int debuffType;
    public int debuffDuration;
    public int multiAttackRange;
    public int bullet_index;
    public int damageEffectIndex;
    public int deadEffectIndex;
    public int fireEffectIndex;
    public int healEffectIndex;
    public int debuffEffectIndex;
    public int appearPos;
    public string filepath;
}

public class MonsterJson : MonoBehaviour
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
        string filepath = Path.Combine(Application.streamingAssetsPath, "Monster.json");
        string jsonString = File.ReadAllText(filepath);

        TurretDatas[] turretdata = JsonMonsterHelper.FromJson<TurretDatas>(jsonString);

        Debug.Log(turretdata[0].filepath);
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
        public T[] Monster;
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Monster;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Monster = array;
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }
}
