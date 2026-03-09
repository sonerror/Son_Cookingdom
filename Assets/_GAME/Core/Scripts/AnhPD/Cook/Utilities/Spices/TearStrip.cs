using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class TearStrip : MonoBehaviour
  {
    [SerializeField] private float zAngle;
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private AudioClip sfx;

    public UnityEvent completeEvent;

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      // AudioManager.PlaySFX(sfx);
      coll2D.enabled = false;

      transform.DORotate(new Vector3(0, 0, zAngle), .3f).SetEase(Ease.InOutBack).OnComplete(() =>
      {
        completeEvent?.Invoke();
      });
    }
  }
}

