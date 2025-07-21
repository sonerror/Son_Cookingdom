using System;

using UnityEngine;
namespace sonnv
{
    public class ItemRotateAround : SonMonoBehaviour
    {
        [SerializeField] private Transform contactObject;
        [SerializeField] private Transform aroundPoint;
        [SerializeField] private float distanceToAcceptContact = 0.5f;
        [SerializeField] private float rotateSensitive = 1f;
        [SerializeField] private float ignoreInputAtCenterRadius = 0.5f;

        public Action<float> OnRotationDiffChanged;

        private bool _canInteract;
        private bool _isDragging;
        private LevelBase _level;
        private Vector2 _lastMouseInput;
        private const float TOLERANCE = 0.0001f;

        private void Awake()
        {
            if (aroundPoint == null) aroundPoint = contactObject;
        }

        private void Start()
        {
            _canInteract = true;
            _level = LevelBase.Ins;
            if (_level)
            {
                _level.onBlockPlayerInteractChanged += OnBlockInteract;
            }
            else
            {
                _canInteract = false;
            }
        }

        private void OnBlockInteract()
        {
            // if (_level.IsAllowInteract)
            // {
            _canInteract = true;
            // }
            // else
            // {
            //     if (_isDragging) EndDetect();
            //     _canInteract = false;
            // }
        }

        public void StartDetect()
        {
            if (!_canInteract || _isDragging) return;
            _isDragging = true;
            _lastMouseInput = Tf.position;
        }

        private void Update()
        {
            if (!_canInteract || !_isDragging) return;

            Vector2 currentMouseInput = Tf.position;

            if (IsValidInput(currentMouseInput))
            {
                float oldZ = contactObject.eulerAngles.z;
                // Rotate around the Z-axis of the aroundPoint
                contactObject.RotateAround(aroundPoint.position, Vector3.forward, GetRotationAngle(currentMouseInput) * rotateSensitive);
                HandleRotationDiffChanged(oldZ);
            }

            _lastMouseInput = currentMouseInput;
        }

        private bool IsValidInput(Vector2 currentMouseInput)
        {
            float distance = Vector2.Distance(currentMouseInput, aroundPoint.position);
            return distance >= ignoreInputAtCenterRadius && distance <= distanceToAcceptContact;
        }

        private float GetRotationAngle(Vector2 currentMouseInput)
        {
            // Compute the rotation direction
            Vector2 direction = currentMouseInput - (Vector2)aroundPoint.position;
            Vector2 previousDirection = _lastMouseInput - (Vector2)aroundPoint.position;
            // Calculate the angle between the current and previous directions
            return Vector2.SignedAngle(previousDirection, direction);
        }

        private void HandleRotationDiffChanged(float oldZ)
        {
            float newZ = contactObject.eulerAngles.z;
            float diff = Mathf.Abs(newZ - oldZ);

            // Handle cases where the difference exceeds 180 degrees if necessary
            if (diff > 180f) diff = 360f - diff;

            if (diff > TOLERANCE)
            {
                OnRotationDiffChanged?.Invoke(newZ - oldZ);
            }
        }

        public void EndDetect()
        {
            if (!_isDragging) return;
            _isDragging = false;
        }

        public void ForceSetInteract(bool value)
        {
            _canInteract = value;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (aroundPoint)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(aroundPoint.position, ignoreInputAtCenterRadius);
                // other gizmos color red 
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(aroundPoint.position, distanceToAcceptContact);
            }
        }
#endif
    }
}
