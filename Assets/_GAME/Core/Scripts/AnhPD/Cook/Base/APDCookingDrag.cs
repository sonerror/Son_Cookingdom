using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
  public class APDCookingDrag : APDCookingToolBase
  {
    [SerializeField] private SpriteRenderer fixedSprite;
    [SerializeField] protected Transform target;
    [SerializeField] private float zAngle;
    [SerializeField] protected bool isMouseUpCheck = true, isHideAfterComplete = true, isActiveTarget = false;

    public UnityEvent completeEvent, mouseDownEvent, mouseUpEvent, landEvent;

    public bool IsComplete;

    protected override void MouseDown(BaseEventData eventData)
    {
      base.MouseDown(eventData);
      mouseDownEvent?.Invoke();
      for (int i = 0; i < parts.Length; i++)
      {
        parts[i].sortingOrder = spriteRenderer.sortingOrder;
      }
      if (fixedSprite != null)
      {
        fixedSprite.enabled = false;
        spriteRenderer.enabled = true;
      }
      Tf.DOLocalRotate(new Vector3(0, Tf.localEulerAngles.y, zAngle), .3f);
    }
    protected override void MouseDrag(BaseEventData eventData)
    {
      base.MouseDrag(eventData);
      if (isMouseUpCheck || !IsReady) return;
      CheckTarget();
    }
    protected override void MouseUp(BaseEventData eventData)
    {
      base.MouseUp(eventData);
      mouseUpEvent?.Invoke();
      if (!isMouseUpCheck || !IsReady) return;
      CheckTarget();
    }
    protected virtual void CheckTarget()
    {
      if (IsInRange(target))
      {
        isDragging = false;
        Tf.DOKill();

        if (isHideAfterComplete) gameObject.SetActive(false);
        else OnComplete();

        IsComplete = true;
        if (isActiveTarget)
        {
          target.gameObject.SetActive(true);
        }
        completeEvent?.Invoke();
      }
      else if (isMouseUpCheck) OnIncorrectUse();
    }
    protected override void Rewind(Action completeAction = null)
    {
      base.Rewind(complete);
      void complete()
      {
        landEvent?.Invoke();
        completeAction?.Invoke();
        for (int i = 0; i < parts.Length; i++)
        {
          parts[i].sortingOrder = spriteRenderer.sortingOrder;
        }
        if (fixedSprite != null)
        {
          fixedSprite.enabled = true;
          spriteRenderer.enabled = false;
        }
      }
    }

    /// <summary>
    /// 0 = None, 1 = VisibleInsideMask, 2 = VisibleOutsideMask
    /// <br/> isAll : set all sprite in parts
    /// </summary>
    public void SetSpriteMaskInteraction(int index = 0, bool isAll = false)
    {
      index = Mathf.Clamp(index, 0, 2);
      SpriteMaskInteraction interaction = (SpriteMaskInteraction)index;
      spriteRenderer.maskInteraction = interaction;

      if (isAll)
      {
        foreach (var part in parts)
        {
          part.maskInteraction = interaction;
        }
      }
    }
    /// <summary>
    /// 0 = None, 1 = VisibleInsideMask, 2 = VisibleOutsideMask
    /// <br/> isAll : set all sprite in parts
    /// </summary>
    public void SetSpriteMaskInteraction(int index = 0)
    {
      index = Mathf.Clamp(index, 0, 2);
      SpriteMaskInteraction interaction = (SpriteMaskInteraction)index;
      spriteRenderer.maskInteraction = interaction;
    }

    public override void OnReReady(bool isReady = true)
    {
      base.OnReReady(isReady);
      Tf.localScale = sizeInit;
    }
  }
}

