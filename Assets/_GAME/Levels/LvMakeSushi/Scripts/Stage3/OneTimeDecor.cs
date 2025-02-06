using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.MakeSushi
{
    public class OneTimeDecor : MonoBehaviour
    {
        [SerializeField] private Collider2D targetCollide;
        [SerializeField] private Transform tfTarget;

        public UnityEvent eventComplete;

        private bool isInTargetCollide = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (targetCollide == collision)
            {
                isInTargetCollide = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (targetCollide == collision)
            {
                isInTargetCollide = false;
            }
        }
        public void CheckTarget()
        {
            if (isInTargetCollide)
            {
                eventComplete?.Invoke();

                transform.DOMove(tfTarget.position, .1f);
            }
        }
    }
}

