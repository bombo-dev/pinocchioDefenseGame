using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Cloud;

    [SerializeField]
    GameObject[] StageImage;
    // Start is called before the first frame update
    void Start()
    {
        //스테이지 가드용 구름 셋팅
        SetCloud();
    }

    /// <summary>
    /// 현재 maxStatNum에 맞게 스테이지 가드를 활성화/비활성화 : 김현진
    /// </summary>
    void SetCloud()
    {
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        //챕터4
        if (userInfo.maxStageNum >= 30 || userInfo.maxStageNum_hard >= 30)
        {
            for (int i = 0; i < Cloud.Length; i++)
            {
                if (i <= 2)
                {
                    if (Cloud[i].activeSelf)
                        Cloud[i].SetActive(false);
                    if (StageImage[i].activeSelf)
                        StageImage[i].SetActive(false);
                }
                else
                {
                    if (!Cloud[i].activeSelf)
                        Cloud[i].SetActive(true);
                    if (!StageImage[i].activeSelf)
                        StageImage[i].SetActive(true);
                }
            }
        }
        //챕터3
        else if (userInfo.maxStageNum > 20 || userInfo.maxStageNum_hard > 20)
        {
            for (int i = 0; i < Cloud.Length; i++)
            {
                if (i <= 1)
                {
                    if (Cloud[i].activeSelf)
                        Cloud[i].SetActive(false);
                    if (StageImage[i].activeSelf)
                        StageImage[i].SetActive(false);
                }
                else
                {
                    if (!Cloud[i].activeSelf)
                        Cloud[i].SetActive(true);
                    if (!StageImage[i].activeSelf)
                        StageImage[i].SetActive(true);
                }
            }
        }
        //챕터2
        else if (userInfo.maxStageNum > 10 || userInfo.maxStageNum_hard > 10)
        {
            for (int i = 0; i < Cloud.Length; i++)
            {
                if (i <= 0)
                {
                    if (Cloud[i].activeSelf)
                        Cloud[i].SetActive(false);
                    if (StageImage[i].activeSelf)
                        StageImage[i].SetActive(false);
                }
                else
                {
                    if (!Cloud[i].activeSelf)
                        Cloud[i].SetActive(true);
                    if (!StageImage[i].activeSelf)
                        StageImage[i].SetActive(true);
                }
            }
        }

        //챕터1
        else
        {
            for (int i = 0; i < Cloud.Length; i++)
            {
                if (!Cloud[i].activeSelf)
                    Cloud[i].SetActive(true);
                if (!StageImage[i].activeSelf)
                    StageImage[i].SetActive(true);
            }
        }

    }
}
