using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;    

    private RectTransform rectTransform;    

    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    private Vector2 inputVector;    
    private bool isInput;

    public Player player;

    public CharacterController characterController;

    Transform joystick;    // 조이스틱 UI
    Transform Lever;    // 레버 UI

    private void Awake()    
    {
        rectTransform = GetComponent<RectTransform>();

        joystick = rectTransform.GetChild(0);
        Lever = rectTransform.GetChild(1);

    }

    void Update()
    {
        if (isInput)
        {
            InputControlVector();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        CalcDistance();
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir
            : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        inputVector = clampedDir / leverRange;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        player.UpdateMove(Vector3.zero);
    }

    private void InputControlVector()
    {
        player.UpdateMove(inputVector);
    }

    void CalcDistance()
    {
        Vector3 inputPos;

        if (Application.platform == RuntimePlatform.Android)
            inputPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0);
        else
            inputPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        // 조이스틱을 움직인 거리
        float dist = Vector3.Distance(inputPos, joystick.transform.position);

        // 조이스틱 이동 방향
        Vector3 dir = (inputPos - joystick.transform.position).normalized;

        player.distance = dist;
        player.direction = -dir;
    }
}
