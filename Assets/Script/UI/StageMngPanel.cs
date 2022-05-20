using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMngPanel : UI_Controller
{
    float limitTime = 30;    // 제한 시간

    private void Update()
    {
        TimerFlow();
    }

    enum Texts
    {
        StageText,
        StageNum,
        StageTimer
    }

    protected override void BindingUI()
    {
        Bind<Text>(typeof(Texts));
               
    }

    void TimerFlow()
    {                
        if (limitTime >= 0)
        {
            limitTime -= Time.deltaTime;
            Debug.Log("limitTime=" + limitTime);
            GetText(2).text = limitTime.ToString();
        }
        else
        {
            GetText(2).text = 0.ToString();
            Debug.Log("GameOver!");
        }
        
    }
}
