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

        public UnityEvent onComplete;

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
                tfTarget.ChangeIsSnap(false);
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
            transform.DOKill();

            col.enabled = false;
            spriteRenderer.sortingOrder = sortOrderMax;

            Vector3 target = new Vector3(
                plateTransform.position.x,
                plateTransform.position.y + detalTarget,
                plateTransform.position.z
            );

            transform
                .DOJump(target, jumpPower, numJumps, timerMove)
                .SetEase(Ease.OutQuad)
                .Join(transform.DORotate(new Vector3(0, 0, 360), timerMove, RotateMode.FastBeyond360))
                .OnComplete(() =>
                {
                    if (sfxSnap != null)
                        SoundManager.PlaySFXOneShot(sfxSnap);

                    Tf.localScale = isChangeScaleOnMove ? scaleOnMove : _originalScale;

                    spriteRenderer.sortingOrder = _originnalLayer;

                    transform.SetParent(plateTransform);

                    CuttingBoard.Instance.RegisterMoveDone(this);

                    onComplete?.Invoke();
                    action?.Invoke();
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
    }
}