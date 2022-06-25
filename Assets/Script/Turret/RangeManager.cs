using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
    //Load한 Range 프리팹 정보
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    [SerializeField]
    public Transform rangeParents;

    //filePath, cacheCount 저장
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    //생성한 게임오브젝트 
    GameObject currentRange;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();
    }

    /// <summary>
    /// 씬 로드 후 Range 캐시 데이터를 바탕으로 생성할 함수 호출 : 김현진
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), rangeParents);
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
    ///  사거리 표시 오브젝트 생성 : 김현진
    /// </summary>
    /// <param name="rangeIndex">사거리 표시 오브젝트 인덱스</param>
    /// <param name="range">표시할 사거리</param>
    /// <param name="rangePos">사거리 위치</param>
    public void EnableRange(int rangeIndex, int range, Vector3 rangePos)
    {

        //예외처리
        if (rangeIndex >= prefabCacheDatas.Length || prefabCacheDatas[rangeIndex].filePath == null)
            return;

        //생성 되어있는 오브젝트 삭제후 재생성
        DisableRange(rangeIndex);

        //생성한 프리팹 게임오브젝트 정보 받아오기
        currentRange = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[rangeIndex].filePath);

        if (currentRange == null)
            return;

        //사거리 표시 오브젝트 위치 변경
        currentRange.transform.position = rangePos;

        //사거리 표시 오브젝트 크기 변경
        float rangeScale = Mathf.Sqrt(range) * 1.4625f;
        currentRange.transform.localScale = new Vector3(rangeScale, rangeScale, currentRange.transform.localScale.z);

    }

    /// <summary>
    /// 사거리 지우기
    /// </summary>
    /// <param name="rangeIndex">지울 사거리 오브젝트 인덱스</param>
    public void DisableRange(int rangeIndex)
    {
        if (currentRange)
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(prefabCacheDatas[rangeIndex].filePath, currentRange);
    }
}
