using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class TanghuluS1Phase2 : APDProgressionPhase
  {
    [SerializeField] private APDv2Drag[] dirtyDrag;
    [SerializeField] private DirtyObject[] dirtyInSink;
    [SerializeField] private APDv2Drag[] cleanDrag;
    [SerializeField] private GeneralPreparation[] preparations;
    [SerializeField] private SimpleSink sink;
    [SerializeField] private GarbageCan garbageCan;
    [SerializeField] private APDv2Drag knife;

    private bool _isBoardEmpty = true;
    private int _index;
    protected override void Setup()
    {
      base.Setup();
      for (int i = 0; i < dirtyDrag.Length; i++)
      {
        var i1 = i;
        dirtyDrag[i].onComplete.AddListener(() =>
        {
          dirtyInSink[i1].transform.Appear();
          dirtyInSink[i1].PutInSink();
          if (sink.IsHaveWater)
          {
            dirtyInSink[i1].OnHaveWater();
            CheckSinkHint();
          }
        });
        dirtyInSink[i].onComplete.AddListener(() =>
        {
          cleanDrag[i1].OnReady();
          cleanDrag[i1].EnableCollider();
        });

        cleanDrag[i].SetCondition(() => _isBoardEmpty);
        cleanDrag[i].onComplete.AddListener(() => Preparation(i1));
      }
      sink.onHaveWater.AddListener(CheckInSink);
      knife.onComplete.AddListener(OnUseKnife);

      for (int i = dirtyDrag.Length; i < cleanDrag.Length; i++)
      {
        var i1 = i;
        cleanDrag[i].OnReady();
        cleanDrag[i].SetCondition(() => _isBoardEmpty);
        cleanDrag[i].onComplete.AddListener(() => Preparation(i1));
      }

      sink.onTurnOff.AddListener(CheckComplete);
      garbageCan.onThrowGarbage.AddListener(CheckComplete);
    }

    private void CheckInSink()
    {
      foreach (var t in dirtyInSink)
      {
        if (t.IsPlaced) t.OnHaveWater();
      }
      CheckSinkHint();
    }

    private void CheckSinkHint()
    {
      foreach (var t in dirtyInSink)
      {
        if (!t.IsDone)
        {
          return;
        }
      }
      DoneStepImageOfGroup(0);
      DoneStepText(1);
    }

    private void Preparation(int index)
    {
      _isBoardEmpty = false;
      _index = index;
      preparations[index].StartPreparation();
      SetHintAccordingToGroup(index + 1);
    }

    private void OnUseKnife()
    {
      preparations[_index].OnHaveTool();
    }

    private int _count;
    public void EmptyBoard()
    {
      _isBoardEmpty = true;
      _count++;
      DoneStepImageOfGroup(_index + 1);
      DoneStepText(_index + 2);
      if (_count >= cleanDrag.Length) CheckComplete();
    }

    private void CheckComplete()
    {
      if (_count >= cleanDrag.Length && !sink.IsWatering && garbageCan.IsCleared) OnComplete();
    }
    public override void OnComplete()
    {
      DoneStepImageOfGroup(8);
      DoneStepText(9);
      // APDLevelBase.Ins.emoji.ShowPositive();
      transform.MoveX(-10f, 1f, true, 1f, () => base.OnComplete());
    }
  }
}
