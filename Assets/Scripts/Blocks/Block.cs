using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Block : MonoBehaviour
{
    [SerializeField]
    bool canIgnore = false;
    [SerializeField]
    Image sprite;

    [ReadOnly]
    public Vector2 pos;

    [ReadOnly]
    public string blockName;

    RectTransform rectTransform;

    abstract public void Active();

    abstract public void Deactive();

    void SetPos() {
        rectTransform.anchoredPosition = BlockGrid.Instance.GridPosToScreenPos((int)pos.x, (int)pos.y);
        rectTransform.localScale = new Vector3(1,1,1);
    }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
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
