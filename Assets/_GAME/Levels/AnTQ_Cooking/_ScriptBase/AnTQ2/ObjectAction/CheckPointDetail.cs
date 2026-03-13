using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine;

namespace sonnv
{
    [Serializable]
    public class CheckPointDetail
    {
        public Transform root;
        public float speed;
        public List<CheckPoint> checkPoints;
        public UnityEvent eventWhenCompleteCheckPoints;
        public UnityEvent eventWhenFailCheckPoints;
    }

    [Serializable]
    public class CheckPoint
    {
        public Transform checkPoint;
        public SpriteRenderer spriteRenderer;
        public bool isDone;
    }
}