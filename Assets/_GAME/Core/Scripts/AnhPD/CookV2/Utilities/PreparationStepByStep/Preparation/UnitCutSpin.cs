using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class UnitCutSpin : FruitPreparation
  {
    [SerializeField] private Transform knife;
    [SerializeField] private AudioClip sfxKnifeAppear, sfxKnifeCut;
    [SerializeField] private Vector3 knifeOffset;
    [SerializeField] private float cutOffset = .5f;
    [SerializeField] private int loopNumber = 2;
    public UnityEvent onDoneStep;

    public override void OnHaveTool()
    {
      base.OnHaveTool();
      // AudioManager.PlaySFX(sfxKnifeAppear);
      knife.Appear();
      MoveKnifeToCurrentPos();
    }

    private void MoveKnifeToCurrentPos()
    {
      if (Count >= steps.Length) return;
      knife.DOMove(steps[Count].transform.position + knifeOffset, .5f).OnComplete(Cut);
    }

    private void Cut()
    {
      // AudioManager.PlaySFX(sfxKnifeCut);

      knife.DOComplete();
      knife.DORotate(new Vector3(0, 0, 360f), .1f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(5 * loopNumber, LoopType.Incremental);

      Sequence seq = DOTween.Sequence();
      seq.SetTarget(knife);
      seq.Append(knife.DOMoveX(knife.position.x - cutOffset, .25f));
      seq.Append(knife.DOMoveX(knife.position.x + cutOffset * 2, .25f));
      seq.SetLoops(loopNumber, LoopType.Restart);
      (steps[Count] as UnitCutSpinStep)?.OnCut();
    }

    public override void OnDoneStep()
    {
      base.OnDoneStep();
      onDoneStep?.Invoke();
      knife.gameObject.SetActive(false);
      if (Count < steps.Length) onStart?.Invoke();
    }
  }
}
