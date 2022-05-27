using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColosseumCameraMove : MonoBehaviour
{    
    Transform cameraMove;   // ���� ī�޶��� �θ� ������Ʈ
    float zoomValue = 0;         // �� ���� �� 

    [Header ("window")]

    [SerializeField]
    float moveSpeed = 2.8f;    // ī�޶� �̵� �ӵ�

    [SerializeField]
    float zoomSpeed = 20.0f;  // ī�޶� �� �ӵ�

    float moveX, moveZ;        // �̵���

    [Header ("android")]

    [SerializeField]
    float touchSpeed = 0.01f;

    Vector2 curPos, prePos;
    Vector3 movePos;

    float preDistance, curDistance, moveDistance;

    Vector3 fixedPos;

    bool isMouseButtonOver;

    bool isMapClick;

    // Start is called before the first frame update
    void Start()
    {        
        cameraMove = Camera.main.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        isCheckPointerOverGameObject();
        UpdateInputAtAnd();
        UpdateInputAtWin();
    }

    /// <summary>
    /// ������ �� �̻����� ����, �ܾƿ����� �ʵ��� ���� : ������
    /// </summary>
    bool ControllZoom()
    {
        if (Camera.main.fieldOfView < 20 || Camera.main.fieldOfView > 80)
        {
            if (Camera.main.fieldOfView < 20)
                zoomValue = 20;
            else
                zoomValue = 80;
            return true;
        }
        return false;
    }

    #region Android

    /// <summary>
    /// �ȵ���̵忡�� ī�޶� ������ ������Ʈ : ������
    /// </summary>
    void UpdateInputAtAnd()
    {
        // ȭ�鿡 ���˵� �հ����� ������ 1���̸�
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
            MoveAndCam();
        else if (Input.touchCount == 2)
            ZoomAndCam();
        else
            return;
    }

    /// <summary>
    /// ī�޶� �̵� : ������
    /// </summary>
    void MoveAndCam()
    {
        // ��ġ ���� ����
        Touch touch = Input.GetTouch(0);    

        // ȭ���� ��ġ�ϴ� ���� �ش� ��ġ �� ����
        if (touch.phase == TouchPhase.Began)
        {    
            prePos = touch.position;
        }
        // ��ġ�� ���·� �巡���ϴ� ���� ȭ�� �̵�
        else if (touch.phase == TouchPhase.Moved)   
        {
            curPos = touch.position;
            // �̵��� ���� ���͸� ����
            Vector3 dir = (prePos - curPos).normalized;            
            movePos = dir * Time.deltaTime * touchSpeed;

            // ī�޶� �̵�
            cameraMove.Translate(movePos);
        }
    }

    /// <summary>
    /// ī�޶� ����, �ܾƿ� : ������
    /// </summary>
    void ZoomAndCam()
    {
        Touch fstTouch = Input.GetTouch(0); // ù ��° ��ġ ����
        Touch scdTouch = Input.GetTouch(1); // �� ��° ��ġ ����
        
        // ���� ȭ�鿡 ���˵� �� �հ��� ������ �Ÿ�
        curDistance = (fstTouch.position - scdTouch.position).magnitude;

        // ȭ�鿡 ó�� �������� ���� �� �հ��� ������ �Ÿ�
        preDistance = ((fstTouch.position - fstTouch.deltaPosition) - (scdTouch.position- scdTouch.deltaPosition)).magnitude;
        
        // ȭ���� �� ��ų ũ�� ���ϱ�
        moveDistance = curDistance - preDistance;

        //  ���� �� �̻� ����, �ܾƿ� ���� ���ϵ��� ����
        if (ControllZoom())
            Camera.main.fieldOfView = zoomValue;

        //  ī�޶� ����, �ܾƿ�

            Camera.main.fieldOfView -= 0.3f * moveDistance;
    }
    

    #endregion

    #region Window

    /// <summary>
    /// ī�޶� ������ ������Ʈ : ������
    /// </summary>
    void UpdateInputAtWin()
    {
        MoveWinCam();

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            ZoomWinCam();
    }

    /// <summary>
    /// ī�޶� �̵� : ������
    /// </summary>
    void MoveWinCam()
    {
        // ���콺 ���� ��ư �巡�� ó��
        if (Input.GetMouseButton(0))
        {
            
            if (!isMouseButtonOver && !EventSystem.current.IsPointerOverGameObject())
            {
                isMapClick = true;

                // ���콺 ������ ���ϱ�
                moveX = Input.GetAxisRaw("Mouse X") * moveSpeed;
                moveZ = Input.GetAxisRaw("Mouse Y") * moveSpeed;

                // ī�޶� �̵�
                cameraMove.Translate(-moveX, 0, -moveZ);
            }
            else if(isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                // ���콺 ������ ���ϱ�
                moveX = Input.GetAxisRaw("Mouse X") * moveSpeed;
                moveZ = Input.GetAxisRaw("Mouse Y") * moveSpeed;

                // ī�޶� �̵�
                cameraMove.Translate(-moveX, 0, -moveZ);
            }
            else if(!isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                isMouseButtonOver = true;
            }
        }
    }

    void isCheckPointerOverGameObject()
    {
        if (!Input.GetMouseButton(0))
        {
            isMouseButtonOver = false;
            isMapClick = false;
        }

    }

    /// <summary>
    /// ī�޶� ����, �ܾƿ� : ������
    /// </summary>
    void ZoomWinCam()
    {
        //  ���� �� �̻� ����, �ܾƿ� ���� ���ϵ��� ����
        if(ControllZoom())
            Camera.main.fieldOfView = zoomValue;

        // ī�޶� ����
        Camera.main.fieldOfView -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;        
    }


    #endregion
}
