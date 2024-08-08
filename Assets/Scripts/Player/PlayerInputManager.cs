using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum ActionName {
    None,
    Up,
    Down,
    Left,
    Right
}

class KeyData {

    public KeyData(KeyCode key, Vector2 direction, int priority) {
        this.key = key;
        this.direction = direction;
        this.priority = priority;
    }
    public KeyCode key;
    public Vector2 direction;
    public int priority;
}

public class PlayerInputManager : MonoBehaviour
{
    Dictionary<ActionName, KeyData> actions = new Dictionary<ActionName, KeyData>();
    
    [SerializeField]
    [ReadOnly]
    KeyData currentKeyData;

    KeyData noneKeyData = new KeyData(KeyCode.None, Vector2.zero, 0);

    private void Awake() {
        currentKeyData = noneKeyData;
        actions.Add(ActionName.Down, new KeyData(KeyCode.S, new Vector2(0,-1),0));
        actions.Add(ActionName.Up, new KeyData(KeyCode.W, new Vector2(0,1),0));
        actions.Add(ActionName.Left, new KeyData(KeyCode.A, new Vector2(-1,0),0));
        actions.Add(ActionName.Right,new KeyData(KeyCode.D, new Vector2(1,0),0));
    }
    
    private void Update() {
        bool isPressed = false;
        foreach(ActionName action in actions.Keys) {
            if(Input.GetKeyDown(actions[action].key)) {
                currentKeyData = actions[action];
                isPressed = true;
            }
        }
        if(!isPressed) currentKeyData = noneKeyData;
    }

    public Vector2 GetCurrentActionDataAsVector() {
        return currentKeyData.direction;
    }
    

}
