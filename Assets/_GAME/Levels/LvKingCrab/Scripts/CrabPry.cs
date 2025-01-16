using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class CrabPry : KingCrabTool
    {
        [SerializeField] CrabBody target;
        [SerializeField] FxType sfxOpen = FxType.OpenShell;
        private bool isAniming;
        protected override void MouseDown(BaseEventData eventData)
        {
            if (isAniming) return;
            base.MouseDown(eventData);

            Tf.DOComplete();
            Tf.DORotate(Vector3.zero, .3f);
            if (IsReady) TutorialManager.Ins.ResetTimeHint();
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady || isAniming) return;

            if (Vector2.Distance(Tf.position, target.transform.position) < dropDistance)
            {
                StartAnim();
            }
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            if (isAniming) return;
            base.MouseUp(eventData);
        }
        private void StartAnim()
        {
            isAniming = true;
            Tf.DOMove(target.transform.position - Vector3.up * 5f, .3f).OnComplete(delay);
            void delay()
            {
                spriteRenderer.sortingOrder = -11;
                Tf.DOMove(target.transform.position, .3f).OnComplete(complete);
            }
            void complete()
            {
                SoundManager.Ins.PlayFx(sfxOpen);
                target.OnOpen();
                isAniming = false;

                LevelKingCrab.Instance.OnCompleteOpenCrab();
            }
        }
    }
}

