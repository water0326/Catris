using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{

    public static GameProgressManager instance;

    [ReadOnly]
    public bool IsPlaying;

    [SerializeField]
    PlayerInputManager playerInputManager;
    [SerializeField]
    PlayerBlockManager playerBlockManager;
    [SerializeField]
    BlockStackManager blockStackManager;
    [SerializeField]
    DataUI dataUI;

    float time;
    [SerializeField]
    float obstacleTime;

    [SerializeField]
    float playerMoveInputInterval;
    float playerMoveInputIntervalCurrent;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        dataUI = UIManager.Instance.GetUIById(4).GetComponent<DataUI>();
        playerMoveInputIntervalCurrent = 0;
        StartGame();
    }

    void ResetMap() {
        BlockGrid.Instance.Reset();
        playerBlockManager.Reset();
    }

    void StartGame() {
        PauseGame();
        ResetMap();
        DataSet();
        dataUI.DataSet();
        blockStackManager.SetGuideBlock();
        ResumeGame();
        
    }

    void DataSet() {
        ScoreManager.instance.ResetScore();
        LevelManager.instance.ResetLevel();
    }

    void PauseGame() {
        IsPlaying = false;
    }

    void ResumeGame() {
        IsPlaying = true;
    }

    void GameEnd() {
        PauseGame();
        UIManager.Instance.EnableUI(6);
    }

    private void Update() {
        if(IsPlaying) {
            PlayerAction();
            ObstacleBlockSet();
            if(blockStackManager.isGameOver) {
                GameEnd();
            }
            dataUI.DataSet();
        }
    }

    void PlayerAction() {
        Vector2 inputDirection = playerInputManager.GetCurrentActionDataAsVector();
        int x = (int)inputDirection.x;
        int y = (int)inputDirection.y;
        playerMoveInputIntervalCurrent += Time.deltaTime;
        if(x != 0 || y != 0) {
            if(playerMoveInputIntervalCurrent >= playerMoveInputInterval) {
                playerMoveInputIntervalCurrent = 0;
                playerBlockManager.Move((int)inputDirection.x, (int)inputDirection.y);
                blockStackManager.SetGuideBlock();
            }
        }
        if(playerInputManager.IsSpeicalKeyPressed(ActionName.Drop)) {
            blockStackManager.DropBlock();
            playerBlockManager.ResetPosition(true);
        }
        if(playerInputManager.IsSpeicalKeyPressed(ActionName.Reset)) {
            playerBlockManager.ResetPosition(false);
        }
    }
    void ObstacleBlockSet() {
        obstacleTime = LevelManager.instance.GetObstacleCoolDown();
        time += Time.deltaTime;
        if(time >= obstacleTime) {
            blockStackManager.CreateObstacle();
            time = 0f;
        }
    }

    public float GetObstacleBarPercentage() {
        return time / obstacleTime;
    }

}
