using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.CookV2
{
  public class SpiceShakingAction : BeforeCompleteAction
  {
    [SerializeField] private Transform localTf;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioClip sfxShake;
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    [SerializeField] private float offsetY = .5f;
    public override void DoAction(Transform target, Action onDone)
    {
      Sequence seq = DOTween.Sequence();
      seq.Append(transform.DOMove(target.position + offset, .1f));
      seq.Append(localTf.DOLocalMoveY(-offsetY, .1f));
      seq.Append(localTf.DOLocalMoveY(0f, .1f).OnComplete(() =>
      {
        // AudioManager.PlaySFX(sfxShake);
        if (particle) particle.Play();
      }));
      seq.Append(localTf.DOLocalMoveY(-offsetY, .1f));
      seq.Append(localTf.DOLocalMoveY(0f, .1f).OnComplete(() =>
      {
        // AudioManager.PlaySFX(sfxShake);
      }));

      seq.OnComplete(() => onDone?.Invoke());
    }
  }
}
