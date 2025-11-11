using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class SimplePourAction : BeforeCompleteAction
  {
    [SerializeField] private SpriteRenderer pouringSprite;
    [SerializeField] private float pouringAngle;
    [SerializeField] private Vector3 offset = new Vector3(1, 1, 0);
    [SerializeField] private AudioClip sfxPour;

    public UnityEvent onStart;
    public override void DoAction(Transform target, Action onDone)
    {
      onStart?.Invoke();
      transform.DOMove(target.position + offset, .15f);
      transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, pouringAngle), .15f)
          .SetDelay(.15f)
          .OnComplete(Pour);
      void Pour()
      {
        // AudioManager.PlaySFX(sfxPour);
        pouringSprite.SetAlpha(1);
        pouringSprite.enabled = true;
        pouringSprite.DOFade(0, .1f).SetDelay(0.3f)
            .OnStart(() =>
            {
              pouringSprite.enabled = false;
              onDone?.Invoke();
            });
      }
    }

    [Button]
    private void TurnOffSprite()
    {
      pouringSprite.enabled = false;
    }
  }
}
