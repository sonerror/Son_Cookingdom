using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
    public class APDCookingDragWithCondition : APDCookingDrag
    {
        public UnityEvent NotSatisfyConditionEvent;
        public Func<bool> conditionFunc;
        protected override void CheckTarget()
        {
            if (conditionFunc != null && !conditionFunc())
            {
                NotSatisfyConditionEvent?.Invoke();
                return;
            }
            base.CheckTarget();
        }

        public void SetupWrongEvent()
        {
            NotSatisfyConditionEvent.AddListener(() =>
            {
                OnIncorrectUse();
            });
        }
        public void SetupConditionAndReady(Func<bool> condition)
        {
            conditionFunc = condition;
            IsReady = true;
            SetupWrongEvent();
        }
    }
}

