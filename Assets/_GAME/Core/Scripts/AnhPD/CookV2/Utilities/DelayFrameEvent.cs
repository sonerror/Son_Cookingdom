using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace AnhPD.CookV2
{
    public class DelayFrameEvent : MonoBehaviour
    {
        [SerializeField] private bool isActiveOnEnable;
        [SerializeField] private int numberFrameDelay = 1;
        public UnityEvent onComplete;

        private void OnEnable()
        {
            if(isActiveOnEnable) Active(); 
        }

        public void Active()
        {
            this.WaitFrames(()=>onComplete?.Invoke() ,numberFrameDelay);
        }
    }
}
