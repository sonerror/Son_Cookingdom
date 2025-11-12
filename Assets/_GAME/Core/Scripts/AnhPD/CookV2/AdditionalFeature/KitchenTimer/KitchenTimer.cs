using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AnhPD.CookV2
{
  public class KitchenTimer : MonoBehaviour
  {
    [SerializeField] private Image cookTimerImage, burntTimerImage;
    [SerializeField] private Transform clockwise;
    [SerializeField] private AudioSource sfxClock;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject vfxDone, vfxBurnt;
    [SerializeField] private ParticleSystem vfxSmoke;

    private float _burntDuration;
    private Sequence _seq;
    private Tween _smokeTween;

    public UnityEvent onCooked, onHalfBurnt, onBurnt;

    [Button]
    public void Show(float cookDuration = 5f, float burntDuration = 5f)
    {
      _seq?.Kill();
      _seq = DOTween.Sequence();

      _burntDuration = burntDuration;
      cookTimerImage.fillAmount = 0f;
      burntTimerImage.fillAmount = 0f;

      transform.localScale = Vector3.zero;
      clockwise.localEulerAngles = new Vector3(clockwise.localEulerAngles.x, clockwise.localEulerAngles.y, 0);


      _seq.Append(transform.DOScale(Vector3.one, .3f));
      _seq.Append(clockwise
                  .DOLocalRotate(new Vector3(clockwise.localEulerAngles.x, clockwise.localEulerAngles.y, 360f),
                                 cookDuration,
                                 RotateMode.FastBeyond360)
                  .SetEase(Ease.Linear)
                  .OnStart(() => sfxClock.Play())
                  .OnComplete(OnCooked));
      _seq.Join(cookTimerImage.DOFillAmount(1f, cookDuration).SetEase(Ease.Linear));
      _seq.Append(clockwise
                  .DOLocalRotate(new Vector3(clockwise.localEulerAngles.x, clockwise.localEulerAngles.y, 360f),
                                 burntDuration,
                                 RotateMode.FastBeyond360)
                  .SetEase(Ease.Linear));
      _seq.Join(burntTimerImage.DOFillAmount(1f, burntDuration).SetEase(Ease.Linear));
      _seq.InsertCallback(cookDuration + burntDuration / 2, OnHalfBurnt);
      _seq.OnComplete(OnBurnt);

      vfxDone.SetActive(false);
      vfxBurnt.SetActive(false);
      var main = vfxSmoke.main;
      main.startColor = Color.white;
    }

    private void OnCooked()
    {
      onCooked?.Invoke();
      fire.SetActive(true);
      vfxDone.SetActive(true);
      vfxSmoke.Play();
    }

    private void OnHalfBurnt()
    {
      onHalfBurnt?.Invoke();
      var main = vfxSmoke.main;
      Color end = Color.black;

      _smokeTween = DOTween.To(() => main.startColor.color,
                                x =>
                                {
                                  var m = vfxSmoke.main;
                                  m.startColor = x;
                                },
                                end,
                                _burntDuration / 2);
    }
    private void OnBurnt()
    {
      onBurnt?.Invoke();
      vfxBurnt.SetActive(true);
      Hide();
    }

    public void Hide()
    {
      sfxClock.Stop();
      _seq.Kill();
      _smokeTween.Kill();
      fire.SetActive(false);
      // if (_seq != null && _seq.IsActive() && _seq.IsPlaying())
      // {
      //     _seq.Complete(true);
      // }
      transform.DOScale(0, .3f);
    }

  }
}
