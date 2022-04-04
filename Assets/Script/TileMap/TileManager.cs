using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] tileArr;

    public GameObject[] CreateTileMapArr(int[] targetIndexArr)
    {
        //예외처리
        if (targetIndexArr.Length == 0)
            return null;

        GameObject[] goArr = new GameObject[targetIndexArr.Length];

        //타일 인덱스 배열을 타일 게임오브젝트 배열으로 사상
        for (int i = 0; i < targetIndexArr.Length; i++)
        {
            goArr[i] = tileArr[i];
        }

        return goArr;
    }
}
