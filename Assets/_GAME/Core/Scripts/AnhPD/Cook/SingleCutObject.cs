using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
    public class SingleCutObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer before, after;
        public UnityEvent cutEvent;
        public void OnCut()
        {
            before.enabled = false;
            after.enabled = true;
            after.gameObject.SetActive(true);

            transform.Appear();
            cutEvent?.Invoke();
        }
    }
}

