using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeOut : MonoBehaviour
{
    [SerializeField]
    Image nightBackGroundImage;

    float alpha;

    bool fadeOut;
    // Start is called before the first frame update
    void Start()
    {
        alpha = 1;
        fadeOut = false;
        nightBackGroundImage.color = new Color(1, 1, 1, alpha);

        //1���� ���̵�ƿ�
        StartCoroutine("StartFadeOut");
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            alpha -= (0.3f * Time.deltaTime);
            nightBackGroundImage.color = new Color(1, 1, 1, alpha);
        }
    }

    /// <summary>
    /// ���̵� �ƿ� ���� : ������
    /// </summary>
    IEnumerator StartFadeOut()
    {
        yield return new WaitForSeconds(1.0f);
        fadeOut = true;
    }

}
