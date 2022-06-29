using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [Header("Nest")]
    //���� ������ ���� ������Ʈ
    public GameObject currenstSelectNest;
    [SerializeField]
    List<Renderer> rendererList;

    //������ ���� ���� ������Ʈ
    [SerializeField]
    GameObject nestPointer;

    // Start is called before the first frame update
    void Start()
    {
        if(nestPointer.activeSelf)
            nestPointer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                TouchObject();
        }
        else 
            TouchObject();
    }

    /// <summary>
    /// ��ġ Ȥ�� ���콺 Ŭ���� �̿��� ������Ʈ ���� : ������
    /// </summary>
    void TouchObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("��ġ");

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);

                if (hit.collider != null)
                {
                    //�ͷ��� ��ȯ�ϰų� �ͷ� ������ Ȯ���� ���� ������Ʈ ����
                    SelectNest(hit.transform.gameObject);

                    //��Ÿ� ǥ��
                    if (currenstSelectNest)
                        ShowRange();
                }

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
        if (currenstSelectNest != null)
            OffHightlightObject(currenstSelectNest);

        //������Ʈ ����
        if(hitGo != null)
            currenstSelectNest = hitGo;

        Nest nest = currenstSelectNest.GetComponent<Nest>();

        if (nest != null)
        {
            if(SystemManager.Instance.PanelManager.turretInfoPanel)
                SystemManager.Instance.PanelManager.turretInfoPanel.GetComponent<UI_TurretInfoPanel>().Reset();
        }

        //���� ������ Ȱ��ȭ
        if (!nestPointer.activeSelf)
        { 
            nestPointer.SetActive(true);
        }
        nestPointer.transform.position = currenstSelectNest.transform.position;

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

        Renderer renderer = go.GetComponent<Renderer>();

        if (renderer != null)
        {
            rendererList.Add(go.GetComponent<Renderer>());
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

        Renderer renderer = go.GetComponent<Renderer>();

        if (renderer != null)
        {
            rendererList.Add(go.GetComponent<Renderer>());
            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 0);
        }
       
    }

    //��Ÿ� �����ֱ�
    public void ShowRange()
    {
        //����ó��
        if (!SystemManager.Instance.RangeManager.rangeParents)
        {
            SystemManager.Instance.RangeManager.DisableRange(0);
            return;
        }

        Nest nest = currenstSelectNest.GetComponent<Nest>();

        //����ó��
        if (!nest || !nest.turret)
        {
            SystemManager.Instance.RangeManager.DisableRange(0);
            return;
        }

        Turret turret = nest.turret.GetComponent<Turret>();

        //����ó��
        if (!turret)
        {
            SystemManager.Instance.RangeManager.DisableRange(0);
            return;
        }

        //��Ÿ� ǥ��
        SystemManager.Instance.RangeManager.EnableRange(0, turret.currentRange, nest.transform.position);
    }
}
