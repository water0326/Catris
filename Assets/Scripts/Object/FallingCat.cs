using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCat : MonoBehaviour
{
    [ReadOnly]
    public Sprite head;
    [ReadOnly]
    public Sprite tail;
    [ReadOnly]
    public Sprite body;

    [SerializeField]
    SpriteRenderer headSR;
    [SerializeField]
    SpriteRenderer tailSR;
    [SerializeField]
    SpriteRenderer bodySR1;
    [SerializeField]
    SpriteRenderer bodySR2;
    [SerializeField]
    float speed;

    public void ChangeSprite() {
        headSR.sprite = head;
        tailSR.sprite = tail;
        bodySR1.sprite = body;
        bodySR2.sprite = body;
    }

    private void Update() {
        transform.Translate(new Vector2(0, speed * Time.deltaTime));
    }

    public bool CheckY(float y) {
        return transform.position.y < y;
    }

}
