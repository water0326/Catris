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
    bool IsEnteredLoading = false;

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

        print("Scene is changing to " + EnumToString(scene));
        StartCoroutine(LoadScene(scene));
        
    }

    IEnumerator LoadScene(Scenes scene) {

        IsFading = true;
        AsyncOperation op = SceneManager.LoadSceneAsync(EnumToString(scene));
        op.allowSceneActivation = false;

        StartCoroutine(Fade(true));

        while(!op.isDone) {

            yield return null;

            if(op.progress >= 0.9f && IsEnteredLoading) {
                op.allowSceneActivation = true;
                print("Entered to " + EnumToString(scene) + " Scene.");
                yield break;
            }
        }
        StartCoroutine(Fade(false));
    }

    IEnumerator Fade(bool isFadeIn) {

        float timer = 0f;

        while(timer <= FadeTime) {

            yield return null;

            timer += Time.unscaledDeltaTime;

            if(isFadeIn) {
                CurrentFadeValue = timer / FadeTime;
                IsEnteredLoading = false;
            }
            else {
                CurrentFadeValue = 1 - timer / FadeTime;
            }
            print(CurrentFadeValue);
        }
        if(isFadeIn) {
            SceneManager.LoadScene(EnumToString(Scenes.Loading));
            print("Entered to Loading Scene.");
            IsEnteredLoading = true;
        }
        else {
            IsFading = false;
        }
    }

    private void Update() {
        _currentScene.OnUpdate();
    }
}
