using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class Garbage : FishableObject
    {
        public enum GType
        {
            Bottle1 = 0,
            Bottle2 = 1,
            Can1 = 2,
            Can2 = 3,
            Bag = 4,
            Bin = 5,
            Shoe = 6,
        }
        public GType gType;
        protected float offsetX = 1f;
        protected float minY = -8.5f, maxY = 0f;

        protected Transform tf;
        public Transform Tf => tf ? tf : tf = transform;
        protected Vector3 minScreenBounds;
        protected Vector3 maxScreenBounds;

        private float initSpeed;

        protected virtual void Awake()
        {
            minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
        }
        private float speed = .3f, timer;
        private int dirX = 1, dirY = 1;
        private void OnEnable()
        {
            transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360f));
        }
        public void OnInit(float speed, int dir)
        {
            this.dirX = dir;
            initSpeed = speed;
            this.speed = initSpeed * Random.Range(0.75f, 1.25f);

            float random = Random.Range(1f, 5f);
            float x = dirX > 0 ? minScreenBounds.x - offsetX * random : maxScreenBounds.x + offsetX * random ;
            Tf.position = new Vector2(x, Random.Range(minY, maxY));
        }
        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 2f;
                dirY *= -1;
            }

            transform.position += Vector3.up * dirY * .1f * Time.deltaTime;
            transform.position += Vector3.right * dirX * speed * Time.deltaTime;

            LockInScreen();
        }

        protected virtual void LockInScreen()
        {
            float y = Random.Range(minY, maxY);
            if (dirX > 0 && Tf.position.x - offsetX > maxScreenBounds.x)
            {
                Tf.position = new Vector2(minScreenBounds.x - offsetX, y);
                speed = initSpeed * Random.Range(0.75f, 1.25f);
            }
            if (dirX < 0 && Tf.position.x + offsetX < minScreenBounds.x)
            {
                Tf.position = new Vector2(maxScreenBounds.x + offsetX, y);
                speed = initSpeed * Random.Range(0.75f, 1.25f);
            }
        }
    }
}

