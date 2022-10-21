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
    /// 디버프 설정
    /// </summary>
    /// <param name="debuffIdx"></param>
    /// <param name="debuffs"></param>
    /// <param name="time"></param>
    public void ShowDebuff(int debuffIdx, Dictionary<Actor.debuff, Debuff> debuffs, float time)
    {
        //Debug.Log("-----------------------------------------SetDebuff "+i++);        

        // 디버프 텍스트 가져오기
        TextMeshProUGUI debuffText = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        // 디버프 게이지 오브젝트 가져오기
        Transform GoTransform = gameObject.transform.GetChild(0).GetChild(0);
        Debug.Log(GoTransform.name);

        Image image = GoTransform.GetComponent<Image>();
        
        // 디버프의 경과시간을 카운트 할 변수 초기화
        debuffFlowTime = 0.0f;

        int stack = debuffs[(Actor.debuff)debuffIdx].stack;

        // 디버프 중첩시
        if (stack >= 2)
        {
            Debug.Log(stack + "중첩");

            // 이전에 실행되던 코루틴 종료
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }

            // 중첩 정보를 화면에 표시
            debuffText.text = "X" + stack.ToString();
        }
        else
        {
            // 디버프 중첩 텍스트를 공백으로 표시
            debuffText.text = " ";
        }

        // 디버프 UI 활성화
        gameObject.SetActive(true);

        // 코루틴 시작
        runningCoroutine = StartCoroutine(DebuffCoroutine(image, time, debuffIdx));
    }

    /// <summary>
    /// 디버프 UI 제어를 위한 코루틴
    /// </summary>
    /// <param name="image">디버프 게이지를 표시할 이미지</param>
    /// <param name="time">디버프 지속시간</param>
    /// <param name="debuffIdx">디버프 인덱스</param>
    /// <returns></returns>
    IEnumerator DebuffCoroutine(Image image, float time, int debuffIdx)
    {
        while (true)
        {
            // 지속시간이 다됐을 때 or 패널이 비활성화 상태일 때
            if (debuffFlowTime >= time || gameObject.activeSelf == false)
            {
                // 코루틴 종료
                StopCoroutine(runningCoroutine);

                // 디버프 패널 비활성화
                gameObject.SetActive(false);
            }
            // 디버프 경과시간 카운트
            debuffFlowTime += Time.deltaTime;

            // 디버프 경과시간을 게이지 UI로 표시
            image.fillAmount = (debuffFlowTime / time);

            yield return new WaitForSeconds(Time.deltaTime);

        }
    }

}
