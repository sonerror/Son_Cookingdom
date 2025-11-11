using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.CookV2
{
  public class APDv2Drag : APDv2CookingToolBase
  {
    [FoldoutGroup("Bool")] public bool isMouseUpCheck = true;
    [FoldoutGroup("Bool")] public bool isHideAfterComplete = true;
    [FoldoutGroup("Bool")] public bool isRotateWhenPickUp;
    [FoldoutGroup("Bool")] public bool isReReadyAfterComplete;

    [SerializeField] protected TargetConfig[] targets;
    [SerializeField] protected Transform target, center;
    [SerializeField] private BeforeCompleteAction beforeCompleteAction;
    [SerializeField] protected float pickupAngle;
    private Func<bool> _conditionFunc;

    [FoldoutGroup("Event")] public UnityEvent onCompleteBaseSwitchTarget;

    protected bool IsSatisfyCondition => _conditionFunc == null || _conditionFunc.Invoke();
    public bool IsComplete { get; protected set; }

    protected override void Start()
    {
      base.Start();
      if (!center) center = transform;
    }
    public void SetCondition(Func<bool> conditionFunc)
    {
      _conditionFunc = conditionFunc;
    }

    #region target

    public virtual void SwitchTarget(int index)
    {
      TargetConfig targetConfig = targets[index];
      target = targetConfig.transform;
      onComplete.RemoveAllListeners();
      onComplete.AddListener(() => targetConfig.onComplete.Invoke());
      onComplete.AddListener(() => onCompleteBaseSwitchTarget?.Invoke());
    }
    public void SetTarget(Transform tar)
    {
      target = tar;
    }
    protected virtual void CheckTarget()
    {
      if (!IsInRange(center.position, target.position)) return;

      if (IsReady && IsSatisfyCondition) OnComplete();
      else if (isMouseUpCheck) OnIncorrectUse();
    }

    #endregion

    public override void OnComplete()
    {
      if (beforeCompleteAction)
      {
        Tf.DOKill();
        coll2D.enabled = false;
        isDragging = false;
        beforeCompleteAction.DoAction(target, () =>
        {
          IsComplete = true;
          coll2D.enabled = true;
          onComplete?.Invoke();
          base.OnComplete();
        });
      }
      else
      {
        IsComplete = true;
        if (isHideAfterComplete)
        {
          IsReady = false;
          Tf.DOKill();
          gameObject.SetActive(false);

          if (isReReadyAfterComplete) OnReReady();
        }
        else base.OnComplete();
        onComplete?.Invoke();
      }
    }

    #region Mouse handle

    protected override void MouseDown(BaseEventData eventData)
    {
      base.MouseDown(eventData);
      if (isRotateWhenPickUp) Tf.DORotate(new Vector3(Tf.eulerAngles.x, Tf.eulerAngles.y, pickupAngle), .3f);
    }

    protected override void MouseDrag(BaseEventData eventData)
    {
      base.MouseDrag(eventData);
      if (!isMouseUpCheck) CheckTarget();
    }

    protected override void MouseUp(BaseEventData eventData)
    {
      base.MouseUp(eventData);
      if (isMouseUpCheck) CheckTarget();
    }
    #endregion

    #region Editor

#if UNITY_EDITOR
    [HorizontalGroup("Action")]
    [Button]
    private void AddPourAction()
    {
      if (beforeCompleteAction) DestroyImmediate(beforeCompleteAction);
      beforeCompleteAction = gameObject.AddComponent<PourAction>();
    }
    [HorizontalGroup("Action")]
    [Button]
    private void AddSimplePourAction()
    {
      if (beforeCompleteAction) DestroyImmediate(beforeCompleteAction);
      beforeCompleteAction = gameObject.AddComponent<SimplePourAction>();
    }
    protected override void OnDrawGizmosSelected()
    {
      base.OnDrawGizmosSelected();
      if (!target) return;
      Gizmos.color = Color.cyan;
      Gizmos.DrawWireSphere(target.position, dropDistance);
    }
#endif

    #endregion
  }
  [Serializable]
  public class TargetConfig
  {
    public Transform transform;
    public UnityEvent onComplete;
  }
}
