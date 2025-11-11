using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.CookV2
{
  public class OvenAppliance : TwiceOpenAppliance
  {
    [FoldoutGroup("References")][SerializeField] private SpriteRenderer[] bakeSprite;
    [FoldoutGroup("Audio")][SerializeField] private AudioClip sfxDone;
    private bool _isReadyBake;

    private void Awake()
    {
      isWaitingAfterClose = false;
    }

    protected override void Close()
    {
      base.Close();
      _isReadyBake = true;
    }

    public void OnButtonClick()
    {
      if (!_isReadyBake) return;
      _isReadyBake = false;
      StartWaiting();
    }

    protected override void StartWaiting()
    {
      base.StartWaiting();
      if (bakeSprite.Length > 0)
      {
        foreach (var sprite in bakeSprite) sprite.DOFade(1, duration);
      }
    }

    protected override void OnDone()
    {
      base.OnDone();
      // AudioManager.PlaySFX(sfxDone);
    }

    protected override void ForceDone()
    {
      base.ForceDone();
      if (bakeSprite.Length > 0)
      {
        foreach (var sprite in bakeSprite) sprite.SetAlpha(1);
      }
    }
  }
}
