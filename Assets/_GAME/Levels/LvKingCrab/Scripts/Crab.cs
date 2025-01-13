using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnhPD.Fishing;
using Spine.Unity;
using DG.Tweening;
// using Utilities;
namespace AnhPD.KingCrab
{
    public class Crab : Fish
    {
        [SpineAnimation(dataField = "skeletonAnimation")] public string animIdle = "idle";
        [SpineAnimation(dataField = "skeletonAnimation")] public string animWalk = "walk_r";

        private void Start()
        {
            MoveToRight();
        }

        private void MoveToRight()
        {
            Walk();
            Tf.DOMoveX(3.5f, 5f).OnComplete(delay);
            void delay()
            {
                Idle();
                float time = Random.Range(2f, 4f);
                // this.WaitToDo(MoveToLeft, time);
            }
        }
        private void MoveToLeft()
        {
            Walk();
            Tf.DOMoveX(-3.5f, 5f).OnComplete(delay);
            void delay()
            {
                Idle();
                float time = Random.Range(2f, 4f);
                // this.WaitToDo(MoveToRight, time);
            }
        }
        private void Idle()
        {
            skeletonAnimation.state.SetAnimation(0, animIdle, true);
        }
        private void Walk()
        {
            skeletonAnimation.state.SetAnimation(0, animWalk, true);
        }
    }
}

