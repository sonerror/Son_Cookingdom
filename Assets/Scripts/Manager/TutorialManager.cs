using System.Collections;
using System.Collections.Generic;
using HoangHH;
using UnityEngine;
using sonnv;
using DG.Tweening;


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

  //step1
  [SerializeField] private Transform tfBtnOn;
  //step2
  [SerializeField] private Transform tfPot;
  [SerializeField] private Transform tfBotlWater;
  //step3
  [SerializeField] private Transform tfBotlSauce;
  //step4
  [SerializeField] private Transform tfBotlSiro;
  //step5
  [SerializeField] private Transform tfBotlFried;

  [SerializeField] private Transform tfCuttingBoard;

  //step9
  [SerializeField] private Transform tfSeaweed;
  //step10
  [SerializeField] private Transform tfBowlFood;
  //step11
  [SerializeField] private Transform tfRoll;
  [SerializeField] private Transform tfRollTarget;
  //step12
  [SerializeField] private Transform tfKnife;
  [SerializeField] private Transform tfKnifeInBoard;
  //step13
  [SerializeField] private Transform tfBotlDone;

  [SerializeField] private Transform tfUnamilDone;

  //step15
  [SerializeField] private Transform tfObj1;
  [SerializeField] private Transform tfObj2;

  //step15
  [SerializeField] private Transform tfLotus;
  [SerializeField] private Transform tfBlend;
  [SerializeField] private Transform tfBtnStep2;
  [SerializeField] private Transform tfSpoon;

  void ShowHint()
  {
    if (disableHand) return;
    enableCountTime = false;
    int index = _level.CurrentStep;
    Debug.Log("index: " + index);
    switch (index)
    {
      case 0:
        ShowHintPosToPos(tfBtnOn, tfBtnOn);
        return;
      case 1:
        ShowHintPosToPos(tfBotlWater, tfPot);
        return;
      case 2:
        ShowHintPosToPos(tfBotlSauce, tfPot);
        return;
      case 3:
        ShowHintPosToPos(tfBotlSiro, tfPot);
        return;
      case 4:
        return;
      case 5:
        ShowHintPosToPos(tfBotlFried, tfPot);
        return;
      case 6:
        return;
      case 7:
        ShowHintPosToPos(tfPot, tfCuttingBoard);
        return;
      case 8:
        ShowHintPosToPos(tfSeaweed, tfCuttingBoard);
        return;
      case 9:
        ShowHintPosToPos(tfBowlFood, tfCuttingBoard);
        return;
      case 10:
        ShowHintPosToPos(tfRoll, tfRollTarget);
        return;
      case 11:
        ShowHintPosToPos(tfKnife, tfCuttingBoard);
        return;
      case 12:
        ShowHintPosToPos(tfKnifeInBoard, tfKnifeInBoard);
        return;
      case 13:
        ShowHintPosToPos(tfUnamilDone, tfBotlDone);
        return;
      case 15:
        ShowHintPosToPos(tfObj1, tfObj2);
        return;
      case 16:
        ShowHintPosToPos(tfLotus, tfBlend);
        return;
      case 17:
        ShowHintPosToPos(tfBtnStep2, tfBtnStep2);
        return;
      case 18:
        ShowHintPosToPos(tfSpoon, tfBlend);
        return;
      default:
        resetTimeHint();
        return;
    }
  }
  private Vector3 _centerPos;
  private float _radius;
  private Tween _spinTween;
  public void ShowHandSpinContinuous(Vector3 center, float radius)
  {
    if (handCtrl == null) return;

    handCtrl.transform.DOKill();
    _spinTween?.Kill();

    handCtrl.gameObject.SetActive(true);

    if (handCtrl.animator != null)
    {
      handCtrl.animator.Play("Hand");
      handCtrl.animator.SetTrigger("HandDown");
    }

    _spinTween = DOVirtual.Float(0f, 360f, 1.5f, (angle) =>
    {
      float rad = angle * Mathf.Deg2Rad;

      handCtrl.transform.position = center + new Vector3(
          Mathf.Cos(rad) * radius,
          Mathf.Sin(rad) * radius,
          0
      );
    })
    .SetLoops(-1, LoopType.Restart)
    .SetEase(Ease.Linear);
  }
  public void StopSpinOnly()
  {
    _spinTween?.Kill();
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

  private void TutorialStep6(int indexStep, List<Transform> _tfItem)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, obj.position);
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