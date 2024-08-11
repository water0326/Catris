using UnityEngine;
using UnityEngine.UI;

public class ColorSheetUI : FixedUI
{
    [SerializeField]
    Image image;
    [SerializeField]
    float blinkTime;
    float time;
    [SerializeField]
    Color warningColor;

    bool isWarning;
    float coefficient;
    [SerializeField]
    float maxOpacity;

    private void Awake() {
        SetWarning(false);
        coefficient = -4f / (blinkTime*blinkTime) * maxOpacity;
    }

    public void SetWarning(bool onoff) {
        if(isWarning == onoff) return;
        isWarning = onoff;
        if(isWarning) {
            time = 0;
            SoundManager.Instance.Play("Warning");
        }
        else {
            image.color = new Color(0, 0, 0, 0);
        }
    }

    private void Update() {
        if(isWarning) {
            time += Time.deltaTime;
            time = time % blinkTime;
            Color color = warningColor;
            color.a = coefficient * time * (time - blinkTime);
            image.color = color;
        }
    }
}
