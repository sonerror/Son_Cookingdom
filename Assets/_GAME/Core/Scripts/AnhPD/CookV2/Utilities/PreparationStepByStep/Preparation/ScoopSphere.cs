using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.CookV2
{
  public class ScoopSphere : FruitPreparation
  {
    [SerializeField] private ScoopSphereSpoon spoon;
    [SerializeField] private Vector3 offset;
    [SerializeField] private AudioClip sfxPop;
    public override void OnHaveTool()
    {
      base.OnHaveTool();
      spoon.gameObject.SetActive(true);
      MoveToCurrentStep();
    }

    public override void OnDoneStep()
    {
      // AudioManager.PlaySFxRandomPitch(sfxPop, .5f);
      steps[Count].gameObject.SetActive(true);
      StartCoroutine(DelayDoneStep());
    }

    private IEnumerator DelayDoneStep()
    {
      yield return new WaitForSeconds(0.3f);
      base.OnDoneStep();
      if (Count < steps.Length)
      {
        MoveToCurrentStep();
      }
    }

    private void MoveToCurrentStep()
    {
      Transform tf = steps[Count].transform;
      spoon.MoveToPos(tf.position + offset);
    }
  }
}
