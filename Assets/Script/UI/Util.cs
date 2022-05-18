using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    //Util -> 유용한 이득을 얻기위해 구성하는 클래스

    /// <summary>
    /// 컴포넌트종류와 게임 오브젝트를 받아 해당오브젝트에 컴포넌트가 존재하지 않는다면 추가한 후
    /// 게임 오브젝트에 존재하는 해당 컴포넌트를 가져온다 : 김현진
    /// </summary>
    /// <typeparam name="T">받아올 컴포넌트의 종류</typeparam>
    /// <param name="go">컴포넌트가 존재하는지 확인하고 추가할 게임 오브젝트</param>
    /// <returns>go게임 오브젝트에 존재하는 T컴포넌트</returns>
    public static T GetOrAddComponenet<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        //T컴포넌트가 존재하지 않을경우 추가
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    /// <summary>
    /// 게임 오브젝트와 이름 문자열을 받아 게임 오브젝트의 직속자식 또는 손자 오브젝트까지 탐색하여
    /// 해당 문자열과 일치하는 자식 게임 오브젝트를 반환 : 김현진
    /// </summary>
    /// <param name="go">탐색할 자식 오브젝트들의 부모 게임 오브젝트</param>
    /// <param name="name">찾을 자식 오브젝트의 이름</param>
    /// <param name="recursive">true면 손자 오브젝트까지 탐색, false면 직속 자식만 탐색</param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    /// <summary>
    /// 게임 오브젝트와 이름 문자열, 컴포넌트를 받아 게임 오브젝트의 직속자식 또는 손자 오브젝트까지 탐색하여
    /// 해당 문자열과 일치하는 자식오브젝트의 컴포넌트를 반환 : 김현진
    /// </summary>
    /// <typeparam name="T">찾은 자식 오브젝트에 존재 유무를 검사하여 추가할 컴포넌트</typeparam>
    /// <param name="go">탐색할 자식 오브젝트들의 부모 게임 오브젝트</param>
    /// <param name="name">찾을 자식 오브젝트의 이름</param>
    /// <param name="recursive">true면 손자 오브젝트까지 탐색, false면 직속 자식만 탐색</param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        //예외처리
        if (go == null)
            return null;

        //오브젝트의 직속 자식 오브젝트만 탐색
        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);

                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        //오브젝트의 손자 오브젝트까지 전부 탐색
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
