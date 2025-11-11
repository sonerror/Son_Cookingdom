using System.Collections;
using System.Collections.Generic;
using AnhPD.Cook;
using HoangHH;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class SugarcanePot : ClockerStoveBase
  {
    // [SerializeField] private BoilWaterFx boilWater;
    [SerializeField] private SpriteRenderer before;
    [SerializeField] private BeaterInBowl whisk;
    protected override void OnTurnOn()
    {
      base.OnTurnOn();
      // boilWater.Boil(1f);
    }

    protected override void OnTurnOff()
    {
      base.OnTurnOff();
      // boilWater.StopBoil(1f);
    }

    public void OnMixing()
    {
      float rate = whisk.Rate;
      before.SetAlpha(1 - rate);
    }
  }
}
