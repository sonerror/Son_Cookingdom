using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class OrderTimer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform timeOut;
        [SerializeField] private float duration = 10f;
        
        public Action OnTimeout, OnTimeOutComplete;
        public bool IsSatisfied => _timer < duration || duration <= 0f;
        public bool isUsed;
        
        private float _timer;
        private Material _material;
        private bool _isWaiting;
        private float _rate;
        private void Awake()
        {
            _material = spriteRenderer.material;
        }

        public void Init(float duration)
        {
            if(!isUsed) return;
            
            _timer = 0f;
            this.duration = duration;
            timeOut.localScale = Vector3.zero;
            
            UpdateDisplay();
            gameObject.SetActive(true);
        }
        public void StartWaiting()
        {
            if(duration <= 0f || !isUsed) return;
            _isWaiting = true;
            UpdateDisplay();
        }

        public void StopWaiting()
        {
            _isWaiting = false;
        }
        private void Update()
        {
            if(!_isWaiting || duration < 0) return;
            _timer += Time.deltaTime;
            UpdateDisplay();
            if (_timer >= duration)
            {
                _isWaiting = false;
                TimeOut();
            }
        }

        private void TimeOut()
        {
            if(!isUsed) return;
            
            OnTimeout?.Invoke();
            timeOut.DOScale(1f, .3f).SetEase(Ease.InOutBack)
                .OnComplete((() => OnTimeOutComplete?.Invoke()));
        }
        private void UpdateDisplay()
        {
            float rate = 1f - _timer / duration;
            float rateLerp = Mathf.Lerp(_rate, rate, 1f);
            if(!_material) _material = spriteRenderer.material;
            _material.SetFloat("_FillAmount", rateLerp);
            _material.SetColor("_Color", Color.Lerp(Color.red, Color.green, rate));
        }

        public void PlusTime(float time)
        {
            if(!isUsed) return;
            
            _timer += time;
            UpdateDisplay();
        }
    }
}
