using DG.Tweening;


using System;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    public class DragController : SonMonoBehaviour
    {
        [SerializeField] private bool checkMove;
        [SerializeField] private Collider col;
        public UnityEvent onDragStart;
        public UnityEvent onDragStop;
        public bool zeroOnDragStart;   // Set position to cursor on drag start (ignores Z-axis)
        public bool restrictX;         // Restrict movement on X-axis
        public bool restrictY;         // Restrict movement on Y-axis
        public bool restrictWithinRect;// Restrict movement within a specified rectangle
        public Rect rect;              // The restriction rectangle for movement
        public bool returnOffScreen;   // Return to screen bounds if dragged offscreen
        public float centerSnapDistance; // Snap to center if within this distance

        private float _centerSnapDistanceSqr; // Squared value of the center snap distance
        private bool _dragging;         // Indicates whether the object is currently being dragged
        private Vector3 _offset;        // Offset from cursor to object position during drag
        private Tween _twnReturn;       // Tween used for returning object to screen

        // Handle MoveBack
        [SerializeField] private bool moveBackOnRelease;
        [SerializeField] private float moveBackDuration = 0.3f;
        [SerializeField] private AnimationCurve movementCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private bool overrideLocalBackPos;
        [SerializeField] private Vector3 localBackPosOverride;
        [SerializeField] private bool isCheck;
        [SerializeField] private GameObject contactCol;




        public bool IsDragging => _dragging;
        public Collider Col => col;

        [SerializeField] protected SonLevelBase level;

        // move back
        private Vector3 _currentBackPos;
        private Vector3 _backPos;
        private float _elapsedTime;
        private bool _isMovingBack;
        private bool _canMoveBack;
        public bool CanMoveBack => _canMoveBack;

        public void SetCurrentPositionAsBackPos()
        {
            _backPos = Tf.position;
        }
        public void ReSetBackPos()
        {
            _backPos = Tf.position;

        }
        protected virtual void Awake()
        {
            if (moveBackOnRelease)
            {
                _backPos = overrideLocalBackPos ? Tf.TransformPoint(localBackPosOverride) : Tf.position;
            }
            if (!col) col = GetComponent<Collider>();
        }

        protected virtual void Start()
        {
            level = LevelBase.Ins as SonLevelBase;
            Debug.Log("LevelBase: " + LevelBase.Ins);
            if (level)
            {
                level.onBlockPlayerInteractChanged += OnBlockPlayerInteractChanged;
                if (moveBackOnRelease) _canMoveBack = true;
            }
            OnStart();
        }

        protected virtual void OnStart()
        {

        }
        public void SetCheckMove(bool _checkMove)
        {
            checkMove = _checkMove;
        }
        private void OnBlockPlayerInteractChanged()
        {
            if (!level.IsAllowInteract)
            {
                OnMouseUp();
            }
        }

        private void Update()
        {
            if (checkMove == true) return;
            if (!level.IsAllowInteract) return;
            if (_dragging)
            {
                HandleDragging();
            }
            else if (moveBackOnRelease && _isMovingBack && _canMoveBack)
            {
                _elapsedTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(_elapsedTime / moveBackDuration);
                float curveValue = movementCurve.Evaluate(normalizedTime);
                Tf.position = Vector3.Lerp(_currentBackPos, _backPos, curveValue);
                if (!(_elapsedTime >= moveBackDuration)) return;
                Tf.position = _backPos;
                if (isCheck == true)
                {
                    contactCol.SetActive(true);
                }
                _isMovingBack = false;

            }
        }

        private void OnDrawGizmos()
        {
            if (checkMove == true) return;
            if (!restrictWithinRect) return;
            // Draw rectangle boundary in the editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(rect.center, rect.size);
        }

        private void OnMouseDown()
        {
            if (checkMove == true) return;
            if (!level.IsAllowInteract) return;
            if (enabled)
            {

                OnDragStart();
            }
        }

        public void OnMouseUp()
        {
            if (checkMove == true) return;
            if (enabled)
            {
                OnDragStop();
                if (moveBackOnRelease)
                {
                    StartRelease();
                }
            }
        }

        private void StartRelease()
        {
            _currentBackPos = Tf.position;
            _elapsedTime = 0;
            _isMovingBack = true;
        }

        /// <summary>
        /// Handles object dragging, including restrictions and snapping logic.
        /// </summary>
        private void HandleDragging()
        {
            // Get current mouse position in world coordinates
            Vector3 mouseWorldPos = level.Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPosition = mouseWorldPos + _offset;

            // Apply movement restrictions
            ApplyMovementRestrictions(ref targetPosition);

            // Apply snapping to center if within snap distance
            ApplyCenterSnapping(ref targetPosition);

            // Update object position
            Tf.position = targetPosition;
        }

        /// <summary>
        /// Apply movement restrictions such as axis locks or boundary constraints.
        /// </summary>
        private void ApplyMovementRestrictions(ref Vector3 position)
        {
            // Restrict movement within a rectangle if enabled
            if (restrictWithinRect)
            {
                position.x = Mathf.Clamp(position.x, rect.xMin, rect.xMax);
                position.y = Mathf.Clamp(position.y, rect.yMin, rect.yMax);
            }

            // Restrict X or Y movement if specified
            if (restrictX) position.x = Tf.position.x;
            if (restrictY) position.y = Tf.position.y;
        }

        /// <summary>
        /// Apply snapping to the parent center if within the snap distance.
        /// </summary>
        private void ApplyCenterSnapping(ref Vector3 position)
        {
            if (!(centerSnapDistance > 0)) return;
            if ((position - Tf.parent.position).sqrMagnitude < _centerSnapDistanceSqr)
            {
                position = Tf.parent.position; // Snap to parent center
            }
        }

        /// <summary>
        /// Called when dragging starts, sets initial state and offsets.
        /// </summary>
        private void OnDragStart()
        {
            // Kill any running tween
            MGUtils.KillTween(_twnReturn);
            // MMVibrationManager.Haptic(HapticTypes.Selection);

            // Set initial position if enabled
            if (zeroOnDragStart)
            {
                Vector3 mouseWorldPos = level.Camera.ScreenToWorldPoint(Input.mousePosition);
                Tf.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, Tf.position.z);
            }

            // Calculate squared snap distance
            _centerSnapDistanceSqr = centerSnapDistance * centerSnapDistance;

            // Calculate the offset between the object and the cursor
            _offset = Tf.position - level.Camera.ScreenToWorldPoint(Input.mousePosition);
            Level628.Ins.PlayPickSfx();
            _dragging = true;
            onDragStart?.Invoke();
        }

        /// <summary>
        /// Called when dragging stops, invokes events and checks offscreen position.
        /// </summary>
        private void OnDragStop()
        {
            _dragging = false;
            onDragStop?.Invoke();

            if (returnOffScreen)
            {
                CheckOffScreen();
            }
        }

        /// <summary>
        /// Checks if the object is off-screen and moves it back within screen bounds if necessary.
        /// </summary>
        private void CheckOffScreen()
        {
            if (!returnOffScreen) return;

            Vector3 screenPos = level.Camera.WorldToScreenPoint(Tf.position);
            if (!IsPositionOffScreen(screenPos)) return;
            // Calculate the nearest point inside the screen bounds
            Vector3 nearestInsideScreenPos = GetNearestOnScreenPosition(screenPos);
            nearestInsideScreenPos.z = Tf.position.z; // Preserve original Z position

            // Move object back to the nearest inside screen position
            MGUtils.KillTween(_twnReturn);
            _twnReturn = Tf.DOMove(nearestInsideScreenPos, 0.5f);

            return;

            bool IsPositionOffScreen(Vector3 scPos)
            {
                return scPos.x < 0 || scPos.x > Screen.width || scPos.y < 0 || scPos.y > Screen.height;
            }
        }

        /// <summary>
        /// Gets the nearest position inside the screen based on the current position.
        /// </summary>
        private Vector3 GetNearestOnScreenPosition(Vector3 screenPos)
        {
            if (screenPos.x < 0) screenPos.x = 0;
            if (screenPos.x > Screen.width) screenPos.x = Screen.width;
            if (screenPos.y < 0) screenPos.y = 0;
            if (screenPos.y > Screen.height) screenPos.y = Screen.height;

            return level.Camera.ScreenToWorldPoint(screenPos);
        }

        public void SetCanMoveBack(bool canMoveBack, bool forceMoveBack = false)
        {
            _canMoveBack = canMoveBack;
            if (_canMoveBack && forceMoveBack)
            {
                StartRelease();
            }
        }

    }
    [Serializable]
    public class SpriteForwarderData
    {
        public IngredientType ingredientType;
        public Sprite sprite;
        public Sprite altSprite;
    }
    public enum IngredientType
    {
        None = -1,
        Flour = 0,
        Sugar = 1,
        Yeast = 2,
        Water = 3,
        Oil = 4,
        Salt = 5,
        Milk = 6,
        Egg0 = 7,
        Egg1 = 8,
        Egg2 = 9,
        Egg3 = 10,
        Egg4 = 11,
        Beater = 12,
        Spatula = 13,
        Butter = 14,
        Sifter = 15,
        Men = 16,
        ColorRed = 17,
        ColorBlue = 18,
        Nilong = 19,
        Donut1 = 20,
        Donut2 = 21,
        Donut3 = 22,
        Donut4 = 29,
        ToppingChoco = 23,
        ToppingColor = 24,
        MenInSpoon = 25,
        Rolling = 26,
        Scissors = 27,
        Chisel = 28,
        Donut2Done = 30,
        Donut3Done = 31,
        Donut1Done = 32,
    }
}