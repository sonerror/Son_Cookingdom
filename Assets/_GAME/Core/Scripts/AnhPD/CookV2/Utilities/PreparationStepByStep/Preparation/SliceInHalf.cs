using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class SliceInHalf : FruitPreparation
  {
    [SerializeField] private Transform knife;
    [SerializeField] private Transform top;
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private AudioClip sfxSlice, sfxKnife;
    [SerializeField] private float distance = 1f;
    private Vector2 _startMousePos;
    private bool _isSliced;
    public UnityEvent onHaveTool;
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || _isSliced) return;
      knife.DOPunchRotation(new Vector3(0, 0, 5f), .3f, 3);
      _startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
      Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector2 dragDir = mousePos - _startMousePos;

      // chỉ xử lý khi vuốt đủ dài
      if (dragDir.magnitude >= 1f)
      {
        Vector2 knifeRight = knife.transform.right; // trục đỏ trong world space
        float dot = Vector2.Dot(dragDir.normalized, knifeRight);

        if (dot > 0.7f) // 0.7 ~ cùng hướng trong khoảng 45 độ
        {
          Slice();
        }
      }
    }

    public override void OnHaveTool()
    {
      base.OnHaveTool();
      // AudioManager.PlaySFX(sfxKnife);
      knife.Appear();
      knife.localPosition -= knife.up * 1f;
      knife.DOLocalMove(knife.localPosition + knife.up * 1f, .3f).SetEase(Ease.InBack).OnComplete(() =>
      {
        coll2D.enabled = true;
        onHaveTool?.Invoke();
      });
    }

    private void Slice()
    {
      if (_isSliced) return;
      _isSliced = true;
      // AudioManager.PlaySFX(sfxSlice);

      Sequence seq = DOTween.Sequence();
      seq.Append(knife.DOLocalMove(knife.localPosition + knife.right * distance, .2f).SetEase(Ease.InBack));
      seq.Append(top.DORotate(top.eulerAngles + new Vector3(0, 0, 5f), 0.3f));
      seq.Join(top.DOLocalMoveX(top.localPosition.x + distance / 2, 0.3f));
      seq.AppendInterval(.5f); // delay thêm 0.5 giây sau tween
      seq.OnComplete(OnDoneStep);
    }
    public override void OnDoneStep()
    {
      // base.OnDoneStep();
      OnComplete();
    }
  }
}
