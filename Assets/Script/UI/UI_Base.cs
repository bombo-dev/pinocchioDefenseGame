using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    //Value���� ������Ʈ �迭�� Key���� Type������Ʈ�� �������ִ�
    Dictionary<Type, UnityEngine.Object[]> objectDictionary = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// enumŸ�Ծ��� �̸��� ���� UI�� Dictionary�� ���� : ������
    /// </summary>
    /// <typeparam name="T">Dictionary�� ������ key Ÿ��</typeparam>
    /// <param name="type">enumŸ��</param>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //UI�̸� �����ͼ� ����
        string[] names = Enum.GetNames(type);

        //objectDictionary����
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        objectDictionary.Add(typeof(T), objects);

        //object�迭 ����
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);    
        }
    }

    /// <summary>
    /// Ÿ�� ������ �ε����� Dictionary���� �ش� �ε����� ��ġ�� ������Ʈ�� �ش� Ÿ�� ���·� ��ȯ : ������
    /// </summary>
    /// <typeparam name="T">Dictionary���� Ž���� key��</typeparam>
    /// <param name="idx">Dictionary�� Value���� object�迭���� Ž���� �ε���</param>
    /// <returns></returns>
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        //Dictionary�� ���� Key�� Ž���Ͽ� �����´�, ����ó�� ����
        if (objectDictionary.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    //���� UnityEngin.Object�߿����� �Ʒ� �����ϴ� Ÿ���� ����ϱ� ������
    //�޼��带 �߰��� ����� �ش�
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    /// <summary>
    /// �̺�Ʈ �ڵ鷯�� ���۽�ų action �߰� : ������
    /// </summary>
    /// <param name="go">UI_EventHandler ������Ʈ�� �������ų� �߰��� ���� ������Ʈ</param>
    /// <param name="action">���۽�ų action</param>
    /// <param name="type">�߻��� �̺�Ʈ Ÿ��</param>
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponenet<UI_EventHandler>(go);

        switch (type)
        {
            //���� ó���� ���� �̹� ������ ���� �ִ� action�� ���� ������ �� ���� �߰�
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
        }
    }

    //�����ε� -> Action<PointerEventData,int>, �ε����� ���� ��ư �ĺ��� �ʿ��� ��� ���
    public static void AddUIEvent(GameObject go, int idx, Action<PointerEventData,int> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponenet<UI_EventHandler>(go);
        evt.idx = idx;

        switch (type)
        {
            //���� ó���� ���� �̹� ������ ���� �ִ� action�� ���� ������ �� ���� �߰�
            case Define.UIEvent.Click:
                evt.OnClickHandler_int -= action;
                evt.OnClickHandler_int += action;
                break;
        }
    }
}
