using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
namespace sonnv
{
    public class MochiInTray : SonMonoBehaviour
    {
        [SerializeField] private int hitNeed = 5;

        [SerializeField] private SpriteRenderer spriteCurrent;
        [SerializeField] private SpriteRenderer spriteDone;
        [SerializeField] private AudioClip sfxRoll;
        [SerializeField] private UnityEvent onDone;
        [SerializeField] private float punchScale = 0.15f;
        [SerializeField] private float punchDuration = 0.2f;

        [SerializeField] private FlourMoveToCream moveController;

        [SerializeField] private MixFlourManager mixFlourManager;
        [SerializeField] private bool isColor = false;
        public bool IsColor => isColor;
        public void ChangeStateIsColor(bool _value)
        {
            isColor = _value;
        }
        private int currentHit = 0;
        private int maxHit = 5;
        private bool isTrigger = true;

        private void Awake()
        {
            Init(hitNeed);
        }

        public void Init(int hitCount)
        {
            maxHit = hitCount;
            currentHit = 0;
            isTrigger = true;

            SetAlpha(spriteCurrent, 1f);
            SetAlpha(spriteDone, 0f);
        }

        public void Hit()
        {
            if (isColor == false) return;
            if (!isTrigger) return;
            if (IsDone()) return;

            Debug.Log("Hit");

            SoundManager.PlaySFXOneShot(sfxRoll);

            currentHit++;

            float progress = Mathf.Clamp01((float)currentHit / maxHit);

            SetAlpha(spriteCurrent, 1f - progress);
            SetAlpha(spriteDone, progress);

            Tf.DOPunchScale(Vector3.one * punchScale, punchDuration, 5, 0.5f);

            if (IsDone())
            {
                isTrigger = false;
                Debug.Log("Done");
                OnDoneRoll();
                onDone?.Invoke();
            }
        }
        private void OnDoneRoll()
        {
            if (moveController == null || mixFlourManager == null) return;
            moveController.SetData(mixFlourManager.GetSnapItem());
            moveController.ChangeCanBlockTap(false);
        }
        public bool IsDone()
        {
            return currentHit >= maxHit;
        }

        private void SetAlpha(SpriteRenderer sr, float a)
        {
            Color c = sr.color;
            c.a = a;
            sr.color = c;
        }
    }
}