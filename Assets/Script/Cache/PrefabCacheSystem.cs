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
    //생성된 프리팹 정보
    Dictionary<string, Queue<GameObject>> prefabCaChes = new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// 프리팹을 여러개 비활성 상태로 미리 생성해 놓고 정보를 딕셔너리에 저장 : 김현진
    /// </summary>
    /// <param name="filePath">프리팹 파일 주소</param>
    /// <param name="cacheCount">생성할 프리팹 숫자</param>
    /// <param name="gameObject">프리팹 게임 오브젝트</param>
    /// <param name="parentsGameObject">생성될 게임 오브젝트의 부모 오브젝트</param>
    public void GeneratePrefabCache(string filePath, int cacheCount, GameObject gameObject, Transform parentsGameObject = null)
    {
        //이미 같은 filePath의 캐시를 생성 한 경우
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
