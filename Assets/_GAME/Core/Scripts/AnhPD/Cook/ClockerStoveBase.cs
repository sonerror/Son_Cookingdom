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
    [SerializeField] private Transform vfxGlow;
    [SerializeField] protected ParticleSystem vfxSmoke;
    // [SerializeField] protected ClockTimer clocker;
    [SerializeField] protected KitchenTimer timer;
    [SerializeField] protected FxType sfxPip = FxType.None;

    public bool IsReady;
    protected bool isOn;

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
      SoundManager.Ins.PlayFx(sfxPip);
      vfxGlow.DOScale(1, 1f);
      vfxSmoke.Play();
      isOn = true;
      IsReady = false;
      onTurnOn?.Invoke();
    }
    protected virtual void OnTurnOff()
    {
      SoundManager.Ins.PlayFx(sfxPip);

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

