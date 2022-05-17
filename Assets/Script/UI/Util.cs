using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    //Util -> ������ �̵��� ������� �����ϴ� Ŭ����

    /// <summary>
    /// ������Ʈ������ ���� ������Ʈ�� �޾� �ش������Ʈ�� ������Ʈ�� �������� �ʴ´ٸ� �߰��� ��
    /// ���� ������Ʈ�� �����ϴ� �ش� ������Ʈ�� �����´� : ������
    /// </summary>
    /// <typeparam name="T">�޾ƿ� ������Ʈ�� ����</typeparam>
    /// <param name="go">������Ʈ�� �����ϴ��� Ȯ���ϰ� �߰��� ���� ������Ʈ</param>
    /// <returns>go���� ������Ʈ�� �����ϴ� T������Ʈ</returns>
    public static T GetOrAddComponenet<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        //T������Ʈ�� �������� ������� �߰�
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    /// <summary>
    /// ���� ������Ʈ�� �̸� ���ڿ��� �޾� ���� ������Ʈ�� �����ڽ� �Ǵ� ���� ������Ʈ���� Ž���Ͽ�
    /// �ش� ���ڿ��� ��ġ�ϴ� �ڽ� ���� ������Ʈ�� ��ȯ : ������
    /// </summary>
    /// <param name="go">Ž���� �ڽ� ������Ʈ���� �θ� ���� ������Ʈ</param>
    /// <param name="name">ã�� �ڽ� ������Ʈ�� �̸�</param>
    /// <param name="recursive">true�� ���� ������Ʈ���� Ž��, false�� ���� �ڽĸ� Ž��</param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    /// <summary>
    /// ���� ������Ʈ�� �̸� ���ڿ�, ������Ʈ�� �޾� ���� ������Ʈ�� �����ڽ� �Ǵ� ���� ������Ʈ���� Ž���Ͽ�
    /// �ش� ���ڿ��� ��ġ�ϴ� �ڽĿ�����Ʈ�� ������Ʈ�� ��ȯ : ������
    /// </summary>
    /// <typeparam name="T">ã�� �ڽ� ������Ʈ�� ���� ������ �˻��Ͽ� �߰��� ������Ʈ</typeparam>
    /// <param name="go">Ž���� �ڽ� ������Ʈ���� �θ� ���� ������Ʈ</param>
    /// <param name="name">ã�� �ڽ� ������Ʈ�� �̸�</param>
    /// <param name="recursive">true�� ���� ������Ʈ���� Ž��, false�� ���� �ڽĸ� Ž��</param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        //����ó��
        if (go == null)
            return null;

        //������Ʈ�� ���� �ڽ� ������Ʈ�� Ž��
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
        //������Ʈ�� ���� ������Ʈ���� ���� Ž��
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
