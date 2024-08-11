using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData) {
        SoundManager.Instance.Play("Hover");
    }
    public void OnPointerDown(PointerEventData eventData) {
        SoundManager.Instance.Play("Click");
    }
}
