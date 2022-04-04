using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] tileArr;

    public GameObject[] CreateTileMapArr(int[] targetIndexArr)
    {
        //����ó��
        if (targetIndexArr.Length == 0)
            return null;

        GameObject[] goArr = new GameObject[targetIndexArr.Length];

        //Ÿ�� �ε��� �迭�� Ÿ�� ���ӿ�����Ʈ �迭���� ���
        for (int i = 0; i < targetIndexArr.Length; i++)
        {
            goArr[i] = tileArr[i];
        }

        return goArr;
    }
}
