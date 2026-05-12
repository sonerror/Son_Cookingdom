using DG.Tweening;
using UnityEngine;

namespace sonnv
{
    public class SonFloatingHandler : SonMonoBehaviour
    {
        [SerializeField] private bool isBounceLoop = false;
        [SerializeField] private float durationFloatingIdle = 2.41f;
        [SerializeField] private float curveTimeOffSet = 0.25f;
        [SerializeField] private float speedFloatingIdle = 0.1f;

        private Vector3 _floatingAnchor;
        private float _timeOffsetFloating;
        private bool _isFloating;
        private Tween _floatTween;

        public bool IsFloating => _isFloating;
        private void Start()
        {
            StartFloating();
        }
        public void StartFloating()
        {
            if (!isBounceLoop) return;
            _isFloating = true;
            _floatingAnchor = Tf.position;
            _timeOffsetFloating =
                (Mathf.Round(Time.time / durationFloatingIdle) + curveTimeOffSet)
                * durationFloatingIdle - Time.time;
            DoFloat();
        }

        public void StopFloating()
        {
            _isFloating = false;
            _floatTween?.Kill();
        }

        private void DoFloat()
        {
            if (!_isFloating) return;
            float targetY = _floatingAnchor.y + speedFloatingIdle;
            _floatTween = Tf.DOMoveY(targetY, durationFloatingIdle / 2f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _floatTween = Tf.DOMoveY(_floatingAnchor.y, durationFloatingIdle / 2f)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(DoFloat);
                });
        }
    }
}