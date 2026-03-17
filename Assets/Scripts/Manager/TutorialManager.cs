using System.Collections.Generic;
using System.Linq;
using AnhPD;
using DG.Tweening;
using sonnv;
using UnityEngine;
using System.Collections;

public class TutorialManager : Singleton<TutorialManager>
{
  [SerializeField] float TimeHint = 5f;
  [SerializeField] public HandCtrl handCtrl;
  [SerializeField] private LevelMochi _level;
  [SerializeField] private int countHintStep1 = 0;

  //Step1
  [SerializeField] private Transform tfHintStep1;
  //Step2
  [SerializeField] private bool isSnapFlour = false;
  public bool IsSnapFlour => isSnapFlour;
  public void ChangeStateIsSnapFlour(bool value)
  {
    isSnapFlour = value;
  }
  [SerializeField] private Transform tfSpoonFlour;
  [SerializeField] private Transform tfFlourInBoard;

  [SerializeField] private bool isSnapColor = false;
  public void ChangeStateIsSnapColor(bool value)
  {
    isSnapColor = value;
  }
  [SerializeField] private List<Transform> listTFStepAddColor;
  [SerializeField] private int countHintSnapColor = 0;
  public void SetCountHintColor()
  {
    countHintSnapColor++;
  }
  [SerializeField] private Transform tfFRoll;
  [SerializeField] private bool isSnapRoll = false;
  public void ChangeStateIsSnapRoll(bool value)
  {
    isSnapRoll = value;
  }
  public bool enableCountTime = false;
  public float timeCountHint = 2f;
  public bool disableHand = false;
  public int CountHintStep1
  {
    get { return countHintStep1; }
    set { countHintStep1 = value; }
  }

  private int CountStepDone = 0;
  int countCollectFail = 0;

  void Start()
  {
    if (_level == null)
      _level = FindObjectOfType<LevelMochi>();

    if (handCtrl == null)
      Debug.LogError("HandCtrl missing in TutorialManager");
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
    if (disableHand) return;
    if (_level == null) return;
    if (handCtrl == null) return;

    enableCountTime = false;

    int index = _level.CurrentStep;
    handCtrl.StopHandCircle();
    switch (index)
    {
      case 0:
        TutorialStep2(tfHintStep1);
        return;
      case 1:
        if (!isSnapFlour)
        {
          Debug.Log("Snap Flour");
          handCtrl.gameObject.SetActive(true);
          handCtrl.ShowHandPosToPos(tfSpoonFlour.position, tfFlourInBoard.position);
        }
        else
        {
          if (!isSnapColor)
          {
            Debug.Log("Color Step: " + (countHintSnapColor + 1));
            handCtrl.gameObject.SetActive(true);
            handCtrl.ShowHandPosToPos(listTFStepAddColor[countHintSnapColor].position, tfFlourInBoard.position);
          }
          else
          {
            if (!isSnapRoll)
            {
              Debug.Log("Roll");
              handCtrl.gameObject.SetActive(true);
              handCtrl.ShowHandPosToPos(tfFRoll.position, tfFlourInBoard.position);
            }
            else
            {
              TutorialStep2(tfFlourInBoard);
            }
          }
        }
        return;
    }
  }
  public void ResetStateStep2()
  {
    isSnapFlour = false;
    isSnapColor = false;
    isSnapRoll = false;
  }
  private void TutorialStep2(Transform tf)
  {
    if (tf == null) return;
    handCtrl.ShowHandLoop(tf.position, tf.position);
  }

  private void TutorialStep1(int indexStep)
  {
    // if (handCtrl == null) return;
    // if (indexStep >= tfItem.Count) return;

    // var obj = tfItem[indexStep];

    // if (obj == null || tfSink == null) return;

    // handCtrl.gameObject.SetActive(true);
    // handCtrl.ShowHandPosToPos(obj.position, tfSink.position);
  }
  private void TutorialStep(List<Transform> listTF, Transform tfTarget, int indexStep)
  {
    if (handCtrl == null) return;
    if (indexStep >= listTF.Count) return;

    var obj = listTF[indexStep];

    if (obj == null || tfTarget == null) return;

    handCtrl.gameObject.SetActive(true);
    handCtrl.ShowHandPosToPos(obj.position, tfTarget.position);
  }
  private void TutorialOneHit(List<Transform> listTF, int index)
  {
    if (index >= listTF.Count) return;

    handCtrl.ShowHandAtPos(listTF[index].position);
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