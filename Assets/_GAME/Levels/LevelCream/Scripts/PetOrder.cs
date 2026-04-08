using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;


namespace sonnv
{
    public class PetOrder : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation anim;
        [SerializeField] private bool isShowPet = false;
        public bool IsShowPet => isShowPet;

        [SerializeField] private ShowObjectEffect showPet;
        [SerializeField] private OrderOfPet orderOfPet;
        [SerializeField] private string animNameIdle = "idle2";
        [SerializeField] private string animNameHappy = "drop";

        public void OnShow()
        {
            isShowPet = true;
            showPet.onShow.RemoveAllListeners();
            showPet.Show(0.25f);
            showPet.onShow.AddListener(() =>
            {
                OnDelayShow(0.5f);
            });
        }
        public void PlayDropThenIdle()
        {
            anim.AnimationState.SetAnimation(0, animNameHappy, false)
                .Complete += _ => anim.AnimationState.SetAnimation(0, animNameIdle, true);
        }
        public void OnShow(float delay)
        {
            DOVirtual.DelayedCall(delay, OnShow);
        }
        public void OnDelayShow(float delay)
        {
            DOVirtual.DelayedCall(delay, DelayShowListOrder);
        }
        private void DelayShowListOrder()
        {
            orderOfPet.OnShow();
        }
        public void OnHide()
        {
            showPet.onShow.RemoveAllListeners();
            orderOfPet.OnHide();
            showPet.Hide(0.5f);
        }
    }
}
