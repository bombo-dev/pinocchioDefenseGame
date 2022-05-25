using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMngPanel : UI_Controller
{
    public string filePath;

    enum Texts
    {
        StageText,
        StageNum,
        StageTimer
    }

    protected override void BindingUI()
    {
        Bind<TextMesh>(typeof(Texts));
    }

    /// <summary>
    /// Ÿ�̸� �ؽ�Ʈ UI�� return���ִ� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetTimerText()
    {
        GameObject go = GetTextMeshProUGUI((int)Texts.StageTimer).gameObject;

        if (!go)
            return null;

        return go;
    }
}
