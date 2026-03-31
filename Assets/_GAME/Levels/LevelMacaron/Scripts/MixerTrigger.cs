using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    [RequireComponent(typeof(Collider2D))]
    public class MixerTrigger : MonoBehaviour
    {
        [SerializeField] private Collider2D targetCollider;

        [SerializeField] private List<SpriteRenderer> spritesBeforeMix;
        [SerializeField] private List<SpriteRenderer> spritesAfterMix;

        [SerializeField] private float mixDuration = 2f;
        [SerializeField] private UnityEvent onMixComplete;
        public UnityEvent OnMixComplete => onMixComplete;

        private float mixTimer = 0f;
        private bool isMixing = false;
        private bool isCompleted = false;

        private void Start()
        {
            SetSpritesAlpha(spritesBeforeMix, 1f);
            SetSpritesAlpha(spritesAfterMix, 0f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isCompleted) return;
            if (other == targetCollider)
            {
                isMixing = true;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (isCompleted) return;

            if (other == targetCollider)
            {
                isMixing = false;
            }
        }
        private void Update()
        {
            if (isMixing && !isCompleted)
            {
                ProcessMixing();
            }
        }

        private void ProcessMixing()
        {
            mixTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(mixTimer / mixDuration);
            SetSpritesAlpha(spritesBeforeMix, 1f - progress);
            SetSpritesAlpha(spritesAfterMix, progress);
            if (progress >= 1f)
            {
                isCompleted = true;
                isMixing = false;
                onMixComplete?.Invoke();
            }
        }
        private void SetSpritesAlpha(List<SpriteRenderer> list, float alphaValue)
        {
            if (list == null) return;
            foreach (var sr in list)
            {
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = alphaValue;
                    sr.color = c;
                }
            }
        }
    }
}