using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataUI : FixedUI
{
    [SerializeField]
    TMP_Text score;
    [SerializeField]
    TMP_Text level;
    [SerializeField]
    TMP_Text combo;

    [SerializeField]
    Image nextBlock;
    [SerializeField]
    TMP_Text nextBlockLength;

    [SerializeField]
    PlayerBlockManager playerBlockManager;

    public void DataSet() {
        nextBlock.sprite = playerBlockManager.GetNextBlockHeadSprite();
        nextBlockLength.text = playerBlockManager.GetNextBlockLength().ToString();
        score.text = "Score : " + ScoreManager.instance.Score.ToString();
        level.text = "Level : " + LevelManager.instance.Level.ToString();
        combo.text = "Combo : " + ScoreManager.instance.Combo.ToString();
    }
}
