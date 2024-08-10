using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    DataUI dataUI;
    
    public static ScoreManager instance;
    private int _score;
    public int Score {
        get { return _score; }
    }

    private int _combo;
    public int Combo {
        get { return _combo; }
    }
    
    [SerializeField]
    int comboScore = 200;
    [SerializeField]
    int scorePerLine = 1000;

    private void Awake() {
        instance = this;
    }

    public void ResetScore() {
        _score = 0;
        _combo = 0;
        dataUI.DataSet();
    }

    public void ChangeScore(int multipleCount) {
        int value = GetComboScore() * multipleCount;
        _score += value;
        dataUI.DataSet();
    }

    public void ResetCombo() {
        _combo = 0;
    }
    public void ComboUp() {
        _combo += 1;
    }

    public int GetComboScore() {
        return scorePerLine + (_combo - 1) * comboScore;
    }

}
