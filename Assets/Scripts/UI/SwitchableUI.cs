using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwitchableUI : BaseUI, ISwitchable
{
    public void UIToggle(bool isActive) {
        if(isActive) {
            UIOn();
        }
        else {
            UIOff();
        }
    }
    public abstract void UIOn();
    public abstract void UIOff();

    
}
