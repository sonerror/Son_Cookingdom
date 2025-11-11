using DG.Tweening;
using HoangHH;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace AnhPD.Cook
{
  public class ClockerStoveBase : MonoBehaviour
  {
    [FoldoutGroup("Base Config")][SerializeField] private Transform vfxGlow;
    [FoldoutGroup("Base Config")][SerializeField] protected ParticleSystem vfxSmoke;
    // [FoldoutGroup("Base Config")][SerializeField] protected ClockTimer clocker;
    [FoldoutGroup("Base Config")][SerializeField] protected KitchenTimer timer;
    [FoldoutGroup("Base Config")][SerializeField] protected AudioClip sfxPip;

    [FoldoutGroup("Base Config")] public bool IsReady;
    [FoldoutGroup("Base Config")] protected bool isOn;

    public UnityEvent onTurnOn, onTurnOff;

    public virtual void OnClick()
    {
      if (!IsReady) return;
      IsReady = false;
      if (!isOn)
      {
        OnTurnOn();
      }
      else
      {
        OnTurnOff();
      }
    }
    protected virtual void OnTurnOn()
    {
      // AudioManager.PlaySFX(sfxPip);
      vfxGlow.DOScale(1, 1f);
      vfxSmoke.Play();
      isOn = true;
      IsReady = false;
      onTurnOn?.Invoke();
    }
    protected virtual void OnTurnOff()
    {
      // AudioManager.PlaySFX(sfxPip);

      IsReady = false;
      isOn = false;
      vfxGlow.DOScale(0, 1f);
      onTurnOff?.Invoke();
    }

    public void OnReady()
    {
      IsReady = true;
    }
  }
}

