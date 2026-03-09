using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.CookV2
{
  public class ReplaceByTargetAction : BeforeCompleteAction
  {
    [SerializeField] private Transform targetReplace;
    [SerializeField] private AudioClip sfxReplace;
    [SerializeField] private float duration = .3f;
    public override void DoAction(Transform target, Action onDone)
    {
      transform.DOComplete();
      if (!targetReplace) targetReplace = target;
      transform.DORotate(targetReplace.eulerAngles, duration);
      transform.DOScale(targetReplace.localScale, duration);
      transform.DOMove(targetReplace.position, duration).OnComplete(() =>
      {
        onDone?.Invoke();
        transform.DOKill();
        targetReplace.gameObject.SetActive(true);
        gameObject.SetActive(false);
        if (sfxReplace) { } // AudioManager.PlaySFX(sfxReplace);

      });
    }
  }
}
