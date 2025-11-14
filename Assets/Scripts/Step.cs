using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Link;
using Sirenix.OdinInspector;
using UnityEngine;

public class Step : MonoBehaviour
{
  [SerializeField] protected float delayStartAction = 0;
  [SerializeField] private StepData[] stepStart;

  [System.Serializable]
  public class StepData
  {
    [HorizontalGroup()]
    public ActionBase item;
    [HorizontalGroup()]
    public float delay;

    public void Active()
    {
      item.Active();
    }
  }

  void Start()
  {
    OnStart();
  }

  public virtual void OnStart()
  {
    DOVirtual.DelayedCall(delayStartAction, () =>
    {
      gameObject.SetActive(true);
      if (stepStart.Length > 0)
      {
        foreach (var item in stepStart)
        {
          item.Active();
        }
      }
    });

    //LevelControl.Ins.BlockControl(blockTime + delayStartAction);
  }
}
