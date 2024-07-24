using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    [SerializeField]
    private static int id;
    public int ID {
        get { return id; }
    }

    public abstract void OnStart();

    private void Awake() {
        if(gameObject.GetComponent<Canvas>() == null) {
            Debug.LogError(gameObject.name + " doesn't have a canvas componenet. Please migrate this script to canvas object.");
            return;
        }
    }
}
