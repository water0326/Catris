using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : BaseScene
{
    public override void OnSceneStarted()
    {
        sceneType = Scenes.Lobby;
    }
    public override void OnStart()
    {
        SoundManager.Instance.Play("BGM");
    }
    public override void OnUpdate()
    {
        
    }
    public override void OnSceneEnded()
    {
        
    }

    public void StartGame() {
        GameSceneManager.Instance.ChangeScene(Scenes.InGame);
    }
}
