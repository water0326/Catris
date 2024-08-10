using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Pos {
    public int x;
    public int y;
    public Pos(int x, int y) {
        this.x = x;
        this.y = y;
    }
}

public class BlockStackManager : MonoBehaviour
{
    [SerializeField]
    PlayerBlockManager playerBlockManager;

    [SerializeField]
    string stackedPlayerBlockName = "StackedCatBlock";

    [SerializeField]
    string obstacleBlockName = "Obstacle";

    [SerializeField]
    string guideBlockName = "Guide";

    [SerializeField]
    string dropParticleName = "Drop";

    [SerializeField]
    Sprite obstacleHeadSprite;
    [SerializeField]
    Sprite obstacleTailSprite;
    [SerializeField]
    Sprite obstacleBodySprite;
    [SerializeField]
    Sprite obstacleAllInOneSprite;

    [SerializeField]
    Vector2 obstacleEmptySpaceRange;

    [ReadOnly]
    public Block[] guideBlocks;

    [SerializeField]
    int warningY = 12;

    ColorSheetUI colorSheetUI;
    

    [ReadOnly]
    public bool isGameOver;

    public void DropBlock() {
        
        PlayerBlock[] playerBlockData = playerBlockManager.GetPlayerBlockData();
        if(playerBlockData == null) return;

        Pos[] resultPlayerBlockPos = GetDeepestPlayerPos(playerBlockData);
        
        for(int i = 0 ; i < guideBlocks.Length ; i++) {
            BlockGrid.Instance.RemoveBlock(guideBlocks[i]);
        }

        for(int i = 0 ; i < playerBlockData.Length ; i++) {
            CreateStackedCatBlock(playerBlockData[i].block, resultPlayerBlockPos[i].x, resultPlayerBlockPos[i].y);
        }

        CheckCanClearLine();

        SetGuideBlock();
        
    }

    public void SetGuideBlock() {

        PlayerBlock[] blocks = playerBlockManager.GetPlayerBlockData();
        if(guideBlocks != null) {
            for(int i = 0 ; i < guideBlocks.Length ; i++) {
                BlockGrid.Instance.RemoveBlock(guideBlocks[i]);
            }
        }
        guideBlocks = new Block[blocks.Length];
        Pos[] pos = GetDeepestPlayerPos(blocks);

        for(int i = 0 ; i < pos.Length ; i++) {
            guideBlocks[i] = BlockGrid.Instance.CreateBlock(guideBlockName, pos[i].x, pos[i].y, false);
        }
    }

    void CheckCanClearLine() {
        int[] canClearLineList = GetCanClearLine();
        if(canClearLineList.Length != 0) {
            ScoreManager.instance.ComboUp();
            ScoreManager.instance.ChangeScore(canClearLineList.Length);
        }
        else {
            ScoreManager.instance.ResetCombo();
        }
        if(canClearLineList != null) {
            for(int i = 0 ; i < canClearLineList.Length ; i++) {
                ClearLine(canClearLineList[i]);
            }
        }
    }

    int[] GetCanClearLine() {
        List<int> result = new List<int>();
        Vector2 mapSize = BlockGrid.Instance.GetMapSize();
        for(int i = (int)mapSize.y-PlayerBlockManager.PlayerZoneYSize-1 ; i >= 0  ; i--) {
            bool canClear = true;
            for(int j = 0 ; j < mapSize.x ; j++) {
                if(BlockGrid.Instance.GetBlockInfo(j,i) == null) {
                    canClear = false;
                    break;
                }
            }
            if(canClear) {
                result.Add(i);
            }
        }
        return result.ToArray();
    }

    void ClearLine(int idx) {
        Vector2 mapSize = BlockGrid.Instance.GetMapSize();
        for(int i = 0 ; i < mapSize.x ; i++) {
            Block block = BlockGrid.Instance.GetBlockInfo(i, idx);
            ParticlePool.instance.SetParticle(dropParticleName, block.x, block.y);
            BlockGrid.Instance.RemoveBlock(block);
        }
        for(int i = idx+1 ; i < mapSize.y-PlayerBlockManager.PlayerZoneYSize ; i++) {
            for(int j = 0 ; j < mapSize.x ; j++) {
                Block block = BlockGrid.Instance.GetBlockInfo(j,i);
                if(block != null) {
                    BlockGrid.Instance.MoveBlock(block, j,i-1); 
                }
            }
        }
    }

    bool CheckGameEnd() {
        Vector2 mapSize = BlockGrid.Instance.GetMapSize();
        for(int i = (int)mapSize.y - PlayerBlockManager.PlayerZoneYSize ; i < mapSize.y ; i++) {
            for(int j = 0 ; j < mapSize.x ; j++) {
                Block block = BlockGrid.Instance.GetBlockInfo(j,i);
                if(block != null && !block.isPlayerBlock && !block.canIgnore) {
                    return true;
                }
            }
        }
        return false;
    }

