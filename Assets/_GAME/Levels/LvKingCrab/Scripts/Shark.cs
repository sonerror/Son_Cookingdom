using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TrungNV;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class Shark : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation anim;
        [SerializeField] AudioClip sfxBite;
        protected Transform tf;
        protected float speed = 1f, offsetX = 2.5f;
        protected float minY = -8.5f, maxY = 1f;
        protected int dir = 1;
        private float biteTimer = 0f;

        protected Vector3 minScreenBounds;
        protected Vector3 maxScreenBounds;

        private Vector2 Velocity;

        public Transform Tf => tf ? tf : tf = transform;

        private State state;
        private enum State
        {
            Biting = 0,
            Moving = 1,
        }

        private void Start()
        {
            minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
        }

        private void Update()
        {
            switch (state)
            {
                case State.Biting:
                    biteTimer -= Time.deltaTime;
                    if (biteTimer < 0f)
                    {
                        Tf.DORotate(new Vector3(0, Tf.eulerAngles.y, 0), .3f);
                        state = State.Moving;
                    }
                    break;
                case State.Moving:

                    dir = Velocity.x > 0 ? 1 : -1;
                    Tf.eulerAngles = new Vector3(0, dir > 0 ? 180f : 0, Tf.eulerAngles.z);

                    Tf.position += (Vector3)Velocity * speed * Time.deltaTime;

                    LockInScreen();
                    break;
            }
        }
        public void OnInit(float speed)
        {
            this.speed = speed * Random.Range(.75f, 1.25f);
            anim.timeScale = speed / 3f;

            state = State.Moving;

            Velocity = Vector2.right * (Random.Range(0, 2) * 2 - 1);
            dir = Velocity.x > 0 ? 1 : -1;

            float random = Random.Range(1f, 5f);
            float x = dir > 0 ? minScreenBounds.x - offsetX * random : maxScreenBounds.x + offsetX * random;
            Tf.position = new Vector2(x, Random.Range(minY, maxY));
        }
        private void LockInScreen()
        {
            float y = Random.Range(minY, maxY);
            if (dir > 0 && Tf.position.x - offsetX > maxScreenBounds.x)
            {
                Tf.position = new Vector2(Tf.position.x, y);

                Velocity *= -1;
            }
            if (dir < 0 && Tf.position.x + offsetX < minScreenBounds.x)
            {
                Tf.position = new Vector2(Tf.position.x, y);

                Velocity *= -1;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Level110HookPlayer hook = TrufnCache<Level110HookPlayer>.GetCol2D(collision);
            if (hook != null && !hook.IsCatching)
            {
                OnBite(collision.transform.position);
                hook.OnBited();
            }
        }
        private void OnBite(Vector2 pos)
        {
            // AudioManager.PlaySFX(sfxBite);
            dir = pos.x > Tf.position.x ? 1 : -1;
            Tf.eulerAngles = new Vector3(0, dir > 0 ? 180f : 0, Tf.eulerAngles.z);

            Velocity = Vector2.right * dir;

            Vector2 directionToTarget = pos - (Vector2)Tf.position;

            float angle = Vector2.SignedAngle(Vector2.right, directionToTarget);

            Tf.rotation = Quaternion.Euler(0, Tf.eulerAngles.y, (angle + (dir < 0 ? 180f : 0)) * (dir > 0 ? -1 : 1));

            state = State.Biting;
            biteTimer = .5f;
        }
    }
}

