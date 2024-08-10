using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbySectionUI : FixedUI
{
    [SerializeField]
    GameObject[] slots;

    GameObject currentSlot;

    [SerializeField]
    TMP_Text BGMText;
    [SerializeField]
    TMP_Text effectText;
    [SerializeField]
    TMP_Text resolutionText;
    [SerializeField]
    TMP_Text fullScreenText;

    Vector2 currentSelectedResolution;
    bool currentFullScreenOption;

    public void SetSlot(int index) {
        if(index < 0 || index >= slots.Length) return;
        if(currentSlot != null) {
            currentSlot.SetActive(false);
        }
        currentSlot = slots[index];
        currentSlot.SetActive(true);

        StaticDataManager.Instance.SaveData();
    }

    public override void OnAwake()
    {
        for(int i= 0 ; i < slots.Length ; i++) {
            slots[i].SetActive(false);
        }
        SetSlot(0);
        currentSelectedResolution = new Vector2(StaticDataManager.Instance.data.resolutionX, StaticDataManager.Instance.data.resolutionY);
        currentFullScreenOption = StaticDataManager.Instance.data.isFullScreen;
        resolutionText.text = currentSelectedResolution.x.ToString() + " X " + currentSelectedResolution.y.ToString();
        fullScreenText.text = currentFullScreenOption.ToString();
        BGMText.text = StaticDataManager.Instance.data.BGMVolume.ToString() + "%";
        effectText.text = StaticDataManager.Instance.data.EffectVolume.ToString() + "%";
    }

    public void ChangeBGMVolume(int value) {
        StaticDataManager.Instance.data.BGMVolume = Mathf.Clamp(StaticDataManager.Instance.data.BGMVolume + value, 0, 100);
        BGMText.text = StaticDataManager.Instance.data.BGMVolume.ToString() + "%";
    }

    public void ChangeEffectVolume(int value) {
        StaticDataManager.Instance.data.EffectVolume = Mathf.Clamp(StaticDataManager.Instance.data.EffectVolume + value, 0, 100);
        effectText.text = StaticDataManager.Instance.data.EffectVolume.ToString() + "%";
    }

    public void ChangeResolution(bool isNext) {
        int idx = GetCurrentResolutionIdx((int)currentSelectedResolution.x, (int)currentSelectedResolution.y);
        if(idx == -1) return;
        currentSelectedResolution = StaticData.resolutions[(idx + (isNext ? 1 : -1) + StaticData.resolutions.Length) % StaticData.resolutions.Length];
        resolutionText.text = currentSelectedResolution.x.ToString() + " X " + currentSelectedResolution.y.ToString();
        
    }

    public void ChangeFullScreenOption() {
        currentFullScreenOption = !currentFullScreenOption;
        fullScreenText.text = currentFullScreenOption.ToString();
    }

    public void ApplyResolution() {
        StaticDataManager.Instance.data.resolutionX = (int)currentSelectedResolution.x;
        StaticDataManager.Instance.data.resolutionY = (int)currentSelectedResolution.y;
        StaticDataManager.Instance.data.isFullScreen = currentFullScreenOption;
        Screen.SetResolution((int)currentSelectedResolution.x, (int)currentSelectedResolution.y, currentFullScreenOption);
    }

    int GetCurrentResolutionIdx(int x, int y) {
        Vector2 value = new Vector2(x, y);
        for(int i = 0 ; i < StaticData.resolutions.Length ; i++) {
            if(StaticData.resolutions[i] == value) {
                return i;
            }
        }
        return -1;
    }

    public void QuitGame() {
        Application.Quit();
    }
}