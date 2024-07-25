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
        StartCoroutine(Test());
    }
    public override void OnUpdate()
    {
        
    }
    public override void OnSceneEnded()
    {
        
    }

    IEnumerator Test() {
        yield return new WaitForSeconds(3f);
        GameSceneManager.Instance.ChangeScene(Scenes.Lobby);
    }
}
