using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public enum BlockState {
    Straight,
    Curve,
    None
}

public abstract class Block : MonoBehaviour
{
    [SerializeField]
    public bool canIgnore = false;
    [SerializeField]
    Sprite sprite;

    [ReadOnly]
    public int x;
    [ReadOnly]
    public int y;

    [ReadOnly]
    public string blockName;

    [ReadOnly]
    public BlockState state;

    RectTransform rectTransform;
    [ReadOnly]
    public Image image;

    [SerializeField]
    public bool isPlayerBlock = false;

    [ReadOnly]
    public Vector3 scale;

    static int GetVectorToDirection(int x, int y) {
        if(x == 1 && y == 0) {
            return 90;
        }
        else if(x == -1 && y == 0) {
            return 270;
        }
        else if(x == 0 && y == 1) {
            return 0;
        }
        else if(x == 0 && y == -1) {
            return 180;
        }
        return 0;
    }

    abstract public void Active();

    abstract public void Deactive();

    void SetPos() {
        rectTransform.anchoredPosition = BlockGrid.Instance.GridPosToScreenPos(x, y);
        rectTransform.localScale = scale;
    }

    public void SetDirection(Vector2 from, Vector2 to) {
        int fromDir = GetVectorToDirection((int)from.x, (int)from.y);
        int toDir = GetVectorToDirection((int)to.x, (int)to.y);

        int diff = (toDir - fromDir + 360) % 360;

        if(diff == 90) { // clockwise
            rectTransform.rotation = Quaternion.Euler(0,0,-toDir);
            scale = new Vector3(1,1,1);
            state = BlockState.Curve;
        }
        else if(diff == 270) { // counterclockwise
            rectTransform.rotation = Quaternion.Euler(0,0,-toDir);
            scale = new Vector3(-1,1,1);
            
            state = BlockState.Curve;
        }
        else if(diff == 180) { // cross
            rectTransform.rotation = Quaternion.Euler(0,0,-toDir);
            scale = new Vector3(1,1,1);
            state = BlockState.Straight;
        }

        
    }

    private void Awake() {
        state = BlockState.None;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        if(sprite != null) {
            image.sprite = sprite;
        }
        scale = new Vector3(1,1,1);
    }

    private void OnEnable() {
        rectTransform.sizeDelta = Camera.main.ViewportToScreenPoint(BlockGrid.gridSetting.blockSize);
    }

    private void FixedUpdate() {
        OnFixedUpdate();
        SetPos();
    }
    protected abstract void OnFixedUpdate();

}
