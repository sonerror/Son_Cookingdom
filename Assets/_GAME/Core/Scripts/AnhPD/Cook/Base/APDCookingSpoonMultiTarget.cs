using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class APDCookingSpoonMultiTarget : APDCookingSpoon
  {
    [SerializeField] private Transform[] targets;
    private bool[] isTargetReady;
    public UnityEvent[] events;

    protected override void Start()
    {
      base.Start();
      isTargetReady = new bool[targets.Length];
    }
    protected override void CheckTarget()
    {
      if (!isDragging) return;
      //base.CheckTarget();
      List<Transform> targetList = new List<Transform>();
      targetList.Clear();
      for (int i = 0; i < targets.Length; i++)
      {
        if (isTargetReady[i])
        {
          targetList.Add(targets[i]);
        }
      }

      Transform nsTarget = spoon.GetNearestTarget(targetList.ToArray());
      int index = Array.IndexOf(targets, nsTarget);
      // int index = targets.IndexOf(nsTarget);
      if (index < 0) return;
      //Debug.Log(index);
      if (spoon.IsInTargetRange(targets[index].position, dropDistance))
      {
        coll2D.enabled = false;
        Tf.DOKill();
        isDragging = false;
        isTargetReady[index] = false;
        Vector3 pos = targets[index].position + offset;
        spoon.OnPour(pos, events[index].Invoke, () =>
        {
          OnComplete();
        });
      }
    }
    public override void OnComplete()
    {
      base.OnComplete();
      for (int i = 0; i < isTargetReady.Length; i++)
      {
        if (isTargetReady[i])
        {
          IsReady = true;
          return;
        }
      }
    }
    public void OnTargetReady(int index)
    {
      IsReady = true;
      isTargetReady[index] = true;
    }
  }
}

