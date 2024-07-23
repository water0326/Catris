using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    [SerializeField]
    private static int id;
    public int ID {
        get { return id; }
    }

    public abstract void OnStart();
}
