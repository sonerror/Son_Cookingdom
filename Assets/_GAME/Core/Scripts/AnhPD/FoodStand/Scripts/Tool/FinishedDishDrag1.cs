using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using AnhPD.FoodStall;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.FoodStand
{
  public class FinishedDishDrag1 : APDv2CookingToolBase
  {
    [SerializeField] private FSTrashCan trashCan;
    public FoodData data;

    protected override void MouseDown(BaseEventData eventData)
    {
      base.MouseDown(eventData);
      trashCan?.Show();
    }

    protected override void MouseUp(BaseEventData eventData)
    {
      base.MouseUp(eventData);
      if (trashCan && IsInRange(trashCan.transform))
      {
        // gameObject.SetActive(false);
        // FSAPDLevelBase.Ins.Restart();
      }
      else { } //FSAPDLevelBase.Ins.CheckOrder();

      trashCan?.Hide();
    }

    public bool IsInRange(Transform target)
    {
      return IsInRange(target.position);
    }

    public override void OnReReady(bool isReady = true)
    {
      base.OnReReady(isReady);
      data.ResetData();
      coll2D.enabled = false;
    }
  }
}
