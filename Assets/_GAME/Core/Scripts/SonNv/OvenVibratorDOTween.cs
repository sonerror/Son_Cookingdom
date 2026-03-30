using DG.Tweening;
using UnityEngine;

namespace sonnv
{
    public class OvenVibratorDOTween : MonoBehaviour
    {
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float strength = 0.1f;
        [SerializeField] private int vibrato = 10;

        private Vector3 _initLocalPos;

        private void Awake()
        {
            _initLocalPos = transform.localPosition;
        }

        public void StartVibration()
        {
            transform.DOKill();

            transform.localPosition = _initLocalPos;

            transform.DOShakePosition(duration, strength, vibrato, 90, false, false)
                     .SetEase(Ease.Linear)
                     .SetLoops(-1, LoopType.Restart);
        }

        public void StopVibration()
        {
            transform.DOKill();

            transform.localPosition = _initLocalPos;
        }
    }
}