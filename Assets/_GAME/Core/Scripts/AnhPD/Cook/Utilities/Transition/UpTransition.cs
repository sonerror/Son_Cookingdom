using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.Cook
{
  public class UpTransition : MonoBehaviour
  {
    [SerializeField] private Transform sprite;
    [SerializeField] private AudioClip sfx;
    [SerializeField][Range(0, 1)] private float sfxVolume = 1f;
    [SerializeField] private float startY, delay = .5f;

    public void StartTransition(Action onCover = null, Action onComplete = null)
    {
      gameObject.SetActive(true);
      // AudioManager.PlaySFX(sfx, sfxVolume);
      sprite.DOMoveY(0, 1f)
          .SetDelay(.5f)
          .OnComplete((() =>
      {
        onCover?.Invoke();
        sprite.DOMoveY(-startY, 1f).OnComplete(() =>
              {
            gameObject.SetActive(false);
            onComplete?.Invoke();
          })
              .SetDelay(delay)
              .OnStart(() =>
              {
                // AudioManager.PlaySFX(sfx);
          });
      }));
    }
  }
}
