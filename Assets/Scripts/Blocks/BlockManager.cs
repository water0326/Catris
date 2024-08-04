using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block GetBlock(int id) {
        return null;    
    }

    public bool RemoveBlock(Block block) {
        return false;
    }

    public void ActiveBlock(Block block) {
        block.Active();
    }
}
