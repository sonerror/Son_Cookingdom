
using System.Collections.Generic;
using System.Linq;
using AnhPD;
using DG.Tweening;
using sonnv;
using UnityEngine;
using UnityEngine.XR;

public class TutorialManager : Singleton<TutorialManager>
{
  public bool disableHand = false;
  [SerializeField] float TimeHint = 5f;
  public bool enableCountTime = false;
  public float timeCountHint = 2f;
  [SerializeField] public HandCtrl handCtrl;
  [SerializeField] private Level1528 _level;
  //[SerializeField] private Animator animHand;
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



  int countCollectFail = 0;
  private bool isTap = false;

  [SerializeField] private int stepInPhase = 0;
  public int StepInPhase => stepInPhase;
  public void IncreaseCountHintStep1()
  {
    stepInPhase++;
  }
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
    CalculateTimeHint();
  }
  private void CalculateTimeHint()
  {
    timeCountHint -= Time.deltaTime;
    if (timeCountHint <= 0)
    {
      Debug.Log("show hint");
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
        if (StepInPhase == 0)
        {
          handCtrl.ShowHandPosToPos(Step1Tf1Tut1.position, Step1Tf2Tut1.position);
        }
        if (StepInPhase == 1)
        {
          handCtrl.ShowHandAtPos(Step1TfTut2.position);
        }
        return;
      case 1:
        TutorialStep1(0);
        return;
      case 2:
        return;
      case 3:
        return;
      case 4:
        return;
      case 5:
        return;
      case 6:
        return;
      default:
        return;
    }
  }
  private void Tutorial(int indexStep)
  {

  }
  private void TutorialStep1(int indexStep)
  {
    if (handCtrl == null) return;
    if (indexStep >= tfItem.Count) return;
    handCtrl.gameObject.SetActive(true);
    var obj = tfItem[indexStep];
    handCtrl.ShowHandPosToPos(obj.position, tfSink.position);
  }

  private void TutorialOneHit(List<Transform> _listTF, int _index)
  {
    handCtrl.ShowHandAtPos(_listTF[_index].position);
  }
  public void SetStateIsTap(bool value)
  {
    isTap = value;
  }
  void HideHint()
  {
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
