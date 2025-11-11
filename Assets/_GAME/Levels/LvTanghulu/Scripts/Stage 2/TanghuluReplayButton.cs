using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class TanghuluReplayButton : MonoBehaviour
  {
    [SerializeField] private Transform skewer;
    [SerializeField] private Transform skewerRoot;
    // [SerializeField] private TanghuluDrag tanghulu;

    public Action OnReplay;

    private bool _isAnimating;
    private bool _isHide;

    private bool isForceHide => !skewer.gameObject.activeSelf;

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || _isAnimating || _isHide) return;

      _isAnimating = true;
      transform.DORotate(new Vector3(0, 0, 360f), .3f, RotateMode.FastBeyond360);
      OnReplay?.Invoke();
    }

    public void OnDoneAnim()
    {
      _isAnimating = false;
    }

    private void Update()
    {
      if (isForceHide)
      {
        if (_isHide) return;
        _isHide = true;
        transform.DOScale(0f, .3f).SetEase(Ease.InBack);
        return;
      }
      float distance = Vector2.Distance(skewer.position, skewerRoot.position);
      switch (distance)
      {
        case > .2f when !_isHide:
          _isHide = true;
          transform.DOScale(0f, .3f).SetEase(Ease.InBack);
          break;
        case <= .2f when _isHide && skewer.gameObject.activeSelf:
          _isHide = false;
          transform.DOScale(.3f, .3f).SetEase(Ease.OutBack);
          break;
      }
    }
  }
}
