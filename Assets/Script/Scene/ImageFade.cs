using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    [SerializeField]
    Image nightBackGroundImage;

    public static float alpha;  //�ܺο��� �ʱ�ȭ

    public static bool fade = false;

    public static bool finFade;

    public static int fadeType = 0; //�ܺο��� �ʱ�ȭ

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
                //���̵� �ƿ�
                alpha -= (1f * Time.deltaTime);
                nightBackGroundImage.color = new Color(1, 1, 1, alpha);

                //���̵� ����
                if (alpha < 0)
                {
                    fade = false;
                    finFade = true;
                }
            }
            else
            {
                //���̵� ��
                alpha += (1f * Time.deltaTime);
                nightBackGroundImage.color = new Color(1, 1, 1, alpha);

                //���̵� ����
                if (alpha > 1)
                {
                    fade = false;
                    finFade = true;
                }
            }  
        }
    }

    /// <summary>
    /// ���̵� �ƿ� ���� : ������
    /// </summary>
    /// <param name="fadeType"> 0-> ���̵�ƿ�, 1-> ���̵���</param>
    IEnumerator StartFadeOut()
    {
       
        yield return new WaitForSeconds(0.1f);
        fade = true;

    }

}
