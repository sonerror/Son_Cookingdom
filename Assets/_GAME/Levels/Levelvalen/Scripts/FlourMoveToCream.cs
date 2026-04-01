using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace sonnv
{
    public class FlourMoveToCream : SonTapItem
    {
        [SerializeField] private bool isCheckShowEmoji = false;
        [SerializeField] private EmojiControl emojiControl;
        [SerializeField] private UnityEvent onEventFail;

        [SerializeField] private InforTFTarget tfTarget;
        public void SetData(InforTFTarget _tfTarget)
        {
            tfTarget = _tfTarget;
            if (tfTarget != null)
            {
                tfTarget.Effect.Show();
            }
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
        [SerializeField] private bool isChangeNewLayer = false;
        [SerializeField] private int newLayerAfterMove = 39;

        public UnityEvent onComplete;
        public UnityEvent onStartMoveComplete;

        private Vector3 forceScaleValue;
        private bool isMoving = false;
        public override void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("1");
            if (canClockTap) return;
            Debug.Log("2");

            base.OnPointerDown(eventData);
            Debug.Log("3");

            OnMove();
        }
        public void OnMoveDelay(float delay)
        {
            DOVirtual.DelayedCall(delay, OnMove);
        }
        public void OnMove()
        {
            if (isCheckShowEmoji == true)
            {
                Debug.Log("4");

                if (!tfTarget.IsSnap)
                {
                    if (emojiControl != null)
                    {
                        Debug.Log("0000000000000000");
                        onEventFail?.Invoke();
                        Debug.Log("1111111111111");

                        emojiControl.ShowNegative();
                        Debug.Log("222222222222");

                    }
                }
            }
            if (tfTarget != null && tfTarget.IsSnap)
            {
                Debug.Log("5");

                isMoving = true;
                Debug.Log("6");

                tfTarget.ChangeIsSnap(false);
                Debug.Log("7");

                onStartMoveComplete?.Invoke();
                Debug.Log("8");

                JumpFlour(tfTarget.Tf, () =>
                {
                    Debug.Log("9");

                    isMoving = false;
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
            if (col != null)
            {
                col.enabled = false;
            }
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
                    if (isChangeNewLayer == false)
                    {
                        spriteRenderer.sortingOrder = _originnalLayer;

                    }
                    else
                    {
                        spriteRenderer.sortingOrder = newLayerAfterMove;
                    }

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

            if (!tfTarget.IsSnap && !isMoving)
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