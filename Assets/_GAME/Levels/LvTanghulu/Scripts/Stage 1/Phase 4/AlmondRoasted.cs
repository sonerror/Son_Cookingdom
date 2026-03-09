using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class AlmondRoasted : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer raw, cooked;
    [SerializeField] private AudioClip sfxFlip;
    public Action OnFlipped;
    public bool IsFlipped;
    private bool _isReadyFlip;

    public void Appear()
    {
      transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360f));
      transform.FallAppear(.1f, .5f, .2f);
    }
    public void Cooking()
    {
      transform.DOShakePosition(.1f, .05f, 100)
          .SetLoops(-1, LoopType.Restart)
          .SetEase(Ease.Linear)
          .SetDelay(UnityEngine.Random.Range(0, .1f));
    }
    public void ReadyFlip()
    {
      _isReadyFlip = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (_isReadyFlip && collision.gameObject.CompareTag("Player"))
      {
        _isReadyFlip = false;
        Flip();
      }
    }
    private void Flip()
    {
      // AudioManager.PlaySFX(sfxFlip);
      transform.DOKill();
      float y = transform.position.y;
      transform.DOMoveY(y + .5f, .15f).OnComplete(() =>
      {
        raw.enabled = false;
        cooked.enabled = true;
      });
      transform.DOMoveY(y, .15f).SetDelay(.15f).OnComplete(() =>
      {
        IsFlipped = true;
        OnFlipped?.Invoke();
        Cooking();
      });
      transform.DORotate(transform.eulerAngles + new Vector3(0, 0, 360f), .3f, RotateMode.FastBeyond360);
    }
  }
}
