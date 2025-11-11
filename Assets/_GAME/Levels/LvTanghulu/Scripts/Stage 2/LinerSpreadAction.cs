using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Tanghulu
{
  public class LinerSpreadAction : BeforeCompleteAction
  {
    [SerializeField] private AudioClip sfx;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float top = 1f, bot = -1f;

    public UnityEvent onStartAction;

    public override void DoAction(Transform target, Action onDone)
    {
      onStartAction?.Invoke();
      transform.DOMove(target.position + offset + Vector3.up * top, 0.1f).OnComplete(() =>
      {
        // AudioManager.PlaySFX(sfx);
      });
      transform.DOMove(target.position + offset + Vector3.up * bot, .4f)
          .SetDelay(.3f)
          .SetEase(Ease.Linear)
          .OnComplete(() => onDone?.Invoke());
    }
  }
}
