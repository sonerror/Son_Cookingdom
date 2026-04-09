using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace sonnv
{
    public class TapPaper : MonoBehaviour
    {
        [SerializeField] private Transform shoppingList;
        [SerializeField] private Transform rootShoppingList;
        [SerializeField] private Transform rootMiniGame;
        [SerializeField] private AudioClip sfxOpenShoppingList;
        [SerializeField] private AudioClip sfxCloseShoppingList;
        [SerializeField] private bool blockClickOpen = false;
        [SerializeField] private bool isTapClose = false;
        [SerializeField] private Transform closeTargetPoint;

        private Vector3 originalPosition;

        public void OnBlockClickOpen(bool value)
        {
            blockClickOpen = value;
        }

        public UnityEvent onClickOn;
        public UnityEvent onClickOff;
        public bool IsOn { get; private set; }

        private bool isOpenShoppingList = false;
        private bool isCanInteract = true;
        private void Start()
        {
            originalPosition = shoppingList.position;
        }
        public void ClickButton()
        {
            IsOn = !IsOn;
            if (IsOn)
            {
                EventOpenShoppingList();
                onClickOn?.Invoke();
            }
            else
            {
                EventCloseShoppingList();
                onClickOff?.Invoke();
            }
        }
        public void OnOpenShoppingList()
        {
            IsOn = !IsOn;
            if (IsOn)
            {
                OpenShoppingList();
                onClickOn?.Invoke();
            }
        }
        private void OpenShoppingList()
        {
            if (blockClickOpen) return;
            if (isOpenShoppingList || !isCanInteract) return;
            isCanInteract = false;
            isOpenShoppingList = true;
            rootShoppingList.gameObject.SetActive(true);
            rootMiniGame.gameObject.SetActive(false);
            shoppingList.position = originalPosition;
            shoppingList.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                isCanInteract = true;
            });
        }
        public void EventOpenShoppingList()
        {
            if (blockClickOpen) return;
            if (isOpenShoppingList || !isCanInteract) return;
            isCanInteract = false;
            isOpenShoppingList = true;
            rootShoppingList.gameObject.SetActive(true);
            rootMiniGame.gameObject.SetActive(false);
            SoundManager.PlaySfx(sfxOpenShoppingList);
            shoppingList.position = originalPosition;
            shoppingList.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                isCanInteract = true;
            });
        }

        public void EventCloseShoppingList()
        {
            if (!isOpenShoppingList || !isCanInteract) return;
            isCanInteract = false;
            isOpenShoppingList = false;
            SoundManager.PlaySfx(sfxCloseShoppingList);

            float duration = 0.3f;

            shoppingList.DOScale(0, duration).SetEase(Ease.InBack);

            if (closeTargetPoint != null)
            {
                shoppingList.DOMove(closeTargetPoint.position, duration).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        rootMiniGame.gameObject.SetActive(true);
                        rootShoppingList.gameObject.SetActive(false);
                        isCanInteract = true;
                    });
            }
            else
            {
                DOVirtual.DelayedCall(duration, () =>
                {
                    rootMiniGame.gameObject.SetActive(true);
                    rootShoppingList.gameObject.SetActive(false);
                    isCanInteract = true;
                });
            }

            if (isTapClose == false)
            {
                isTapClose = true;
            }
        }
    }
}