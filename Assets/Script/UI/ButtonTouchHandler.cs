using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTouchHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ScrollRect scrollrect;

    private void Awake()
    {
        scrollrect = transform.parent.parent.parent.parent.GetComponent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        scrollrect.OnBeginDrag(e);
    }
    public void OnDrag(PointerEventData e)
    {
        scrollrect.OnDrag(e);
    }
    public void OnEndDrag(PointerEventData e)
    {
        scrollrect.OnEndDrag(e);
    }
}