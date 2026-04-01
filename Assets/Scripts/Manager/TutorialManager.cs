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
  private int CountStepDone = 0;
  [SerializeField] private List<Transform> tutorialNode = new List<Transform>();
  [SerializeField] private List<Transform> tfItem = new List<Transform>();
  public List<Transform> TutorialNode => tutorialNode;
  public List<Transform> TfItem => tfItem;
  [SerializeField] private List<Transform> tfItemMakeup = new List<Transform>();
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
  [SerializeField] private bool isTapPaper = false;

  [SerializeField] private Transform tfPapper;
  [SerializeField] private Transform tfBtnBack;
  [SerializeField] private bool isTapClose = false;
  public void ChangeTapClose(bool value)
  {
    isTapClose = value;
  }
  [SerializeField] private List<Transform> listTfListShopping = new List<Transform>();
  public List<Transform> ListTfListShopping => listTfListShopping;
  [SerializeField] private Transform tfCart;
  private void TutorialStepShopping(int indexStep)
  {
    if (handCtrl == null) return;
    if (indexStep >= listTfListShopping.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = listTfListShopping[indexStep];
    handCtrl.ShowHandPosToPos(obj.position, obj.position);
  }
  //Step2
  [SerializeField] private Transform tfToolPink;
  [SerializeField] private Transform tfCakePink;
  [SerializeField] private Transform tfToolGreen;
  [SerializeField] private Transform tfCakeGreen;
  [SerializeField] private bool isDoneCakePink = false;
  public void ChangeIsDoneCake(bool value)
  {
    isDoneCakePink = value;
  }


  //Step3 
  [SerializeField] private Transform tfTray;
  [SerializeField] private Transform tfOven;
  [SerializeField] private bool isDoneCake = false;
  [SerializeField] private Transform tfTrayDone;
  //step4
  [SerializeField] private Transform tfCakeFisrt;
  //step5
  [SerializeField] private List<Transform> listTfListCream = new List<Transform>();
  public List<Transform> ListTfListCream => listTfListCream;
  [SerializeField] private Transform tfBowl;
  [SerializeField] private LevelControl levelControl;

  [SerializeField] private Transform tfMix;
  [SerializeField] private Transform tfToolCream;

  [SerializeField] private Transform trTrayDoneLeft;
  [SerializeField] private Transform trTrayDoneRight;


  [SerializeField] private Transform tf1;
  [SerializeField] private Transform tf2;

  [SerializeField] private bool isDoneCakePinkDone = false;
  public void ChangeIsDoneCakeDone(bool value)
  {
    isDoneCakePinkDone = value;
  }

  private void TutorialStepMixCream(int indexStep)
  {
    if (handCtrl == null) return;
    if (indexStep >= listTfListCream.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = listTfListCream[indexStep];
    handCtrl.ShowHandPosToPos(obj.position, tfBowl.position);
  }
  public void ChangeIsDoneCakeInOven(bool value)
  {
    isDoneCake = value;
  }
  void ShowHint()
  {
    if (disableHand) return;
    enableCountTime = false;
    int index = _level.CurrentStep;
    switch (index)
    {
      case 0:

        if (levelControl.TapPaper.IsOn)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfPapper.position, tfPapper.position);
        }
        else
        {
          TutorialStepShopping(0);
        }
        TutorialManager.Ins.SetNewTime(3f);
        return;
      case 1:
        if (isDoneCakePink == false)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfToolPink.position, tfCakePink.position);
        }
        else
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfToolGreen.position, tfCakeGreen.position);
        }
        return;
      case 2:
        if (isDoneCake == false)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfTray.position, tfOven.position);
        }
        else
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfOven.position, tfTrayDone.position);
        }
        return;
      case 3:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tfCakeFisrt.position, tfCakeFisrt.position);
        return;
      case 4:
        if (levelControl.IsSnapSugar == false)
        {
          TutorialStepMixCream(0);
        }
        if (levelControl.IsSnapVani == false)
        {
          TutorialStepMixCream(1);
        }
        if (levelControl.IsSnapButter == false)
        {
          TutorialStepMixCream(2);
        }
        return;
      case 5:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tfMix.position, tfBowl.position);
        return;
      case 6:
        if (isDoneCakePinkDone == false)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfToolCream.position, trTrayDoneLeft.position);
        }
        else
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfToolCream.position, trTrayDoneRight.position);
        }
        return;
      case 7:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPos(tf1.position, tf2.position);
        return;
      case 8:
        return;
      case 9:

        return;
      case 10:

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