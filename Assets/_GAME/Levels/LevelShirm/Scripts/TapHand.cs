using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace sonnv
{
    [RequireComponent(typeof(Collider2D))]
    public class TapHand : SonMonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private SonSnapPoint snapPoint;
        public SonSnapPoint SnapPoint => snapPoint;

        [SerializeField] private ItemMortar itemMortarBase;

        [SerializeField] private ItemMortar itemMaccaInMortar;
        [SerializeField] private ItemMortar itemWalnutInMortar;
        [SerializeField] private ItemMortar itemChestnutInMortar;

        [SerializeField] private Transform handTransform;

        [SerializeField] private Transform tfTarget;

        [SerializeField] private float tfY = 50f;
        [SerializeField] private float moveDuration = 0.3f;
        [SerializeField] private Ease moveEase = Ease.OutQuad;
        [SerializeField] private float returnDuration = 0.3f;
        [SerializeField] private Ease returnEase = Ease.InQuad;

        [SerializeField] private bool canTap = true;
        [SerializeField] private AudioClip sfxTap, sfxHit;

        [SerializeField] private UnityEvent onStartTap;
        [SerializeField] private UnityEvent onReachTarget;
        [SerializeField] private UnityEvent onReturnStart;

        private Vector3 _startPosition;
        private Sequence _tapSequence;

        private void Awake()
        {
            if (handTransform == null)
            {
                handTransform = transform;
            }
            _startPosition = handTransform.localPosition;
        }
        public void SetItemInMortar(int _index)
        {
            itemMortarBase = null;
            if (_index == 0)
            {
                if (itemWalnutInMortar != null)
                {
                    itemMortarBase = itemWalnutInMortar;
                }
            }
            if (_index == 1)
            {
                if (itemMaccaInMortar != null)
                {
                    itemMortarBase = itemMaccaInMortar;
                }
            }
            if (_index == 2)
            {
                if (itemChestnutInMortar != null)
                {
                    itemMortarBase = itemChestnutInMortar;

                }
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (canTap)
            {
                canTap = false;
                PlayTap();
            }
        }

        [Button("Reset Tap State")]
        public void ResetTap()
        {
            canTap = true;
            handTransform.gameObject.SetActive(true);

            StopTap();
        }

        private void PlayTap()
        {
            _tapSequence?.Kill();
            handTransform.localPosition = _startPosition;

            onStartTap?.Invoke();

            float targetPositionY = tfTarget != null ? tfTarget.localPosition.y : (_startPosition.y - tfY);

            _tapSequence = DOTween.Sequence();

            SoundManager.PlaySFXOneShot(sfxTap);

            _tapSequence.Append(handTransform.DOLocalMoveY(targetPositionY, moveDuration)
                .SetEase(moveEase));

            _tapSequence.AppendCallback(() =>
            {
                SoundManager.PlaySFXOneShot(sfxHit);
                if (itemMortarBase != null)
                {
                    itemMortarBase.OnPlayEffect();
                }
                onReachTarget?.Invoke();
            });

            _tapSequence.Append(handTransform.DOLocalMoveY(_startPosition.y, returnDuration)
                .SetEase(returnEase));

            _tapSequence.AppendCallback(() =>
            {
                StartCoroutine(IE_DelayMove());
                onReturnStart?.Invoke();
            });
        }
        IEnumerator IE_DelayMove()
        {
            yield return new WaitForSeconds(0.5f);
            handTransform.gameObject.SetActive(false);
            snapPoint.ForceChangeSnap(false);
            if (itemMortarBase != null)
            {
                itemMortarBase.OnPlayMove();
            }
        }
        public void StopTap()
        {
            _tapSequence?.Kill();
            handTransform.localPosition = _startPosition;
        }

        private void OnDestroy()
        {
            _tapSequence?.Kill();
        }
    }
}