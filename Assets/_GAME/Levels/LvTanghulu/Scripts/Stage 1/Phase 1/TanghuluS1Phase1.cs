using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class TanghuluS1Phase1 : APDProgressionPhase
  {
    [SerializeField] private GeneralPreparation[] preparations;
    [SerializeField] private APDv2Drag[] caneDrag, caneCutDrag;
    [SerializeField] private APDv2Drag knife;
    [SerializeField] private CaneJuicer juicer;
    [SerializeField] private Transform strainer;
    [SerializeField] private APDv2Drag bowl;
    private int _index;
    private bool _isBoardEmpty = true;
    protected override void Setup()
    {
      base.Setup();
      knife.onComplete.AddListener(OnUseKnife);
      for (int i = 0; i < caneDrag.Length; i++)
      {
        caneDrag[i].OnReady();
        var i1 = i;
        caneDrag[i].onComplete.AddListener(() => PreparationCane(i1));
        caneDrag[i].SetCondition(() => _isBoardEmpty);
      }
      foreach (var t in caneCutDrag)
      {
        t.OnReady();
        t.onComplete.AddListener(juicer.OnPutCaneIn);
        t.onComplete.AddListener(ClearBoard);
      }

      juicer.OnFullCane += () => DoneStepText(3);
      juicer.OnComplete += OnCompleteJuice;
    }

    private int _caneCount = 0;
    private void PreparationCane(int index)
    {
      _isBoardEmpty = false;
      _index = index;
      preparations[index].StartPreparation();
    }

    public void DonePeelCane()
    {
      DoneStepText(1);
    }
    private void OnUseKnife()
    {
      preparations[_index].OnHaveTool();
    }

    private void ClearBoard()
    {
      DoneStepText(2);
      _isBoardEmpty = true;
    }
    private void OnCompleteJuice()
    {
      DoneStepText(4);
      strainer.MoveX(-5f, .5f, true, 0, () =>
      {
        bowl.OnReady();
        bowl.EnableCollider();
      });
    }

    public override void OnComplete()
    {
      DoneStepText(5);
      // APDLevelBase.Ins.emoji.ShowPositive();
      transform.MoveX(-10f, 1f, true, .5f, () => base.OnComplete());
    }
  }
}
