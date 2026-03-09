using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class KeepStartRotation : MonoBehaviour
    {
        private Vector3 _startRotation;

        private void Start()
        {
            _startRotation = transform.eulerAngles;
        }

        private void Update()
        {
            transform.eulerAngles = _startRotation;
        }
    }

}