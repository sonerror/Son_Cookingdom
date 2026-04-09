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

        // Dùng realtime để tránh bị ảnh hưởng bởi Time.timeScale và deltaTime spike
        private float _holdStartRealtime = -1f;
        private float _accumulatedTime = 0f;     // thời gian đã tích lũy trước khi exit
        private float _lastHoldRealtime = -1f;   // frame cuối cùng được gọi OnSpatulaHold
        private bool isCompleted = false;

        private const float MAX_DELTA = 0.05f;   // clamp tối đa 50ms/frame (~20fps min)

        private Color targetColor = new Color32(163, 231, 112, 255);

        public void OnSpatulaHold()
        {
            if (isCompleted) return;

            float now = Time.realtimeSinceStartup;

            // Lần đầu chạm vào sau khi exit
            if (_holdStartRealtime < 0f)
            {
                _holdStartRealtime = now;
                _lastHoldRealtime = now;
            }

            // Clamp delta để tránh spike khi tab mất focus
            float delta = Mathf.Min(now - _lastHoldRealtime, MAX_DELTA);
            _lastHoldRealtime = now;

            _accumulatedTime += delta;
            float currentHoldTime = _accumulatedTime;
            float progress = Mathf.Clamp01(currentHoldTime / timeToChange);

            foreach (SpriteRenderer sprite in listSprite)
            {
                sprite.SetAlpha(1f - progress);
            }

            spriteRenderer.color = Color.Lerp(Color.white, targetColor, progress);
            tfScale.localScale = Vector3.one * (1f + progress);

            if (currentHoldTime >= timeToChange)
            {
                CompleteAction();
            }
        }

        public void OnSpatulaExit()
        {
            if (isCompleted) return;

            // Reset tracking realtime, giữ lại accumulatedTime để resume sau
            _holdStartRealtime = -1f;
            _lastHoldRealtime = -1f;
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