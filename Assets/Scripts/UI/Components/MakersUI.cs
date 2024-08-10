using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakersUI : FixedUI
{
    [SerializeField]
    float blinkTime;

    [SerializeField]
    Image image;

    float time;
    float coefficient;
    bool once;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        once = true;
        coefficient = -4f / (blinkTime*blinkTime);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Color color = new Color(1,1,1,coefficient * time * (time - blinkTime));
        image.color = color;

        if(time > blinkTime) {
            if(once) {
                once = false;
                GameSceneManager.Instance.ChangeScene(Scenes.Lobby);
            }
        }
    }
}
