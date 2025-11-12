using System;
using System.Collections;
using System.Collections.Generic;
// using AnhPD.FoodStall.RolledIceCream;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.FoodStall
{
  public abstract class FoodData : MonoBehaviour
  {
    public FoodDisplay display;
    public OrderType orderType;
    public FoodColorTag colorTag;
    public bool isOrder;

    public void SetOrderType(OrderType newOderType)
    {
      orderType = newOderType;
      display.SetOrderType(orderType);
      display.SetOrderDisplay(isOrder);
    }
    public bool Compare(FoodData other)
    {
      switch (orderType)
      {
        case OrderType.ColorShape:
          return CompareColorTag(other);
        default:
          return Equals(other);
      }
    }
    protected abstract bool Equals(FoodData other);

    private bool CompareColorTag(FoodData other)
    {
      Debug.Log(colorTag.IsSatisfiedTag(other.colorTag));
      return colorTag.IsSatisfiedTag(other.colorTag);
    }

    [Button]
    public void RandomOrder()
    {
      ResetData();
      switch (orderType)
      {
        case OrderType.ColorShape:
          colorTag.Random();
          break;
        default:
          RandomData();
          break;
      }
      display.Display(this);
    }

    protected abstract void RandomData();

    [Button]
    public virtual void ResetData()
    {
      colorTag?.Clear();
      display.ClearDisplay();
    }
  }

}
