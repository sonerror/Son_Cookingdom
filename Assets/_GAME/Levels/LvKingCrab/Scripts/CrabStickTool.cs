using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnhPD.KingCrab
{
    public class CrabStickTool : KingCrabTool
    {
        enum State
        {
            Drag = 0,
            Push = 1,
            Animing = 2,
        }
        [SerializeField] List<Transform> meats;
        [SerializeField] Transform pos;
        [SerializeField] AudioClip sfxPush;
        private State state;
        private float pushDistance;

        Transform meatPos;

        protected override void MouseDown(BaseEventData eventData)
        {
            if (state != State.Drag)
            {
                isDragging = true;
                return;
            }

            base.MouseDown(eventData);
            Tf.DOComplete();
            Tf.DORotate(Vector3.zero, .3f);
        }
        protected override void MouseDrag(BaseEventData eventData)
        {
            if (state == State.Animing) return;

            switch (state)
            {
                case State.Drag:
                    base.MouseDrag(eventData);
                    if (!IsReady) return;
                    for (int i = 0; i < meats.Count; i++)
                    {
                        if (Vector2.Distance(Tf.position, meats[i].position) < dropDistance)
                        {
                            meatPos = meats[i];
                            meats.Remove(meats[i]);
                            ChangePushState();
                            return;
                        }
                    }
                    break;
                case State.Push:
                    Vector2 mousePos = GetMouseWorldPos();
                    if (mousePos.y > Tf.position.y)
                    {
                        float currentY = transform.position.y;
                        float nextY = Mathf.Lerp(currentY, mousePos.y, Time.deltaTime * 2);

                        Tf.position = new Vector2(Tf.position.x, nextY);

                        float pushDis = Mathf.Abs(nextY - currentY);
                        pushDistance += pushDis;

                        meatPos.position += new Vector3(0, pushDis);

                        if (pushDistance > 2f)
                        {
                            ChangeDragState();
                        }
                    }
                    break;
            }
        }
        protected override void MouseUp(BaseEventData eventData)
        {
            if (state != State.Drag)
            {
                isDragging = false;
                return;
            }

            base.MouseUp(eventData);
        }
        private void ChangePushState()
        {
            //AudioManager.PlaySFX(sfxPush);
            pushDistance = 0;
            state = State.Animing;
            Tf.DORotate(Vector3.zero, .3f);
            Tf.DOMove((Vector2)meatPos.position - Vector2.up * 4f, .3f).OnComplete(() =>
            {
                spriteRenderer.sortingOrder = 4;
                Tf.DOMove((Vector2)meatPos.position - Vector2.up * 2.5f, .3f).OnComplete(completeAnim);
            });
            void completeAnim()
            {
                state = State.Push;
            }
        }
        private void ChangeDragState()
        {
            state = State.Animing;
            Tf.DOMove((Vector2)meatPos.position - Vector2.up * 4f, .3f).OnComplete(delay);
            meatPos = null;
            void delay()
            {
                spriteRenderer.sortingOrder = LevelKingCrab.maxLayer;
                if (isDragging)
                {
                    Tf.position = (Vector2)(GetMouseWorldPos() + mOffset);
                    Tf.DOPunchScale(Vector3.one * .1f, .3f);
                }
                else
                {
                    Rewind();
                }
                completeAnim();
            }
            void completeAnim()
            {
                state = State.Drag;
                if (meats.Count < 1)
                {
                    LevelKingCrab.Instance.OnCompleteCrabStick();
                }
            }
        }
    }
}

