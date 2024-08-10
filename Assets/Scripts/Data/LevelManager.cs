using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField]
    DataUI dataUI;

    public Dictionary<int, int> scoreToLevel = new Dictionary<int, int>() {
        {0, 1},
        {10000, 2},
        {25000, 3},
        {45000, 4},
        {75000, 5},
        {120000, 6},
        {180000, 7},
        {250000, 8},
        {340000, 9},
        {450000, 10}
    };
    public Dictionary<int, float> levelToObstacleCooldown = new Dictionary<int, float>() {
        {1, 7},
        {2, 6},
        {3, 5},
        {4, 4.5f},
        {5, 4},
        {6, 3.5f},
        {7, 3f},
        {8, 2.75f},
        {9, 2.4f},
        {10, 2.2f}
    };

    public int Level {
        get {return _level;}
    }
    private int _level;

    private void Awake() {
        instance = this;
    }

    public void ResetLevel() {
        _level = 1;
        dataUI.DataSet();
    }

    private void Update() {
        int tempLevel = 1;
        foreach(int score in scoreToLevel.Keys) {
            if(ScoreManager.instance.Score >= score) {
                tempLevel = scoreToLevel[score];
            }
            else {
                break;
            }
        }
        _level = tempLevel;
    }

    public float GetObstacleCoolDown() {
        return levelToObstacleCooldown[_level];
    }


}
