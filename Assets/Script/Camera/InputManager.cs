using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

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
            Debug.Log("터치");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                Debug.Log(hit.transform.parent.gameObject.name);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("터치업");
        }
    }
}
