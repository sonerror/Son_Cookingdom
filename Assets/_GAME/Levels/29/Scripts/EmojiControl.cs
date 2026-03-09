using DG.Tweening;
using UnityEngine;

namespace Satisgame
{
    public class EmojiControl : MonoBehaviour
    {
        private static readonly int AnimPositive = Animator.StringToHash("Positive");
        private static readonly int AnimNegative = Animator.StringToHash("Negative");

        [Header("Refs")]
        public Animator spriteAnimator;
        public Transform scaleTransform;

        [Header("Timing")]
        public float durationShow = 0.25f;
        public float durationHold = 2f;
        public float durationHide = 0.25f;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip sfxPositive;
        public AudioClip sfxNegative;

        private Sequence seq;
        private Vector3 originScale;

        private void Awake()
        {
            originScale = scaleTransform.localScale;

            if (originScale == Vector3.zero)
                originScale = Vector3.one;

            scaleTransform.localScale = Vector3.zero;
        }

        public void HideEmoji()
        {
            seq?.Kill();

            seq = DOTween.Sequence()
                .Append(scaleTransform.DOScale(Vector3.zero, durationHide * 0.5f).SetEase(Ease.OutQuad))
                .SetUpdate(true);
        }

        public void ShowPositive(float delay = 0f)
        {
            PlayEmoji(AnimPositive, sfxPositive, delay);
        }

        public void ShowNegative(float delay = 0f)
        {
            PlayEmoji(AnimNegative, sfxNegative, delay);
        }

        private void PlayEmoji(int animHash, AudioClip clip, float delay)
        {
            seq?.Kill();

            spriteAnimator.Play(animHash);

            seq = DOTween.Sequence();

            if (delay > 0)
                seq.AppendInterval(delay);

            seq.Append(scaleTransform.DOScale(originScale, durationShow).SetEase(Ease.OutBack));

            if (clip && audioSource)
                seq.AppendCallback(() => audioSource.PlayOneShot(clip));

            seq.AppendInterval(durationHold);

            seq.Append(scaleTransform.DOScale(Vector3.zero, durationHide).SetEase(Ease.InBack));
        }
    }
}