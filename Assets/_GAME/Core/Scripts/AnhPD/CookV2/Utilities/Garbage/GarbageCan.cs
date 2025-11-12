using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class GarbageCan : MonoBehaviour
  {
    [SerializeField] private List<GarbageDrag> garbage;
    public Transform center;

    [SerializeField] private float offsetY = 3f;
    public UnityEvent onThrowGarbage;

    private float _minY;
    public bool IsCleared
    {
      get
      {
        for (int i = 0; i < garbage.Count; i++)
        {
          if (garbage[i].gameObject.activeSelf) return false;
        }
        return true;
      }
    }

    private void Start()
    {
      _minY = transform.position.y;
      if (!center) center = transform;
    }

    public void Show()
    {
      transform.DOComplete();
      transform.DOMoveY(_minY + offsetY, 0.5f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
      transform.DOComplete();
      transform.DOMoveY(_minY, 0.5f).SetEase(Ease.OutBack);
    }

    public void ThrowGarbage()
    {
      onThrowGarbage?.Invoke();
    }
#if UNITY_EDITOR
    [Button]
    private void Init()
    {
      garbage = transform.parent.GetComponentsInChildren<GarbageDrag>(true).ToList();
      foreach (GarbageDrag gar in garbage) gar.Setup(this);
    }
#endif
  }
}