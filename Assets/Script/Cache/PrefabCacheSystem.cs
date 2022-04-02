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
    //������ ������ ����
    Dictionary<string, Queue<GameObject>> prefabCaChes = new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// �������� ������ ��Ȱ�� ���·� �̸� ������ ���� ������ ��ųʸ��� ���� : ������
    /// </summary>
    /// <param name="filePath">������ ���� �ּ�</param>
    /// <param name="cacheCount">������ ������ ����</param>
    /// <param name="gameObject">������ ���� ������Ʈ</param>
    /// <param name="parentsGameObject">������ ���� ������Ʈ�� �θ� ������Ʈ</param>
    public void GeneratePrefabCache(string filePath, int cacheCount, GameObject gameObject, Transform parentsGameObject = null)
    {
        //�̹� ���� filePath�� ĳ�ø� ���� �� ���
        if (prefabCaChes.ContainsKey(filePath))
            return;

        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < cacheCount; i++)
        {
            GameObject go = Instantiate<GameObject>(gameObject, parentsGameObject);
            go.SetActive(false);
            queue.Enqueue(go);
           // queue.Enqueue(Instantiate<GameObject>(gameObject, parentGameObject));
        }
        prefabCaChes.Add(filePath, queue);
    }
}
