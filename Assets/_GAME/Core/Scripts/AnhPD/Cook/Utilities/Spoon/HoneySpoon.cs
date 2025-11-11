using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
    public class HoneySpoon : APDCookingToolBase
    {
        [SerializeField] private Transform honey, target;
        private Func<bool> _conditionFunc;
        public UnityEvent onComplete;
        protected override void MouseUp(BaseEventData eventData)
        {
            base.MouseUp(eventData);
            if(!IsReady) return;
            CheckTarget();
        }

        protected  void CheckTarget()
        {
            if (_conditionFunc != null && !_conditionFunc())
            {
                OnIncorrectUse();
                return;
            }

            if (IsInRange(honey.position,target.position))
            {
                honey.gameObject.SetActive(false);
                onComplete.Invoke();
            }
        }

        public override void OnReReady(bool isReady = true)
        {
            base.OnReReady(isReady);
            honey.gameObject.SetActive(true);
        }

        public void SetupConditionAndReady(Func<bool> condition)
        {
            _conditionFunc = condition;
            IsReady = true;
        }
    }
}
