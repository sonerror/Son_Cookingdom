using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.Tanghulu
{
  public class TanghuluFruitDrag : APDv2CookingToolBase
  {
    // [SerializeField] private TanghuluSkewer skewer;
    [SerializeField] private Transform target;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private TanghuluData.FruitType fruitType;
    private Vector3 _bottom;

    private bool _isVerticalDrag;

    private Func<bool> _conditionFunc;
    protected bool IsSatisfyCondition => _conditionFunc == null || _conditionFunc.Invoke();
    public void SetCondition(Func<bool> conditionFunc)
    {
      _conditionFunc = conditionFunc;
    }
    public void Init(Transform circle)
    {
      // this.skewer = skewer;
      this.target = circle;
    }

    protected override void MouseDown(BaseEventData eventData)
    {
      base.MouseDown(eventData);
      if (!_isVerticalDrag && IsSatisfyCondition) target.gameObject.SetActive(true);
    }

    protected override void MouseUp(BaseEventData eventData)
    {
      base.MouseUp(eventData);
      target.gameObject.SetActive(false);
      if (_isVerticalDrag)
      {
        Tf.DOKill();
        coll2D.enabled = false;
        Tf.DOMove(_bottom, .2f).OnComplete(OnComplete);
      }
    }

    protected override void MouseDrag(BaseEventData eventData)
    {
      if (!_isVerticalDrag)
      {
        base.MouseDrag(eventData);
        if (IsSatisfyCondition && IsInRange(target))
        {
          particle.Play();
          target.gameObject.SetActive(false);
          transform.position = target.position;
          _isVerticalDrag = true;
        }
      }
      else
      {
        Vector3 pos = GetMouseWorldPos() + mOffset;
        pos = new Vector3(target.position.x,
            Mathf.Clamp(pos.y, _bottom.y, target.position.y - .1f), 0);
        transform.position = pos;

        if (IsInRange(_bottom))
        {
          OnComplete();
        }
      }
    }

    public override void OnComplete()
    {
      // base.OnComplete();
      coll2D.enabled = true;
      _isVerticalDrag = false;
      isDragging = false;
      OnReReady();
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
      base.OnDrawGizmosSelected();
      if (!target) return;
      Gizmos.color = Color.cyan;
      Gizmos.DrawWireSphere(target.position, dropDistance);
    }
#endif
  }
}
