
using DG.Tweening;
using Satisgame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AnhPD
{
  public class APDLevelBase : LevelBase
  {

    protected override void Awake()
    {
      base.Awake();

      maxLayer = 30;
    }

    [FoldoutGroup("AnhPD Base")] public EmojiControl emoji;
    [FoldoutGroup("AnhPD Base")] public static int maxLayer = 30;
    [FoldoutGroup("AnhPD Base")][SerializeField] protected Sprite[] hints;
    [FoldoutGroup("AnhPD Base")][SerializeField] protected GameObject[] stages;

    protected int hintIndex = 0;
    public void NextHint()
    {
      hintIndex++;
      if (hintIndex > hints.Length) return;
      _hint = hints[hintIndex];
    }

    public void SetHintIndex(int index)
    {
      hintIndex = index;
      if (hintIndex >= hints.Length) return;
      _hint = hints[hintIndex];
    }
    public void OnCompleteStage(int currentStageIndex, float x)
    {
      emoji.ShowPositive();
      Camera.main.transform.DOMoveX(x, 1f).SetDelay(1f);
      this.WaitToDo(() =>
      {
        stages[currentStageIndex].SetActive(false);
      }, 1.5f);
    }

    public void IncreaseMaxLayer(int increment = 4)
    {
      maxLayer += increment;
    }
  }
}

