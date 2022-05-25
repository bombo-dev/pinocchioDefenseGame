using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelManager : MonoBehaviour
{
    public static PanelManager instance = null;

    public static PanelManager Instance
    {
        get
        {
            return instance;
        }
    }

    // 활성화된 turret의 HPBar 패널을 저장할 리스트
    public List<GameObject> turretHPBars;

    // 활성화된 enemy의 HPBar 패널을 저장할 리스트
    public List<GameObject> enemyHPBars;

    [Header("PanelCachesInfo")]
    //Load한 Panel 프리팹 정보
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // 활성화된 panel를 받아올 리스트
    public UI_TurretMngPanel turretMngPanel;
    public UI_TurretInfoPanel turretInfoPanel;
    public StageMngPanel stageMngPanel;
    public StatusMngPanel statusMngPanel;

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
        else if (typeof(T) == typeof(StageMngPanel))
        {
            stageMngPanel = (compoenent as StageMngPanel);
        }
        /*
        else if (typeof(T) == typeof(StatusMngPanel))
        {
            statusMngPanel = (compoenent as StatusMngPanel);
            // (compoenent as StatusMngPanel).Reset();
            
        }
        */
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelIndex"></param>
    /// <param name="startPos"></param>
    /// <param name="hpBarsPanelIndex"></param>
    public void EnablePanel<T>(int panelIndex, Vector3 startPos, int hpBarsPanelIndex, System.Type type) where T : UnityEngine.Component
    {
        
        //예외처리
        if (panelIndex >= prefabCacheDatas.Length || prefabCacheDatas[panelIndex].filePath == null)
            return;

        //생성한 프리팹 게임오브젝트 정보 받아오기
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[panelIndex].filePath);

        if (go == null)
            return;

        // HPBar 리스트에 삽입
        if (type.Name == "Turret")
            turretHPBars.Add(go);
        else if (type.Name == "Enemy")
            enemyHPBars.Add(go);

        T compoenent = go.GetComponent<T>();

        if (typeof(T) == typeof(StatusMngPanel))
        {
            statusMngPanel = (compoenent as StatusMngPanel);
            // (compoenent as StatusMngPanel).Reset();
            
        }
        else
            return;

        //패널 위치 초기화
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(startPos.x, startPos.y+30, startPos.z));
        go.transform.position = screenPos;
       
        return;

    }
    
    public void ReorganizationPanelList(int removePanelIndex)
    {
        List<GameObject> tempPanels = new List<GameObject>();
        int index = 0;

        for (int i = 0; i < turretHPBars.Count; i++)
        {
            //제거할 gameObject면 제외
            if (i != removePanelIndex)
            {
                //enemies[i]가 null이면 제외
                if (turretHPBars[i])
                {
                    //리스트 재구성
                    tempPanels.Add(turretHPBars[i]);
                    //panelIndex번호 초기화
                    turretHPBars[i].GetComponent<StatusMngPanel>().HPBarsListIndex = index;

                    index++;
                }
            }
        }//end of for

        turretHPBars = tempPanels;
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
        else if (typeof(T) == typeof(StageMngPanel))
        {
            filePath = (compoenent as StageMngPanel).filePath;
            stageMngPanel = null;
        }
        else if (typeof(T) == typeof(StatusMngPanel))
        {
            filePath = (compoenent as StatusMngPanel).filePath;
            statusMngPanel = null;
        }
        else
            return;

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, go);
    }

}
