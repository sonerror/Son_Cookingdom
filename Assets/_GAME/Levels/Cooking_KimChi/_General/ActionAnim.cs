using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    [RequireComponent(typeof(Animation))]
    public class ActionAnim : ActionBase
    {
        [SerializeField] Animation animation;
        [SerializeField] AnimationClip animName;

        public override void OnActive()
        {
            gameObject.SetActive(startActive);
            SonUtilities.DelayedCallScaled(delay, () =>
             {
                 gameObject.SetActive(true);
                 animation.Play(animName.name);
                 SonUtilities.DelayedCallScaled(animName.length, OnDone);
                 PlayFx();
             });
        }

        private void OnValidate()
        {
            animation = GetComponent<Animation>();
            if (animName != null && animation.GetClip(animName.name) != null)
            {
                animation.AddClip(animName, animName.name);
            }
        }

        protected override void Setup()
        {
            base.Setup();
            OnValidate();
            if (GetComponent<ItemAlpha>() == null)
            {
                gameObject.AddComponent<ItemAlpha>();
            }

            animation.playAutomatically = false;
        }

        public ActionMove.State GetState()
        {
            return animName.name switch
            {
                "IdleItemAppear" => ActionMove.State.MoveIn,
                "IdleItemHide" => ActionMove.State.MoveOut,
                _ => ActionMove.State.MoveIn,
            };
        }
    }
}