using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : BaseScene
{
    public override void OnSceneStarted()
    {
        sceneType = Scenes.InGame;
    }
    public override void OnStart()
    {
        SoundManager.Instance.Play("BGM2");
    }
    public override void OnUpdate()
    {
        
    }
    public override void OnSceneEnded()
    {
        
    }
}
