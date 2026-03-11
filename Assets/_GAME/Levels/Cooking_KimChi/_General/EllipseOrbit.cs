using System;
using UnityEngine;

namespace sonnv
{
    public class EllipseOrbit : MonoBehaviour
    {
        public Transform centerPoint; // The point around which the object rotates
        public float a = 5f; // Semi-major axis (horizontal radius)
        public float b = 3f; // Semi-minor axis (vertical radius)

        private float _angle; // Current angle

        private void Start()
        {
            SetupCurrentAngle();
        }

        private void SetupCurrentAngle()
        {
            if (centerPoint == null) return;

            Vector3 direction = transform.position - centerPoint.position;
            _angle = Mathf.Atan2(direction.y, direction.x);
            UpdatePosition();
        }

        public void AddAngle(float value)
        {
            _angle += value; // Increment the angle
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (centerPoint == null) return;

            float x = a * Mathf.Cos(_angle);
            float y = b * Mathf.Sin(_angle);

            transform.position = centerPoint.position + new Vector3(x, y, 0);
        }

        // Draw the ellipse in the Scene View when selected
        private void OnDrawGizmosSelected()
        {
            if (centerPoint == null) return;

            Gizmos.color = Color.green;

            int segments = 100;
            Vector3 previousPoint = centerPoint.position + new Vector3(a, 0, 0);

            for (int i = 1; i <= segments; i++)
            {
                float theta = i / (float)segments * Mathf.PI * 2;
                float x = a * Mathf.Cos(theta);
                float y = b * Mathf.Sin(theta);
                Vector3 nextPoint = centerPoint.position + new Vector3(x, y, 0);

                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }
        }
    }
}