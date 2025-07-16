using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScreen : UIScreen
{
    public GameObject btnPlay;

    public void gotoStore()
    {
        GameManager.Ins.gotoStore();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            btnPlay.SetActive(true);

        }
    }

    public override void Resize(Vector2 gameSize)
    {
        base.Resize(gameSize);
        RectTf.sizeDelta = gameSize;
    }
}
