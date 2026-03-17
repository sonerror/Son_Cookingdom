using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace sonnv
{
    public class PulseObject : MonoBehaviour
    {
        [SerializeField] private float scaleUp = 1.15f;     // độ phóng t
        [SerializeField] private float duration = 0.5f;     // thời gian mỗi bước

        private Transform objectTransform;
        private Vector3 startScale;
        private Sequence pulseSequence;

        private void Start()
        {
            objectTransform = transform;
            startScale = objectTransform.localScale;
            StartPulse();
        }

        private void OnDisable()
        {
            pulseSequence?.Kill();
        }

        private void StartPulse()
        {
            pulseSequence?.Kill();

            pulseSequence = DOTween.Sequence();

            pulseSequence
                .Append(objectTransform.DOScale(startScale * scaleUp, duration).SetEase(Ease.OutQuad))
                .Append(objectTransform.DOScale(startScale, duration).SetEase(Ease.InQuad))
                .SetLoops(-1, LoopType.Restart);
        }

        public void EventPickUp()
        {
            pulseSequence?.Kill();
            objectTransform.localScale = startScale;
        }

        public void EventDrop()
        {
            StartPulse();
        }

        private void OnDestroy()
        {
            pulseSequence?.Kill();
        }
    }
}