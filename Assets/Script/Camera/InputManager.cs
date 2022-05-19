using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Nest")]
    //���� ������ ���� ������Ʈ
    public GameObject currenstSelectNest;
    [SerializeField]
    List<Renderer> rendererList;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        TouchObject();
    }

    /// <summary>
    /// ��ġ Ȥ�� ���콺 Ŭ���� �̿��� ������Ʈ ���� : ������
    /// </summary>
    void TouchObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("��ġ");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                //�ͷ��� ��ȯ�ϰų� �ͷ� ������ Ȯ���� ���� ������Ʈ ����
                SelectNest(hit.transform.gameObject);
            }

        }

        /*
        if (Input.GetMouseButtonUp(0))
        {
            //����ó��
            if (!currenstSelectNest)
                return;

            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 0);
            rendererList.Clear();

            //������Ʈ ���� ����
            currenstSelectNest = null;

            Debug.Log("��ġ��");
        }*/
    }

    /// <summary>
    /// �ͷ��� ��ȯ�� ������ �����Ѵ� : ������
    /// </summary>
    ///<param name="hitObject">������ ���� ������Ʈ</param>
    void SelectNest(GameObject hitGo)
    {
        //������ ������ ������Ʈ ȿ���ʱ�ȭ
        if(currenstSelectNest != null)
            OffHightlightObject(currenstSelectNest);

        //������Ʈ ����
        if(hitGo != null)
            currenstSelectNest = hitGo;

        Nest nest = currenstSelectNest.GetComponent<Nest>();

        if (nest != null)
        {
            //�ͷ��� ��ġ�� Nest�ǰ��
            if (nest.haveTurret)
                SystemManager.Instance.PanelManager.EnablePanel<UI_TurretMngPanel>(0);
        }    

        //������ ������Ʈ ���̶���Ʈ ȿ��
        OnHighlightObject(currenstSelectNest);
    }


    /// <summary>
    /// ���� ������Ʈ�� ���̴� �ܰ����� ���� ���̶���Ʈ ȿ���� �Ѵ� �Լ� : ������
    /// </summary>
    /// <param name="go">���̶���Ʈ ȿ���� ǥ���� ���� ������Ʈ</param>
    void OnHighlightObject(GameObject go)
    {
        rendererList.Clear();

        Renderer renderer = currenstSelectNest.GetComponent<Renderer>();

        if (renderer != null)
        {
            rendererList.Add(currenstSelectNest.GetComponent<Renderer>());
            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 2);
        }
    }

    /// <summary>
    /// ���� ������Ʈ�� ���̴� �ܰ����� ���� ���̶���Ʈ ȿ���� ���� �Լ� : ������
    /// </summary>
    /// <param name="go">���̶���Ʈ ȿ���� ǥ���� ���� ������Ʈ</param>
    void OffHightlightObject(GameObject go)
    {
        rendererList.Clear();

        Renderer renderer = currenstSelectNest.GetComponent<Renderer>();

        if (renderer != null)
        {
            rendererList.Add(currenstSelectNest.GetComponent<Renderer>());
            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 0);
        }
       
    }
}
