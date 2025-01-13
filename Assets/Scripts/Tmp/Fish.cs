
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class Fish : FishableObject
    {
        public enum ColorType
        {
            None = 0,
            Curver = 1,
            Speeder = 2,
            Wander = 3,
            Accer = 4,
            Siner = 5,
            KingCrab = 6,
        }
        [SerializeField] private ColorType color;
        [SerializeField] protected SkeletonAnimation skeletonAnimation;

        protected Transform tf;
        protected float speed = 1f, offsetX = .6f;
        protected float minY = -10f, maxY = 0f;
        protected int dir = 1;

        protected Vector3 minScreenBounds;
        protected Vector3 maxScreenBounds;

        public ColorType Color => color;
        public Transform Tf => tf ? tf : tf = transform;
        protected virtual void Awake()
        {
            minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
        }
        protected virtual void Update()
        {
            LockInScreen();
        }
        public virtual void OnInit(float speed, int dir)
        {
            this.speed = speed * Random.Range(.75f, 1.25f);
            this.dir = dir;

            skeletonAnimation.timeScale = speed / 3f;

            Tf.eulerAngles = new Vector3(0, dir > 0 ? 180f : 0, Tf.eulerAngles.z);

            float random = Random.Range(1f, 5f);
            float x = dir > 0 ? minScreenBounds.x - offsetX * random : maxScreenBounds.x + offsetX * random;
            Tf.position = new Vector2(x, RandomPositonY());
        }
        protected virtual void LockInScreen()
        {
            if (dir > 0 && Tf.position.x - offsetX > maxScreenBounds.x)
            {
                Tf.position = new Vector2(minScreenBounds.x - offsetX, RandomPositonY());
            }
            if (dir < 0 && Tf.position.x + offsetX < minScreenBounds.x)
            {
                Tf.position = new Vector2(maxScreenBounds.x + offsetX, RandomPositonY());
            }
        }
        private float RandomPositonY()
        {
            return Random.Range(minY, maxY);
        }
    }
}

