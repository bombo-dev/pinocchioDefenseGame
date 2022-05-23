using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    //Load한 Enemy 프리팹 정보
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // 활성화된 enemy를 받아올 리스트
    public List<GameObject> effects;

    [SerializeField]
    Transform effectParents;

    //filePath, cacheCount 저장
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();
    }

    /// <summary>
    /// 씬 로드 후 Enemy 캐시 데이터를 바탕으로 생성할 함수 호출 : 김현진
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), effectParents);
        }
    }

    /// <summary>
    /// 프리팹 경로를 통해 게임오브젝트를 가져온다 : 김현진
    /// </summary>
    /// <param name="filePath">프리팹이 저장되있는 경로</param>
    /// <returns>경로에서 가져온 게임 오브젝트</returns>
    GameObject Load(string filePath)
    {
        //이미 캐시에 포함되어 있을 경우
        if (prefabCaChes.ContainsKey(filePath))
            return prefabCaChes[filePath];
        else
        {
            GameObject go = Resources.Load<GameObject>(filePath);
            prefabCaChes.Add(filePath, go);

            return go;
        }
    }

    /// <summary>
    /// Effect객체를 생성
    /// </summary>
    /// <param name="effectIndex">생성할 이팩트 번호</param>
    public GameObject EnableEffect(int effectIndex, Vector3 appearPos)
    {
        //예외처리
        if (effectIndex >= prefabCacheDatas.Length || prefabCacheDatas[effectIndex].filePath == null)
            return null;

        //생성한 프리팹 게임오브젝트 정보 받아오기
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[effectIndex].filePath);

        //예외처리
        if (!go)
            return null;

        //이펙트 위치 설정
        go.transform.position = appearPos;

        return go;
    }
}
