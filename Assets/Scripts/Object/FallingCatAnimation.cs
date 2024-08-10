using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCatAnimation : MonoBehaviour
{
    [SerializeField]
    float cooldown;
    float time;

    [SerializeField]
    Sprite[] heads;
    [SerializeField]
    Sprite[] tails;
    [SerializeField]
    Sprite[] bodys;

    [SerializeField]
    FallingCatPool pool;

    [SerializeField]
    Vector2 xRange;
    [SerializeField]
    Vector2 yRange;

    List<FallingCat> objList;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        objList = new List<FallingCat>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > cooldown) {
            time = 0;
            FallingCat obj = pool.GetObject().GetComponent<FallingCat>();
            
            int idx = Random.Range(0, heads.Length);
            obj.head = heads[idx];
            obj.tail = tails[idx];
            obj.body = bodys[idx];
            obj.ChangeSprite();

            objList.Add(obj);
            
            obj.transform.position = new Vector2(Random.Range(xRange.x, xRange.y), yRange.y);

            obj.gameObject.SetActive(true);

        }

        for(int i = 0 ; i < objList.Count ; i++) {
            if(objList[i].CheckY(yRange.x)) {
                pool.ReturnObject(objList[i].gameObject);
                objList.Remove(objList[i]);
                i--;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(xRange.x, -100), new Vector2(xRange.x,100));
        Gizmos.DrawLine(new Vector2(xRange.y, -100), new Vector2(xRange.y,100));
        Gizmos.DrawLine(new Vector2(-100, yRange.x), new Vector2(100,yRange.x));
        Gizmos.DrawLine(new Vector2(-100, yRange.y), new Vector2(100,yRange.y));
    }
}
