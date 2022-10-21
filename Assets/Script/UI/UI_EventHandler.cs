using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    //�ε���
    public int idx = 0;

    //�̺�Ʈ �ڵ鷯
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;

    public Action<PointerEventData,int> OnClickHandler_int = null;

    //Ŭ�� �̺�Ʈ �߻��� ȣ��
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
        if (OnClickHandler_int != null)
            OnClickHandler_int.Invoke(eventData, idx);

        //Ŭ�� ���� ���
        if (SoundEffectManager.Instance.buttonClickAudioClip)
            SoundEffectManager.Instance.ChangeEffectAudioClip(SoundEffectManager.Instance.buttonClickAudioClip);
    }

    //�巡�� �̺�Ʈ �߻��� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }
    }

}
