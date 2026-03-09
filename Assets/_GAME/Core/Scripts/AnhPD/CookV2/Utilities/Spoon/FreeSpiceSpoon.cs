using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.CookV2
{
  public class FreeSpiceSpoon : APDv2Drag
  {
    [SerializeField] private List<SpiceConfig> spiceConfigs;
    [SerializeField] private float sourceRangeRation = .5f;
    private int _index;
    private bool _isHaveSpice;
    private Transform _spiceInSpoon;
    private List<Transform> _sourceTf;

    public bool isSingleTarget;
    public bool isReadyAfterComplete = true;
    protected override void Start()
    {
      base.Start();
      _sourceTf = new List<Transform>();
      foreach (SpiceConfig spiceConfig in spiceConfigs)
      {
        _sourceTf.Add(spiceConfig.source);
      }
    }

    protected override void MouseDrag(BaseEventData eventData)
    {
      base.MouseDrag(eventData);
      if (!_isHaveSpice) CheckSource();
      else CheckSpiceTarget();
    }

    protected override void MouseUp(BaseEventData eventData)
    {
      base.MouseUp(eventData);
      if (_isHaveSpice)
      {
        _isHaveSpice = false;
        _spiceInSpoon.gameObject.SetActive(false);
      }
    }

    private void CheckSource()
    {
      int index = APDUtilities.GetNearestTranformIndex(center, _sourceTf);
      if (index == -1) return;
      if (IsInRange(center.position, _sourceTf[index].position, sourceRangeRation))
      {
        if (!IsReady)
        {
          OnIncorrectUse();
          return;
        }
        _isHaveSpice = true;
        _spiceInSpoon = spiceConfigs[index].spiceInSpoon;
        _spiceInSpoon.Appear();
        onComplete.RemoveAllListeners();
        onComplete.AddListener(() =>
        {
          onCompleteBaseSwitchTarget?.Invoke();
          spiceConfigs[index].onComplete?.Invoke();
        });
        _index = index;
        if (!isSingleTarget) target = spiceConfigs[index].target;
      }
    }

    protected override void CheckTarget()
    {
      // base.CheckTarget();
    }

    //avoid call CheckTarget in mouse up & mouse drag
    private void CheckSpiceTarget()
    {
      base.CheckTarget();
    }

    public override void OnComplete()
    {
      base.OnComplete();
      spiceConfigs.RemoveAt(_index);
      _sourceTf.RemoveAt(_index);
      _isHaveSpice = false;
      _spiceInSpoon.gameObject.SetActive(false);
      if (isReadyAfterComplete && spiceConfigs.Count > 0)
      {
        IsReady = true;
        IsComplete = false;
      }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
      base.OnDrawGizmosSelected();
      if (spiceConfigs.Count < 1) return;
      Gizmos.color = Color.cyan;
      foreach (SpiceConfig spiceConfig in spiceConfigs)
      {
        Gizmos.DrawWireSphere(spiceConfig.source.position, dropDistance * sourceRangeRation);
      }
    }
#endif
  }

  [Serializable]
  public class SpiceConfig
  {
    public Transform source;
    public Transform target;
    public Transform spiceInSpoon;
    public UnityEvent onComplete;
  }
}
