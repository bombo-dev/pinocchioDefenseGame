using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    [SerializeField]
    Image nightBackGroundImage;

    public static float alpha;  //외부에서 초기화

    public static bool fade = false;

    public static bool finFade;

    public static int fadeType = 0; //외부에서 초기화

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartFadeOut");
        fade = false;
        finFade = false;
        nightBackGroundImage.color = new Color(1, 1, 1, alpha);
    }

    // Update is called once per frame
    void Update()
    {

        if (fade)
        {
            if (fadeType == 0)
            {
                //페이드 아웃
                alpha -= (1f * Time.deltaTime);
                nightBackGroundImage.color = new Color(1, 1, 1, alpha);

                //페이드 종료
                if (alpha < 0)
                {
                    fade = false;
                    finFade = true;
                }
            }
            else
            {
                //페이드 인
                alpha += (1f * Time.deltaTime);
                nightBackGroundImage.color = new Color(1, 1, 1, alpha);

                //페이드 종료
                if (alpha > 1)
                {
                    fade = false;
                    finFade = true;
                }
            }  
        }
    }

    /// <summary>
    /// 페이드 아웃 시작 : 김현진
    /// </summary>
    /// <param name="fadeType"> 0-> 페이드아웃, 1-> 페이드인</param>
    IEnumerator StartFadeOut()
    {
       
        yield return new WaitForSeconds(0.1f);
        fade = true;

    }

}
