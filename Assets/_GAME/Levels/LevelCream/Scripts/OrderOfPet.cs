using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace sonnv
{
    public class OrderOfPet : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform tfOrder;

        [Space(10)]
        [Header("Events")]
        [SerializeField] private UnityEvent onShowComplete;
        [SerializeField] private UnityEvent onHideComplete;

        private Tween _loopTween;
        private Tween _delayTween;

        // ─── Show ───────────────────────────────────────────
        public void OnShow()
        {
            tfOrder.gameObject.SetActive(true);
            tfOrder.localScale = Vector3.zero;

            tfOrder.DOKill();
            tfOrder.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    StartLoopScale();
                    onShowComplete?.Invoke();
                    //StartDelayStep(0.5f);
                });
        }

        // ─── Hide ───────────────────────────────────────────
        public void OnHide()
        {
            KillAll();
            tfOrder.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    tfOrder.gameObject.SetActive(false);
                    onHideComplete?.Invoke();
                });
        }

        // ─── Loop Scale ─────────────────────────────────────
        private void StartLoopScale()
        {
            _loopTween?.Kill();
            _loopTween = tfOrder.DOScale(Vector3.one * 1.1f, 0.4f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void StopLoopScale()
        {
            _loopTween?.Kill();
            _loopTween = null;
            tfOrder.localScale = Vector3.one;
        }

        // ─── Step ────────────────────────────────────────────
        private void StartDelayStep(float delay)
        {
            _delayTween?.Kill();
            _delayTween = DOVirtual.DelayedCall(delay, () =>
            {
                CookManager.Ins.OnStartStep();
            });
        }

        // ─── Cleanup ─────────────────────────────────────────
        private void KillAll()
        {
            _loopTween?.Kill();
            _loopTween = null;
            _delayTween?.Kill();
            _delayTween = null;
            tfOrder.DOKill();
        }

        private void OnDestroy()
        {
            KillAll();
        }
    }
}