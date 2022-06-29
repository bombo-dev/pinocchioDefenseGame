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
    

    private void Awake()    
    {
        rectTransform = GetComponent<RectTransform>();
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
        ControlJoystickLever(eventData);  // 추가
        isInput = true;    // 추가    
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);    // 추가
        
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
    
    

}