    Pos[] GetDeepestPlayerPos(PlayerBlock[] playerBlock) {

        bool canMove = true;
        int dx = 0;
        int dy = -1;
        Pos[] currentPos = new Pos[playerBlock.Length];

        for(int i = 0 ; i < playerBlock.Length ; i++) {
            currentPos[i] = new Pos(playerBlock[i].block.x,playerBlock[i].block.y);
        }
        
        while(canMove) {
            canMove = true;
            for(int i = 0 ; i < playerBlock.Length ; i++) {
                int toX = currentPos[i].x + dx;
                int toY = currentPos[i].y + dy;
                
                if(BlockGrid.Instance.CanMove(toX, toY) || (BlockGrid.Instance.GetBlockInfo(toX, toY) != null && BlockGrid.Instance.GetBlockInfo(toX, toY).isPlayerBlock)) continue;
        
                canMove = false;
                break;
                
            }

            if(canMove) {
                for(int i = 0 ; i < playerBlock.Length ; i++) {
                    currentPos[i].x += dx;
                    currentPos[i].y += dy;
                }
            }
        }

        return currentPos;        

    }

    Block CreateStackedCatBlock(Block playerBlock, int x, int y) {

        Block block = BlockGrid.Instance.ForceCreateBlock(stackedPlayerBlockName, x, y, false); 
        block.state = playerBlock.state;
        block.image.sprite = playerBlock.image.sprite;
        block.GetComponent<RectTransform>().rotation = playerBlock.GetComponent<RectTransform>().rotation;
        block.scale = playerBlock.scale;
        return block;
    }

    private void Update() {
        if(CheckGameEnd()) {
            isGameOver = true;
        }
        colorSheetUI.SetWarning(GetHighestBlock() >= warningY);
    }

    int GetHighestBlock() {
        Vector2 mapSize = BlockGrid.Instance.GetMapSize();
        for(int i = (int)mapSize.y - PlayerBlockManager.PlayerZoneYSize - 1 ; i >= 0 ; i--) {
            for(int j = 0 ; j < mapSize.x ; j++) {
                if(BlockGrid.Instance.GetBlockInfo(j, i) != null && !BlockGrid.Instance.GetBlockInfo(j, i).canIgnore) {
                    return i;
                }
            }
        }
        return -1;
    }

    public void CreateObstacle() {
        int map_xSize = (int)BlockGrid.Instance.GetMapSize().x;
        MoveGridUp();
        int randomValue = Random.Range((int)Mathf.Max(0, obstacleEmptySpaceRange.x), (int)Mathf.Min(map_xSize-1, obstacleEmptySpaceRange.y));
        bool[] isThereBlock = GetRandomPosition(randomValue, map_xSize);
        for(int i = 0 ; i < map_xSize ; i++) {
            if(isThereBlock[i]) {
                Block block = BlockGrid.Instance.CreateBlock(obstacleBlockName, i, 0, false);
                Sprite selectedSprite;
                if(i+1 != map_xSize && isThereBlock[i+1] && (i == 0 || !isThereBlock[i-1])) {
                    selectedSprite = obstacleHeadSprite;
                }
                else if(i > 0 && i < map_xSize-1 && isThereBlock[i-1] && isThereBlock[i+1]) {
                    selectedSprite = obstacleBodySprite;
                }
                else if((i == 0 || !isThereBlock[i-1]) && (i+1==map_xSize || !isThereBlock[i+1])) {
                    selectedSprite = obstacleAllInOneSprite;
                }
                else {
                    selectedSprite = obstacleTailSprite;
                }
                block.GetComponent<Image>().sprite = selectedSprite;
                block.SetDirection(new Vector2(1, 0), new Vector2(-1, 0));
            }
        }
        
    }

    bool[] GetRandomPosition(int emptySpaceCount, int map_xSize) {
        List<int> isThereBlock = new List<int>();
        for(int i = 0 ; i < map_xSize ; i++) {
            isThereBlock.Add(i);
        }
        for(int i = 0 ; i < emptySpaceCount ; i++) {
            isThereBlock.RemoveAt(Random.Range(0, isThereBlock.Count));
        }
        bool[] result = new bool[map_xSize];
        for(int i = 0 ; i < result.Length ; i++) {
            result[i] = false;
        }
        for(int i = 0 ; i < isThereBlock.Count ; i++) {
            result[isThereBlock[i]] = true;
        }
        return result;
    }

    void MoveGridUp() {
        Vector2 mapSize = BlockGrid.Instance.GetMapSize();
        for(int i = (int)mapSize.y - PlayerBlockManager.PlayerZoneYSize - 1 ; i >= 0  ; i--) {
            for(int j = 0 ; j < mapSize.x ; j++) {
                Block block = BlockGrid.Instance.GetBlockInfo(j, i);
                if(block != null) {
                    BlockGrid.Instance.ForceMoveBlock(block, block.x, block.y+1);
                }
            }
        }
    }

    private void Awake() {
        isGameOver = false;
    }
    private void Start() {
        colorSheetUI = UIManager.Instance.GetUIById(8).GetComponent<ColorSheetUI>();
    }
}
