using DG.Tweening;
using Satisgame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class RiceCooker : MonoBehaviour
    {
        [SerializeField] Transform tfSink, tfCooker;
        [SerializeField] SpriteRenderer waterCooker;
        [SerializeField] GameObject goCookerSide, goCookerFont;
        [SerializeField] DraggableObject draggable;
        [SerializeField] EmojiControl emoji;
        [SerializeField] WaterValue value;
        //Lid
        [SerializeField] DraggableObject lidDrag;
        [SerializeField] ObjectMoveToTarget objMoving;
        private bool isHaveWater = false, isInSink = false, isCooked = false;

        public bool IsInSink => isInSink;

        public void CheckDistance()
        {

            if (Vector2.Distance(transform.position, tfSink.position) < 1f)
            {
                if (!isHaveWater)
                {
                    emoji.ShowPositive();
                    transform.DOMove(tfSink.position, .1f).OnComplete(() =>
                    {
                        draggable.LockPosition();
                        isInSink = true;
                        LevelMakeSushi.Ins.OnPutCookerInSink();

                        draggable.unmoveOrder = 3;
                        draggable.SetUnmoveOrder();
                        draggable.ResetStartPos();
                    });

                    draggable.isActive = false;

                    LevelMakeSushi.Ins.UnlockCanCheckBroadVegetable();
                    TutorialManager.Ins.removeState(0);
                }
            }
            else
            {

                isInSink = false;
                LevelMakeSushi.Ins.OnCookerLeaveSink();
                draggable.unmoveOrder = 5;
            }


            if (!isCooked && isHaveWater
                && Mathf.Abs(transform.position.x - tfCooker.position.x) < .5f
                && Mathf.Abs(transform.position.y - tfCooker.position.y) < 1f)
            {
                Sequence sequence = DOTween.Sequence();
                draggable.LockPosition();
                draggable.SetMoveOrder();

                sequence.Append(transform.DOMove(tfCooker.position + Vector3.up * .75f, .3f).OnComplete(() =>
                {
                    draggable.unmoveOrder = 2;
                    draggable.SetUnmoveOrder();
                }));
                sequence.Append(transform.DOMove(tfCooker.position + Vector3.up * .3f, .2f));

                lidDrag.endDragEvents.AddListener(objMoving.CheckTarget);

                isCooked = true;

                TutorialManager.Ins.removeState(2);

            }

        }

        public void OnWater()
        {
            if (isHaveWater) return;
            isHaveWater = true;

            emoji.ShowPositive();
            waterCooker.DOFade(1f, .5f);
            draggable.startDragEvents.AddListener(dragStart);

            draggable.UnlockPosition();
            draggable.unmoveOrder = 5;

            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(.5f);
                value.TurnOff();
            }
        }
        void dragStart()
        {
            goCookerSide.SetActive(true);
            goCookerFont.SetActive(false);
        }
    }

}
