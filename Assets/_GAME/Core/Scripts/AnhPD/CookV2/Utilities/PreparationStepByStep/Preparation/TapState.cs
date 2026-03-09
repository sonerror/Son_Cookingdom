using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.CookV2
{
  public class TapState : FruitPreparation
  {
    [SerializeField] private GameObject[] state;
    [SerializeField] private AudioClip sfxTap;
    private bool _isPunching;
    private void OnMouseDown()
    {
      if (_isPunching || !LevelBase.Ins.IsAllowInteract) return;
      _isPunching = true;
      transform.DOPunchScale(Vector3.up * .1f, .15f).OnComplete(() =>
      {
        _isPunching = false;
      });
      // AudioManager.PlaySFX(sfxTap);
      state[Count].SetActive(false);
      OnDoneStep();
      if (Count >= state.Length)
      {
        OnComplete();
        return;
      }
      state[Count].SetActive(true);
    }
  }
}
