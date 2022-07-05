using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> targetList;

    //Load한 Block 프리팹 정보
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // 활성화된 block을 받아올 리스트
    public List<GameObject> blocks;

    [SerializeField]
    Transform blockParents;

    //filePath, cacheCount 저장
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    [SerializeField]
    GameObject fieldGo;

    [SerializeField]
    GameObject[] field;

    [SerializeField]
    GameObject Light_RealTime_Normal;
    [SerializeField]
    GameObject Light_RealTime_Hard_Enemy;
    [SerializeField]
    GameObject Light_RealTime_Hard;

    public GameObject[] tutorialNest;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();

        EnableField();
    }

    /// <summary>
    /// 스테이지에 맞는 맵 활성화 : 김현진
    /// </summary>
    void EnableField()
    {
        fieldGo.SetActive(true);

        if (SystemManager.Instance.UserInfo.selectMode == 0) // - 노말
        {
            if (!Light_RealTime_Normal.activeSelf)
                Light_RealTime_Normal.SetActive(true);

            if (Light_RealTime_Hard.activeSelf)
                Light_RealTime_Hard.SetActive(false);

            if (Light_RealTime_Hard_Enemy.activeSelf)
                Light_RealTime_Hard_Enemy.SetActive(false);
        }
        else
        {
            if (Light_RealTime_Normal.activeSelf)
                Light_RealTime_Normal.SetActive(false);

            if (!Light_RealTime_Hard.activeSelf)
                Light_RealTime_Hard.SetActive(true);

            if (!Light_RealTime_Hard_Enemy.activeSelf)
                Light_RealTime_Hard_Enemy.SetActive(true);
        }
    }

    /// <summary>
    /// 씬 로드 후 Block 캐시 데이터를 바탕으로 생성할 함수 호출 : 김현진
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), blockParents);
        }

        //스테이지에 맞는 맵 생성
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        if (userInfo.selectMode == 0)// -노말
        {
            if (userInfo.selectedStageNum == 0)//튜토리얼
                fieldGo = field[0];
            else if (userInfo.selectedStageNum <= 5)
                fieldGo = field[1];
            else if (userInfo.selectedStageNum <= 10)
                fieldGo = field[2];
            else if (userInfo.selectedStageNum <= 15)
                fieldGo = field[3];
            else if (userInfo.selectedStageNum <= 20)
                fieldGo = field[4];
            else if (userInfo.selectedStageNum <= 25)
                fieldGo = field[5];
            else if (userInfo.selectedStageNum <= 30)
                fieldGo = field[6];
            else if (userInfo.selectedStageNum <= 35)
                fieldGo = field[7];
            else if (userInfo.selectedStageNum <= 40)
                fieldGo = field[8];
        }
        else // - 하드
        {
            if (userInfo.selectedStageNum_hard == 0)//튜토리얼
                fieldGo = field[0];
            else if (userInfo.selectedStageNum_hard <= 5)
                fieldGo = field[1];
            else if (userInfo.selectedStageNum_hard <= 10)
                fieldGo = field[2];
            else if (userInfo.selectedStageNum_hard <= 15)
                fieldGo = field[3];
            else if (userInfo.selectedStageNum_hard <= 20)
                fieldGo = field[4];
            else if (userInfo.selectedStageNum_hard <= 25)
                fieldGo = field[5];
            else if (userInfo.selectedStageNum_hard <= 30)
                fieldGo = field[6];
            else if (userInfo.selectedStageNum_hard <= 35)
                fieldGo = field[7];
            else if (userInfo.selectedStageNum_hard <= 40)
                fieldGo = field[8];
        }

        //targetArr초기화
        for (int i = 0; i < fieldGo.transform.GetChild(0).childCount; i++)
        {
            targetList.Add(fieldGo.transform.GetChild(0).GetChild(i).gameObject);
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
    /// Block 객체를 생성
    /// </summary>
    /// <param name="enemyIndex">생성할 Block이 저장될 인덱스</param>
    public void EnableBlock(int blockIndex)
    {
        //예외처리
        if (blockIndex >= prefabCacheDatas.Length || prefabCacheDatas[blockIndex].filePath == null)
            return;

        //생성한 프리팹 게임오브젝트 정보 받아오기
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[blockIndex].filePath);

        if (go == null)
            return;

        blocks.Add(go);

        //targetArr초기화
        for (int i = 0; i < go.transform.GetChild(0).childCount; i++)
        {
            targetList.Add(go.transform.GetChild(0).GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 정수형 인덱스 배열을 가져와 인덱스에 맞는 게임오브젝트 배열을 생성하여 반환
    /// </summary>
    /// <param name="targetIndexArr">타일맵 번호 인덱스로 이루어진 배열</param>
    /// <returns>타일맵 게임 오브젝트로 이루어진 배열</returns>
    public GameObject[] CreateTargetArr(int[] targetIndexArr)
    {
        //예외처리
        if (targetIndexArr.Length == 0)
            return null;

        GameObject[] goArr = new GameObject[targetIndexArr.Length];

        //타일 인덱스 배열을 타일 게임오브젝트 배열으로 사상
        for (int i = 0; i < targetIndexArr.Length; i++)
        {
            goArr[i] = targetList[targetIndexArr[i]];
        }

        return goArr;
    }
}
