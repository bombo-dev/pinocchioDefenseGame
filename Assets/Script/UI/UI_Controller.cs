using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UI_Controller : UI_Base
{
    enum Buttons
    {
        TestButton
    }

    enum Texts
    {
        
    }

    enum GameObjects
    {
        
    }

    enum Images
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //UI바인딩
        BindingUI();
        
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩
    /// </summary>
    void BindingUI()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        AddUIEvent(GetButton((int)Buttons.TestButton).gameObject, OnClickTestButton,Define.UIEvent.Click);
    }

    public void OnClickTestButton(PointerEventData data)
    {
        Debug.Log("asd");
    }
}
