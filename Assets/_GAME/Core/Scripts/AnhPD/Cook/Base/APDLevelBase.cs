
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

    public EmojiControl emoji;
    public static int maxLayer = 30;



    public void OnCompleteStage(int currentStageIndex, float x)
    {
      emoji.ShowPositive();
      Camera.main.transform.DOMoveX(x, 1f).SetDelay(1f);
    }

    public void IncreaseMaxLayer(int increment = 4)
    {
      maxLayer += increment;
    }

    public void ShowNegativeEmojiAtPos(Vector3 position)
    {
      emoji.transform.position = position + Vector3.up * 0.5f + Vector3.left * 0.5f;
      emoji.ShowNegative();
    }

    public void ShowPositiveEmojiAtPos(Vector3 position)
    {
      emoji.transform.position = position + Vector3.up * 0.5f + Vector3.left * 0.5f;
      emoji.ShowPositive();
    }
  }
}

