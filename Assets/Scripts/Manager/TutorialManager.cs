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
  [SerializeField] private Transform tfBolt;
  //Step1 
  [SerializeField] private Transform tfMeat;
  //Step2

  [SerializeField] private Transform tfVermicelli;
  //Step3
  [SerializeField] private List<Transform> listItem = new List<Transform>();
  public List<Transform> ListItem => listItem;
  //Step4
  [SerializeField] private List<Transform> listItemBotl = new List<Transform>();
  public List<Transform> ListItemBotl => listItemBotl;
  //Step5
  [SerializeField] private Transform tfSpoon;
  //Step6
  [SerializeField] private Transform tfCuttingBoard;
  //step7
  [SerializeField] private List<Transform> listMando = new List<Transform>();
  public List<Transform> ListMando => listMando;


  [SerializeField] private Transform tfOil;
  [SerializeField] private Transform tfPan;

  void ShowHint()
  {
    if (disableHand) return;
    enableCountTime = false;
    int index = _level.CurrentStep;
    Debug.Log("index: " + index);
    switch (index)
    {
      case 0:
        ShowHintPosToPos(tfMeat, tfBolt);
        return;
      case 1:
        ShowHintPosToPos(tfVermicelli, tfBolt);
        return;
      case 2:
        TutorialStepList(0, listItem);
        return;
      case 3:
        TutorialStepList(0, listItemBotl);
        return;
      case 4:
        ShowHintPosToPos(tfSpoon, tfBolt);
        return;
      case 5:
        handCtrl.gameObject.SetActive(true);
        handCtrl.ShowHandPosToPosToPos(tfSpoon.position, tfBolt.position, tfCuttingBoard.position);
        return;
      case 6:
        TutorialStep6(0, listMando);
        return;
      case 7:
        ShowHintPosToPos(tfOil, tfPan);
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
  private void TutorialStepList(int indexStep, List<Transform> _tfItem)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, tfBolt.position);
  }
  private void TutorialStep6(int indexStep, List<Transform> _tfItem)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, obj.position);
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