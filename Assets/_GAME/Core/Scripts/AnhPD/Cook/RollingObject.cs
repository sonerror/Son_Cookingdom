using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
    public class RollingObject : MonoBehaviour
    {
        [SerializeField] private Transform left, right;
        [SerializeField] private float scaleX_min = 1f;
        [SerializeField] private float scaleX_max = 1.5f;
        [SerializeField] private float duration = 3f;

        public Transform Left => left;
        public Transform Right => right;

        public UnityEvent completeEvent;

        private float timer;
        private bool isComplete = false;
        public void OnRolling(float rate)
        {
            if (isComplete) return;
            float x = scaleX_min + rate * (scaleX_max - scaleX_min);
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

            timer += Time.deltaTime;
            if (timer >= duration)
            {
                isComplete = true;
                completeEvent?.Invoke();
            }
        }
    }
}

