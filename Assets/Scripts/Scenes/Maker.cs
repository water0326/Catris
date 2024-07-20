using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maker : BaseScene
{
    public override void OnSceneStarted()
    {
        sceneType = Scenes.Maker;
    }
    public override void OnStart()
    {
        GameSceneManager.Instance.ChangeScene(Scenes.Lobby);
    }
    public override void OnUpdate()
    {
        
    }
    public override void OnSceneEnded()
    {
        
    }
}
