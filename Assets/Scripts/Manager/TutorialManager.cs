using System.Collections;
using System.Collections.Generic;
using HoangHH;
using UnityEngine;
using sonnv;

public class TutorialManager : Singleton<TutorialManager>
{
  public bool disableHand = false;
  [SerializeField] float TimeHint = 5f;
  public bool enableCountTime = false;
  public float timeCountHint = 2f;
  [SerializeField] public HandCtrl handCtrl;
  [SerializeField] public HandCtrl handCtrlMakeup;
  [SerializeField] private LevelControl _level;

  [SerializeField] private int countHintStep1 = 0;
  [SerializeField] private Transform tfStraw;
  [SerializeField] private Transform tfBoard;
  [SerializeField] private Transform tfMortar;
  [SerializeField] private Transform tfKnife;
  [SerializeField] private CuttingBoard snapPointKnife;
  public int CountHintStep1
  {
    get { return countHintStep1; }
    set { countHintStep1 = value; }
  }
  public void IncreaseCountHintStep1()
  {
    countHintStep1++;
  }
  [SerializeField] private Transform tfMango;
  [SerializeField] private Transform tfMacca;
  [SerializeField] private ItemInBoard itemMaccaInBoard;
  [SerializeField] private Transform tfHandTapMiniGame;
  [SerializeField] private TapHand tapHand;

  [SerializeField] private Transform tfWalnut;
  [SerializeField] private ItemInBoard itemWalnutInBoard;


  [SerializeField] private Transform tfBtnOven;
  [SerializeField] private Transform tfPistachio;
  [SerializeField] private Transform tfPan;
  [SerializeField] private Transform tfChopsticks;

  private int CountStepDone = 0;

  [SerializeField] private List<Transform> tutorialNode = new List<Transform>();
  [SerializeField] private List<Transform> tfItem = new List<Transform>();
  public List<Transform> TutorialNode => tutorialNode;
  public List<Transform> TfItem => tfItem;
  [SerializeField] private List<Transform> tfItemMakeup = new List<Transform>();
  [SerializeField] private Transform tfFace;
  [SerializeField] private Transform tfHair;
  int countCollectFail = 0;
  private bool isTap = false;

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      HideHint();
      timeCountHint = countCollectFail >= 3 ? 1.5f : TimeHint;
      enableCountTime = true;
      return;
    }
    if (Input.GetMouseButton(0))
    {
      return;
    }
    if (!enableCountTime) return;
    if (handCtrl.gameObject.activeSelf) return;
    if (handCtrlMakeup.gameObject.activeSelf) return;
    CalculateTimeHint();
  }
  private void CalculateTimeHint()
  {
    timeCountHint -= Time.deltaTime;
    if (timeCountHint <= 0)
    {
      ShowHint();
    }
  }
  void ShowHint()
  {
    if (disableHand) return;
    enableCountTime = false;
    int index = _level.CurrentStep;
    switch (index)
    {
      case 0:
        if (countHintStep1 == 0)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfStraw.position, tfStraw.position);
        }
        if (countHintStep1 == 1)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfStraw.position, tfBoard.position);

        }
        return;
      case 1:
        if (snapPointKnife.SnapPointKnife.canSnap == true && snapPointKnife.SnapPointKnife.isSnap == true)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfBoard.position, tfBoard.position);
        }
        else
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfKnife.position, tfBoard.position);
        }
        return;
      case 2:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tfMango.position, tfBoard.position);
        return;
      case 3:
        if (snapPointKnife.SnapPointKnife.canSnap == true && snapPointKnife.SnapPointKnife.isSnap == true)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfBoard.position, tfBoard.position);
        }
        else
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfKnife.position, tfBoard.position);
        }
        return;
      case 4:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tfMacca.position, tfBoard.position);
        return;
      case 5:
        if (itemMaccaInBoard.IsDone == true)
        {
          if (tapHand.SnapPoint.canSnap == true && tapHand.SnapPoint.isSnap == true)
          {
            handCtrl.gameObject.SetActive(true);
            handCtrl.ShowHandPosToPos(tfMortar.position, tfMortar.position);
          }
          else
          {
            handCtrl.gameObject.SetActive(true);
            handCtrl.ShowHandPosToPos(tfBoard.position, tfMortar.position);
          }
        }
        else
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfHandTapMiniGame.position, tfHandTapMiniGame.position);
        }
        return;
      case 6:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tfWalnut.position, tfBoard.position);
        return;
      case 7:
        if (itemWalnutInBoard.IsDone == true)
        {
          if (tapHand.SnapPoint.canSnap == true && tapHand.SnapPoint.isSnap == true)
          {
            handCtrl.gameObject.SetActive(true);
            handCtrl.ShowHandPosToPos(tfMortar.position, tfMortar.position);
          }
          else
          {
            handCtrl.gameObject.SetActive(true);
            handCtrl.ShowHandPosToPos(tfBoard.position, tfMortar.position);
          }
        }
        else
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfHandTapMiniGame.position, tfHandTapMiniGame.position);
        }

        return;
      case 8:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tfBtnOven.position, tfBtnOven.position);
        return;
      case 9:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tfPistachio.position, tfPan.position);
        return;
      case 10:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tfChopsticks.position, tfPan.position);
        return;
      default:
        resetTimeHint();
        return;
    }
  }
  private void Tutorial(int indexStep)
  {

  }
  private void TutorialStep1(int indexStep)
  {
    if (handCtrl == null) return;
    if (indexStep >= tfItem.Count || indexStep >= tutorialNode.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = tfItem[indexStep];
    var node = tutorialNode[indexStep];
    handCtrl.ShowHandPosToPos(obj.position, node.position);
  }
  private void TutorialStepMakeUp(int indexStep, Transform tf)
  {
    if (handCtrlMakeup == null) return;
    if (indexStep >= tfItemMakeup.Count || tf == null) return;
    handCtrlMakeup.gameObject.SetActive(true);
    var obj = tfItemMakeup[indexStep];
    handCtrlMakeup.ShowHandPosToPos(obj.position, tf.position);
  }
  public void SetStateIsTap(bool value)
  {
    isTap = value;
  }
  void HideHint()
  {
    handCtrl.gameObject.SetActive(false);
    handCtrlMakeup.gameObject.SetActive(false);
  }
  public void resetTimeHint()
  {
    HideHint();
    timeCountHint = countCollectFail >= 3 ? 1.5f : TimeHint;
    enableCountTime = true;
  }
  public void OnStepDone()
  {
    CountStepDone++;
  }
  public void IncreaseTimeHide()
  {
    TimeHint = 5f;
    resetTimeHint();
  }
  public void OnCollectFail()
  {
    countCollectFail++;

    if (countCollectFail >= 3)
    {
      timeCountHint = 1.5f;
    }
  }
  public void OnCollectSuccess()
  {
    countCollectFail = 0;
    resetTimeHint();
  }
  public void SetNewTime(float timer)
  {
    TimeHint = timer;
    timeCountHint = timer;
  }

}