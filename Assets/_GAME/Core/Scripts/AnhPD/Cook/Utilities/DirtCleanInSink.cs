using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
  public class DirtCleanInSink : MonoBehaviour
  {
    [SerializeField] private Sink sink;
    [SerializeField] private SpriteRenderer dirt;
    [SerializeField] private APDCookingDrag drag;
    [SerializeField] private float duration = 2f, scale = .2f;
    [SerializeField] private AudioClip sfxPlace, sfxSplash;

    public bool IsClean;
    private bool isPlace;

    [Button]
    private void Init()
    {
      dirt = GetComponent<SpriteRenderer>();
      drag = GetComponent<APDCookingDrag>();
    }
    public void OnPlace()
    {
      isPlace = true;

      if (sink.IsContainWater)
      {
        // AudioManager.PlaySFX(sfxSplash);
        transform.Appear();
        OnClean();
      }
      else
      {
        // AudioManager.PlaySFX(sfxPlace);
        transform.FallAppear();
      }
    }
    public void CheckInSink()
    {
      if (isPlace && sink.IsContainWater)
      {
        OnClean();
      }
    }

    public void OnClean()
    {
      if (IsClean) return;
      Floating();
      IsClean = true;
      dirt.DOFade(0f, duration).OnComplete(() =>
      {
        APDCookEffectManager.Instance.SpawnEffect_Clean(transform.position, .7f);
        drag.EnableCollider();
        drag.OnReady();
      });
    }
    public void Floating()
    {
      transform.Floating(-.075f);
    }
  }
}

