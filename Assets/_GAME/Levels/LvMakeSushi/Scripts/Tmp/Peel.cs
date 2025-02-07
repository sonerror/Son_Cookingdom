using System.Collections;
using System.Collections.Generic;
using AnhPD.MakeSushi;
using DG.Tweening;
using UnityEngine;

public class Peel : MonoBehaviour
{
    public DraggableObject draggable;
    public float distaceCheck = 1f;
    public Transform target;


    public virtual void CheckToTarget()
    {
        if (Vector3.Distance(transform.position, target.position) < distaceCheck)
        {
            draggable.enabled = false;
            transform.DOMove(target.position, 0.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
                LevelMakeSushi.Ins.OnGarbageThrowed();
            });
        }
    }

    public void ResetPosition()
    {
        draggable.ResetStartPos();
    }

}
