using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Title_Fade : MonoBehaviour
{
    public Image fade;
    /// <summary>
    /// ���̵�ƿ� �ڷ�ƾ : ������
    /// </summary>
    IEnumerator FadeOut()
    {
        for (float ff = 1.0f; ff >= 0.0f;)
        {
            ff -= 0.05f;
            fade.color = new Color(1, 1, 1, ff);
            yield return new WaitForSeconds(0.05f);
        }

        fade.gameObject.SetActive(false);
    }

    /// <summary>
    /// �г� ���̵� �ƿ� �� �κ�� : ������
    /// </summary>
    public void OnClickStart()
    {
        EventSystem.current.currentSelectedGameObject.SetActive(false);
        StartCoroutine("FadeOut");
    }
}
