using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    //Load한 Enemy 프리팹 정보
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    [SerializeField]
    Transform enemyParents;

    //filePath, cacheCount 저장
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();
    }

    /// <summary>
    /// 씬 로드 후 Bullet 캐시 데이터를 바탕으로 생성할 함수 호출 : 김현진
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), enemyParents);
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
    /// 총알 한개 생성 : 김현진
    /// </summary>
    /// <param name="bulletIndex">생성할 총알 번호</param>
    /// <param name="bulletPos">총알이 생성될 위치</param>
    /// <param name="attackTarget">총알의 최종 목적지</param>
    public void EnableBullet(int bulletIndex, Vector3 bulletPos, GameObject attackTarget)
    {

        //예외처리
        if (bulletIndex >= prefabCacheDatas.Length || prefabCacheDatas[bulletIndex].filePath == null)
            return;

        //생성한 프리팹 게임오브젝트 정보 받아오기
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[bulletIndex].filePath);

        if (go == null)
            return;

        // 다중 공격 유닛일때 bullet 비활성화
        if (bulletIndex == -1)
            go.SetActive(false);

        go.transform.position = bulletPos;

        Bullet bullet = go.GetComponent<Bullet>();

        //예외처리
        if (bullet == null)
            return;

        Actor actor = attackTarget.GetComponent<Actor>();

        //예외처리
        if (actor == null)
            return;

        bullet.attackTarget = actor.hitPos;
        bullet.bulletLifeTime = Time.time;


        //Actor attacker = actor.attackOwner.GetComponentInParent<Actor>();
        //if (attacker == null)
         //   return;

        //bullet.bulletSpeed = attacker.bulletSpeed;
        //Debug.Log("bullet.bulletSpeed= " + bullet.bulletSpeed);



    }

}
