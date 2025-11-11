using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class RefillClock : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform clock;

        private Material _material;
        private bool _isShowing;
        private void Awake()
        {
            _material = spriteRenderer.material;
        }

        public void Show(float rate)
        {
            if(!FSAPDLevelBase.IsShowCooldownClock) return;
            if (!_isShowing)
            {
                _isShowing = true;
                clock.DOScale(1f, .3f).SetEase(Ease.InOutBack);
            }
            
            _material.SetFloat("_FillAmount", rate);
        }
        
        public void Hide()
        {
            if (_isShowing)
            {
                _isShowing = false;
                clock.DOScale(0f, .3f).SetEase(Ease.InBack);
            }
        }

        public void SetColor(Color color)
        {
            _material.color = color;
        }

#if UNITY_EDITOR
        public void ShowImmediate()
        {
            clock.localScale = Vector3.one;
        }

        public void HideImmediate()
        {
            clock.localScale = Vector3.zero;
        }
#endif
    }
}
