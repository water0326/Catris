using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GridSetting {
    public float gridSizeX {
        get {
            return Camera.main.ScreenToViewportPoint(new Vector2(Camera.main.ViewportToScreenPoint(new Vector2(0, gridSizeY)).y * grid_cell_ratio, 0)).x;
        }
    }
    public float gridSizeY = 0f;
    public Vector2 gridCenterPos = new Vector2(0, 0);
    public float grid_cell_ratio {
        get { return (float)grid_cell_count_x / grid_cell_count_y;}
    }
    public int grid_cell_count_x = 10;
    public int grid_cell_count_y = 20;
    public Vector2 blockSize {
        get { return new Vector2(gridSizeX / grid_cell_count_x, gridSizeY / grid_cell_count_y); }
    }
    public float paddingRatio = 0.02f;
}

public class BlockGrid : MonoBehaviour
{

    public static BlockGrid Instance {
        get {
            if(_instance == null) {
                Debug.LogError("There isn't BlockGrid Object");
            }
            return _instance;
        }
    }

    static BlockGrid _instance;

    List<Block> ActivedBlockList = new List<Block>();

    [SerializeField]
    BlockManager blockManager;
    
    public static GridSetting gridSetting {
        get { return setting; }
        private set {}
    }

    static GridSetting setting = new GridSetting();

    [SerializeField]
    GridSetting _gridSetting;

    [SerializeField]
    Transform activatedBlocksParent;
    [SerializeField]
    Transform activatedBGBlocksParent;

    [SerializeField]
    [ReadOnly]
    Block[,] blockGrid;
    [SerializeField]
    [ReadOnly]
    Block[,] backgroundBlockGrid;

    [SerializeField]
    Canvas screenCanvas;

    [SerializeField]
    string chessBlockName = "Chess";
    [SerializeField]
    string breakLineBlockName = "BreakLine";
    [SerializeField]
    string playerBoardBlockName = "PlayerBoard";

    [SerializeField]
    int[,] catFaceGrid = new int[10,20] {
        { 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 2, 1, 2, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 1, 2, 1, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 1, 2, 1, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 2, 1, 2, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };


    [SerializeField]
    RectTransform gameScreenRectTransform;

    private void Awake() {
        _instance = this;
        setting = _gridSetting;
        SetGameScreenSize();   
    }

    void SetGameScreenSize() {
        int ySize = (int)Camera.main.ViewportToScreenPoint(new Vector2(0, _gridSetting.gridSizeY)).y;
        int xSize = ySize / _gridSetting.grid_cell_count_y * _gridSetting.grid_cell_count_x;
        int paddingValue = (int)(ySize * _gridSetting.paddingRatio);
        xSize += paddingValue;
        ySize += paddingValue;

        gameScreenRectTransform.sizeDelta = new Vector2(xSize, ySize);
    }

    public Block CreateBlock(string blockName, int x, int y, bool isBG) {

        if(!IsPosInRange(x, y)) {
            Debug.LogWarning("the position of the block is not in range");
            return null;
        }
        if(!CanMove(x, y)) return null;

        Block block = blockManager.GetBlock(blockName, x, y);
        block.transform.SetParent(!isBG ? activatedBlocksParent : activatedBGBlocksParent);
        block.SetPos();

        if(!isBG) {
            ActivedBlockList.Add(block);
            blockGrid[x, y] = block;
        }
        else {
            backgroundBlockGrid[x, y] = block;
        }
        
        block.Active();

        return block;

    }

    public Block ForceCreateBlock(string blockName, int x, int y, bool isBG) {

        if(!IsPosInRange(x, y)) {
            Debug.LogWarning("the position of the block is not in range");
            return null;
        }

        Block block = blockManager.GetBlock(blockName, x, y);
        block.transform.SetParent(!isBG ? activatedBlocksParent : activatedBGBlocksParent);

        if(!isBG) {
            ActivedBlockList.Add(block);
            blockGrid[x, y] = block;
        }
        else {
            backgroundBlockGrid[x, y] = block;
        }
        
        block.Active();

        return block;

    }

    public bool RemoveBlock(Block block) {

        if(!ActivedBlockList.Contains(block)) return false;

        block.Deactive();

        blockGrid[block.x, block.y] = null;
        ActivedBlockList.Remove(block);
        blockManager.RemoveBlock(block);

        return true;

    }

    public bool MoveBlock(Block block, int x, int y) {
        
        if(!IsPosInRange(x, y)) {
            Debug.LogWarning("the position of the block is not in range");
            return false;
        }
        if(!CanMove(x, y)) {
            return false;
        }
        blockGrid[block.x, block.y] = null;
        blockGrid[x, y] = block;
        block.x = x;
        block.y = y;

        return true;
        
    }
    public void ForceMoveBlock(Block block, int x, int y) {
        
        if(!IsPosInRange(x, y)) {
            Debug.LogWarning("the position of the block is not in range");
        }
        
        blockGrid[block.x, block.y] = null; 
        blockGrid[x, y] = block;
        block.x = x;
        block.y = y;
    }

    public bool CanMove(int x, int y) {
        
        if(!IsPosInRange(x, y)) return false;

        return blockGrid[x, y] == null || blockGrid[x, y].canIgnore;
    }
    
    public Block GetBlockInfo(int x, int y) {
        if(!IsPosInRange(x, y)) return null;
        return blockGrid[x, y];
    }

    public void Reset() {

        for(int i = 0 ; i < ActivedBlockList.Count ; i++) {
            RemoveBlock(ActivedBlockList[i]);
        }

        ActivedBlockList = new List<Block>();
        blockGrid = new Block[_gridSetting.grid_cell_count_x, _gridSetting.grid_cell_count_y];
        backgroundBlockGrid = new Block[_gridSetting.grid_cell_count_x, _gridSetting.grid_cell_count_y];

        SetToEmptyBlock();

    }

    void SetToEmptyBlock() {
        for(int i = 0 ; i < _gridSetting.grid_cell_count_x ; i++) {
            for(int j = 0 ; j < _gridSetting.grid_cell_count_y; j++) {
                if(j < _gridSetting.grid_cell_count_y - PlayerBlockManager.PlayerZoneYSize) {       // Chess
                    backgroundBlockGrid[i, j] = CreateBlock(chessBlockName, i, j, true);
                    backgroundBlockGrid[i, j].gameObject.GetComponent<Chess>().SetColor(catFaceGrid[i,j]);
                }
                else if(j == _gridSetting.grid_cell_count_y - PlayerBlockManager.PlayerZoneYSize) { // BreakLine
                    backgroundBlockGrid[i, j] = CreateBlock(breakLineBlockName, i, j, true);
                }
                else {                                                                              // PlayerBoard
                    backgroundBlockGrid[i, j] = CreateBlock(playerBoardBlockName, i, j, true);
                }
            }
        }
    }

    private void OnDrawGizmos() {

        Vector2 ViewPortOffset = new Vector2(0.5f, 0.5f);

        
        Vector2 blockSizeVector = new Vector2(_gridSetting.gridSizeX / _gridSetting.grid_cell_count_x, _gridSetting.gridSizeY / _gridSetting.grid_cell_count_y);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            Camera.main.ViewportToWorldPoint(_gridSetting.gridCenterPos),
            Camera.main.ViewportToWorldPoint(new Vector2(_gridSetting.gridSizeX, _gridSetting.gridSizeY) + ViewPortOffset)
        );

        Gizmos.color = Color.green;
        for(int i = 0 ; i < _gridSetting.grid_cell_count_x ; i++) {
            for(int j = 0 ; j < _gridSetting.grid_cell_count_y ; j++) {
                Gizmos.DrawWireCube(
                    Camera.main.ViewportToWorldPoint(GridPosToViewPort(i, j)),
                    Camera.main.ViewportToWorldPoint(blockSizeVector + ViewPortOffset)
                );
            }
        }
        
    }

