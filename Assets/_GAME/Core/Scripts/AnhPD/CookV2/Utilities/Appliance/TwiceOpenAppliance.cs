using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HoangHH;
using Sirenix.OdinInspector;
// using TidyCooking;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace AnhPD.CookV2
{
  public class TwiceOpenAppliance : LiddedAppliance
  {
    // [SerializeField] private ClockTimer clock;
    [SerializeField] protected float duration = 5f;
    private bool _isDoneWaiting;

    public bool isShake;
    public bool isWaitingAfterClose = true;
    public UnityEvent onOpenAgain, onStartWaiting;

    private void Start()
    {
      if (isWaitingAfterClose) onClose.AddListener(StartWaiting);
    }

    protected virtual void StartWaiting()
    {
      onStartWaiting?.Invoke();

      // clock.Show(duration);
      if (isShake)
      {
        transform.DOShakePosition(duration, Vector3.one * .05f, (int)duration * 100);
      }
      this.WaitToDo(OnDone, duration);
    }

    protected virtual void ForceDone()
    {
      // clock.Hide();
      transform.DOComplete();
      StopAllCoroutines();
      OnDone();
    }

    protected virtual void OnDone()
    {
      _isDoneWaiting = true;
      OnReady();
    }

    protected override void Open()
    {
      if (!_isDoneWaiting) base.Open();
      else onOpenAgain?.Invoke();
    }
  }
}
