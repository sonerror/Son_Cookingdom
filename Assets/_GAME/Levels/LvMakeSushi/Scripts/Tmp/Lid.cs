using System.Collections;
using System.Collections.Generic;
using AnhPD.MakeSushi;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Lid : Peel
{

    public bool isState2 = false;
    public bool moveToTarget = false;
    public Vector3 startPos;

    public bool LockWhenDone = false;

    public int stateIndex;

    public UnityEvent onCompleted;

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
            draggable.LockPosition();
            if (moveToTarget)
            {
                transform.DOMove(target.position, 0.5f).OnComplete(() =>
                {
                    LevelMakeSushi.Ins.OnGarbageThrowed();
                    onCompleted?.Invoke();
                });
            }
            else
            {
                transform.DOMove(startPos, 0.5f).OnComplete(() =>
                {
                    LevelMakeSushi.Ins.OnGarbageThrowed();
                    onCompleted?.Invoke();
                });
            }

            TutorialManager.Ins.removeState(stateIndex);
        }
    }

    public void SetCanMove()
    {
        isState2 = true;
        draggable.enabled = true;
    }
}
