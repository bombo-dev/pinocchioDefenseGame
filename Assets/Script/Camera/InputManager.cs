using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //���� ���õ� ������Ʈ
    [SerializeField]
    GameObject currenstSelectObject;
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
    /// ��ġ Ȥ�� ���콺 Ŭ���� �̿��� ������Ʈ ����
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
                //������Ʈ ����
                currenstSelectObject = hit.transform.gameObject;

                rendererList.Clear();
                rendererList.Add(currenstSelectObject.GetComponent<Renderer>());
                SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList,2);

            }

            
        }

        if (Input.GetMouseButtonUp(0))
        {
            //����ó��
            if (!currenstSelectObject)
                return;

            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 0);
            rendererList.Clear();

            //������Ʈ ���� ����
            currenstSelectObject = null;

            Debug.Log("��ġ��");
        }
    }
}
