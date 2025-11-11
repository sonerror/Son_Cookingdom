
using System.Collections.Generic;
using System.Linq;
using sonnv;
using UnityEngine;
using UnityEngine.XR;

public class TutorialManager : Singleton<TutorialManager>
{
  [SerializeField] private HandCtrl handCtrl;
  [SerializeField] float TimeHint = 5f;
  private float timeCountHint = 2f;

  public float timeEndGame = 30f;

  public Transform CandyBag, CandyBowl, Oven, ChocoBowl, FlourBowl, Pot;

  private bool isEndGame = false;

  private int[] tutData = new int[] { 0, 1, 2, 3 };

  public void RemoveStep(int step)
  {
    tutData = tutData.Where(x => x != step).ToArray();
    ResetTimeHint();
  }

  private void Update()
  {
    if (isEndGame) return;

    if (Input.GetMouseButtonDown(0))
    {
      ResetTimeHint();
    }

    if (Input.GetMouseButton(0)) return;

    CalculateTimeHint();
  }

  private bool isShowHint = false;
  private bool enableCountTime = true;
  private void CalculateTimeHint()
  {
    if (!enableCountTime) return;
    if (isShowHint) return;
    timeCountHint -= Time.deltaTime;
    if (timeCountHint <= 0)
    {
      PlayTutState();
    }
  }

  private void PlayTutState()
  {
    if (isEndGame) return;
    isShowHint = true;

    if (tutData.Length == 0)
    {
      handCtrl.gameObject.SetActive(false);
      return;
    }


    var currstate = tutData[0];
    switch (currstate)
    {
      case 0:
        PlayTutState0();
        break;
      case 1:
        PlayTutState1();
        break;
      case 2:
        PlayTutState2();
        break;
      case 3:
        PlayTutState3();
        break;
      default:
        handCtrl.gameObject.SetActive(false);
        break;
    }
  }

  private void PlayTutState0()
  {
    Debug.Log("CandyBag");
    var pos2 = CandyBag.position;
    handCtrl.ShowHandAtPos(pos2);
  }
  private void PlayTutState1()
  {
    var pos1 = CandyBowl.position;
    var pos2 = Oven.position;
    handCtrl.ShowHandPosToPos(pos1, pos2);
  }
  private void PlayTutState2()
  {
    var pos1 = ChocoBowl.position;
    var pos2 = Pot.position;
    handCtrl.ShowHandPosToPos(pos1, pos2);
  }
  private void PlayTutState3()
  {
    var pos1 = FlourBowl.position;
    var pos2 = Pot.position;
    handCtrl.ShowHandPosToPos(pos1, pos2);


  }

  public void ResetTimeHint()
  {
    if (isEndGame) return;

    StopState();
    this.isShowHint = false;
    this.timeCountHint = TimeHint;
  }

  public void MouseDownItem()
  {
    if (isEndGame) return;

    enableCountTime = false;
    handCtrl.HideHand();
  }

  public void MouseUpItem()
  {
    if (isEndGame) return;

    enableCountTime = true;
    ResetTimeHint();
  }

  public void StopState()
  {
    if (isEndGame) return;

    handCtrl.HideHand();
  }

  public void FinishTutorial()
  {
    isEndGame = true;
    var pos1 = FlourBowl.position;
    var pos2 = Pot.position;
    handCtrl.ShowHandPosToPos(pos1, pos2);
    // handCtrl.gameObject.SetActive(false);
  }
}
