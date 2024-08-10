using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StaticData
{
    public static Vector2[] resolutions = new Vector2[2] {
        new Vector2(1920, 1080),
        new Vector2(2560, 1440)
    };
    public int resolutionX = 1920;
    public int resolutionY = 1080;
    public bool isFullScreen = false;
    public int BGMVolume = 50;
    public int EffectVolume = 50;
}
