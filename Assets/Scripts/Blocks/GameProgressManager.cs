using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{

    [ReadOnly]
    public bool IsPlaying;

    [SerializeField]
    PlayerInputManager playerInputManager;
    [SerializeField]
    PlayerBlockManager playerBlockManager;

    private void Start() {
        StartGame();
    }

    void ResetMap() {
        BlockGrid.Instance.Reset();
        playerBlockManager.Reset();
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
        PlayerAction();
        ObstacleBlockSet();
    }

    void PlayerAction() {
        Vector2 inputDirection = playerInputManager.GetCurrentActionDataAsVector();
        
    }
    void ObstacleBlockSet() {

    }

}
