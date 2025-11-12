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
      if (IsHaveHint)
      {
        _hint = hints[0];
        OnHintChange?.Invoke(_hint);
      }
      onStart?.Invoke();
      gameObject.SetActive(true);
    }

    [Button]
    public virtual void OnComplete()
    {
      onComplete?.Invoke();
      OnCompleteAction?.Invoke(phaseIndex);
    }

    #region hint
    [SerializeField] private Sprite[] hints;
    [SerializeField] protected ParallelHintController hintSupporter;
    [SerializeField] private int hintTextPhase;
    private bool IsHaveHint => hints.Length > 0;

    public Action<Sprite> OnHintChange;

    private Sprite _hint;

    public virtual void DoneStepImageOfGroup(int groupIndex = 0)
    {
      if (IsHaveHint)
      {
        hintSupporter.OnDoneStepOfHintGroup(groupIndex);
        _hint = GetUnfinishedHint(groupIndex);
        OnHintChange?.Invoke(_hint);
      }
    }

    public void DoneStepText(int stepIndex = 0)
    {
      // LevelBase.Ins.SetDonePhaseAndStep(hintTextPhase, stepIndex);
    }

    protected Sprite GetUnfinishedHint(int groupIndex = -1)
    {
      if (groupIndex >= 0 && !hintSupporter.IsGroupFinished(groupIndex))
        return GetHintOfGroup(groupIndex);

      return hints[hintSupporter.GetUnfinishedIndex()];
    }

    protected Sprite GetHintOfGroup(int groupIndex)
    {
      return hints[hintSupporter.GetCurrentIndexOfGroup(groupIndex)];
    }

    protected void SetHintAccordingToGroup(int groupIndex)
    {
      _hint = GetUnfinishedHint(groupIndex);
      OnHintChange?.Invoke(_hint);
    }

    protected void ResetHintOfGroup(int groupIndex)
    {
      hintSupporter.RestartGroup(groupIndex);
    }

    protected void ResetAllHint()
    {
      hintSupporter.RestartAll();
    }
    #endregion
  }
}