    bool IsPosInRange(int x, int y) {
        return GridMath.IsNumberInRange(x, 0, _gridSetting.grid_cell_count_x-1) && GridMath.IsNumberInRange(y, 0, _gridSetting.grid_cell_count_y-1);
    }

    public Vector2 GridPosToViewPort(int x, int y) {

        Vector2 startPos = new Vector2((1 - _gridSetting.gridSizeX) * 0.5f, (1 - _gridSetting.gridSizeY) * 0.5f);

        float blockSizeX = _gridSetting.gridSizeX / _gridSetting.grid_cell_count_x;
        float blockSizeY = _gridSetting.gridSizeY / _gridSetting.grid_cell_count_y;

        Vector2 result = new Vector2(startPos.x + blockSizeX * (x+0.5f), startPos.y + blockSizeY * (y+0.5f));

        return result;

    }

    public Vector2 GridPosToScreenPos(int x, int y) {

        Vector2 startPos = new Vector2((1 - gridSetting.gridSizeX) * 0.5f, (1 - gridSetting.gridSizeY) * 0.5f);

        float blockSizeX = gridSetting.gridSizeX / gridSetting.grid_cell_count_x;
        float blockSizeY = gridSetting.gridSizeY / gridSetting.grid_cell_count_y;

        Vector2 screenPos = new Vector2(startPos.x + blockSizeX * (x+0.5f), startPos.y + blockSizeY * (y+0.5f));


        Vector2 result;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(screenCanvas.GetComponent<RectTransform>(), Camera.main.ViewportToScreenPoint(screenPos), Camera.main, out result);

        return result;

    }

    public Vector2 GetMapSize() {
        return new Vector2(_gridSetting.grid_cell_count_x, _gridSetting.grid_cell_count_y);
    }

    public Vector2 GetParticlePos(float x, float y) {
        Vector2 startPos = new Vector2((1 - gridSetting.gridSizeX) * 0.5f, (1 - gridSetting.gridSizeY) * 0.5f);

        float blockSizeX = gridSetting.gridSizeX / gridSetting.grid_cell_count_x;
        float blockSizeY = gridSetting.gridSizeY / gridSetting.grid_cell_count_y;

        Vector2 screenPos = new Vector2(startPos.x + blockSizeX * (x+0.5f), startPos.y + blockSizeY * (y+0.5f));
        return Camera.main.ViewportToScreenPoint(screenPos);

    }

}
