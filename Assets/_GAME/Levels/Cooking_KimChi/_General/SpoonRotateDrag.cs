using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace sonnv
{
    public class SpoonRotateDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Transform aroundPoint;
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private float smooth = 10f;
        [SerializeField] private bool isDone = false;
        public void SetIsDone()
        {
            isDone = true;
        }
        public event Action<float> OnPositionChanged;
        public event Action OnPositionUnChanged;

        private Camera cam;
        private float currentAngle;
        private float targetAngle;
        private float radius;
        private float initialZ;

        private const float MIN_DELTA = 0.001f;

        void Awake()
        {
            cam = Camera.main;

            radius = Vector2.Distance(transform.position, aroundPoint.position);
            initialZ = transform.position.z;

            Vector2 dir = (transform.position - aroundPoint.position).normalized;
            currentAngle = Mathf.Atan2(dir.y, dir.x);
            targetAngle = currentAngle;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDone) return;
            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(eventData.position);

            Vector2 direction = (mouseWorldPos - (Vector2)aroundPoint.position).normalized;

            float newAngle = Mathf.Atan2(direction.y, direction.x);

            float angleDelta =
                Mathf.DeltaAngle(currentAngle * Mathf.Rad2Deg, newAngle * Mathf.Rad2Deg) * Mathf.Deg2Rad;

            if (Mathf.Abs(angleDelta) < MIN_DELTA)
                return;

            float rotateDir = Mathf.Sign(angleDelta);

            float step = rotateDir * rotateSpeed * Time.deltaTime;

            targetAngle += step;
        }

        void Update()
        {
            if (isDone) return;
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, smooth * Time.deltaTime);

            Vector2 newPos =
                (Vector2)aroundPoint.position +
                new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * radius;

            transform.position = new Vector3(newPos.x, newPos.y, initialZ);

            float movement = Mathf.Clamp(Mathf.Abs(targetAngle - currentAngle), 0f, 0.02f);

            OnPositionChanged?.Invoke(movement);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnPositionUnChanged?.Invoke();
        }
    }
}