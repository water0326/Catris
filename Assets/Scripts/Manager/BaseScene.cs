using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{

    public Scenes sceneType;

    public abstract void OnSceneStarted();
    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnSceneEnded();

    
}
