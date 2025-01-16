using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class CrabPart : KingCrabTool
    {
        [SerializeField] public Transform target;
        [SerializeField] SpriteRenderer meat;
        protected override void MouseDown(BaseEventData eventData)
        {
            IsReady = true;
            base.MouseDown(eventData);
            Tf.DORotate(target.eulerAngles, .3f);
            spriteRenderer.sortingOrder = LevelKingCrab.maxLayer += 4;
            if (meat != null)
            {
                meat.sortingOrder = spriteRenderer.sortingOrder - 1;
            }
            TutorialManager.Ins.MouseDownItem();
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            //base.MouseUp(eventData);
            if (Vector2.Distance(Tf.position, target.position) < dropDistance)
            {
                coll2D.enabled = false;
                isDragging = false;

                Tf.DOScale(sizeInit, .3f);
                Tf.DOMove(target.position, .3f);
                Tf.DORotate(target.eulerAngles, .3f).OnComplete(() =>
                {
                    target.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                });

                LevelKingCrab.Instance.OnPutCrabPartIn();
            }
            else
            {
                Tf.DORotate(startRotation, .3f);
            }

            TutorialManager.Ins.MouseUpItem();
        }
    }
}

