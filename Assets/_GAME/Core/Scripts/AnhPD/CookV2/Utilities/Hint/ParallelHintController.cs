using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.CookV2
{
  [Serializable]
  public class ParallelHintController
  {
    public SequenceHintGroup[] hintGroups;
    public int startIndex;
    [ReadOnly] public int endIndex;
    public ParallelHintController(int startIndex)
    {
      this.startIndex = startIndex;
    }

    [Button]
    public void Init()
    {
      int index = startIndex;
      for (int i = 0; i < hintGroups.Length; i++)
      {
        hintGroups[i].startIndex = index;
        index += hintGroups[i].stepNumber;
      }
      endIndex = hintGroups[hintGroups.Length - 1].startIndex + (hintGroups[hintGroups.Length - 1].stepNumber - 1);
    }

    public int GetUnfinishedIndex()
    {
      for (int i = 0; i < hintGroups.Length; i++)
      {
        if (!hintGroups[i].IsDone)
        {
          return hintGroups[i].CurrentIndex;
        }
      }
      return endIndex;
    }

    public int GetCurrentIndexOfGroup(int groupIndex)
    {
      return hintGroups[groupIndex].CurrentIndex;
    }
    public int GetStepIndexOfGroup(int groupIndex)
    {
      return hintGroups[groupIndex].currentStep;
    }
    public void OnDoneStepOfHintGroup(int groupIndex)
    {
      hintGroups[groupIndex].OnDoneStep();
    }

    public void RestartGroup(int groupIndex)
    {
      hintGroups[groupIndex].Restart();
    }

    public void RestartAll()
    {
      foreach (var t in hintGroups) t.Restart();
    }

    public void DoneAllStepOfGroup(int groupIndex)
    {
      hintGroups[groupIndex].DoneAllStep();
    }

    public bool IsGroupFinished(int groupIndex)
    {
      return hintGroups[groupIndex].IsDone;
    }
  }

  [Serializable]
  public class SequenceHintGroup
  {
    public int startIndex;
    public int stepNumber = 1;
    public int currentStep = 0;

    public int CurrentIndex => currentStep + startIndex;
    public bool IsDone => currentStep == stepNumber;

    public void OnDoneStep()
    {
      if (currentStep == stepNumber) return;
      currentStep++;
    }

    public void Restart()
    {
      currentStep = 0;
    }

    public void DoneAllStep()
    {
      currentStep = stepNumber;
    }
  }
}
