using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_Controller : UI_Base
{
    // Start is called before the first frame update
    void Start()
    {
        //UI바인딩
        BindingUI();
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩
    /// </summary>
    protected virtual void BindingUI()
    {

    }

}
