using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    List<BaseUI> UIList = new List<BaseUI>();
    Stack<BaseUI> switchableUIStack = new Stack<BaseUI>();

    const int FIXED_UI_OREDER_IN_LAYER = 10;
    const int SWITCHABLE_UI_OREDER_IN_LAYER = 50;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        
        GetAllUIInScene();
        PushActiveUI();

    }

    void GetAllUIInScene() {
        UIList = new List<BaseUI>(GameObject.FindObjectsOfType<BaseUI>());
    }
    void PushActiveUI() {

        switchableUIStack = new Stack<BaseUI>();

        for(int i = 0; i < UIList.Count ; i++) {
            if(UIList[i].gameObject.GetComponent<FixedUI>() != null) {
                UIList[i].gameObject.GetComponent<Canvas>().sortingOrder = FIXED_UI_OREDER_IN_LAYER;
            }
        }
    }
    public BaseUI GetUIById(int id) {
        for(int i = 0 ; i < UIList.Count ; i++) {
            if(UIList[i].ID == id) {
                return UIList[i];
            }
        }
        return null;
    }
    
    public void EnableUI(int id) {

        BaseUI baseui = GetUIById(id);
        if(baseui == null) {
            Debug.LogWarning("There isn't id : " + id + " BaseUI object");
            return;
        }
        
        SwitchableUI s_ui = baseui.GetComponent<SwitchableUI>();

        if(s_ui == null) {
            Debug.LogWarning("There isn't id : " + id + " SwitchableUI object");
            return;
        }

        if(s_ui.IsEnabled) {
            Debug.LogWarning("id : " + id + " SwitchableUI object is already enabled");
            return;
        }

        baseui.gameObject.GetComponent<Canvas>().sortingOrder = SWITCHABLE_UI_OREDER_IN_LAYER + switchableUIStack.Count;
        switchableUIStack.Push(baseui);
        s_ui.ToggleActive(true);

    }
    public void DisableUI() {

        SwitchableUI s_ui = switchableUIStack.Pop().GetComponent<SwitchableUI>();
        s_ui.ToggleActive(false);

    }

}
