using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PouringObject))]
    public class APDCookingPour : APDCookingToolBase
    {
        [SerializeField] private PouringObject pouringObject;
        [SerializeField] protected Transform target;
        [SerializeField] private Vector3 offset = new Vector3(-1f, 1.5f);
        public UnityEvent eventStartAnim, eventComplete;
        public bool IsComplete;
        public override void InitProperties()
        {
            base.InitProperties();
            pouringObject = GetComponent<PouringObject>();
            shadow = GetComponent<Shadow>();
        }
        protected override void MouseDown(BaseEventData eventData)
        {
            base.MouseDown(eventData);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].sortingOrder = spriteRenderer.sortingOrder;
            }
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady) return;
            CheckTarget();
        }

        protected virtual void CheckTarget()
        {
            if (IsInRange(target.transform))
            {
                Tf.DOKill();
                isDragging = false;
                coll2D.enabled = false;
                Vector3 pos = target.transform.position + offset;
                IsComplete = true;
                eventStartAnim?.Invoke();
                pouringObject.Pouring(pos, eventComplete.Invoke, OnComplete);
            }
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            if (!isDragging) return;
            base.MouseUp(eventData);
        }
        protected override void Rewind(Action completeAction = null)
        {
            coll2D.enabled = true;
            base.Rewind(complete);
            void complete()
            {
                completeAction?.Invoke();
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i].sortingOrder = spriteRenderer.sortingOrder;
                }
            }
        }
    }
}

