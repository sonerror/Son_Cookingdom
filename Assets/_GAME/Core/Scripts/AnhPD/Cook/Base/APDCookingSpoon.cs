using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
  [RequireComponent(typeof(Spoon))]
  [RequireComponent(typeof(BoxCollider2D))]
  public class APDCookingSpoon : APDCookingToolBase
  {
    [SerializeField] protected Spoon spoon;
    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 offset = new Vector3(0, 1f, 0);
    [SerializeField] private float startZ;
    [SerializeField] protected int currentSourceIndex = 0;
    public UnityEvent eventComplete, mouseDownEvent, mouseUpEvent, landEvent;

    [ShowInInspector, ReadOnly] public bool IsContinueAfterComplete { private set; get; }
    public override void InitProperties()
    {
      base.InitProperties();
      spoon = GetComponent<Spoon>();
      shadow = GetComponent<Shadow>();
    }
    protected override void MouseDown(BaseEventData eventData)
    {
      base.MouseDown(eventData);
      Tf.DORotate(new Vector3(0, 0, startZ), .3f);
      mouseDownEvent?.Invoke();
    }
    protected override void MouseDrag(BaseEventData eventData)
    {
      base.MouseDrag(eventData);
      if (!IsReady) return;
      if (!spoon.IsHaveSpice)
        spoon.CheckSource(currentSourceIndex, dropDistance);
      else
      {
        CheckTarget();
      }
    }
    protected override void Rewind(Action completeAction = null)
    {
      base.Rewind(complete);
      void complete()
      {
        landEvent?.Invoke();
        completeAction?.Invoke();
        if (IsContinueAfterComplete)
        {
          IsReady = true;
        }
      }
    }
    public override void OnComplete()
    {
      coll2D.enabled = true;
      base.OnComplete();
      spoon.TurnOffSpices();
    }
    protected override void MouseUp(BaseEventData eventData)
    {
      if (!isDragging) return;
      spoon.TurnOffSpices();
      mouseUpEvent?.Invoke();
      base.MouseUp(eventData);
    }
    protected virtual void CheckTarget()
    {
      if (spoon.IsInTargetRange(target.transform.position, dropDistance))
      {
        coll2D.enabled = false;
        Tf.DOKill();
        isDragging = false;

        Vector3 pos = target.transform.position + offset;
        spoon.OnPour(pos, eventComplete.Invoke, OnComplete);
      }
    }
    public void NextSource()
    {
      if (currentSourceIndex < spoon.sourcesNumber - 1)
      {
        currentSourceIndex++;
      }
      else
      {
        IsContinueAfterComplete = false;
        IsReady = false;
      }
    }
    public void SetContinueAfterComplete(bool enable = true)
    {
      IsContinueAfterComplete = enable;
    }
    public override void OnReReady(bool isReady = true)
    {
      base.OnReReady(isReady);
      currentSourceIndex = 0;
    }
  }
}

