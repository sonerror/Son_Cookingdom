using System.Collections;
using System.Collections.Generic;
using HoangHH;
using UnityEngine;
using sonnv;

public class TutorialManager : Singleton<TutorialManager>
{
  [SerializeField] private bool blockShowHint = false;

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
  [SerializeField] private List<SonSnapObject> tfItem = new List<SonSnapObject>();
  public List<Transform> TutorialNode => tutorialNode;
  public List<SonSnapObject> TfItem => tfItem;
  int countCollectFail = 0;
  private bool isTap = false;
  private void Update()
  {
    if (blockShowHint) return;
    if (Input.GetMouseButtonDown(0) && isTap == false)
    {
      SetNewTime(4f);
      isTap = true;
    }
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
  [SerializeField] private Transform tfBolt;
  //Step1 
  [SerializeField] private List<Transform> listEgg = new List<Transform>();
  public List<Transform> ListEgg => listEgg;

  //step22
  [SerializeField] private List<Transform> listBoltSaltSugar = new List<Transform>();
  public List<Transform> ListBoltSaltSugar => listBoltSaltSugar;
  [SerializeField] private Transform tfSpoon;

  //step3
  [SerializeField] private List<Transform> listBolt = new List<Transform>();
  public List<Transform> ListBolt => listBolt;
  //step4

  [SerializeField] private Transform tfWhisk;
  //sep6 
  [SerializeField] private Transform tfBoltStep2;
  [SerializeField] private Transform tfOil;
  //step7
  [SerializeField] private Transform tfBtnOn;
  //step7
  [SerializeField] private Transform tfBoltEGgg;
  //step8

  [SerializeField] private List<Transform> listItemInPan = new List<Transform>();
  public List<Transform> ListItemInPan => listItemInPan;

  //step9 
  [SerializeField] private Transform tfSpoonStep2;
  [SerializeField] private Transform tfBoltSugarStep2;
  //step10
  [SerializeField] private Transform tfSpoonStep3;

  void ShowHint()
  {
    if (disableHand) return;
    enableCountTime = false;
    int index = _level.CurrentStep;
    Debug.Log("index: " + index);
    switch (index)
    {
      case 0:
        TutorialStepList(0, listEgg);
        return;
      case 1:
        TutorialStepListThree(0, listBoltSaltSugar);
        return;
      case 2:
        TutorialStepList(0, listBolt);
        return;
      case 3:
        ShowHintPosToPos(tfWhisk, tfBolt);
        return;
      case 4:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandSpinContinuous(tfBolt.position, 0.75f);
        return;
      case 5:
        handCtrl.StopSpinOnly();
        ShowHintPosToPos(tfOil, tfBoltStep2);
        return;
      case 6:
        ShowHintPosToPos(tfBtnOn, tfBtnOn);

        return;
      case 7:
        ShowHintPosToPos(tfBoltEGgg, tfBoltStep2);
        return;
      case 8:
        TutorialStep2List(0, listItemInPan);
        return;
      case 9:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPosToPos(tfSpoonStep2.position, tfBoltSugarStep2.position, tfBoltStep2.position);
        return;
      case 11:
        ShowHintPosToPos(tfSpoonStep3, tfBoltStep2);
        return;
      default:
        resetTimeHint();
        return;
    }
  }

  private void Tutorial(int indexStep)
  {

  }
  private void ShowHintPosToPos(Transform _pos1, Transform _pos2)
  {
    handCtrl.gameObject.SetActive(true);
    handCtrl.ShowHandPosToPos(_pos1.position, _pos2.position);
  }
  private void TutorialStep7(int indexStep)
  {
    if (handCtrl == null) return;
    if (indexStep >= tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = tfItem[indexStep];
    //handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, tfBowl.position);
  }
  private void TutorialStepList(int indexStep, List<Transform> _tfItem)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, tfBolt.position);
  }
  private void TutorialStep2List(int indexStep, List<Transform> _tfItem)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, tfBoltStep2.position);
  }
  private void TutorialStepListThree(int indexStep, List<Transform> _tfItem)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    handCtrl.ShowHandPosToPosToPos(tfSpoon.position, _tfItem[indexStep].position, tfBolt.position);
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