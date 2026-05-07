using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
namespace sonnv
{
    [RequireComponent(typeof(Collider2D))]
    public class RollObject : SonMonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private SpriteRenderer sp;
        [SerializeField] private Collider2D col;
        public Collider2D Col => col;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform rollTarget;
        [SerializeField] private Transform tfOri;
        [SerializeField] private GameObject objItem;

        [SerializeField] private float rollSpeed = 5f;
        [SerializeField] private float minDragUp = 0.01f;

        public UnityEvent OnRollStart;
        public UnityEvent OnRollComplete;

        private Vector2 _lastMousePos;
        private float _startY;      // ✅ lưu y ban đầu 1 lần duy nhất
        private float _currentY;
        private bool _isRolling;
        private bool _isDone;
        private bool _isShow;

        private void Start()
        {
            _startY = rollTarget.position.y;    // ✅ fix: lưu đúng 1 lần
            _currentY = _startY;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isDone) return;
            rollTarget.DOKill();                // ✅ cancel tween snap đang chạy
            _lastMousePos = GetWorldPos(eventData.position);

            if (!_isShow)
            {
                sp.enabled = true;
                _isShow = true;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDone) return;

            Vector2 currentPos = GetWorldPos(eventData.position);
            float delta = currentPos.y - _lastMousePos.y;
            _lastMousePos = currentPos;

            if (delta <= 0) return;     // chỉ kéo lên

            if (!_isRolling)
            {
                _isRolling = true;
                OnRollStart?.Invoke();
            }

            // ✅ fix: clamp dùng _startY cố định thay vì rollTarget.position.y
            _currentY = Mathf.Clamp(_currentY + delta, _startY, endPoint.position.y);

            rollTarget.position = new Vector3(
                rollTarget.position.x,
                _currentY,
                rollTarget.position.z
            );

            if (_currentY >= endPoint.position.y)
                Complete();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isDone) return;
            _isRolling = false;
        }

        private void Complete()
        {
            if (_isDone) return;
            _isDone = true;
            _isRolling = false;
            col.enabled = false;
            float distance = Mathf.Abs(endPoint.position.y - _currentY);
            rollTarget.DOMoveY(endPoint.position.y, distance / rollSpeed)
                    .SetEase(Ease.OutSine)
                     .OnComplete(() =>
                    {
                        objItem.SetActive(false);
                        StartCoroutine(IE_DelayMoveBack());
                    });
        }
        IEnumerator IE_DelayMoveBack()
        {
            yield return new WaitForSeconds(0.5f);
            this.transform.DOMove(tfOri.position, 0.25f).OnComplete(() =>
                      {
                          OnRollComplete?.Invoke();
                      });
        }
        public void Reset()
        {
            _isDone = false;
            _isRolling = false;
            _currentY = _startY;
            rollTarget.DOKill();
            rollTarget.position = new Vector3(
                rollTarget.position.x,
                _startY,
                rollTarget.position.z
            );
        }

        private Vector2 GetWorldPos(Vector2 screenPos)
        {
            float z = Camera.main.WorldToScreenPoint(rollTarget.position).z;
            return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
        }
    }
}