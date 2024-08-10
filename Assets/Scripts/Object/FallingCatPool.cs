using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallingCatPool : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int init_count;
    [SerializeField]
    Transform parent;

    Queue<GameObject> list;

    private void Awake() {
        list = new Queue<GameObject>();
        for(int i = 0; i < init_count ; i++) {
            CreateObject();
        }
    }
    void CreateObject() {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        obj.transform.SetParent(parent);
        list.Enqueue(obj);
    }

    public GameObject GetObject() {
        if(list.Count == 0) {
            CreateObject();
        }
        return list.Dequeue();
    }

    public void ReturnObject(GameObject obj) {
        obj.SetActive(false);
        list.Enqueue(obj);
    }
}
