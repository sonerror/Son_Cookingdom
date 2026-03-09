using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class MixingSauce : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer mixingEffect;
        [SerializeField] private float cooldown = .2f;
        private float _timer;
        private bool _isFlipX = true, _isMixing = false;

        public bool isMouseDownEffect = true;
        private void FlipX()
        {
            float rotationX = transform.localEulerAngles.x;
            rotationX = Mathf.Approximately(rotationX, 180f) ? 0f : 180f;
            transform.localEulerAngles = new Vector3(rotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        private void FlipY()
        {
            float rotationY = transform.localEulerAngles.y;
            rotationY = Mathf.Approximately(rotationY, 180f) ? 0f : 180f;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationY, transform.localEulerAngles.z);
        }

        private void FixedUpdate()
        {
            _timer -= Time.deltaTime;
            if(isMouseDownEffect)
                mixingEffect.enabled = _isMixing;
            _isMixing = false;
        }

        public void OnMixing()
        {
            _isMixing = true;
            mixingEffect.enabled = _isMixing;
;            if (_timer < 0f)
            {
                _timer = cooldown;
                
                if(_isFlipX) FlipX();
                else FlipY();
                
                _isFlipX = !_isFlipX;
            }
        }

        public void OnRestart()
        {
            _isMixing = false;
            mixingEffect.enabled = false;
        }

        public void OnComplete()
        {
            transform.localEulerAngles = Vector3.zero;
            transform.Appear();
        }
    }
}
