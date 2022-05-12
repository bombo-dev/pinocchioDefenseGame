using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //현재 선택된 오브젝트
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
    /// 터치 혹은 마우스 클릭을 이용해 오브젝트 선택
    /// </summary>
    void TouchObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("터치");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                //오브젝트 선택
                currenstSelectObject = hit.transform.gameObject;

                rendererList.Clear();
                rendererList.Add(currenstSelectObject.GetComponent<Renderer>());
                SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList,2);

            }

            
        }

        if (Input.GetMouseButtonUp(0))
        {
            //예외처리
            if (!currenstSelectObject)
                return;

            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 0);
            rendererList.Clear();

            //오브젝트 선택 해제
            currenstSelectObject = null;

            Debug.Log("터치업");
        }
    }
}
