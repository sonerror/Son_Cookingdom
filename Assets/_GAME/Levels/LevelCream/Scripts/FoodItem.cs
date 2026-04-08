using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    public class FoodItem : SonMonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Collider2D col;
        public Collider2D Col => col;

        [SerializeField] private List<SpriteRenderer> listSprite;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float timeToChange = 2.0f;
        [SerializeField] private UnityEvent onDone;
        [SerializeField] private Transform tfScale;
        public UnityEvent OnDone => onDone;

        private float currentHoldTime = 0f;
        private bool isCompleted = false;
        private Color targetColor = new Color32(163, 231, 112, 255);
        private Vector3 originalScale;

        void Awake()
        {
        }

        public void OnSpatulaHold()
        {
            if (isCompleted) return;

            currentHoldTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentHoldTime / timeToChange);
            foreach (SpriteRenderer sprite in listSprite)
            {
                sprite.SetAlpha(1 - progress);
            }
            spriteRenderer.color = Color.Lerp(Color.white, targetColor, progress);

            tfScale.localScale = Vector3.one * (1f + (progress));

            if (currentHoldTime >= timeToChange)
            {
                CompleteAction();
            }
        }

        public void OnSpatulaExit()
        {
            if (isCompleted) return;
        }

        private void CompleteAction()
        {
            isCompleted = true;
            spriteRenderer.color = targetColor;
            tfScale.localScale = Vector3.one * 2.1f;
            col.enabled = false;
            onDone?.Invoke();
        }
    }
}