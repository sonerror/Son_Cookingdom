using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class CrabHammer : KingCrabTool
    {
        [SerializeField] Transform pos;
        [SerializeField] CrabBody crabBody;
        [SerializeField] FxType sfxHammer = FxType.Hammer;
        private bool isSmashing;

        protected override void MouseDown(BaseEventData eventData)
        {
            base.MouseDown(eventData);
            Tf.DOComplete();
            Tf.DORotate(new Vector3(0, 0, 15f), .3f);
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            base.MouseDrag(eventData);
            if (!IsReady || isSmashing) return;
            if (Vector2.Distance(Tf.position, crabBody.transform.position) < dropDistance)
            {
                Smash();
            }
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            if (!isDragging) return;
            Tf.DOKill();
            isSmashing = false;
            base.MouseUp(eventData);
            if (!IsReady) return;

        }
        private void Smash()
        {
            isSmashing = true;
            Tf.DORotate(new Vector3(0, 0, -55f), .2f);
            Tf.DORotate(new Vector3(0, 0, 115f), .3f).OnComplete(checkTarget).SetDelay(.3f);

            void checkTarget()
            {
                if (Vector2.Distance(pos.position, crabBody.transform.position) < dropDistance)
                {
                    crabBody.OnSmash();
                    SoundManager.Ins.PlayFx(sfxHammer);
                }
                Tf.DORotate(new Vector3(0, 0, -15f), .3f).OnComplete(() =>
                {
                    isSmashing = false;
                });
            }
        }
    }
}

