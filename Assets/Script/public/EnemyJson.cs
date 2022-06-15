using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class EnemyData
{
    public int enemyNum;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Monster Json 테스트 
    /// </summary>
    void Test()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
        string jsonString = File.ReadAllText(filepath);

        EnemyData[] enemyData = JsonMonsterHelper.FromJson<EnemyData>(jsonString);

        // Debug.Log(enemyData[0].appearPos[0].X + " , " + enemyData[0].appearPos[0].Y + " , " + enemyData[0].appearPos[0].Z);
        // Debug.Log(enemyData[0].appearPos[1].X + " , " + enemyData[0].appearPos[1].Y + " , " + enemyData[0].appearPos[1].Z);
        // Debug.Log(enemyData[0].appearPos[2].X + " , " + enemyData[0].appearPos[2].Y + " , " + enemyData[0].appearPos[2].Z);
    }

    // enemyData 객체를 받아 Enemy 스크립트로 보내기
    public EnemyData[] GetEnemyData()
    {

        //윈도우 유니티 에디터
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {

            string filepath = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
            string jsonString = File.ReadAllText(filepath);

            EnemyData[] enemyData = JsonMonsterHelper.FromJson<EnemyData>(jsonString);

            return enemyData;
        }
        // 윈도우 플레이어 유니티 에디터
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string filepath = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
            string jsonString = File.ReadAllText(filepath);

            EnemyData[] enemyData = JsonMonsterHelper.FromJson<EnemyData>(jsonString);

            return enemyData;
        }
        // 맥 OS 유니티 에디터
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            string filepath = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
            string jsonString = File.ReadAllText(filepath);

            EnemyData[] enemyData = JsonMonsterHelper.FromJson<EnemyData>(jsonString);

            return enemyData;
        }

        //맥 OS 유니티 플레이어
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            string filepath = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
            string jsonString = File.ReadAllText(filepath);

            EnemyData[] enemyData = JsonMonsterHelper.FromJson<EnemyData>(jsonString);

            return enemyData;
        }
        // 안드로이드
        else
        {
            string originPath = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
            #pragma warning disable 612, 618
            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);

            string jsonString = File.ReadAllText(realPath);

            EnemyData[] enemyData = JsonMonsterHelper.FromJson<EnemyData>(jsonString);

            return enemyData;
        }
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
