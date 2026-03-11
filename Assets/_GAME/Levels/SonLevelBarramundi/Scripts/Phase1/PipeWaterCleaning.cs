using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace sonnv
{
    public class PipeWaterCleaning : SonMonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("References")]
        [SerializeField] private SonSinkWaterCleaning sink;
        [SerializeField] private Transform holePos;
        [SerializeField] private SpriteRenderer pipeSprite;

        [Header("Properties")]
        [SerializeField] private float dragSlerpSpeed;
        [SerializeField] private Vector3 dragPosOffset;
        [SerializeField] private int spriteOrderOnDrop;
        [SerializeField] private int spriteOrderOnDrag;
        [SerializeField] private int spriteOrderOnHole;
        [SerializeField] private AudioData pickUpSound;
        [SerializeField] private AudioData dropSound;
        [SerializeField] private Collider2D col;
        public Collider2D Col => col;

        [Header("Event")]
        [SerializeField] private UnityEvent onStartDrag;
        [SerializeField] private UnityEvent onEndDrag;
        [SerializeField] private UnityEvent onPipeInHole;
        [SerializeField] private UnityEvent onMoveBack;

        public UnityEvent OnStartDrag => onStartDrag;
        public UnityEvent OnPipeInHole => onPipeInHole;
        public UnityEvent OnMoveBack => onMoveBack;

        private bool _isDragging;
        private bool _canInteract;
        private Vector3 _nextPos;

        [SerializeField] private LevelBase _levelBase;

        private Camera _mainCamera;
        private Vector3 _scale;
        private float _initialZ;

        [Header("Other")]
        [SerializeField] private bool isMoveBack;

        private Vector3 _initLocalPos;
        private Tween _moveBackTween;

        public bool IsInHole { get; private set; }

        private bool IsNearHole => DistanceToInSqrVec2(holePos) < 1f;

        public void SetInteract(bool canMove)
        {
            _canInteract = canMove;

            if (col != null)
                col.enabled = canMove;
        }

        public void SetIsInHole(bool value)
        {
            IsInHole = value;
        }

        private void Awake()
        {
            _nextPos = Tf.position;
            _mainCamera = Camera.main;
            _scale = Tf.localScale;
            _initialZ = Tf.position.z;

            SetInteract(true);

            if (isMoveBack)
                _initLocalPos = Tf.localPosition;

            if (col == null)
                col = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (_isDragging || IsInHole)
            {
                Tf.position = Vector3.Slerp(
                    Tf.position,
                    _nextPos,
                    dragSlerpSpeed * Time.deltaTime
                );
            }
        }

        private void OnBlockInteract()
        {
            if (!_levelBase.IsAllowInteract)
            {
                if (_canInteract)
                {
                    if (!IsInHole)
                        PointerUp();

                    SetInteract(false);
                }
            }
            else
            {
                if (sink.IsFillWater)
                    return;

                SetInteract(true);
            }
        }

        // POINTER DOWN (replace OnMouseDown)

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_canInteract) return;

            _nextPos = _mainCamera.ScreenToWorldPoint(eventData.position);
            _nextPos.z = Tf.position.z;
            _nextPos += dragPosOffset;

            pipeSprite.sortingOrder = spriteOrderOnDrag;

            _isDragging = true;
            IsInHole = false;

            Tf.localScale = _scale * 1.1f;

            SoundManager.PlaySFX(pickUpSound.clip, pickUpSound.volume);

            onStartDrag.Invoke();

            if (_canInteract && isMoveBack)
                _moveBackTween?.Kill();
        }

        // DRAG (replace OnMouseDrag)

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canInteract) return;
            if (!_isDragging) return;

            _nextPos = _mainCamera.ScreenToWorldPoint(eventData.position) + dragPosOffset;
            _nextPos.z = _initialZ;
            _nextPos += dragPosOffset;
        }

        // POINTER UP (replace OnMouseUp)

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp();
        }

        private void PointerUp()
        {
            if (!_canInteract) return;

            _isDragging = false;

            pipeSprite.sortingOrder = spriteOrderOnDrop;

            Tf.localScale = _scale;

            if (_levelBase.IsAllowInteract)
            {
                onEndDrag.Invoke();

                if (IsNearHole)
                {
                    IsInHole = true;

                    pipeSprite.sortingOrder = spriteOrderOnHole;

                    _nextPos = holePos.position;

                    SoundManager.PlaySFX(dropSound.clip, dropSound.volume);
                    sink.IgnoreNextClick();
                    onPipeInHole.Invoke();
                }
                else if (isMoveBack)
                {
                    MoveBack();
                }
            }
            else if (isMoveBack)
            {
                MoveBack();
            }
        }

        private void MoveBack()
        {
            _moveBackTween?.Kill();
            _moveBackTween = Tf.DOLocalMove(_initLocalPos, 0.3f).OnComplete(() =>
            {
                onMoveBack?.Invoke();
            });
        }
    }
}