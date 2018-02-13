using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class MouseOverFadeIn : MonoBehaviour, 
        IPointerEnterHandler,
        IPointerExitHandler
{
    public FadeAll fadeAllInstance;

    public void OnPointerEnter(PointerEventData eventData)
    {
        fadeAllInstance.setFadeIn(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        fadeAllInstance.setFadeIn(false);
    }
}
