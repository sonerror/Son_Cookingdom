using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScreen : UIScreen
{
    public Transform btnPlay;
    public Transform btnHint;
    public Transform logo;

    private Vector3 scaleUp = new Vector3(1.5f, 1.5f, 1.5f);
    private Vector3 scaleDown = new Vector3(1f, 1f, 1f);


    public void gotoStore()
    {
        GameManager.Ins.gotoStore();
    }

    public override void Resize(Vector2 gameSize)
    {
        base.Resize(gameSize);
        RectTf.sizeDelta = gameSize;
    }
}
