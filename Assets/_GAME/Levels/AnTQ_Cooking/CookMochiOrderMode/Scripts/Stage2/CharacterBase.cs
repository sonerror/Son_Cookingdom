using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace sonnv
{
    public class CharacterBase : CharacterUnit
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        private string currentAnimationName;
        public IState<CharacterBase> currentState;

        public virtual void Update()
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
        }

        public virtual void ChangeState(IState<CharacterBase> newState)
        {
            if (currentState != null)
            {
                currentState.OnExit(this);
            }

            currentState = newState;

            if (currentState != null)
            {
                currentState.OnEnter(this);
            }
        }

        public virtual void ChangeSkeletonAnimationState(string animationName, bool loop = true)
        {
            if (currentAnimationName != animationName)
            {
                currentAnimationName = animationName;
                skeletonAnimation.AnimationState.SetAnimation(0, currentAnimationName, loop);
            }
        }

        public virtual float GetDurationAnimation(string animationName)
        {
            return skeletonAnimation.Skeleton.Data.FindAnimation(animationName).Duration;
        }

        public virtual void PlayAnimation(string animationName, bool loop = true)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
        }

        public virtual bool CheckCurrentState(IState<CharacterBase> state)
        {
            return currentState == state;
        }

        public virtual void PauseCurrentAnimation()
        {
            // set time scale to 0
            skeletonAnimation.AnimationState.TimeScale = 0;
        }

        public virtual void ResumeCurrentAnimation()
        {
            // set time scale to 1
            skeletonAnimation.AnimationState.TimeScale = 1;
        }


        public virtual void OnIdleEnter() { }
        public virtual void OnIdleExecute() { }
        public virtual void OnIdleExit() { }

        public virtual void OnWaitEnter() { }
        public virtual void OnWaitExecute() { }
        public virtual void OnWaitExit() { }

        public virtual void OnPoundEnter() { }
        public virtual void OnPoundExecute() { }
        public virtual void OnPoundExit() { }

        public virtual void OnKneadEnter() { }
        public virtual void OnKneadExecute() { }
        public virtual void OnKneadExit() { }

        public virtual void OnHitEnter() { }
        public virtual void OnHitExecute() { }
        public virtual void OnHitExit() { }

        public virtual void OnHurtEnter() { }
        public virtual void OnHurtExecute() { }
        public virtual void OnHurtExit() { }

        public virtual void OnSorryEnter() { }
        public virtual void OnSorryExecute() { }
        public virtual void OnSorryExit() { }
    }
}