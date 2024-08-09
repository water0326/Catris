using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum ActionName {
    None,
    Up,
    Down,
    Left,
    Right,
    Drop
}

class BlockMoveKeyData {

    public BlockMoveKeyData(KeyCode key, Vector2 direction, int priority) {
        this.key = key;
        this.direction = direction;
        this.priority = priority;
    }
    public KeyCode key;
    public Vector2 direction;
    public int priority;
}

class BlockSpecialKeyData {
    public BlockSpecialKeyData(KeyCode key) {
        this.key = key;
        isPressed = false;   
    }
    public KeyCode key;
    public bool isPressed;
}

public class PlayerInputManager : MonoBehaviour
{
    Dictionary<ActionName, BlockMoveKeyData> blockMoveActions = new Dictionary<ActionName, BlockMoveKeyData>();
    Dictionary<ActionName, BlockSpecialKeyData> blockSpecialActions = new Dictionary<ActionName, BlockSpecialKeyData>();

    [SerializeField]
    [ReadOnly]
    BlockMoveKeyData currentKeyData;

    BlockMoveKeyData noneKeyData = new BlockMoveKeyData(KeyCode.None, Vector2.zero, 0);

    private void Awake() {
        currentKeyData = noneKeyData;
        blockMoveActions.Add(ActionName.Down, new BlockMoveKeyData(KeyCode.S, new Vector2(0,-1),0));
        blockMoveActions.Add(ActionName.Up, new BlockMoveKeyData(KeyCode.W, new Vector2(0,1),0));
        blockMoveActions.Add(ActionName.Left, new BlockMoveKeyData(KeyCode.A, new Vector2(-1,0),0));
        blockMoveActions.Add(ActionName.Right,new BlockMoveKeyData(KeyCode.D, new Vector2(1,0),0));

        blockSpecialActions.Add(ActionName.Drop, new BlockSpecialKeyData(KeyCode.Space));
    }

    public Vector2 GetCurrentActionDataAsVector() {
        bool isPressed = false;
        foreach(ActionName action in blockMoveActions.Keys) {
            if(Input.GetKeyDown(blockMoveActions[action].key)) {
                currentKeyData = blockMoveActions[action];
                isPressed = true;
            }
        }
        if(!isPressed) currentKeyData = noneKeyData;
        return currentKeyData.direction;
    }

    public bool IsSpeicalKeyPressed(ActionName action) {
        return Input.GetKeyDown(blockSpecialActions[action].key);
    }
    

}
