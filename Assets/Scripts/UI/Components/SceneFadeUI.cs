using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeUI : SwitchableUI
{

    const int SCREEN_FADE_UI_ORDER_IN_LAYER = 100;

    [SerializeField]
    GameObject parentObject;
    [SerializeField]
    Image backgroundImage;

    Canvas canvas;

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
    private void Update() {
        if(canvas.worldCamera == null) {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    private void Awake() {
        canvas = GetComponent<Canvas>();
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = SCREEN_FADE_UI_ORDER_IN_LAYER;
    }
}
