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
