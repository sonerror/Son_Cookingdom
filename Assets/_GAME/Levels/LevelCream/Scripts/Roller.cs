using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace sonnv
{
    public class Roller : MonoBehaviour, IPointerDownHandler,
        IDragHandler
    {
        [SerializeField] private CreamRollController rollController;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private AudioClip sfxAppear, sfxRoll;

        private bool _isMoving, _isDragging;

        // Trong Roller.cs
        public void MoveToNextRoll(Vector3 targetPos, Vector3 targetRot) // Đổi từ Transform sang Vector3
        {
            spriteRenderer.sortingOrder = 30;
            _isDragging = false;
            _isMoving = true;

            transform.DORotate(targetRot, 0.15f);
            transform.DOMove(targetPos, 0.15f).OnComplete(() =>
            {
                SoundManager.PlaySFX(sfxAppear);
                spriteRenderer.sortingOrder = -51;
                transform.DOPunchPosition(transform.up * -.5f, .3f, 1).OnComplete(() =>
                {
                    _isMoving = false;
                    rollController.ShowCurrentRoll();
                });
            });
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        [SerializeField] private float rollSpeed = 1f;

        private Vector2 _lastMousePos;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isMoving) return;
            _isDragging = true;
            _lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SoundManager.PlaySFX(sfxRoll);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isMoving || !_isDragging) return;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float delta = mousePos.y - _lastMousePos.y;
            _lastMousePos = mousePos;

            if (delta > 0)
                rollController.OnRolling(delta * rollSpeed);
        }
    }
}
