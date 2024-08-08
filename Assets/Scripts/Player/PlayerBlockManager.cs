using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockManager : MonoBehaviour
{
    [SerializeField]
    int playerBlockLength;
    [SerializeField]
    string playerBlockName;
    [SerializeField]
    [ReadOnly]
    Vector2[] posList;

    Block[] playerBlocks;

    public void Reset() {
        Vector2 mapSize = BlockGrid.Instance.GetMapSize();
        playerBlocks = new Block[playerBlockLength];
        for(int i = 0 ; i < playerBlockLength ; i++) {
            //playerBlocks[i] = BlockGrid.Instance.CreateBlock(playerBlockName);
        }
        BlockGrid.Instance.CreateBlock("TestBlock", 0, (int)mapSize.y - 1);
        BlockGrid.Instance.CreateBlock("TestBlock", 1, (int)mapSize.y - 1);
        BlockGrid.Instance.CreateBlock("TestBlock", 2, (int)mapSize.y - 1);
    }
}
