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


    /// <summary>
    /// ��Ȱ��ȭ ������ �������� Ȱ��ȭ : ������
    /// </summary>
    /// <param name="filePath">Ȱ��ȭ�� �������� ���</param>
    /// <param name="appearPosition">Ȱ��ȭ�� �������� ���ܳ� ��ġ</param>
    public void EnablePrefabCache(string filePath, Vector3 appearPosition)
    {
        //ĳ�ð� �������� �������
        if (!prefabCaChes.ContainsKey(filePath))
            return;

        //ť�� ����� ���
        if (prefabCaChes[filePath].Count == 0)
            return;

        GameObject go = prefabCaChes[filePath].Dequeue();
        go.SetActive(true);
    }

    /// <summary>
    /// Ȱ��ȭ ������ �������� ��Ȱ��ȭ : ������
    /// </summary>
    /// <param name="filePath">��Ȱ��ȭ�� �������� ���</param>
    /// <param name="gameObject">��Ȱ��ȭ�� ������ ���ӿ�����Ʈ</param>
    public void DisablePrefabCache(string filePath, GameObject gameObject)
    {
        //ĳ�ð� �������� �������
        if (!prefabCaChes.ContainsKey(filePath))
            return;

        prefabCaChes[filePath].Enqueue(gameObject);
        gameObject.SetActive(false);
    }
}