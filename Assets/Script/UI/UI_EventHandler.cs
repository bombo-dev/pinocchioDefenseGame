using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    //인덱스
    public int idx = 0;

    //이벤트 핸들러
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;

    public Action<PointerEventData,int> OnClickHandler_int = null;

    //클릭 이벤트 발생시 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
        if (OnClickHandler_int != null)
            OnClickHandler_int.Invoke(eventData, idx);

        //클릭 사운드 재생
        if (SoundEffectManager.Instance.buttonClickAudioClip)
            SoundEffectManager.Instance.ChangeEffectAudioClip(SoundEffectManager.Instance.buttonClickAudioClip);
    }

    //드래그 이벤트 발생시 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }
    }

}
