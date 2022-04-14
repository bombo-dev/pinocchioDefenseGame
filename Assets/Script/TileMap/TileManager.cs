using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] tileArr;

    //test
    [SerializeField]
    public List<GameObject> turret;

    /// <summary>
    /// 정수형 인덱스 배열을 가져와 인덱스에 맞는 게임오브젝트 배열을 생성하여 반환
    /// </summary>
    /// <param name="targetIndexArr">타일맵 번호 인덱스로 이루어진 배열</param>
    /// <returns>타일맵 게임 오브젝트로 이루어진 배열</returns>
    public GameObject[] CreateTileMapArr(int[] targetIndexArr)
    {
        //예외처리
        if (targetIndexArr.Length == 0)
            return null;

        GameObject[] goArr = new GameObject[targetIndexArr.Length];

        //타일 인덱스 배열을 타일 게임오브젝트 배열으로 사상
        for (int i = 0; i < targetIndexArr.Length; i++)
        {
            goArr[i] = tileArr[targetIndexArr[i]];
        }

        return goArr;
    }
}
