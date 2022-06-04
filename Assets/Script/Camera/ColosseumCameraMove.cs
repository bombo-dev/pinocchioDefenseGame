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

    float preDistance, curDistance, moveDistance;   // ȭ�� ���� ���� ����

    bool isMouseButtonOver;     // ���콺(��ġ)�� UI ���� �ִ� ��� 

    bool isMapClick;    // UI�� �ƴ� ��(���� ȭ��)�� Ŭ���� ���

    // Start is called before the first frame update
    void Start()
    {        
        cameraMove = Camera.main.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateInputAtAnd();
        UpdateInputAtWin();
        UpdateAndFlag();
        UpdateWinFlag();
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
            else
            {
                UpdateInputAtAnd();
                UpdateAndFlag();
            }
        }
        */
        /*else
        {
            UpdateInputAtWin();
            UpdateWinFlag();
        }
        */
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
        if (Input.touchCount == 1)
        {

            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !isMouseButtonOver)
            {
                isMapClick = true;

                MoveAndCam();
            }
            else if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && isMapClick)
            {
                MoveAndCam();
            }
            else if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !isMapClick)
            {
                isMouseButtonOver = true;
            }
            else
                return;

        }
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

    void UpdateAndFlag()
    {
        if (Input.touchCount <= 0)
        {
            isMouseButtonOver = false;
            isMapClick = false;
        }
    }

    #endregion

  
    #region Window

    /// <summary>
    /// ī�޶� ������ ������Ʈ : ������
    /// </summary>
    void UpdateInputAtWin()
    {
        // ���콺 ���� ��ư�� Ŭ���ϴ� ����
        IsWinCamMove();

        // ���콺 ��ũ�� �Է��� ������
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            ZoomWinCam();
    }

    /// <summary>
    /// ī�޶� �̵� ���� �˻� : ������
    /// </summary>
    void IsWinCamMove()
    {
        if (Input.GetMouseButton(0))
        {
            // UI�� ������ ī�޶� ȭ�鿡�� �Է��� ������
            if (!isMouseButtonOver && !EventSystem.current.IsPointerOverGameObject())
            {
                isMapClick = true;

                MoveWinCam();
            }
            // ȭ���� �巡���ϰ� �ִ� ���¿��� Ŀ���� UI ������ ����
            else if (isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                MoveWinCam();
            }
            // ȭ���� �巡���ϰ� ���� ���� ���¿��� UI�� Ŭ���ϸ�
            else if (!isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                isMouseButtonOver = true;
            }
        }
    }

    /// <summary>
    /// ī�޶� �̵� : ������
    /// </summary>
    void MoveWinCam()
    {
        // ���콺 ������ ���ϱ�
        moveX = Input.GetAxisRaw("Mouse X") * moveSpeed;
        moveZ = Input.GetAxisRaw("Mouse Y") * moveSpeed;

        // ī�޶� �̵�
        cameraMove.Translate(-moveX, 0, -moveZ);
    }

    /// <summary>
    /// �Է��� ������ �÷��� ���� �ʱ�ȭ : ������
    /// </summary>
    void UpdateWinFlag()
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
