using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
    public class APDCookingPourWithCondition : APDCookingPour
    {
        private Func<bool> _conditionFunc;
        protected override void CheckTarget()
        {
            if (_conditionFunc != null && !_conditionFunc())
            {
                OnIncorrectUse();
                return;
            }
            base.CheckTarget();
        }
        
        public void SetupConditionAndReady(Func<bool> condition)
        {
            _conditionFunc = condition;
            IsReady = true;
        }
    }
}
