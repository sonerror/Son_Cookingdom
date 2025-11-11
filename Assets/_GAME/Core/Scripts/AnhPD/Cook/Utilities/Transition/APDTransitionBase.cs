using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class APDTransitionBase : MonoBehaviour
    {
        protected Action onCover, onComplete;
        public APDTransitionBase OnCover(Action action)
        {
            onCover += action;
            return this;
        }

        public APDTransitionBase OnComplete(Action action)
        {
            onComplete += action;
            return this;
        }
        public APDTransitionBase ClearAllAction()
        {
            onComplete = null;
            onCover = null;
            return this;
        }
    }
}
