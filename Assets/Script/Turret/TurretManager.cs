using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    //특수터렛 인덱스
    int BASETURRET_INDEX = 23;
    public int CONSTRUCTIONTURRET_INDEX = 24;


    //Load한 Turret 프리팹 정보
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // 활성화된 turret 받아올 리스트
    public List<GameObject> turrets;

    [SerializeField]
    Transform turretParents;

    //filePath, cacheCount 저장
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    //TurretNum으로 액세스 할 수 있는 터렛 건설시간 배열
    public float[] turretConstructionTime;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();

        //베이스 터렛 건설
        EnableBase();
    }

    /// <summary>
    /// 씬 로드 후 Enemy 캐시 데이터를 바탕으로 생성할 함수 호출 : 김현진
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), turretParents);
        }
    }

    /// <summary>
    /// 기지 역할을 하는 베이스 터렛 건설 : 김현진
    /// </summary>
    void EnableBase()
    {
        GameObject go = EnableTurret(BASETURRET_INDEX, Vector3.zero);

        if (!go)
            return;

        //위치 초기화
        go.transform.localPosition = new Vector3(0f, 0f, -178f);

        Turret baseTurret = go.GetComponent<Turret>();
        // 터렛 상태 관리 패널 생성
        GameObject statusMngPanelGo = SystemManager.Instance.PanelManager.EnablePanel<StatusMngPanel>(3, go);

        if (!SystemManager.Instance.PanelManager.statusMngPanel)
            return;

        StatusMngPanel statusMngPanel = statusMngPanelGo.GetComponent<StatusMngPanel>();
        baseTurret.statusMngPanel = statusMngPanel;

        statusMngPanel.panelPos = baseTurret.transform.position;
        statusMngPanel.hpBarOwner = go;
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
    /// Turret 객체를 생성 : 김현진
    /// </summary>
    /// <param name="enemyIndex">생성할 Turret이 저장될 인덱스</param>
    /// <param name="turretPos">Turret이 생성될 위치</param>
    public GameObject EnableTurret(int turretIndex, Vector3 turretPos)
    {
        //예외처리
        if (turretIndex >= prefabCacheDatas.Length || prefabCacheDatas[turretIndex].filePath == null)
            return null;

        //생성한 프리팹 게임오브젝트 정보 받아오기
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[turretIndex].filePath);

        if (go == null)
            return null;

        //공사터렛일 경우
        if (turretIndex == CONSTRUCTIONTURRET_INDEX)
        {
            go.transform.position = turretPos;
            return go;
        }

        //생성한 프리팹이 유효할 경우
        Turret turret = go.GetComponent<Turret>();
        turrets.Add(go);

        //터렛 위치 초기화
        go.transform.position = turretPos;

        //생성한 프리팹 객체 변수 초기화
        turret.turretIndex = turrets.FindIndex(x => x == go); //enemise 리스트의 인덱스와 일치하는 번호 저장

        //터렛을 초기상태로
        turret.Reset();

        return go;
    }

    /// <summary>
    /// 리스트에서 삭제될 turret을 제외하고 리스트를 재구성
    /// </summary>
    /// <param name="removeEnemyIndex">재구성시 제거할 gameObject</param>
    public void ReorganizationEnemiesList(int removeTurretndex)
    {
        List<GameObject> tempTurrets = new List<GameObject>();
        int index = 0;

        for (int i = 0; i < turrets.Count; i++)
        {
            //제거할 gameObject면 제외
            if (i != removeTurretndex)
            {
                //enemies[i]가 null이면 제외
                if (turrets[i])
                {
                    //리스트 재구성
                    tempTurrets.Add(turrets[i]);
                    //enemyIndex번호 초기화
                    turrets[i].GetComponent<Turret>().turretIndex = index;

                    index++;
                }
            }
        }//end of for

        turrets = tempTurrets;
    }
}
