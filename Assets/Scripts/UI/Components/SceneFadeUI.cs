using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeUI : SwitchableUI
{

    [SerializeField]
    GameObject parentObject;
    [SerializeField]
    Image backgroundImage;

    public override void UIOn()
    {
        parentObject.SetActive(true);
    }
    public override void UIOff()
    {
        parentObject.SetActive(false);
    }
    public void SetAlpha(float value) {
        Color tempColor = backgroundImage.color;
        tempColor.a = value;
        backgroundImage.color = tempColor;
    }
}
