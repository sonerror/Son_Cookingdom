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
  public bool enableCountTime = false;

  [SerializeField] public HandCtrl handCtrl;
  [SerializeField] public HandCtrl handCtrlMakeup;
  [SerializeField] private LevelControl _level;
  [SerializeField] private List<Transform> tutorialNode = new List<Transform>();
  [SerializeField] private List<SonSnapObject> tfItem = new List<SonSnapObject>();
  public List<Transform> TutorialNode => tutorialNode;
  public List<SonSnapObject> TfItem => tfItem;

  // ── Biến đếm ──────────────────────────────────────────
  [SerializeField] private int _failCount = 0;
  public void SetFailCount()
  {
    _failCount = _failCountToShowHint;
    TryReshowAfterDelay();
  }
  public void ReSetFailCount()
  {
    _failCount = 0;
  }
  [SerializeField] private int _failCountToShowHint = 3;
  [SerializeField] private float timeReShow = 3f;

  public void SetNewTime(float newTime)
  {
    timeReShow = newTime;
  }

  private Coroutine _corHideAndReshow;
  private bool isTap = false;

  // ── Update ─────────────────────────────────────────────
  private void Update()
  {
    if (blockShowHint) return;
    if (Input.GetMouseButtonDown(0))
    {
      if (!isTap) isTap = true;

      if (handCtrl.gameObject.activeSelf || handCtrlMakeup.gameObject.activeSelf)
      {
        HideHint();
        TryReshowAfterDelay();
      }
    }
  }

  // ── Biến đếm fail ─────────────────────────────────────
  public void OnCollectFail()
  {
    _failCount++;
    if (_failCount >= _failCountToShowHint)
    {
      _failCount = _failCountToShowHint;
      ShowHint();
    }
  }

  public void OnCollectSuccess()
  {
    _failCount = 0;
    StopReshowCoroutine();
    HideHint();
  }

  // ── Show / Hide ────────────────────────────────────────
  void ShowHint()
  {
    if (disableHand) return;
    if (_failCount < _failCountToShowHint) return;

    int index = _level.CurrentStep;
    switch (index)
    {
      case 0:
        ShowHintPosToPos(tfFlour, tfFlour);
        break;
      case 1:
        if (isSnapSpoon)
        {
          if (isSnapSpoonDone)
            TutorialStep2(0, listSnapObjHint, listTargetStep1);
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
        break;
      default:
        break;
    }
  }

  void HideHint()
  {
    handCtrl.gameObject.SetActive(false);
    handCtrlMakeup.gameObject.SetActive(false);
  }

  // ── Reshow sau 3s ─────────────────────────────────────
  private void TryReshowAfterDelay()
  {
    StopReshowCoroutine();
    if (_failCount >= _failCountToShowHint)
      _corHideAndReshow = StartCoroutine(IEReshowHint());
  }

  private IEnumerator IEReshowHint()
  {
    yield return new WaitForSeconds(timeReShow);
    if (_failCount >= _failCountToShowHint)
      ShowHint();
  }

  private void StopReshowCoroutine()
  {
    if (_corHideAndReshow != null)
    {
      StopCoroutine(_corHideAndReshow);
      _corHideAndReshow = null;
    }
  }

  // ── Step data ──────────────────────────────────────────
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

  public void SetIsSnapSpoon() => isSnapSpoon = true;
  public void SetIsSnapSpoonDone() => isSnapSpoonDone = true;

  // ── Spin hand ─────────────────────────────────────────
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
    }).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
  }

  public void StopSpinOnly() => _spinTween?.Kill();

  // ── Tutorial helpers ───────────────────────────────────
  private void ShowHintPosToPos(Transform _pos1, Transform _pos2)
  {
    handCtrl.gameObject.SetActive(true);
    handCtrl.ShowHandPosToPos(_pos1.position, _pos2.position);
  }

  private void TutorialStep2(int indexStep, List<SonSnapObject> _tfItem, List<Transform> _tfItemTarget)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count || indexStep >= _tfItemTarget.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    var objTarget = _tfItemTarget[indexStep];
    if (!_tfItem[indexStep].IsSnaps)
      handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, objTarget.gameObject.transform.position);
    else
      handCtrl.ShowHandPosToPosToPos(obj.gameObject.transform.position, tfPlate1.position, tfPlate2.position);
  }

  private void TutorialStep6(int indexStep, List<Transform> _tfItem)
  {
    if (handCtrl == null) return;
    if (indexStep >= _tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = _tfItem[indexStep];
    handCtrl.ShowHandPosToPos(obj.gameObject.transform.position, obj.position);
  }

  public void SetStateIsTap(bool value) => isTap = value;

  public void OnStepDone() { }
}
