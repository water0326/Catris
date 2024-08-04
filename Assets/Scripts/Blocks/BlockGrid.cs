using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    Block[,] blockGrid;

    [SerializeField]
    Canvas screenCanvas;

    private void Awake() {
        _instance = this;
        setting = _gridSetting;
    }

    public Block CreateBlock(string blockName, int x, int y) {

        if(!IsPosInRange(x, y)) {
            Debug.LogWarning("the position of the block is not in range");
            return null;
        }

        Block block = blockManager.GetBlock(blockName);
        block.pos = new Vector2(x, y);
        block.transform.parent = activatedBlocksParent;

        ActivedBlockList.Add(block);
        blockGrid[x, y] = block;
        block.Active();

        return block;

    }

    public bool RemoveBlock(Block block) {

        if(!ActivedBlockList.Contains(block)) return false;

        block.Deactive();

        ActivedBlockList.Remove(block);
        blockManager.RemoveBlock(block);
        blockGrid[(int)block.pos.x, (int)block.pos.y] = null;

        return true;

    }

    public bool MoveBlock(Block block, int x, int y) {
        
        if(!IsPosInRange(x, y)) {
            Debug.LogWarning("the position of the block is not in range");
            return false;
        }
        if(!CanMoveTo(x, y)) return false;

        blockGrid[(int)block.pos.x, (int)block.pos.y] = null;
        blockGrid[x, y] = block;

        return true;
        
    }

    bool CanMoveTo(int x, int y) {
        return blockGrid[x, y] == null;
    }
    
    public void Reset() {

        for(int i = 0 ; i < ActivedBlockList.Count ; i++) {
            RemoveBlock(ActivedBlockList[i]);
        }

        ActivedBlockList = new List<Block>();
        blockGrid = new Block[_gridSetting.grid_cell_count_x, _gridSetting.grid_cell_count_y];

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

        RectTransformUtility.ScreenPointToLocalPointInRectangle(screenCanvas.GetComponent<RectTransform>(), Camera.main.ViewportToScreenPoint(screenPos), Camera.main, out  result);

        return result;

    }

    

}
