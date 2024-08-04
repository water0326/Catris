using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{

    [SerializeField]
    BlockGrid blockGrid;
    [ReadOnly]
    public bool IsPlaying;

    private void Start() {
        StartGame();
    }

    void ResetMap() {
        blockGrid.Reset();
    }

    void StartGame() {
        PauseGame();
        ResetMap();
        ResumeGame();
    }

    void PauseGame() {
        IsPlaying = false;
    }

    void ResumeGame() {
        IsPlaying = true;
    }

    void GameEnd() {
        PauseGame();
    }

    private void Update() {
        PlayerInput();
        PlayerBlockSet();
        ObstacleBlockSet();
    }

    void PlayerInput() {

    }
    void PlayerBlockSet() {

    }
    void ObstacleBlockSet() {

    }

}
