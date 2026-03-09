using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace AnhPD.Cook
{
  public class StoveBase : MonoBehaviour
  {
    [SerializeField] private GameObject stove_on;
    [SerializeField] private Transform vfxGlow;
    [SerializeField] private Image waitingBar;
    [SerializeField] protected AudioClip sfxClick;

    public bool IsReady;
    protected bool isOn;

    public void OnClick()
    {
      if (!IsReady) return;
      // AudioManager.PlaySFX(sfxClick);
      if (!isOn)
      {
        stove_on.SetActive(true);
        //turn on
        if (!IsReady)
        {
          this.WaitToDo(() =>
          {
            // AudioManager.PlaySFX(sfxClick, .2f);
            stove_on.SetActive(false);
          }, .3f);
          return;
        }
        OnTurnOn();
      }
      else
      {
        //turn off
        if (!IsReady)
        {
          return;
        }
        OnTurnOff();
      }
    }
    protected virtual void OnTurnOn()
    {
      stove_on.SetActive(true);
      vfxGlow.DOScale(1, 1f);
      isOn = true;
      IsReady = false;
    }
    protected virtual void OnTurnOff()
    {
      IsReady = false;
      isOn = false;
      vfxGlow.DOScale(0, 1f);
      stove_on.SetActive(false);
    }
    public virtual void StartWaiting(float time, Action action = null)
    {
      //// AudioManager.PlaySFX(sfxClick, .2f);
      waitingBar.fillAmount = 0;
      waitingBar.DOFillAmount(1, time).OnComplete(() =>
      {
        action?.Invoke();
      }).SetEase(Ease.Linear);
    }
  }
}

