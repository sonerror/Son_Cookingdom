using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class ActionMove : ActionBase
    {
        public enum Direction
        {
            Up, Down, Left, Right
        }

        public enum State
        {
            MoveIn, MoveOut, None
        }

        [SerializeField] Transform tf;
        [SerializeField] public float time = .5f;
        [SerializeField, Range(0, 1)] float rate = 0.9f;
        [SerializeField] Vector2 mid, finish;
        [SerializeField] Direction direction;
        [field: SerializeField] public State state { get; private set; }

        public override void OnActive()
        {
            gameObject.SetActive(startActive);
            SonUtilities.DelayedCallScaled(delay, () =>
            {
                gameObject.SetActive(true);
                if (Vector2.Distance(mid, Vector2.zero) < 0.05f)
                {
                    tf.DOMove((Vector3)finish + tf.position, time).OnComplete(OnDone);
                }
                else
                {
                    Vector2 start = tf.position;
                    tf.DOMove(start + mid, time * rate).SetEase(Ease.InOutQuart).OnComplete(() => tf.DOMove(start + finish, time * (1 - rate)).OnComplete(OnDone));
                }
            });
        }

        public void MoveBack()
        {
            var targetPos = new Vector3(transform.localPosition.x - finish.x, transform.localPosition.y - finish.y, transform.localPosition.z);
            gameObject.SetActive(true);
            tf.DOMove((Vector3)targetPos, time).OnComplete(OnDone);
        }

        protected override void OnDone()
        {
            base.OnDone();
            PlayFx();
        }

        private void OnValidate()
        {
            tf = transform;
        }

        protected override void Setup()
        {
            base.Setup();

            switch (state)
            {
                case State.MoveIn:
                    startActive = false;
                    doneActive = true;
                    break;
                case State.MoveOut:
                    startActive = true;
                    doneActive = false;
                    break;
            }

            switch (direction)
            {
                case Direction.Up:
                    switch (state)
                    {
                        case State.MoveIn:
                            mid = new Vector2(0, 4.2f);
                            finish = new Vector2(0, 4);
                            rate = 0.8f;
                            break;
                        case State.MoveOut:
                            rate = 0.2f;
                            mid = new Vector2(0, -.2f);
                            finish = new Vector2(0, 4);
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (state)
                    {
                        case State.MoveIn:
                            mid = new Vector2(0, -4.2f);
                            finish = new Vector2(0, -4);
                            rate = 0.8f;
                            break;
                        case State.MoveOut:
                            mid = new Vector2(0, .2f);
                            finish = new Vector2(0, -4);
                            rate = 0.2f;
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (state)
                    {
                        case State.MoveIn:
                            mid = new Vector2(-4.2f, 0);
                            finish = new Vector2(-4, 0);
                            rate = 0.8f;
                            break;
                        case State.MoveOut:
                            mid = new Vector2(.2f, 0);
                            finish = new Vector2(-4, 0);
                            rate = 0.2f;
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (state)
                    {
                        case State.MoveIn:
                            mid = new Vector2(4.2f, 0);
                            finish = new Vector2(4, 0);
                            rate = 0.8f;
                            break;
                        case State.MoveOut:
                            mid = new Vector2(-.2f, 0);
                            finish = new Vector2(4, 0);
                            rate = 0.2f;
                            break;
                    }
                    break;
            }
        }

        [Button()]
        public void SetUpPosition()
        {
            transform.localPosition = new Vector3(transform.localPosition.x - finish.x, transform.localPosition.y - finish.y, transform.localPosition.z);
        }

        [Button()]
        public void OnBackPosition()
        {
            transform.localPosition = new Vector3(transform.localPosition.x + finish.x, transform.localPosition.y + finish.y, transform.localPosition.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(tf.position, (Vector2)tf.position + mid);
            Gizmos.DrawLine((Vector2)tf.position + mid, (Vector2)tf.position + finish);
            Gizmos.DrawWireSphere((Vector2)tf.position + mid, 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere((Vector2)tf.position, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)tf.position + finish, 0.1f);
        }
    }
}