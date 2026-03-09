using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class TapButton : MonoBehaviour
  {
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private FxType fxType = FxType.Xe;

    public bool IsReady;
    public bool isAlwaysReady;
    public UnityEvent clickEvent;

    public void OnMouseDown1()
    {
      if (!LevelBase.Ins.IsAllowInteract || !IsReady) return;

      if (!isAlwaysReady) IsReady = false;
      SoundManager.Ins.PlayFx(fxType);
      clickEvent?.Invoke();
    }
    public void EnableCollider(bool isEnable = true)
    {
      coll2D.enabled = isEnable;
    }

    public void OnReady()
    {
      IsReady = true;
    }
    [Button]
    private void AddBoxCollider()
    {
      if (!coll2D)
      {
        coll2D = gameObject.AddComponent<BoxCollider2D>();
        coll2D.isTrigger = true;
      }
    }
  }
}

