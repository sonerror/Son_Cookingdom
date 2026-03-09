using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnhPD.Cupcake
{
    public class FloatingV2 : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform shadow;

        [Header("Floating Settings")] [SerializeField]
        private float deltaY = .5f;

        [SerializeField] private float duration = 2f;
        [SerializeField] private float delayMax = 2f;

        [Header("Rotation Settings")] [SerializeField]
        private float rotationSpeed = 10f;

        [SerializeField] private bool isRotate = true;

        private float _startPositionY;
        private Vector3 _shadowScale;
        private Vector3 _startEulerAngles;

        private Tween floatTween;
        private Tween scaleTween;
        private Tween rotateTween;

        private void Start()
        {
            if (shadow)
                _shadowScale = shadow.localScale;
            _startPositionY = transform.position.y;
            _startEulerAngles = transform.eulerAngles;
            
            InitFloating(); // Gọi khởi tạo ban đầu
        }

        public void InitFloating()
        {
            StartFloating(withDelay: true);
        }

        public void RestartFloating()
        {
            StartFloating(withDelay: false);
        }

        private void StartFloating(bool withDelay)
        {
            StopFloating(); // Dọn dẹp tween cũ

            float delay = withDelay ? Random.Range(0f, delayMax) : 0f;
            float heightOffset = deltaY * Random.Range(.9f, 1.1f);

            Vector3 startPos = transform.position;
            Vector3 upDir = Vector3.up;
            Vector3 targetPos = startPos + upDir * heightOffset;

            floatTween = transform.DOMove(targetPos, duration).SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(delay);

            // Shadow tween
            if (shadow)
            {
                shadow.localScale = _shadowScale;

                scaleTween = shadow.DOScale(_shadowScale * 0.5f, duration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                    .SetDelay(delay);
            }

            // Rotation tween
            if (isRotate)
            {
                float direction = Random.value < 0.5f ? 1f : -1f; // 50% xoay phải hoặc trái
                float speed = 360f / rotationSpeed * direction;

                rotateTween = transform
                    .DORotate(new Vector3(0, 0f, 360f * direction), Mathf.Abs(speed), RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear)
                    .SetDelay(delay);
            }
        }


        public void StopFloating()
        {
            floatTween?.Kill();
            scaleTween?.Kill();
            rotateTween?.Kill();

            // Reset state
            transform.position = new Vector3(transform.position.x, _startPositionY, transform.position.z);

            if (shadow)
                shadow.localScale = _shadowScale;

        }

        public void StopRotation()
        {
            rotateTween?.Kill();
        }
    }
}
