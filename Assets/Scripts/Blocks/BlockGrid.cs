using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BlockGrid : MonoBehaviour
{
    const int GRID_CELL_COUNT_X = 10;
    const int GRID_CELL_COUNT_Y = 20;

    List<Block> ActivedBlockList = new List<Block>();

    [SerializeField]
    BlockManager blockManager;

    [SerializeField]
    Vector2 gridDimensions;
    [SerializeField]
    Vector2 gridCenterPos;

    Block[,] blockGrid;

    [SerializeField]
    Canvas screenCanvas;

    public Block CreateBlock(int id, int x, int y) {

        if(!IsPosInRange(x, y)) {
            Debug.LogWarning("the position of the block is not in range");
            return null;
        }

        Block block = blockManager.GetBlock(id);
        block.pos = new Vector2(x, y);

        ActivedBlockList.Add(block);
        blockGrid[x, y] = block;
        blockManager.ActiveBlock(block);

        return block;

    }

    public bool RemoveBlock(Block block) {

        if(!ActivedBlockList.Contains(block)) return false;

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
        blockGrid = new Block[GRID_CELL_COUNT_X, GRID_CELL_COUNT_Y];

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        float blockSizeX = gridDimensions.x / GRID_CELL_COUNT_X;
        float blockSizeY = gridDimensions.y / GRID_CELL_COUNT_Y;

        Gizmos.DrawWireCube(gridCenterPos, Camera.main.ViewportToWorldPoint(gridDimensions));
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(0, 0), Camera.main.ViewportToWorldPoint(new Vector2(blockSizeX, blockSizeY)));
    }

    bool IsPosInRange(int x, int y) {
        return Math.IsNumberInRange(x, 0, GRID_CELL_COUNT_X-1) && Math.IsNumberInRange(y, 0, GRID_CELL_COUNT_Y-1);
    }

    public Vector2 GetCanvasPosByIndex(int x, int y) {
        return new Vector2(0, 0);
    }

    Vector2 GridPosToViewPort(int x, int y) {
        float blockSizeX = gridDimensions.x / GRID_CELL_COUNT_X;
        float blockSizeY = gridDimensions.y / GRID_CELL_COUNT_Y;

        Vector2 result = new Vector2(blockSizeX * (x+0.5f) - gridDimensions.x * 0.5f, blockSizeY * (y+0.5f) - gridDimensions.y * 0.5f);
        print(result);
        print(blockSizeX);
        print(blockSizeY);
        return result;

    }

}
