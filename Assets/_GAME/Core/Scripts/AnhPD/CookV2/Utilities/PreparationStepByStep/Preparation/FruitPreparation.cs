using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class FruitPreparation : MonoBehaviour
  {
    [SerializeField] protected GeneralPreparation general;
    [SerializeField] protected FruitPreparationStep[] steps;
    public UnityEvent onStart, onComplete;
    public bool isNeedTool = false;

    protected int Count = 0;

    protected virtual void OnEnable()
    {
      onStart?.Invoke();
    }

    public virtual void OnHaveTool()
    {
    }

    public virtual void OnRemoveTool()
    {

    }
    public virtual void OnDoneStep()
    {
      Count++;
      if (Count == steps.Length)
      {
        OnComplete();
      }
    }

    protected virtual void OnComplete()
    {
      general.OnDonePreparation();
      onComplete?.Invoke();
      gameObject.SetActive(false);
    }


#if UNITY_EDITOR
    public void SetGeneral(GeneralPreparation general)
    {
      this.general = general;
      Setup();
    }
    [Button]
    private void Setup()
    {
      steps = GetComponentsInChildren<FruitPreparationStep>(true);
      foreach (FruitPreparationStep step in steps)
      {
        step.Setup(this);
      }
    }
#endif
  }
}
