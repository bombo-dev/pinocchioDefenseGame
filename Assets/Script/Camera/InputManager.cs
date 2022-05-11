using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //���� ���õ� ������Ʈ
    [SerializeField]
    GameObject currenstSelectObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TouchObject();
    }

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
                currenstSelectObject = hit.transform.parent.gameObject;

                List<Renderer> rendererList = new List<Renderer>();
                rendererList.Add(currenstSelectObject.GetComponent<Renderer>());
                SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList,2);

                Debug.Log(hit.transform.parent.gameObject.name);
            }

            
        }

        if (Input.GetMouseButtonUp(0))
        {
            //����ó��
            if (!currenstSelectObject)
                return;

            List<Renderer> rendererList = new List<Renderer>();
            rendererList.Add(currenstSelectObject.GetComponent<Renderer>());
            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 0);

            //������Ʈ ���� ����
            currenstSelectObject = null;

            Debug.Log("��ġ��");
        }
    }
}
