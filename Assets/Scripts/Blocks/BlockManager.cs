using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class BlockInfo{
    public string name;
    public GameObject prefab;
    public int init_pool_count;
}

public class BlockManager : MonoBehaviour
{

    [SerializeField]
    BlockInfo[] blockInfoData;
    [SerializeField]
    Transform poolsParent;
    
    Dictionary<string, BlockPool> pools = new Dictionary<string, BlockPool>();

    private void Awake() {
        for(int i = 0 ; i < blockInfoData.Length ; i++) {
            BlockInfo currentBlockInfo = blockInfoData[i];
            GameObject parentObject = new GameObject(currentBlockInfo.name + " (Pool)");
            parentObject.transform.SetParent(poolsParent);
            pools.Add(currentBlockInfo.name, new BlockPool(currentBlockInfo.name, currentBlockInfo.prefab, parentObject.GetComponent<Transform>(), currentBlockInfo.init_pool_count));
            pools[currentBlockInfo.name].Reset();
            print(currentBlockInfo.name + " pool was loaded.");
        }
    }

    public Block GetBlock(string blockName, int x, int y) {
        if(!pools.ContainsKey(blockName)) {
            Debug.LogError("There isn't any block pool which name is " + blockName + ".");
            return null;
        }
        Block block = pools[blockName].GetObject().GetComponent<Block>();
        block.x = x;
        block.y = y;
        
        block.gameObject.SetActive(true);
        return block;
    }

    public bool RemoveBlock(Block block) {
        if(!pools.ContainsKey(block.blockName)) {
            Debug.LogError("There isn't any block which name is " + block.blockName + ".");
            return false;
        }
        pools[block.blockName].ReturnObject(block);
        block.gameObject.SetActive(false);
        return true;
    }
}
