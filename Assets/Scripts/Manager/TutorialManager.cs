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

  //Step1 
  [SerializeField] private Transform tfBoard;
  [SerializeField] private Transform tfKiwi;
  //step2
  [SerializeField] private Transform tfSpatula0;
  [SerializeField] private Transform tfSpatula1;
  [SerializeField] private Transform tfSpatula2;
  [SerializeField] private int countStep2 = 0;
  public void CountStep2()
  {
    countStep2++;
    Debug.Log("countStep2 : " + countStep2);
  }
  //step2
  [SerializeField] private Transform tfMilk;
  //step5
  [SerializeField] private bool isSnapSpatula = false;
  public void SetIsSnapSpatula()
  {
    isSnapSpatula = true;
  }
  [SerializeField] private List<Transform> listTfSpatula = new List<Transform>();
  [SerializeField] private List<Transform> listTfTargetSpatula = new List<Transform>();
  public List<Transform> ListTfSpatula => listTfSpatula;
  public List<Transform> ListTfTargetSpatula => listTfTargetSpatula;
  private void TutorialStep5(int indexStep)
  {
    if (handCtrl == null) return;
    if (indexStep >= listTfSpatula.Count || indexStep >= listTfTargetSpatula.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = listTfSpatula[indexStep];
    var node = listTfTargetSpatula[indexStep];
    handCtrl.ShowHandPosToPos(obj.position, node.position);
  }
  [SerializeField] private int countStep5 = 0;
  public void CountStep5()
  {
    countStep5++;
  }


  //step6

  [SerializeField] private bool isSnapSpatulaStep6 = false;
  public void SetIsSnapSpatulaStep6()
  {
    isSnapSpatulaStep6 = true;
  }
  [SerializeField] private CreamRollController creamRollController;



  //step7 
  [SerializeField] private Transform tfBowl;
  [SerializeField] private Transform tfCreamDone;

  //step8
  [SerializeField] private bool isSnapBowlChoco = false;
  public void SetIsSnapBowlChoco()
  {
    isSnapBowlChoco = true;
  }
  [SerializeField] private Transform tfBowlChoco;
  //step9
  [SerializeField] private Transform tfPet;
  //step10
  [SerializeField] private Transform tfWaterLemnon;

  void ShowHint()
  {
    if (disableHand) return;
    enableCountTime = false;
    int index = _level.CurrentStep;
    switch (index)
    {
      case 0:
        ShowHintPosToPos(tfKiwi, tfBoard);
        return;
      case 1:
        if (countStep2 == 0)
        {
          ShowHintPosToPos(tfSpatula0, tfBoard);
        }
        if (countStep2 == 1)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfSpatula1.position + (Vector3.up * 0.5f), tfSpatula1.position + (Vector3.up * 0.5f));
        }
        if (countStep2 == 2)
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfSpatula2.position + (Vector3.up * 0.5f), tfSpatula2.position + (Vector3.up * 0.5f));
        }
        return;
      case 2:
        ShowHintPosToPos(tfMilk, tfBoard);
        return;
      case 3:
        ShowHintPosToPos(tfSpatula0, tfBoard);
        return;
      case 4:
        if (isSnapSpatula == false)
        {
          ShowHintPosToPos(tfSpatula0, tfBoard);
        }
        else
        {
          TutorialStep5(countStep5);
        }
        return;
      case 5:
        if (isSnapSpatulaStep6 == false)
        {
          ShowHintPosToPos(tfSpatula0, tfBoard);
        }
        else
        {
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(creamRollController.StartPos, creamRollController.EndPos);
        }
        return;
      case 6:
        ShowHintPosToPos(tfCreamDone, tfBowl);
        return;
      case 7:
        if (isSnapBowlChoco == false)
        {
          ShowHintPosToPos(tfBowlChoco, tfBowl);
        }
        else
        {
          TutorialStep7(0);
        }
        return;
      case 8:
        ShowHintPosToPos(tfBowl, tfPet);

        return;
      case 9:
        ShowHintPosToPos(tfWaterLemnon, tfBoard);

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
    handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, tfBowl.position);
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