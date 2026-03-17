using DG.Tweening;
using sonnv;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace sonnv
{
    public class FlourMoveToCream : SonTapItem
    {
        [SerializeField] private InforTFTarget tfTarget;
        public void SetData(InforTFTarget _tfTarget)
        {
            tfTarget = _tfTarget;
        }
        [Header("move")]
        [SerializeField] private int sortOrderMax = 10;
        [SerializeField] private float timerMove = 0.5f;
        [SerializeField] private float jumpPower = 2f;
        [SerializeField] private int numJumps = 1;
        [SerializeField] private float detalTarget = 0.145f;
        [SerializeField] private AudioClip sfxSnap;

        [SerializeField] private Vector3 scaleOnMove = Vector3.zero;
        [SerializeField] private bool isChangeScaleOnMove = false;
        [SerializeField] private bool canClockTap = false;
        [SerializeField] private bool isChangeScale = false;
        [SerializeField] private float detalSacle = 1;

        [SerializeField] private int forceScaleFrame = 0;

        public UnityEvent onComplete;
        public UnityEvent onStartMoveComplete;

        private Vector3 forceScaleValue;

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (canClockTap) return;

            base.OnPointerDown(eventData);
            OnMove();
        }

        public void OnMove()
        {
            if (tfTarget != null && tfTarget.IsSnap)
            {
                isMoving = true;
                tfTarget.ChangeIsSnap(false);
                onStartMoveComplete?.Invoke();
                JumpFlour(tfTarget.Tf, () =>
                {
                    ChangeScale();
                });
            }
        }

        private void ChangeScale()
        {
            if (isChangeScale)
            {
                Tf.localScale = Vector3.one * detalSacle;
            }
        }

        private void JumpFlour(Transform plateTransform, UnityAction action = null)
        {
            Tf.DOKill(true);

            col.enabled = false;
            spriteRenderer.sortingOrder = sortOrderMax;

            Vector3 target = new Vector3(
                plateTransform.position.x,
                plateTransform.position.y + detalTarget,
                plateTransform.position.z
            );

            Tf
                .DOJump(target, jumpPower, numJumps, timerMove)
                .SetEase(Ease.OutQuad)
                .Join(Tf.DORotate(new Vector3(0, 0, 360), timerMove, RotateMode.FastBeyond360))
                .OnComplete(() =>
                {
                    if (sfxSnap != null)
                        SoundManager.PlaySFXOneShot(sfxSnap);

                    spriteRenderer.sortingOrder = _originnalLayer;

                    Tf.SetParent(plateTransform, true);
                    Tf.localPosition = Vector3.zero;

                    onComplete?.Invoke();
                    action?.Invoke();

                    if (isChangeScale)
                    {
                        forceScaleValue = Vector3.one * detalSacle;
                        forceScaleFrame = 10;
                    }
                });
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (canClockTap) return;

            if (!tfTarget.IsSnap)
                base.OnPointerUp(eventData);
        }

        public void ChangeCanBlockTap(bool value)
        {
            canClockTap = value;
        }

        private void LateUpdate()
        {
            if (forceScaleFrame > 0)
            {
                if (Tf.localScale != forceScaleValue)
                {
                    Tf.localScale = forceScaleValue;
                }

                forceScaleFrame--;
            }
        }
    }
}