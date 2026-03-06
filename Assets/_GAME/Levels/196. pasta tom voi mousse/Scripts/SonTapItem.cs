using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace sonnv
{
    public class SonTapItem : SonMonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] protected int maxLayer = 10;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected float scaleAmount = 1.1f;
        [SerializeField] protected Collider2D col;
        public Collider2D ColD => col;
        [SerializeField] protected AudioClip sfxTap;
        [SerializeField] private bool canBlock = false;
        public UnityEvent eventOnPointDown;
        protected Vector3 _originalScale;
        protected int _originnalLayer;
        private bool blockTap = false;

        protected virtual void Awake()
        {
            if (col == null)
                col = GetComponent<Collider2D>();

            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            _originnalLayer = spriteRenderer.sortingOrder;
            _originalScale = Tf.localScale;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (blockTap) return;
            Tf.localScale = _originalScale * scaleAmount;
            spriteRenderer.sortingOrder = maxLayer;
            if (canBlock)
            {
                blockTap = true;
                col.enabled = false;
            }
            if (sfxTap != null)
                SoundManager.PlaySFXOneShot(sfxTap);
            eventOnPointDown?.Invoke();

        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Tf.localScale = _originalScale;
            spriteRenderer.sortingOrder = _originnalLayer;
        }

    }
}