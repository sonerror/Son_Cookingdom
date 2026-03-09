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
  public class SoupSpoon : APDCookingToolBase
  {
    [SerializeField] private SpriteRenderer soup;
    [SerializeField] private Transform source, target;
    [SerializeField] private AudioClip sfxWater, sfxPutDown;
    [SerializeField] private float zAngle;

    public UnityEvent completeEvent, mouseDownEvent, mouseUpEvent, landEvent, haveSoupEvent, giveBackSoupEvent;

    public bool IsHideSource, IsBlock = false;
    private bool isHaveSoup;

    protected override void MouseDown(BaseEventData eventData)
    {
      base.MouseDown(eventData);
      soup.sortingOrder = spriteRenderer.sortingOrder;
      Tf.DORotate(new Vector3(0, 0, zAngle), .3f);
      mouseDownEvent?.Invoke();
    }
    protected override void MouseDrag(BaseEventData eventData)
    {
      base.MouseDrag(eventData);
      if (!IsReady || IsBlock) return;
      if (!isHaveSoup)
      {
        if (IsInRange(source.position, soup.transform.position))
        {
          // AudioManager.PlaySFX(sfxWater);
          isHaveSoup = true;
          soup.transform.Appear();
          haveSoupEvent?.Invoke();
          if (IsHideSource)
            source.gameObject.SetActive(false);
        }
      }
      else
      {
        if (IsInRange(target.position, soup.transform.position))
        {
          // AudioManager.PlaySFX(sfxPutDown);

          isHaveSoup = false;
          soup.gameObject.SetActive(false);

          OnComplete();
          completeEvent?.Invoke();
        }
      }
    }
    protected override void MouseUp(BaseEventData eventData)
    {
      base.MouseUp(eventData);
      mouseUpEvent?.Invoke();
      if (isHaveSoup)
      {
        soup.gameObject.SetActive(false);
        isHaveSoup = false;
        giveBackSoupEvent?.Invoke();
        if (IsHideSource)
        {
          source.Appear();
        }
      }
    }
    protected override void Rewind(Action completeAction = null)
    {
      base.Rewind(complete);
      void complete()
      {
        completeAction?.Invoke();
        landEvent?.Invoke();
      }
    }

    public void Block(bool enable = true)
    {
      IsBlock = enable;
    }
  }
}

