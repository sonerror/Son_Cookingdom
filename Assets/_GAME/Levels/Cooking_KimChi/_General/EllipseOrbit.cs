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
            _angle += value;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (centerPoint == null) return;

            float x = a * Mathf.Cos(_angle);
            float y = b * Mathf.Sin(_angle);

            transform.position = centerPoint.position + new Vector3(x, y, 0);
        }

    }
}