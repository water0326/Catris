using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Block : MonoBehaviour
{
    [SerializeField]
    Image sprite;

    [SerializeField]
    public Vector2 pos;

    abstract public void Active();

    abstract public void Deactive();

}
