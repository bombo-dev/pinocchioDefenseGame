using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


/// <summary>
/// turret Json Data 변수 
/// </summary>
[Serializable]
public class TurretDatas
{
    // private int turretNum_value;
    public int turretNum;
    public int maxHP;
    public int power;
    public int defense;
    public double attackSpeed;
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
    public double turretAppearPosY;
    public string filepath;
}

public class TurretJson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// TurretJson 불러오기
    /// </summary>
    void Load()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "turret.json");
        string jsonString = File.ReadAllText(filepath);

        TurretDatas[] turretdata = JsonHelper.FromJson<TurretDatas>(jsonString);

        //Debug.Log(turretdata[0].filepath);
    }

    public TurretDatas[] GetTurretData()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "turret.json");
        string jsonString = File.ReadAllText(filepath);

        TurretDatas[] turretdata = JsonMonsterHelper.FromJson<TurretDatas>(jsonString);

        return turretdata;
    }

}

/// <summary>
/// Turret Json Parsing 도와주는 클래스
/// </summary>
public static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Turret;
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Turret;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Turret = array;
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }
}
