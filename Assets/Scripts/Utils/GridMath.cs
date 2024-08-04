using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridMath
{
    public static bool IsNumberInRange(float number, float min, float max) {
        return number >= min && number <= max;
    }

    public static Vector2 WorldToCanvasPosition(Canvas canvas, Vector2 pos)
    {

        RectTransform CanvasRect=canvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(pos);
        Vector2 WorldObject_ScreenPosition=new Vector2(
        ((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
        ((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));
        
        return WorldObject_ScreenPosition;
    }

    
}
