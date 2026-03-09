using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class PourAction : BeforeCompleteAction
  {
    [SerializeField] private SpriteRenderer defaultSprite, pourSprite, pouringSprite;
    [SerializeField] private float pouringAngle;
    [SerializeField] private Vector3 offset = new Vector3(1, 1, 0);
    [SerializeField] private AudioClip sfxPour;

    public UnityEvent onStart;

    public override void DoAction(Transform target, Action onDone)
    {
      onStart?.Invoke();
      transform.DOMove(target.position + offset, .3f);
      transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, pouringAngle), .3f)
          .SetDelay(.3f)
          .OnStart(PrePour)
          .OnComplete(Pour);

      void PrePour()
      {
        defaultSprite.DOFade(0, .3f);
        pourSprite.DOFade(1, .3f);
      }
      void Pour()
      {
        // AudioManager.PlaySFX(sfxPour);
        pouringSprite.enabled = true;
        defaultSprite.DOFade(0, .3f).OnComplete(() =>
        {
          pouringSprite.enabled = false;
          defaultSprite.DOFade(1, .3f);
          pourSprite.DOFade(0, .3f);
          onDone?.Invoke();
        });
      }
    }

    [Button]
    private void Setup()
    {
      pourSprite.enabled = true;
      pourSprite.SetAlpha(0);
      pouringSprite.enabled = false;
    }
  }
}
