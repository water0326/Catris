using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPool
{
    public BlockPool(string name, GameObject prefab, Transform parent, int init_pool_count) {
        this.name = name;
        this.prefab = prefab;
        this.parent = parent;
        this.init_pool_count = init_pool_count;
    }
    public string name;
    public GameObject prefab;
    public Transform parent;
    public int init_pool_count;

    Queue<GameObject> pool;

    void CreateNewObject() {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.SetActive(false);
        pool.Enqueue(obj);
        obj.transform.SetParent(parent);
        obj.GetComponent<Block>().blockName = name;
    }

    public void Reset() {
        pool = new Queue<GameObject>();
        for(int i = 0 ; i < init_pool_count ; i++) {
            CreateNewObject();
        }
    }

    public GameObject GetObject() {
        if(pool.Count == 0) {
            CreateNewObject();
        }
        GameObject obj = pool.Dequeue();
        return obj;
    }

    public bool ReturnObject(Block block) {
        if(block.blockName != name) return false;
        block.gameObject.SetActive(false);
        block.transform.SetParent(parent);
        pool.Enqueue(block.gameObject);
        return true;
    }
}
