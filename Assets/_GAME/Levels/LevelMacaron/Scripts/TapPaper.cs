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
        [SerializeField] private Transform shoppingCart;
        [SerializeField] private AudioClip sfxOpenShoppingList;
        [SerializeField] private AudioClip sfxCloseShoppingList;
        public UnityEvent onClickOn;
        public UnityEvent onClickOff;
        public bool IsOn { get; private set; }

        private bool isOpenShoppingList = false;
        private bool isCanInteract = true;

        private void Start()
        {

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
        public void EventOpenShoppingList()
        {
            if (isOpenShoppingList || !isCanInteract) return;
            isCanInteract = false;
            isOpenShoppingList = true;
            rootShoppingList.gameObject.SetActive(true);
            rootMiniGame.gameObject.SetActive(false);
            shoppingCart.gameObject.SetActive(false);
            SoundManager.PlaySfx(sfxOpenShoppingList);
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
            shoppingList.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                rootMiniGame.gameObject.SetActive(true);
                shoppingCart.gameObject.SetActive(true);
                rootShoppingList.gameObject.SetActive(false);
                isCanInteract = true;
            });
        }
    }

}
