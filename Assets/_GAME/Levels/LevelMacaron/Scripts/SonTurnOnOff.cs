using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
namespace sonnv
{
    public class SonTurnOnOff : SonMonoBehaviour, IPointerDownHandler, IPointerClickHandler
    {
        [SerializeField] private Collider2D col;
        [SerializeField] private SpriteRenderer srTurnOn;
        [SerializeField] private SpriteRenderer srTurnOff;
        [SerializeField] private AudioClip turnOnSound;
        [SerializeField] private AudioClip turnOffSound;
        [SerializeField] private bool blockInteractManually;
        [SerializeField] private bool isBolckTap = false;
        [SerializeField] private bool isHideByObj = false;
        [SerializeField] private bool isCheckEvent = false;

        public UnityEvent onClickOn;
        public UnityEvent onClickOff;
        public System.Action onBlock;

        private bool _canInteract = true;
        public Collider2D Col => col;

        public bool IsOn { get; private set; }
        public UnityEvent onStart;
        private void Start()
        {
            if (isBolckTap == false)
            {
                col.enabled = _canInteract;
            }
            OnCheckEvent();
            UpdateSpriteState();
        }

        public void SetBlockManually(bool block)
        {
            blockInteractManually = block;
        }

        private void OnBlockInteract()
        {
        }
        private void OnCheckEvent()
        {
            if (isCheckEvent)
            {
            }
        }
        public void OnPointerDown(PointerEventData eventData)

        {
            if (!_canInteract) return;
            if (blockInteractManually) return;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_canInteract) return;
            if (blockInteractManually || !IsMatchCondition())
            {
                onBlock?.Invoke();
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
            UpdateSpriteState();

            SoundManager.PlaySFX(IsOn ? turnOnSound : turnOffSound);

            if (IsOn)
            {
                onClickOn.Invoke();
            }
            else
            {
                onClickOff.Invoke();
            }
        }

        private void UpdateSpriteState()
        {
            if (isHideByObj)
            {
                srTurnOn.gameObject.SetActive(IsOn);
                srTurnOff.gameObject.SetActive(!IsOn);
            }
            else
            {
                srTurnOn.enabled = IsOn;
                srTurnOff.enabled = !IsOn;
            }
        }
    }
}
