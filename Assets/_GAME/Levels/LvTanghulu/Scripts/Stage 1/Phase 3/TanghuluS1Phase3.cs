using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using UnityEngine;
using Utilities;

namespace AnhPD.Tanghulu
{
  public class TanghuluS1Phase3 : APDProgressionPhase
  {
    [SerializeField] private APDv2Drag[] ingredientDrag;
    [SerializeField] private TanghuluChocoPot pot;
    [SerializeField] private Transform bowlBrown, bowlWhite;
    [SerializeField] private Transform left, right;
    [SerializeField] private MoveToTarget move;

    protected override void Setup()
    {
      base.Setup();

      for (int i = 0; i < ingredientDrag.Length; i++)
      {
        int i1 = i;

        ingredientDrag[i].OnReady();
        if (i < 1)
        {
          ingredientDrag[i].SetCondition(() => !pot.isHaveChoco);
          // ingredientDrag[i].onComplete.AddListener(() => SetHintAccordingToGroup(i1));
        }

        ingredientDrag[i].onComplete.AddListener(() => pot.PutInIngredient(i1));
      }

      pot.OnRestart += () =>
      {
        ingredientDrag[ingredientDrag.Length - 1].OnReReady(false);
      };
      pot.OnDone += OnStoveComplete;
      pot.onFullIngredient.AddListener(OnPotFull);

      move.onComplete.AddListener(base.OnComplete);
    }

    private bool _isDonePurple, _isDoneChoco;

    public void OnBowlInOven()
    {
      // DoneStepImageOfGroup(3);
      DoneStepText(7);
    }
    public void DonePurple()
    {
      _isDonePurple = true;
      // DoneStepImageOfGroup(3);
      DoneStepText(8);
      CheckComplete();
    }

    private void OnPotFull()
    {
      // DoneStepImageOfGroup(pot.isBrown ? 0 : 1);
      DoneStepText(pot.isBrown ? 1 : 3);
    }
    public void DoneChoco()
    {
      if (pot.isBrown)
      {
        bowlBrown.MoveX(5f, 1, true, .5f);
        // DoneStepImageOfGroup(0);
        DoneStepText(2);
      }
      else
      {
        bowlWhite.MoveX(5f, 1, true, .5f);
        // DoneStepImageOfGroup(1);
        DoneStepText(4);
      }
    }
    private void OnStoveComplete()
    {
      // DoneStepImageOfGroup(2);
      DoneStepText(5);
      _isDoneChoco = true;
      CheckComplete();
    }
    private void CheckComplete()
    {
      if (_isDonePurple && _isDoneChoco)
      {
        OnComplete();
      }
    }
    public override void OnComplete()
    {
      // APDLevelBase.Ins.emoji.ShowPositive();
      left.MoveX(-10f, 1, true, 1f);
      right.MoveX(10f, 1, true, 1.5f);
      move.Move();
    }
  }
}
