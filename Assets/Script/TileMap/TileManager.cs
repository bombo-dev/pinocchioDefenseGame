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
    /// ������ �ε��� �迭�� ������ �ε����� �´� ���ӿ�����Ʈ �迭�� �����Ͽ� ��ȯ
    /// </summary>
    /// <param name="targetIndexArr">Ÿ�ϸ� ��ȣ �ε����� �̷���� �迭</param>
    /// <returns>Ÿ�ϸ� ���� ������Ʈ�� �̷���� �迭</returns>
    public GameObject[] CreateTileMapArr(int[] targetIndexArr)
    {
        //����ó��
        if (targetIndexArr.Length == 0)
            return null;

        GameObject[] goArr = new GameObject[targetIndexArr.Length];

        //Ÿ�� �ε��� �迭�� Ÿ�� ���ӿ�����Ʈ �迭���� ���
        for (int i = 0; i < targetIndexArr.Length; i++)
        {
            goArr[i] = tileArr[targetIndexArr[i]];
        }

        return goArr;
    }
}
