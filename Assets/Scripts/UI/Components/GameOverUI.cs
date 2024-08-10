using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : SwitchableUI
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    TMP_Text scoreText;
    [SerializeField]
    TMP_Text levelText;
    public override void OnAwake()
    {
        
    }
    public override void UIOn()
    {
        scoreText.text = "Score : " + ScoreManager.instance.Score;
        levelText.text = "Max Level : " + LevelManager.instance.Level;
        parent.SetActive(true);
    }
    public override void UIOff()
    {
        parent.SetActive(false);
    }

    public void ReturnToLobby() {
        GameSceneManager.Instance.ChangeScene(Scenes.Lobby);
    }
}
