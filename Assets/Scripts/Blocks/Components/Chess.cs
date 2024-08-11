using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : Block
{

    [SerializeField]
    Color[] colors;

    public void SetColor(int colorNum) {
        image.color = colors[colorNum];
    }

    public override void Active()
    {
    }
    public override void Deactive()
    {
        
    }
    protected override void OnFixedUpdate()
    {
        
    }
}