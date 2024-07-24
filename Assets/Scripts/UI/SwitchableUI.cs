using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwitchableUI : BaseUI, ISwitchable
{
    [SerializeField]
    bool isenabled = false;
    public bool IsEnabled {
        get {return isenabled;}
    }

    public void ToggleActive(bool isActive) {
        if(isActive) {
            UIOn();
            isenabled = true;
        }
        else {
            UIOff();
            isenabled = false;
        }
    }
    public abstract void UIOn();
    public abstract void UIOff();
    
}
