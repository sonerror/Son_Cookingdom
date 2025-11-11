using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
  public class StoveButton : MonoBehaviour
  {
    [SerializeField] private StoveBase stoveBase;
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      stoveBase.OnClick();
    }
  }

}
