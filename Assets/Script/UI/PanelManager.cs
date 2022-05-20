using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelManager : MonoBehaviour
{
    [Header("PanelCachesInfo")]
    //Load한 Panel 프리팹 정보
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // 활성화된 panel를 받아올 리스트
    public UI_TurretMngPanel turretMngPanel;
    public UI_TurretInfoPanel turretInfoPanel;

    [SerializeField]
    Transform canvas;

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
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), canvas);
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
    ///  Panel 객체를 생성
    /// </summary>
    /// <typeparam name="T">패널이 가지고 있는 UI_Panel 스크립트</typeparam>
    /// <param name="panelIndex">생성할 패널 번호</param>
    public void EnablePanel<T>(int panelIndex) where T : UnityEngine.Component
    {
        //예외처리
        if (panelIndex >= prefabCacheDatas.Length || prefabCacheDatas[panelIndex].filePath == null)
            return;

        //생성한 프리팹 게임오브젝트 정보 받아오기
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[panelIndex].filePath);

        if (go == null)
            return;

        T compoenent = go.GetComponent<T>();

        if (typeof(T) == typeof(UI_TurretMngPanel))
            turretMngPanel = (compoenent as UI_TurretMngPanel);
        else if (typeof(T) == typeof(UI_TurretInfoPanel))
        {
            turretInfoPanel = (compoenent as UI_TurretInfoPanel);
            (compoenent as UI_TurretInfoPanel).Reset();
        }
    }


    public void DisablePanel<T>(GameObject go) where T: UnityEngine.Component
    {
        //예외처리
        if (go == null)
            return;

        T compoenent = go.GetComponent<T>();

        //예외처리
        if (compoenent == null)
            return;

        string filePath = null;

        if (typeof(T) == typeof(UI_TurretMngPanel))
        {
            filePath = (compoenent as UI_TurretMngPanel).filePath;
            turretMngPanel = null;
        }
        else if (typeof(T) == typeof(UI_TurretInfoPanel))
        {
            filePath = (compoenent as UI_TurretInfoPanel).filePath;
            turretInfoPanel = null;
        }
        else
            return;

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, go);
    }
}
