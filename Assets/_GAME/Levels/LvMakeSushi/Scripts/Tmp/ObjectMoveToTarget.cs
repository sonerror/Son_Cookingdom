using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD
{
    public class ObjectMoveToTarget : MonoBehaviour
    {
        [SerializeField] float distance = 0.5f;
        [SerializeField] Transform target;
        public UnityEvent eventComplete, eventArrived;

        public void CheckTarget()
        {
            if(Vector2.Distance(target.position, transform.position) < distance)
            {
                eventArrived?.Invoke();
                transform.DOMove(target.position, .15f).OnComplete(() =>
                {
                    eventComplete?.Invoke();
                });
            }
        }
    }
}

