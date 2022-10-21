using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;


/// <summary>
/// turret Json Data ���� 
/// </summary>
[Serializable]
public class TurretDatas
{
    public int turretNum;
    public int maxHP;
    public int power;
    public int defense;
    public float attackSpeed;
    public int range;
    public int regeneration;
    public int attackRangeType;
    public bool isRecoveryTower;
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
    public float turretAppearPosY;
    public int turretCost;
    public int turretConstructionTime;
    public string filepath;
}

public class TurretJson : MonoBehaviour
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
    /// TurretJson �ҷ�����
    /// </summary>
    void Load()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, "turret.json");
        string jsonString = File.ReadAllText(filepath);

        TurretDatas[] turretdata = JsonTurretHelper.FromJson<TurretDatas>(jsonString);
	}

    // TurretData, Turret ��ũ��Ʈ�� ��ü ������
	public TurretDatas[] GetTurretData()
    {
        // ������ ����Ƽ ������
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            string filepath = Path.Combine(Application.streamingAssetsPath, "turret.json");
            string jsonString = File.ReadAllText(filepath);
            // �ͷ� Json ��ȣȭ
            //jsonString = EncryptDecrypt.Decrypt(jsonString, "chungwoonPinocchio");

            TurretDatas[] turretdata = JsonTurretHelper.FromJson<TurretDatas>(jsonString);

            return turretdata;
        }
        // ������ ����Ƽ �÷��̾�
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string filepath = Path.Combine(Application.streamingAssetsPath, "turret.json");
            string jsonString = File.ReadAllText(filepath);
            // �ͷ� Json ��ȣȭ
            //jsonString = EncryptDecrypt.Decrypt(jsonString, "chungwoonPinocchio");

            TurretDatas[] turretdata = JsonTurretHelper.FromJson<TurretDatas>(jsonString);

            return turretdata;
        }
        // �� ����Ƽ ������
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            string filepath = Path.Combine(Application.streamingAssetsPath, "turret.json");
            string jsonString = File.ReadAllText(filepath);
            // �ͷ� Json ��ȣȭ
            //jsonString = EncryptDecrypt.Decrypt(jsonString, "chungwoonPinocchio");

            TurretDatas[] turretdata = JsonTurretHelper.FromJson<TurretDatas>(jsonString);

            return turretdata;
        }
        // �� ����Ƽ �÷��̾�
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            string filepath = Path.Combine(Application.streamingAssetsPath, "turret.json");
            string jsonString = File.ReadAllText(filepath);
            // �ͷ� Json ��ȣȭ
            //jsonString = EncryptDecrypt.Decrypt(jsonString, "chungwoonPinocchio");

            TurretDatas[] turretdata = JsonTurretHelper.FromJson<TurretDatas>(jsonString);

            return turretdata;
        }
        // �ȵ���̵� �����
        else
        {
            string originPath = Path.Combine(Application.streamingAssetsPath, "turret.json"); ;
            #pragma warning disable 612, 618
            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + "Turret.Json";
            File.WriteAllBytes(realPath, reader.bytes);

            string jsonString = File.ReadAllText(realPath);
            // �ͷ� Json ��ȣȭ
            //jsonString = EncryptDecrypt.Decrypt(jsonString, "chungwoonPinocchio");

            TurretDatas[] turretdata = JsonTurretHelper.FromJson<TurretDatas>(jsonString);

            return turretdata;
        }
    }

}

/// <summary>
/// Turret Json Parsing �����ִ� Ŭ����
/// </summary>
public static class JsonTurretHelper
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
