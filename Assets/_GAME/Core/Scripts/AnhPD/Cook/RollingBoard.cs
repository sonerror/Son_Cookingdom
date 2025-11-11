using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class RollingBoard : MonoBehaviour
    {
        [SerializeField] protected RollingObject obj;
        [SerializeField] protected RollingPin pin;

        private void Start()
        {
            if(obj != null)
            {
                pin.InitObjectRange(obj);
            }
        }
        public void InitObject(RollingObject rollingObject)
        {
            obj = rollingObject;
            pin.InitObjectRange(obj);
        }
        public void OnRolling(float rate)
        {
            obj.OnRolling(rate);
        }
        public void OnPutPinIn()
        {
            pin.transform.Appear();
            pin.InitObjectRange(obj);
        }
    }
}

