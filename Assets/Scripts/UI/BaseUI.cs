using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
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
    }
}
