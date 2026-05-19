using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace sonnv
{
    public class SpriteButtonOnOff : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {

        [SerializeField] private bool isBlockClick = false;
        public void SetStateIsBlock(bool value)
        {
            isBlockClick = value;
        }
        [SerializeField] private EmojiControl emoji;
        [SerializeField] private Collider2D col;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Sprite clickButtonSprite;
        [SerializeField] private Sprite unClickButtonSprite;
        [SerializeField] private AudioClip turnOnSound;
        [SerializeField] private AudioClip turnOffSound;
        [SerializeField] private bool blockInteractManually;
        [SerializeField] private bool isBolckTap = false;
        [SerializeField] private SpriteRenderer spriteHot;

        public UnityEvent onClickOn;
        public UnityEvent onClickOff;
        public System.Action onBlock;

        private bool _canInteract = true;
        public Collider2D Col => col;

        public bool IsOn { get; private set; }

        private void Start()
        {
            if (isBolckTap == false)
            {
                col.enabled = _canInteract;
            }
            sr.sprite = IsOn ? clickButtonSprite : unClickButtonSprite;
        }

        public void SetBlockManually(bool block)
        {
            blockInteractManually = block;
        }

        private void OnBlockInteract()
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_canInteract) return;
            if (blockInteractManually) return;

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            sr.color = Color.white;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_canInteract) return;
            if (blockInteractManually || !IsMatchCondition())
            {
                onBlock?.Invoke();
                return;
            }
            if(isBlockClick == true)
            {
                emoji.ShowNegative();
                return;
            }
            ClickButton();
        }

        protected virtual bool IsMatchCondition()
        {
            return true;
        }

        public void ClickButton()
        {
            IsOn = !IsOn;

            if (SoundManager.Instance != null)
            {
                SoundManager.PlaySFX(IsOn ? turnOnSound : turnOffSound);
            }

            sr.sprite = IsOn ? clickButtonSprite : unClickButtonSprite;

            if (spriteHot != null) spriteHot.enabled = IsOn;

            if (IsOn)
            {
                onClickOn?.Invoke();
            }
            else
            {
                onClickOff?.Invoke();
            }
        }
    }
}