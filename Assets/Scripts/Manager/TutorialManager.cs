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
  [SerializeField] private Transform tfFlour;
  [SerializeField] private List<SonSnapObject> listSnapObjHint;
  public List<SonSnapObject> ListSnapObjHint => listSnapObjHint;
  [SerializeField] private List<Transform> listTargetStep1;
  public List<Transform> ListTargetStep1 => listTargetStep1;
  [SerializeField] private bool isSnapSpoon = false;
  [SerializeField] private bool isSnapSpoonDone = false;
  [SerializeField] private Transform tfSpoon;
  [SerializeField] private Transform tfBowl;
  [SerializeField] private Transform tfPlate;
  [SerializeField] private Transform tfPlate1;
  [SerializeField] private Transform tfPlate2;


  public void SetIsSnapSpoon()
  {
    isSnapSpoon = true;
  }
  public void SetIsSnapSpoonDone()
  {
    isSnapSpoonDone = true;
  }
  void ShowHint()
  {
    if (disableHand) return;
    enableCountTime = false;
    int index = _level.CurrentStep;
    Debug.Log("index: " + index);
    switch (index)
    {
      case 0:
        ShowHintPosToPos(tfFlour, tfFlour);
        return;
      case 1:
        if (isSnapSpoon)
        {
          if (isSnapSpoonDone)
          {
            TutorialStep2(0, listSnapObjHint, listTargetStep1);
          }
          else
          {
            handCtrl.gameObject.SetActive(true);
            handCtrl.ShowHandPosToPosToPos(tfSpoon.position, tfBowl.position, tfPlate.position);
          }
        }
        else
        {
          TutorialStep2(0, listSnapObjHint, listTargetStep1);
        }
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
  private void TutorialStep2(int indexStep, List<SonSnapObject> _tfItem, List<Transform> _tfItemTarget)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count && indexStep >= _tfItemTarget.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    var objTarget = _tfItemTarget[indexStep];
    if (_tfItem[indexStep].IsSnaps == false)
    {
      handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, objTarget.gameObject.transform.position);
    }
    else
    {
      handCtrl.ShowHandPosToPosToPos(obj.gameObject.transform.position, tfPlate1.position, tfPlate2.position);
    }
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