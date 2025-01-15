// using AnhPD.FootSpa;
using DG.Tweening;
// using Satisgame;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class KingCrabTool : MonoBehaviour
    {
        [SerializeField] protected Collider2D coll2D;
        [SerializeField] protected SpriteRenderer spriteRenderer;

        [SerializeField] protected FxType pickSfx = FxType.Pick;
        [SerializeField] protected FxType placeSfx = FxType.Drop;

        [SerializeField] protected Vector3 sizeInit;
        [SerializeField] protected Vector3 sizeInCrease;
        [SerializeField] protected int minLayer = 2;
        [SerializeField] protected float dropDistance = 0.5f;
        [SerializeField] protected float minY = -10f;

        [SerializeField] protected Vector2 startPos;
        [SerializeField] protected Vector3 startRotation;

        public bool IsReady { get; set; }
        public bool IsBlocked { get; set; }
        protected bool isDragging = false, isWarned;

        private Transform tf;
        public Transform Tf => tf ? tf : tf = transform;

        protected Vector3 mOffset;

        // protected EmojiControl emoji => LevelKingCrab.Instance.emoji;

        protected virtual void InitProperties()
        {
            coll2D = GetComponent<Collider2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            sizeInit = transform.localScale;
            sizeInCrease = transform.localScale * 1.02f;

            startPos = Tf.position;
            startRotation = Tf.eulerAngles;
        }


        protected virtual Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }



        protected virtual void MouseDown(BaseEventData eventData)
        {
            if (IsBlocked) return;

            SoundManager.Ins.PlayFx(pickSfx);
            mOffset = Tf.position - GetMouseWorldPos();

            Tf.DOKill();
            Tf.DORotate(startRotation, 0.15f);
            Tf.DOScale(sizeInCrease, 0.15f);

            spriteRenderer.sortingOrder = LevelKingCrab.maxLayer;

            isDragging = true;
            isWarned = false;

            StopAllCoroutines();
            DelayWarning();

            //nho xoa di
            //IsReady = true;
        }

        protected virtual void MouseUp(BaseEventData eventData)
        {
            //if (IsBlocked) return;
            if (!isDragging) return;

            SoundManager.Ins.PlayFx(placeSfx);
            //Tf.DOScale(sizeInit, 0.3f);
            if (!IsReady)
            {
                MouseUpWarning();
            }

            Rewind();
        }

        protected virtual void MouseDrag(BaseEventData eventData)
        {
            if (!isDragging) return;

            if (IsBlocked)
            {
                MouseUp(eventData);
                isDragging = false;
                return;
            }
            Vector3 pos = GetMouseWorldPos() + mOffset;
            Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
            pos = new Vector3(
                Mathf.Clamp(pos.x, minScreenBounds.x, maxScreenBounds.x),
                Mathf.Clamp(pos.y, minY, maxScreenBounds.y), 0
            );
            Tf.position = (Vector2)pos;
        }

        protected virtual void Rewind(Action completeAction = null)
        {
            isDragging = false;
            Tf.DOMove(startPos, .5f).OnComplete(() =>
            {
                SoundManager.Ins.PlayFx(placeSfx);
                spriteRenderer.sortingOrder = minLayer;
                completeAction?.Invoke();
            });
            Tf.DOScale(sizeInit, 0.3f);
            Tf.DORotate(startRotation, .3f);
        }
        protected void DelayWarning()
        {
            StartCoroutine(delay());
        }
        private IEnumerator delay()
        {
            yield return new WaitForSeconds(1f);
            if (isDragging && !isWarned && !IsReady)
            {
                // emoji.ShowNegative();
                isWarned = true;
            }
        }
        protected void MouseUpWarning()
        {
            if (!isWarned)
            {
                // emoji.ShowNegative();
                isWarned = true;
            }
        }
        public virtual void OnComplete()
        {
            IsReady = false;
            Rewind();
        }
#if UNITY_EDITOR
        private void SetUpEventTrigger()
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            AddEventTriggerEntry(EventTriggerType.PointerDown, MouseDown);
            AddEventTriggerEntry(EventTriggerType.PointerUp, MouseUp);
            AddEventTriggerEntry(EventTriggerType.Drag, MouseDrag);

            void AddEventTriggerEntry(EventTriggerType eventType, UnityAction<BaseEventData> action)
            {
                var entry = new EventTrigger.Entry
                {
                    eventID = eventType
                };
                UnityEditor.Events.UnityEventTools.AddPersistentListener(entry.callback, action);
                eventTrigger.triggers.Add(entry);
            }
        }
#endif
    }
}

