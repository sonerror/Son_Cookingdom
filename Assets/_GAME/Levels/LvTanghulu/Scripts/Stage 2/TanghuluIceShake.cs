using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HoangHH;
using UnityEngine;
using UnityEngine.Rendering;
using Utilities;

namespace AnhPD.Tanghulu
{
  public class TanghuluIceShake : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer[] ices;
    [SerializeField] private Transform skewer;
    [SerializeField] private SortingGroup sortingGroup;
    // [SerializeField] private ClockTimer clock;
    [SerializeField] private ParticleSystem vfxStar;
    [SerializeField] private AudioClip sfxStar;

    private const int MIN_ORDER = -540, MAX_ORDER = -400;

    public Action OnDone;

    public void OnHaveIce()
    {
      sortingGroup.sortingOrder = MAX_ORDER;
      foreach (var ice in ices)
      {
        ice.SetAlpha(1);
        ice.transform.FallAppear();
      }

      // clock.Show(2f);
      this.WaitToDo(() =>
      {
        OnDone?.Invoke();
        skewer.Appear();
        sortingGroup.sortingOrder = MIN_ORDER;
        // AudioManager.PlaySFX(sfxStar, .5f);
        vfxStar.Play();
      }, 2f);
    }

    public void Restart()
    {
      foreach (var ice in ices)
      {
        ice.DOFade(0, .3f);
      }
    }
  }
}
