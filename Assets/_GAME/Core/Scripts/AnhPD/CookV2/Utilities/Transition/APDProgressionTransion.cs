using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.CookV2
{
    public class APDProgressionTransion : MonoBehaviour
    {
        protected float delay = 0f;
        protected float transDuration = 0.5f;
        protected float stayDuration = .25f;
        protected Action onCover, onComplete;

        public APDProgressionTransion OnCover(Action action)
        {
            onCover += action;
            return this;
        }

        public APDProgressionTransion OnComplete(Action action)
        {
            onComplete = action;
            return this;
        }

        public APDProgressionTransion SetDelay(float newDelay)
        {
            delay = newDelay;
            return this;
        }

        public virtual APDProgressionTransion Play()
        {
            return this;
        }
    }
}
