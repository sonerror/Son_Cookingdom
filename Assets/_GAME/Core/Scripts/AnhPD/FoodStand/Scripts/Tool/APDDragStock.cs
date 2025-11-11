using AnhPD.Cook;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AnhPD.FoodStall
{
  public class APDDragStock : APDCookingDragWithCondition
  {
    [SerializeField] private RefillStock stock;
    protected override void Start()
    {
      base.Start();

      mouseDownEvent.AddListener(stock.TakeAwayUnit);
      landEvent.AddListener(() =>
      {
        if (!IsComplete) stock.GiveBackUnit();
      });

      completeEvent.AddListener(stock.UseUnit);
    }

    public override void OnReReady(bool isReady = true)
    {
      IsComplete = false;
      base.OnReReady(isReady);
    }
  }
}
