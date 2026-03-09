using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.CookV2
{
    public abstract class BeforeCompleteAction : MonoBehaviour
    {
        public abstract void DoAction(Transform target, Action onDone);
    }
}
