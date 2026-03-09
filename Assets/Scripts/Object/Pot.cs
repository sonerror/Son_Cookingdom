using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using DG.Tweening;
using UnityEngine;

public class Pot : MonoBehaviour
{

  public APDv2Drag targetDrag;
  public void OnDone()
  {
    DOVirtual.DelayedCall(1f, () =>
    {
      targetDrag.OnComplete();
      //TutorialManager.Ins.RemoveStep(5);
    });


    // DOVirtual.DelayedCall(3f, () =>
    // {
    //   GameManager.Ins.showEndGame();
    //   // TutorialManager.Ins.FinishTutorial();
    // });

  }
}
