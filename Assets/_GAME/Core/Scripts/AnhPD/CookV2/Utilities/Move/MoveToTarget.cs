using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class MoveToTarget : MonoBehaviour
  {
    [SerializeField] private Transform target;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float delay;

    public bool isOnEnableMove = false;
    public UnityEvent onComplete;

    private void OnEnable()
    {
      if (isOnEnableMove) Move();
    }

    public void Move()
    {
      transform.DOMove(target.position, duration).SetDelay(delay).SetEase(Ease.InBack);
      transform.DOScale(target.localScale, duration).SetDelay(delay).SetEase(Ease.Linear);
      transform.DORotate(target.eulerAngles, duration).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
      {
        target.gameObject.SetActive(true);
        gameObject.SetActive(false);
        onComplete?.Invoke();
      });
    }

  }
}
