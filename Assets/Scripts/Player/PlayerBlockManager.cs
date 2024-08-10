using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBlock {
    public PlayerBlock(Block block) {
        this.block = block;
    }
    public Block block;
}

public class PlayerBlockManager : MonoBehaviour
{
    [SerializeField]
    int playerBlockLength;
    [SerializeField]
    [ReadOnly]
    int currentBlockIdx = -1;
    [SerializeField]
    [ReadOnly]
    int nextBlockIdx = -1;
    [SerializeField]
    string playerBlockName;
    [SerializeField]
    [ReadOnly]
    Vector2[] posList;

    [ReadOnly]
    public static int PlayerZoneYSize = 5;

    PlayerBlock[] playerBlocks;
    PlayerBlock headBlock;
    PlayerBlock tailBlock;

    [SerializeField]
    Sprite[] straightSprite;
    [SerializeField]
    Sprite[] curveSprite;
    [SerializeField]
    Sprite[] headSprite;
    [SerializeField]
    Sprite[] tailSprite;
    [SerializeField]
    int[] blockCounts;

    [SerializeField]
    BlockStackManager blockStackManager;

    public void Reset() {

        if(playerBlocks != null) {
            for(int i = 0 ; i < playerBlocks.Length ; i++) {
                BlockGrid.Instance.RemoveBlock(playerBlocks[i].block);
            }
        }

        Vector2 mapSize = BlockGrid.Instance.GetMapSize();
        playerBlockLength = Mathf.Min(playerBlockLength, (int)mapSize.x / 2 + 1);

        ResetPosition(true);
    }

    public void ResetPosition(bool changePattern) {
        if(changePattern) {
            if(nextBlockIdx == -1) {
                nextBlockIdx = Random.Range(0, blockCounts.Length);
            }
            currentBlockIdx = nextBlockIdx;
            playerBlockLength = blockCounts[currentBlockIdx];
            nextBlockIdx = Random.Range(0, blockCounts.Length);
        }
        

        if(playerBlocks != null) {
            for(int i = 0 ; i < playerBlocks.Length ; i++) {
                if(playerBlocks[i]!=null) {
                    BlockGrid.Instance.RemoveBlock(playerBlocks[i].block);
                }
            }
        }

        playerBlocks = new PlayerBlock[playerBlockLength];

        Vector2 mapSize = BlockGrid.Instance.GetMapSize();

        for(int i = 0 ; i < playerBlockLength ; i++) {
            playerBlocks[i] = new PlayerBlock(BlockGrid.Instance.CreateBlock(playerBlockName, (int)mapSize.x / 2 - i, (int)mapSize.y - 1, false));
            if(i==0) {
                headBlock = playerBlocks[i];
                headBlock.block.image.sprite = headSprite[currentBlockIdx];
            }
            else if(i==playerBlockLength - 1) {
                tailBlock = playerBlocks[i];
                tailBlock.block.image.sprite = tailSprite[currentBlockIdx];
            }
            else {
                playerBlocks[i].block.image.sprite = straightSprite[currentBlockIdx];
            }
            playerBlocks[i].block.SetDirection(new Vector2(-1, 0), new Vector2(1, 0));
            playerBlocks[i].block.state = BlockState.Straight;
        }

        blockStackManager.SetGuideBlock();
        
    }

    public PlayerBlock[] GetPlayerBlockData() {
        return playerBlocks;
    }

    public bool Move(int distanceX, int distanceY) {

        int previousX = headBlock.block.x;
        int previousY = headBlock.block.y;

        int tempX = headBlock.block.x + distanceX;
        int tempY = headBlock.block.y + distanceY;

        if(IsInPlayerZone(tempX, tempY)) {
            if(!BlockGrid.Instance.MoveBlock(headBlock.block, tempX, tempY)) {
                if(tempX == tailBlock.block.x && tempY == tailBlock.block.y) {
                    BlockGrid.Instance.ForceMoveBlock(headBlock.block, tempX, tempY);
                    
                }
                else {
                    return false;
                }
            }
        }
        else {
            return false;
        }

        Vector2 toDir = new Vector2(headBlock.block.x - previousX, headBlock.block.y - previousY);
        headBlock.block.SetDirection(-toDir, toDir);
        
        for(int i = 1 ; i < playerBlockLength ; i++) {

            if(playerBlocks[i]==headBlock) continue;

            

            int toPosX = previousX;
            int toPosY = previousY;

            if(playerBlocks[i] != tailBlock) {
                // Get Direction Part
                // toPos : current
                // playerBlocks[i].block : previous
                // playerBlocks[i-1].block : next

                // get : previous - current & to - current
                Vector2 from = new Vector2(playerBlocks[i].block.x - toPosX, playerBlocks[i].block.y - toPosY);
                Vector2 to = new Vector2(playerBlocks[i-1].block.x - toPosX, playerBlocks[i-1].block.y - toPosY);
                playerBlocks[i].block.SetDirection(from, to);
                
                previousX = playerBlocks[i].block.x;
                previousY = playerBlocks[i].block.y;
            }
            

            BlockGrid.Instance.MoveBlock(playerBlocks[i].block, toPosX, toPosY);
            if(playerBlocks[i]!=tailBlock) {
                if(playerBlocks[i].block.state == BlockState.Straight) {
                    playerBlocks[i].block.image.sprite = straightSprite[currentBlockIdx];
                }
                else if(playerBlocks[i].block.state == BlockState.Curve) {
                    playerBlocks[i].block.image.sprite = curveSprite[currentBlockIdx];
                }
            }
            else {
                int toX = playerBlocks[i-1].block.x - playerBlocks[i].block.x;
                int toY = playerBlocks[i-1].block.y - playerBlocks[i].block.y;
                playerBlocks[i].block.SetDirection(new Vector2(-toX, -toY), new Vector2(toX, toY));
            }
        }

        BlockGrid.Instance.MoveBlock(headBlock.block, headBlock.block.x, headBlock.block.y);

        return true;
    }

    bool IsInPlayerZone(int x, int y) {
        Vector2 mapSize = BlockGrid.Instance.GetMapSize();
        return y < (int)mapSize.y && y >= (int)mapSize.y - PlayerZoneYSize && x < (int)mapSize.x && x >= 0;
    }

    public string GetDirection(int prevX, int prevY, int toX, int toY) {
        if(toX - prevX > 0) {
            return "Right";
        }
        else if(toX - prevX < 0) {
            return "Left";
        }
        else if(toY - prevY > 0) {
            return "Up";
        }
        else {
            return "Down";
        }
    }

    public int GetNextBlockLength() {
        return blockCounts[nextBlockIdx];
    }
    public Sprite GetNextBlockHeadSprite() {
        return headSprite[nextBlockIdx];
    }
}
