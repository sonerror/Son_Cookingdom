using System.Collections.Generic;
using System.Linq;
using AnhPD;
using DG.Tweening;
using sonnv;
using UnityEngine;
using System.Collections;

public class TutorialManager : Singleton<TutorialManager>
{
  public bool disableHand = false;

  [SerializeField] float TimeHint = 5f;
  public bool enableCountTime = false;

  public float timeCountHint = 2f;

  [SerializeField] public HandCtrl handCtrl;

  [SerializeField] private Level1528 _level;

  [SerializeField] private int countHintStep1 = 0;
  public int CountHintStep1
  {
    get { return countHintStep1; }
    set { countHintStep1 = value; }
  }

  private int CountStepDone = 0;

  //Step1
  [SerializeField] private Transform Step1Tf1Tut1;
  [SerializeField] private Transform Step1Tf2Tut1;
  [SerializeField] private Transform Step1TfTut2;

  //Step2
  [SerializeField] private List<Transform> tfItem = new List<Transform>();
  public List<Transform> TfItem => tfItem;

  [SerializeField] private Transform tfSink;

  //step3
  [SerializeField] private List<Transform> tfBroad = new List<Transform>();
  public List<Transform> TfBroad => tfBroad;

  [SerializeField] private bool canCutItem = false;
  public bool CanCutItem => canCutItem;

  public void SetCanCutItem(bool value)
  {
    canCutItem = value;
  }

  IEnumerator IEDlaySetCutItem(bool value)
  {
    yield return new WaitForSeconds(0.001f);
    canCutItem = value;
  }

  public void SetDelayCanCutItem(bool value)
  {
    StartCoroutine(IEDlaySetCutItem(value));
  }

  int countCollectFail = 0;

  private bool isTap = false;

  [SerializeField] private CuttingBoard cuttingBoard;

  [SerializeField] private int stepInPhase = 0;
  public int StepInPhase => stepInPhase;

  //Step broad 
  [SerializeField] private Transform tfBroadCenter;
  [SerializeField] private Transform tfKnife;
  [SerializeField] private Transform tfKnifeInBroad;

  [SerializeField] private bool isSnapKinfe = false;

  //step4 
  [SerializeField] private Transform lidOutSink;
  [SerializeField] private Transform lidInSink;

  //step5
  [SerializeField] private Transform tfItem1;
  [SerializeField] private Transform tfItem2;
  // dataCanCut
  [SerializeField] private bool canItemInBroad = false;
  public void SetCanItemInBroad(bool value)
  {
    canItemInBroad = value;
  }
  void Start()
  {
    if (_level == null)
      _level = FindObjectOfType<Level1528>();

    if (handCtrl == null)
      Debug.LogError("HandCtrl missing in TutorialManager");
  }

  public void SetIsSnapKnife(bool value)
  {
    isSnapKinfe = value;
  }

  public void IncreaseCountHintStep1()
  {
    stepInPhase++;
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
    {
      HideHint();
      timeCountHint = countCollectFail >= 3 ? 1.5f : TimeHint;
      enableCountTime = true;
      return;
    }

    if (!enableCountTime) return;

    if (handCtrl != null && handCtrl.gameObject.activeInHierarchy)
      return;

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
    // if (disableHand) return;
    // if (_level == null) return;
    // if (handCtrl == null) return;

    // enableCountTime = false;

    // int index = _level.CurrentStep;

    // switch (index)
    // {
    //   case 0:

    //     if (StepInPhase == 0)
    //     {
    //       handCtrl.ShowHandPosToPos(Step1Tf1Tut1.position, Step1Tf2Tut1.position);
    //     }

    //     if (StepInPhase == 1)
    //     {
    //       handCtrl.ShowHandLoop(Step1TfTut2.position, Step1TfTut2.position);
    //     }

    //     return;

    //   case 1:

    //     TutorialStep1(0);
    //     return;

    //   case 2:

    //     if (cuttingBoard == null) return;

    //     var cuttingObj = cuttingBoard.CurrentCuttingObject;

    //     if (cuttingObj == null)
    //     {
    //       TutorialStep2(0);
    //       return;
    //     }
    //     canCutItem = !cuttingBoard.InforTFTarget.IsSnap;
    //     if (canCutItem)
    //     {
    //       if (cuttingObj.IsTapItem)
    //       {
    //         if (cuttingObj.TapItem != null && cuttingObj.TapItem.IsTap)
    //         {
    //           if (isSnapKinfe)
    //           {
    //             handCtrl.ShowHandLoop(tfBroadCenter.position, tfBroadCenter.position);
    //           }
    //           else
    //           {
    //             if (cuttingObj.CutDone)
    //               handCtrl.ShowHandLoop(tfBroadCenter.position, tfBroadCenter.position);
    //             else
    //               handCtrl.ShowHandLoop(tfKnife.position, tfBroadCenter.position);
    //           }
    //         }
    //         else
    //         {
    //           handCtrl.ShowHandLoop(tfBroadCenter.position, tfBroadCenter.position);
    //         }
    //       }
    //       else
    //       {
    //         if (cuttingObj.CutDone)
    //           handCtrl.ShowHandLoop(tfBroadCenter.position, tfBroadCenter.position);
    //         else
    //           handCtrl.ShowHandLoop(tfKnife.position, tfBroadCenter.position);
    //       }
    //     }
    //     else
    //     {
    //       TutorialStep2(0);
    //     }

    //     return;

    //   case 3:

    //     handCtrl.ShowHandLoop(lidInSink.position, lidOutSink.position);
    //     return;

    //   case 4:

    //     handCtrl.ShowHandLoop(tfItem1.position, tfItem2.position);
    //     return;
    // }
  }

  [SerializeField] private int countHint3 = 0;

  public void CountHint3()
  {
    countHint3++;
  }

  private void TutorialStep2(int indexStep)
  {
    if (indexStep >= tfBroad.Count) return;

    handCtrl.ShowHandLoop(tfBroad[indexStep].position, tfBroad[indexStep].position);
  }

  private void TutorialStep1(int indexStep)
  {
    if (handCtrl == null) return;
    if (indexStep >= tfItem.Count) return;

    var obj = tfItem[indexStep];

    if (obj == null || tfSink == null) return;

    handCtrl.gameObject.SetActive(true);
    handCtrl.ShowHandPosToPos(obj.position, tfSink.position);
  }

  private void TutorialOneHit(List<Transform> listTF, int index)
  {
    if (index >= listTF.Count) return;

    handCtrl.ShowHandAtPos(listTF[index].position);
  }

  public void SetStateIsTap(bool value)
  {
    isTap = value;
  }

  void HideHint()
  {
    if (handCtrl != null)
      handCtrl.gameObject.SetActive(false);
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
}