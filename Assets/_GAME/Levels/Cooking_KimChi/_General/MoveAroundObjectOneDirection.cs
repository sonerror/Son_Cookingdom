using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace sonnv
{
    public class MoveAroundObjectOneDirection : MonoBehaviour
    {
        [SerializeField] private Transform aroundPoint;
        [SerializeField] private float moveSensitivity = 1f;
        [SerializeField] private float maxAngleDelta = 30f;
        [SerializeField] private bool manualBlockInteract;
        [SerializeField] private bool rotateClockwise = true;
        [SerializeField] private Rect constraintRect;

        public event Action<float> OnPositionChanged;
        public event Action OnPositionUnChanged;

        [ShowInInspector] private bool _canInteract;
        [ShowInInspector] private bool _isDragging;

        private LevelBase _level;
        private Camera _mainCam;

        private float _initialZ;
        private float _currentAngle;
        private float timeRotate;

        private const float TOLERANCE = 0.00001f;

        private Transform Tf => transform;

        public float TimeRotate => timeRotate;

        void Awake()
        {
            _mainCam = Camera.main;

            if (aroundPoint == null)
                aroundPoint = Tf;

            _initialZ = Tf.position.z;

            Vector2 dir = (Tf.position - aroundPoint.position).normalized;
            _currentAngle = Mathf.Atan2(dir.y, dir.x);
        }

        void Start()
        {
            _canInteract = true;

            _level = LevelBase.Ins;
            if (_level)
                _level.onBlockPlayerInteractChanged += OnBlockInteract;
        }

        void Update()
        {
            HandleInput();

            if (Input.GetMouseButtonUp(0) || Input.touchCount == 0)
            {
                timeRotate = 0;
                HandleMouseUp();
            }
        }

        void OnBlockInteract()
        {
            if (_level.IsAllowInteract)
                _canInteract = true;
            else
            {
                if (_isDragging)
                    HandleMouseUp();

                _canInteract = false;
            }
        }

        void HandleInput()
        {
            Vector2 mouseWorldPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (!_canInteract || _isDragging)
                    return;

                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

                if (hit && hit.transform == transform)
                    _isDragging = true;
            }

            if (Input.GetMouseButton(0) && _isDragging)
            {
                if (!_canInteract || manualBlockInteract)
                    return;

                if (!constraintRect.Contains(mouseWorldPos))
                {
                    HandleMouseUp();
                    return;
                }

                Vector2 prevPos = Tf.position;

                Vector2 dir = (mouseWorldPos - (Vector2)aroundPoint.position).normalized;
                float newAngle = Mathf.Atan2(dir.y, dir.x);

                float angleDelta =
                    Mathf.DeltaAngle(_currentAngle * Mathf.Rad2Deg, newAngle * Mathf.Rad2Deg) * Mathf.Deg2Rad;

                if (rotateClockwise && angleDelta < 0)
                    angleDelta += Mathf.PI * 2;

                if (!rotateClockwise && angleDelta > 0)
                    angleDelta -= Mathf.PI * 2;

                float clampedDelta =
                    Mathf.Clamp(angleDelta, -maxAngleDelta * Mathf.Deg2Rad, maxAngleDelta * Mathf.Deg2Rad);

                // ổn định FPS thấp
                _currentAngle += clampedDelta * moveSensitivity * Time.deltaTime * 60f;

                float radius = Vector2.Distance(prevPos, aroundPoint.position);

                Vector2 newPos =
                    (Vector2)aroundPoint.position +
                    new Vector2(Mathf.Cos(_currentAngle), Mathf.Sin(_currentAngle)) * radius;

                Vector3 targetPos = new Vector3(newPos.x, newPos.y, _initialZ);

                // smoothing
                Tf.position = Vector3.Lerp(Tf.position, targetPos, 15f * Time.deltaTime);

                float distance = (newPos - prevPos).magnitude;

                if (distance > TOLERANCE)
                {
                    timeRotate += Time.deltaTime;
                    OnPositionChanged?.Invoke(distance);
                }
                else
                {
                    OnPositionUnChanged?.Invoke();
                }
            }
        }

        public void HandleMouseUp()
        {
            if (!_isDragging)
                return;

            _isDragging = false;
            OnPositionUnChanged?.Invoke();
        }

        public void SetManualBlockInteract(bool canInteract)
        {
            manualBlockInteract = canInteract;
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;

            Vector3 min = new Vector3(constraintRect.xMin, constraintRect.yMin, transform.position.z);
            Vector3 max = new Vector3(constraintRect.xMax, constraintRect.yMax, transform.position.z);

            Gizmos.DrawWireCube((min + max) / 2, new Vector3(constraintRect.width, constraintRect.height, 0));
        }
#endif
    }
}