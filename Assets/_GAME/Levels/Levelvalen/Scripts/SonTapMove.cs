using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

namespace sonnv
{
    public class SonTapMove : SonTapItem
    {
        [SerializeField] private Transform moveTarget;   // object cần di chuyển
        [SerializeField] private Transform destination;  // vị trí đích
        [SerializeField] private float moveDuration = 0.3f;
        [SerializeField] private Ease moveEase = Ease.OutSine;

        public UnityEvent OnMoveComplete;
        public UnityEvent OnStartMouseDown;
        public UnityEvent OnMoveBackComplete;

        private Vector3 _originPos;
        private bool _isMoved;

        protected override void Awake()
        {
            base.Awake();
            _originPos = moveTarget.position;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnStartMouseDown?.Invoke();
            if (_isMoved) return;

            _isMoved = true;
            moveTarget.DOMove(destination.position, moveDuration)
                      .SetEase(moveEase)
                      .OnComplete(() => OnMoveComplete?.Invoke());
        }

        public void MoveBack()
        {
            if (!_isMoved) return;
            _isMoved = false;

            moveTarget.DOMove(_originPos, moveDuration)
                      .SetEase(moveEase)
                      .OnComplete(() => OnMoveBackComplete?.Invoke());
        }
    }
}