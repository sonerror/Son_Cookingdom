using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace sonnv
{
    public class SpoonRotateDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private Transform aroundPoint;
        [SerializeField] private float rotateSpeed = 1f;
        [SerializeField] private bool isDone = false;

        [SerializeField] private AudioClip sfxSplash;

        public void SetIsDone()
        {
            isDone = true;
        }

        public event Action<float> OnPositionChanged;
        public event Action OnPositionUnChanged;

        private Camera cam;

        private float currentAngle;
        private float lastMouseAngle;

        private float radius;
        private float initialZ;

        private bool isDragging;

        private float rotateAccum;
        private const float ROTATE_TO_SPLASH = Mathf.PI / 2f;

        private float nextPlayTime;

        void Awake()
        {
            cam = Camera.main;

            radius = Vector2.Distance(transform.position, aroundPoint.position);
            initialZ = transform.position.z;

            Vector2 dir = (transform.position - aroundPoint.position).normalized;
            currentAngle = Mathf.Atan2(dir.y, dir.x);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isDone) return;

            isDragging = true;

            Vector2 mouseWorld = cam.ScreenToWorldPoint(eventData.position);
            Vector2 dir = mouseWorld - (Vector2)aroundPoint.position;

            lastMouseAngle = Mathf.Atan2(dir.y, dir.x);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDone || !isDragging) return;

            Vector2 mouseWorld = cam.ScreenToWorldPoint(eventData.position);
            Vector2 dir = mouseWorld - (Vector2)aroundPoint.position;

            float mouseAngle = Mathf.Atan2(dir.y, dir.x);

            float delta = Mathf.DeltaAngle(
                lastMouseAngle * Mathf.Rad2Deg,
                mouseAngle * Mathf.Rad2Deg
            ) * Mathf.Deg2Rad;

            lastMouseAngle = mouseAngle;

            currentAngle += delta * rotateSpeed;

            UpdatePosition();

            float movement = Mathf.Abs(delta);

            OnPositionChanged?.Invoke(movement);

            HandleSplashSound(movement);
        }

        void HandleSplashSound(float movement)
        {
            rotateAccum += movement;

            if (rotateAccum < ROTATE_TO_SPLASH)
                return;

            rotateAccum = 0f;

            if (Time.time < nextPlayTime)
                return;

            SoundManager.PlaySFXOneShot(sfxSplash);

            nextPlayTime = Time.time + sfxSplash.length * 0.8f;
        }

        void UpdatePosition()
        {
            Vector2 pos =
                (Vector2)aroundPoint.position +
                new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * radius;

            transform.position = new Vector3(pos.x, pos.y, initialZ);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            OnPositionUnChanged?.Invoke();
        }
    }
}