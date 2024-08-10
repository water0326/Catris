using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    CanvasScaler canvasScaler;
    [SerializeField]
    [ReadOnly]
    Vector2 currentResolution;

    [SerializeField]
    private int id;
    public int ID {
        get { return id; }
    }

    private void Awake() {
        if(gameObject.GetComponent<Canvas>() == null) {
            Debug.LogError(gameObject.name + " doesn't have a canvas componenet. Please migrate this script to canvas object.");
            return;
        }
        canvasScaler = GetComponent<CanvasScaler>();
        currentResolution = new Vector2(StaticDataManager.Instance.data.resolutionX, StaticDataManager.Instance.data.resolutionY);
        canvasScaler.referenceResolution = currentResolution;

        OnAwake();
    }

    private void Update() {
        if(currentResolution.x != StaticDataManager.Instance.data.resolutionX || currentResolution.y != StaticDataManager.Instance.data.resolutionY) {
            currentResolution = new Vector2(StaticDataManager.Instance.data.resolutionX, StaticDataManager.Instance.data.resolutionY);
            canvasScaler.referenceResolution = currentResolution;
        }
    }

    

    public virtual void OnAwake() {}
}
