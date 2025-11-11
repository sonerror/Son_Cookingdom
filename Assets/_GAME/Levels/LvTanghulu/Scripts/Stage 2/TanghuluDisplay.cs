using System.Collections;
using System.Collections.Generic;
using AnhPD.FoodStall;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class TanghuluDisplay : DisplayBase
  {
    // [SerializeField] private TanghuluSkewer skewer;
    public override void Display(FoodData foodData)
    {
      if (!isOrder) return;
      // skewer.LoadData((TanghuluData)foodData);
    }

    public override void ClearDisplay()
    {
      if (!isOrder) return;
      // skewer?.ClearFruits();
    }
  }
}
