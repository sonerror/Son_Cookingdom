using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class APDProgressionPhase : MonoBehaviour
  {
    public int phaseIndex;
    public Action<int> OnCompleteAction;

    public UnityEvent onStart, onComplete;

    protected virtual void Awake()
    {
      Setup();
    }

    protected virtual void Setup()
    {

    }

    [Button]
    public virtual void OnStart()
    {
      onStart?.Invoke();
      gameObject.SetActive(true);
    }

    [Button]
    public virtual void OnComplete()
    {
      onComplete?.Invoke();
      OnCompleteAction?.Invoke(phaseIndex);
    }



    public void DoneStepText(int stepIndex = 0)
    {
      // LevelBase.Ins.SetDonePhaseAndStep(hintTextPhase, stepIndex);
    }
  }
}
