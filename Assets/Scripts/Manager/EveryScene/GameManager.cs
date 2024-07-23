using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsPaused {
        get { return _isPaused; }
    }
    private bool _isPaused = false;
    
    public void Resume() {
        Time.timeScale = 1;
        _isPaused = false;
    }
    public void Pause() {
        Time.timeScale = 0;
        _isPaused = true;
    }
}
