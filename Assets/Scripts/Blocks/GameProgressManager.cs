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
    [SerializeField]
    BlockStackManager blockStackManager;

    float time;
    [SerializeField]
    float obstacleTime;

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
        if(IsPlaying) {
            PlayerAction();
            ObstacleBlockSet();
            if(blockStackManager.isGameOver) {
                GameEnd();
            }
        }
    }

    void PlayerAction() {
        Vector2 inputDirection = playerInputManager.GetCurrentActionDataAsVector();
        int x = (int)inputDirection.x;
        int y = (int)inputDirection.y;
        
        if(x != 0 || y != 0) {
            
            playerBlockManager.Move((int)inputDirection.x, (int)inputDirection.y);
        }
        if(playerInputManager.IsSpeicalKeyPressed(ActionName.Drop)) {
            blockStackManager.DropBlock();
            playerBlockManager.ResetPosition();
        }
    }
    void ObstacleBlockSet() {
        time += Time.deltaTime;
        if(time >= obstacleTime) {
            blockStackManager.CreateObstacle();
            time = 0f;
        }
    }

}
