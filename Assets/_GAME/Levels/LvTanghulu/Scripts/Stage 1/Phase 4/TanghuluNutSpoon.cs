using System;
using AnhPD.CookV2;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.Tanghulu
{
    public class TanghuluNutSpoon : APDv2Drag
    {
        [SerializeField] private Transform almond;
        [SerializeField] private GameObject spoonAlmond;
        private bool _isDrag, _isHaveAlmond;

        public Action OnScoop;
        public Action OnReturnObject;
        
        protected override void CheckTarget()
        {
            //if (!_isDrag) return;
            //base.CheckTarget();
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady) return;
            if (!_isHaveAlmond)
            {
                if(IsInRange(almond.position, center.position))
                {
                    OnScoop?.Invoke();
                    almond.DOComplete();
                    almond.gameObject.SetActive(false);
                    _isHaveAlmond = true;
                    spoonAlmond.SetActive(true);
                }
            }
            else
            {
                if (IsInRange(target.position, center.position))
                {
                    IsReady = false;
                    _isHaveAlmond = false;
                    spoonAlmond.SetActive(false);

                    OnComplete();
                    ReturnToStartPos();
                }
            }
        }

        protected override void MouseUp(BaseEventData eventData)
        {
            base.MouseUp(eventData);
            if (!IsReady) return;
            _isHaveAlmond = false;
            spoonAlmond.SetActive(false);
            almond.gameObject.SetActive(true);
            OnReturnObject?.Invoke();
        }

        public void DragMode()
        {
            _isDrag = true;
            IsReady = true;
        }
    }
}
