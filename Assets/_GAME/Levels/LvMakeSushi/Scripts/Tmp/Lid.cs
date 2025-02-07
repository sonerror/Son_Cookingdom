using System.Collections;
using System.Collections.Generic;
using AnhPD.MakeSushi;
using DG.Tweening;
using UnityEngine;

public class Lid : Peel
{

    public bool isState2 = false;
    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }
    public override void CheckToTarget()
    {
        if (!isState2) return;
        if (Vector3.Distance(transform.position, target.position) > distaceCheck)
        {
            draggable.isActive = false;
            transform.DOMove(startPos, 0.5f).OnComplete(() =>
            {
                LevelMakeSushi.Ins.OnGarbageThrowed();
            });
        }
    }
}
