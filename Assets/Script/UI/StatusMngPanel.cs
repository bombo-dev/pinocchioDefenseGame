using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusMngPanel : UI_Controller
{
    const int MAXDEBUFF = 4;    // 최대 디버프 수

    
    enum Images
    {
        Debuff_1_img,
        Debuff_2_img,
        Debuff_3_img,
        Debuff_4_img,
    }

    enum Sliders
    {
        HPBar
    }

    protected override void BindingUI()
    {
        Bind<Slider>(typeof(Sliders));

        base.BindingUI();


    }

}
