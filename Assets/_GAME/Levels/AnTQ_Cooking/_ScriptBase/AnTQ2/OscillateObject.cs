using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace sonnv
{
    public class OscillateObject : MonoBehaviour
    {
        [SerializeField] private float swingDuration = 2f;
        [SerializeField] private float swingDistance = 0.1f;

        private Transform objectTransform;
        private Vector3 startPosition;
        private Sequence swingSequence;

        private void Start()
        {
            objectTransform = this.transform;
            startPosition = objectTransform.localPosition;
            StartSwingAnimation();
        }

        private void OnDisable()
        {
            swingSequence?.Kill();
        }

        private void StartSwingAnimation()
        {
            swingSequence?.Kill();
            swingSequence = DOTween.Sequence();

            // Tạo chuyển động ngẫu nhiên
            for (int i = 0; i < 4; i++)
            {
                float randomX = Random.Range(-swingDistance, swingDistance);
                float randomY = Random.Range(-swingDistance, swingDistance);
                swingSequence.Append(objectTransform.DOLocalMove(startPosition + new Vector3(randomX, randomY, 0), swingDuration));
            }
            swingSequence.Append(objectTransform.DOLocalMove(startPosition, swingDuration))
                         .SetLoops(-1, LoopType.Restart);
        }

        public void EventPickUp()
        {
            swingSequence?.Kill();
        }

        public void EventDrop()
        {
            StartSwingAnimation();
        }

        private void OnDestroy()
        {
            swingSequence?.Kill();
        }
    }
}

