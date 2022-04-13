using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject attackTarget;    //총알의 최종 목적지  

    [SerializeField]
    string filePath; 

    [SerializeField]
    int bulletType; //0: 직선형 총알, 1: 곡선형 총알 

    [SerializeField]
    float reduceHeight;         // 곡선형 공격의 포물선 높이 조절 변수

    [SerializeField]
    float journeyTime;      // bullet이 시작점에서 도착점에 도달하는 시간

    // Update is called once per frame
    void Update()
    {
        UpdateBullet(); 
    }

    /// <summary>
    /// 총알 발사 업데이트 : 하은비
    /// </summary>
    void UpdateBullet()
    {
        Vector3 bulletPos = transform.position;   // 총알의 위치
        Vector3 targetPos = attackTarget.transform.position;  // 타겟이 총알을 맞는 위치

        if (bulletType == 0)//직선형
            transform.position = Vector3.Lerp(bulletPos, targetPos, 0.05f);
        else if (bulletType == 1)//곡선형
        {
            /*
            Vector3 center = (bulletPos + targetPos) / 2;
            center -= new Vector3(0, reduceHeight * 1.0f, 0);
            Vector3 startPos = bulletPos - center;
            Vector3 endPos = targetPos - center;
            float fracCmplete = (Time.time - attackTimer) / journeyTime;
            bullet[bulletIdx].transform.position = Vector3.Slerp(startPos, endPos, fracCmplete);
            bullet[bulletIdx].transform.position += center;
            */
        }

        // bullet과 target의 거리가 1보다 작을 경우 불렛 비활성화
        float distance = (targetPos - bulletPos).magnitude;

        if (Mathf.Round(distance * 10) / 10 < 1.0f)
        {
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, gameObject);

            // 다중 타겟인 경우, 이펙트 출력
        }
    }

}
