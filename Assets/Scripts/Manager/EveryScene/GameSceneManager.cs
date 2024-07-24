using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes {
    None = -1,
    Maker = 0,
    Lobby = 1,
    InGame = 2,
    Loading = 100
}

public class GameSceneManager : Singleton<GameSceneManager>
{

    [SerializeField]
    float FadeTime = 1f;
    float CurrentFadeValue = 0f;
    bool IsFading = false;

    [SerializeField]
    SceneFadeUI sceneFadeUI;

    public static Scenes CurrentSceneType {
        get {
            return _currentSceneType;
        }
    }
    private static Scenes _currentSceneType = Scenes.None;

    public static BaseScene CurrentScene {
        get {
            return _currentScene;
        }
    }
    private static BaseScene _currentScene = null;

    public static string Name {
        get {
            return EnumToString(_currentSceneType);
        }
    }

    
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        _currentSceneType = StringToEnum(scene.name);
        _currentScene = FindObjectOfType<BaseScene>();
        if(_currentScene == null) {
            Debug.LogError(scene.name + "'s manager object is not found. please add the manager of the scene.");
        }
        else {
            print(scene.name + " scene was loaded.");
        }

        GameManager.Instance.Resume();

        _currentScene.OnSceneStarted();
        _currentScene.OnStart();
    }

    static Scenes StringToEnum(string sceneName) {
        return (Scenes)Enum.Parse(typeof(Scenes), sceneName);
    }
    static string EnumToString(Scenes scene) {
        return scene.ToString();
    }

    public void ChangeScene(Scenes scene) {
        
        if(CurrentSceneType == scene) return;
        _currentScene.OnSceneEnded();

        GameManager.Instance.Pause();

        print("Scene is changing to " + EnumToString(scene));
        StartCoroutine(LoadScene(scene));
        
    }

    IEnumerator LoadScene(Scenes scene) {

        if(scene == Scenes.Loading) yield break;

        AsyncOperation loading_op = SceneManager.LoadSceneAsync(EnumToString(Scenes.Loading));
        loading_op.allowSceneActivation = false;
        AsyncOperation op = SceneManager.LoadSceneAsync(EnumToString(scene));
        op.allowSceneActivation = false;

        Fade(true);

        yield return null;

        yield return new WaitUntil(() => !IsFading && loading_op.progress >= 0.9f);

        loading_op.allowSceneActivation = true;

        yield return new WaitUntil(() => op.progress >= 0.9f);

        op.allowSceneActivation = true;
        
        print("Entered to " + EnumToString(scene) + " Scene.");
        Fade(false);

        yield break;

    }

    void Fade(bool isFadeIn) {
        
        IsFading = true;
        if(isFadeIn) sceneFadeUI.ToggleActive(true);

        StartCoroutine(FadeCoroutine(isFadeIn));

    }

    IEnumerator FadeCoroutine(bool isFadeIn) {

        float timer = 0f;

        yield return null;

        while(timer <= FadeTime) {

            yield return null;

            timer += Time.unscaledDeltaTime;

            if(isFadeIn) {
                CurrentFadeValue = timer / FadeTime;
            }
            else {
                CurrentFadeValue = 1 - timer / FadeTime;
            }
            sceneFadeUI.SetAlpha(CurrentFadeValue);
            
        }
        if(!isFadeIn) {
            sceneFadeUI.ToggleActive(false);
        }
        IsFading = false;
    }

    private void Update() {
        _currentScene.OnUpdate();
    }
}
