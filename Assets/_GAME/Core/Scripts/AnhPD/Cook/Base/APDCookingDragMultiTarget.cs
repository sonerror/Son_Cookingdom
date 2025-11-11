using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
    public class APDCookingDragMultiTarget : APDCookingToolBase
    {
        [SerializeField] private SpriteRenderer[] parts;
        [SerializeField] private SpriteRenderer fixedSprite;
        [SerializeField] private List<Transform> target;
        [SerializeField] private float zAngle;
        [SerializeField] private bool isMouseUpCheck = true;

        [FoldoutGroup("Event")] public UnityEvent mouseDownEvent, mouseUpEvent, landEvent, completeEvent;
        [FoldoutGroup("Event")] public List<UnityEvent> completeEvents;

        protected override void MouseDown(BaseEventData eventData)
        {
            base.MouseDown(eventData);
            mouseDownEvent?.Invoke();
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].sortingOrder = spriteRenderer.sortingOrder;
            }
            if (fixedSprite != null)
            {
                fixedSprite.enabled = false;
                spriteRenderer.enabled = true;
            }
            Tf.DOLocalRotate(new Vector3(0, 0, zAngle), .3f);
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (isMouseUpCheck || !IsReady) return;
            CheckTarget();
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            base.MouseUp(eventData);
            mouseUpEvent?.Invoke();
            if (!isMouseUpCheck || !IsReady) return;
            CheckTarget();
        }
        protected virtual void CheckTarget()
        {
            for(int i = 0; i < target.Count; i++)
            {
                if (IsInRange(target[i]))
                {
                    isDragging = false;
                    Tf.DOKill();

                    OnComplete();

                    completeEvents[i]?.Invoke();
                    completeEvents.RemoveAt(i);
                    target.RemoveAt(i);
                    if(target.Count < 1)
                    {
                        completeEvent?.Invoke();
                    }
                    return;
                }
            }
        }
        public override void OnComplete()
        {
            base.OnComplete();
            spriteRenderer.sortingOrder = -1000;
            UpdateSortingOrder(spriteRenderer.sortingOrder);
            if(target.Count > 0)
            {
                IsReady = true;
            }
        }
        protected override void Rewind(Action completeAction = null)
        {
            base.Rewind(complete);
            void complete()
            {
                landEvent?.Invoke();
                completeAction?.Invoke();
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i].sortingOrder = spriteRenderer.sortingOrder;
                }
                if (fixedSprite != null)
                {
                    fixedSprite.enabled = true;
                    spriteRenderer.enabled = false;
                }
            }
        }
        private void UpdateSortingOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].sortingOrder = spriteRenderer.sortingOrder;
            }
        }
    }
}

