using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetDebuffMng : MonoBehaviour
{

    float debuffFlowTime = 0.0f;

    Coroutine runningCoroutine = null;

    /// <summary>
    /// ����� ����
    /// </summary>
    /// <param name="debuffIdx"></param>
    /// <param name="debuffs"></param>
    /// <param name="time"></param>
    public void ShowDebuff(int debuffIdx, Dictionary<Actor.debuff, Debuff> debuffs, float time)
    {
        //Debug.Log("-----------------------------------------SetDebuff "+i++);        

        // ����� �ؽ�Ʈ ��������
        TextMeshProUGUI debuffText = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        // ����� ������ ������Ʈ ��������
        Transform GoTransform = gameObject.transform.GetChild(0).GetChild(0);
        Debug.Log(GoTransform.name);

        Image image = GoTransform.GetComponent<Image>();
        
        // ������� ����ð��� ī��Ʈ �� ���� �ʱ�ȭ
        debuffFlowTime = 0.0f;

        int stack = debuffs[(Actor.debuff)debuffIdx].stack;

        // ����� ��ø��
        if (stack >= 2)
        {
            Debug.Log(stack + "��ø");

            // ������ ����Ǵ� �ڷ�ƾ ����
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }

            // ��ø ������ ȭ�鿡 ǥ��
            debuffText.text = "X" + stack.ToString();
        }
        else
        {
            // ����� ��ø �ؽ�Ʈ�� �������� ǥ��
            debuffText.text = " ";
        }

        // ����� UI Ȱ��ȭ
        gameObject.SetActive(true);

        // �ڷ�ƾ ����
        runningCoroutine = StartCoroutine(DebuffCoroutine(image, time, debuffIdx));
    }

    /// <summary>
    /// ����� UI ��� ���� �ڷ�ƾ
    /// </summary>
    /// <param name="image">����� �������� ǥ���� �̹���</param>
    /// <param name="time">����� ���ӽð�</param>
    /// <param name="debuffIdx">����� �ε���</param>
    /// <returns></returns>
    IEnumerator DebuffCoroutine(Image image, float time, int debuffIdx)
    {
        while (true)
        {
            // ���ӽð��� �ٵ��� �� or �г��� ��Ȱ��ȭ ������ ��
            if (debuffFlowTime >= time || gameObject.activeSelf == false)
            {
                // �ڷ�ƾ ����
                StopCoroutine(runningCoroutine);

                // ����� �г� ��Ȱ��ȭ
                gameObject.SetActive(false);
            }
            // ����� ����ð� ī��Ʈ
            debuffFlowTime += Time.deltaTime;

            // ����� ����ð��� ������ UI�� ǥ��
            image.fillAmount = (debuffFlowTime / time);

            yield return new WaitForSeconds(Time.deltaTime);

        }
    }

}
