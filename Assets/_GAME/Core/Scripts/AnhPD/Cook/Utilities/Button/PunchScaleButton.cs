using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class PunchScaleButton : MonoBehaviour
  {
    [SerializeField] private AudioClip sfxClick;
    public UnityEvent onClick;

    private bool _isBlock;
    private bool _isScaling;
    private float _scale;

    private void Start()
    {
      _scale = transform.localScale.x;
    }

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || _isScaling) return;
      // AudioManager.PlaySFX(sfxClick);
      _isScaling = true;
      transform.DOScale(.9f * _scale, .05f).SetEase(Ease.Linear);
      transform.DOScale(1f * _scale, .1f).SetDelay(.1f).SetEase(Ease.OutBack).OnComplete((() =>
      {
        _isScaling = false;
      }));
      if (!_isBlock) onClick?.Invoke();
    }

    public void Block()
    {
      _isBlock = true;
    }
  }
}
