using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Tanghulu
{
  public class TanghuluMortar : MonoBehaviour
  {
    [SerializeField] private Transform nut, pestle;
    [SerializeField] private SpriteRenderer crush;
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private AudioClip sfxCrush;

    private bool _isCrushing;
    private int _count;

    public Action OnDone;

    public void PutInNut()
    {
      nut.FallAppear();
    }
    public void PutInPestle()
    {
      pestle.Appear();
      coll2D.enabled = true;
    }

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || _isCrushing) return;
      _isCrushing = true;

      pestle.DOMoveY(pestle.position.y - .5f, .1f).SetEase(Ease.InBack).OnComplete(() =>
      {
        // AudioManager.PlaySFX(sfxCrush);
        _count++;
        float rate = (float)_count / 5;
        crush.SetAlpha(rate);
        nut.DOPunchPosition(Vector3.up * .2f, .1f);
      });
      pestle.DOMoveY(pestle.position.y, .2f).SetDelay(.1f).SetEase(Ease.Linear).OnComplete(() =>
      {
        _isCrushing = false;
        float rate = (float)_count / 10;
        if (rate >= 1f)
        {
          coll2D.enabled = false;
          OnDone?.Invoke();
          pestle.gameObject.SetActive(false);
        }
      });
    }
  }
}
