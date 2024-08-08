using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : Block
{

    [SerializeField]
    Color color1;
    [SerializeField]
    Color color2;

    public override void Active()
    {
        if(x % 2 == 0) {
            if(y % 2 == 0) {
                image.color = color1;
            }
            else {
                image.color = color2;
            }
        }
        else {
            if(y % 2 == 0) {
                image.color = color2;
            }
            else {
                image.color = color1;
            }
        }
    }
    public override void Deactive()
    {
        
    }
    protected override void OnFixedUpdate()
    {
        
    }
}
