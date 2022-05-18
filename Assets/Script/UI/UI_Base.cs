using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    //Value값인 오브젝트 배열은 Key값인 Type컴포넌트를 가지고있다
    Dictionary<Type, UnityEngine.Object[]> objectDictionary = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// enum타입안의 이름을 가진 UI를 Dictionary에 저장 : 김현진
    /// </summary>
    /// <typeparam name="T">Dictionary에 저장할 key 타입</typeparam>
    /// <param name="type">enum타입</param>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //UI이름 가져와서 저장
        string[] names = Enum.GetNames(type);

        //objectDictionary갱신
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        objectDictionary.Add(typeof(T), objects);

        //object배열 갱신
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);    
        }
    }

    /// <summary>
    /// 타입 종류와 인덱스로 Dictionary에서 해당 인덱스에 위치한 오브젝트를 해당 타입 형태로 반환 : 김현진
    /// </summary>
    /// <typeparam name="T">Dictionary에서 탐색할 key값</typeparam>
    /// <param name="idx">Dictionary의 Value값인 object배열에서 탐색할 인덱스</param>
    /// <returns></returns>
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        //Dictionary의 값을 Key로 탐색하여 가져온다, 예외처리 포함
        if (objectDictionary.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    //보통 UnityEngin.Object중에서도 아래 존재하는 타입을 사용하기 때문에
    //메서드를 추가로 만들어 준다
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    /// <summary>
    /// 이벤트 핸들러에 동작시킬 action 추가 : 김현진
    /// </summary>
    /// <param name="go">UI_EventHandler 컴포넌트를 가져오거나 추가할 게임 오브젝트</param>
    /// <param name="action">동작시킬 action</param>
    /// <param name="type">발생한 이벤트 타입</param>
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponenet<UI_EventHandler>(go);

        switch (type)
        {
            //예외 처리를 위해 이미 존재할 수도 있는 action을 먼저 제거해 준 다음 추가
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
        }
    }

    //오버로딩 -> Action<PointerEventData,int>, 인덱스를 통한 버튼 식별이 필요할 경우 사용
    public static void AddUIEvent(GameObject go, int idx, Action<PointerEventData,int> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponenet<UI_EventHandler>(go);
        evt.idx = idx;

        switch (type)
        {
            //예외 처리를 위해 이미 존재할 수도 있는 action을 먼저 제거해 준 다음 추가
            case Define.UIEvent.Click:
                evt.OnClickHandler_int -= action;
                evt.OnClickHandler_int += action;
                break;
        }
    }
}
