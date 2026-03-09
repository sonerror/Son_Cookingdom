using AnhPD.Cook;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace AnhPD.Tanghulu
{
  public class TanghuluNutPot : ClockerStoveBase
  {
    [SerializeField] private AudioSource sfxWorking;

    private bool _isAlmond;
    private bool _isDoneAlmond, _isDoneSesame;

    public bool isHaveObject { get; private set; }

    private void Awake()
    {
      foreach (var almond in almonds)
      {
        almond.OnFlipped += CheckAlmond;
      }
    }
    protected override void OnTurnOn()
    {
      sfxWorking.Play();
      base.OnTurnOn();
      if (_isAlmond) CookAlmond();
      else CookSesame();
    }
    protected override void OnTurnOff()
    {
      base.OnTurnOff();
      sfxWorking.Stop();
      vfxSmoke.Stop();
    }
    public void EmptyPot()
    {
      isHaveObject = false;
      if (_isDoneAlmond && _isDoneSesame)
      {
        IsReady = true;
      }
    }

    public void OnNutBack(bool isAlmond)
    {
      isHaveObject = true;
      _isAlmond = isAlmond;
    }
    #region almond
    [SerializeField] private AlmondRoasted[] almonds;
    [SerializeField] private AudioClip sfxPop;
    [SerializeField] private Transform almondGroup;

    public Action OnAlmondReadyFlip, OnAllAlmondFlipped, OnAlmondCooked;

    public void OnPutAlmondIn()
    {
      _isAlmond = true;
      isHaveObject = true;
      StartCoroutine(DelayAppear());
    }
    private IEnumerator DelayAppear()
    {
      foreach (var almond in almonds)
      {
        // AudioManager.PlaySFx(sfxPop, .2f);
        almond.Appear();
        yield return new WaitForSeconds(.05f);
      }

      if (!isOn) IsReady = true;
      else CookAlmond();
    }
    private void CookAlmond()
    {
      foreach (var almond in almonds)
      {
        almond.Cooking();
      }
      // clocker.Show(5f);
      this.WaitToDo(() =>
      {
        OnAlmondReadyFlip?.Invoke();
        foreach (var almond in almonds)
        {
          almond.ReadyFlip();
        }
      }, 5f);
    }
    private void CheckAlmond()
    {
      foreach (var almond in almonds)
      {
        if (!almond.IsFlipped) return;
      }
      OnAllAlmondFlipped?.Invoke();

      // clocker.Show(5f);
      this.WaitToDo(() =>
      {
        foreach (var almond in almonds)
        {
          almond.gameObject.SetActive(false);
        }
        almondGroup.FallAppear();
        _isDoneAlmond = true;
        OnAlmondCooked?.Invoke();
      }, 5f);
    }
    #endregion

    #region sesame
    [SerializeField] private SpriteRenderer sesameDone, salt;
    [SerializeField] private Transform sesameRaw;

    public Action OnSesameCooked;

    public void OnPutSesameIn()
    {
      _isAlmond = false;
      isHaveObject = true;
      sesameRaw.FallAppear();
    }

    public void OnPutSaltIn()
    {
      salt.transform.FallAppear();
      if (isOn) CookSesame();
      else
      {
        IsReady = true;
      }
    }
    private void CookSesame()
    {
      sesameRaw.DOShakePosition(.1f, .05f, 100)
          .SetLoops(-1, LoopType.Restart)
          .SetEase(Ease.Linear);
      sesameDone.DOFade(1, 5f);
      // clocker.Show(5f);
      salt.DOFade(0, 5f).OnComplete(() =>
      {
        _isDoneSesame = true;
        OnSesameCooked?.Invoke();
      });

    }
    #endregion
  }
}
