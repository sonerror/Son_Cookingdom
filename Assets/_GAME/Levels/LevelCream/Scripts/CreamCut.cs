using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace sonnv
{
    public class CreamCut : SonMonoBehaviour,
        IPointerDownHandler,
        IDragHandler
    {
        [SerializeField] private Transform spatula, spatulaSprite, spriteCreamCut;
        [SerializeField] private Collider2D coll2D;
        [SerializeField] private Vector3 startPos, endPos;
        [SerializeField] private AudioClip sfxSlice;
        private Vector2 _startPointerWorldPos;
        private bool _isSliced;
        public UnityEvent onComplete;
        public void Show()
        {
            _isSliced = false;
            gameObject.SetActive(true);
            spatula.localPosition = startPos;
            coll2D.enabled = true;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isSliced) return;

            _startPointerWorldPos = ScreenToWorld(eventData.position);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (_isSliced) return;

            Vector2 currentWorldPos = ScreenToWorld(eventData.position);

            if (currentWorldPos.y - _startPointerWorldPos.y <= -.5f)
            {
                _isSliced = true;
                Slice();
            }
        }
        private void Slice()
        {
            SoundManager.PlaySFX(sfxSlice, .5f);
            spatula.DOLocalMove(endPos, .3f)
                   .SetEase(Ease.InBack)
                   .OnComplete(() =>
                   {
                       coll2D.enabled = false;
                       spatulaSprite.gameObject.SetActive(false);
                       spriteCreamCut.gameObject.SetActive(true);
                       onComplete?.Invoke();
                       Debug.Log("Done");
                   });
        }
        private static Vector2 ScreenToWorld(Vector2 screenPos)
            => Camera.main.ScreenToWorldPoint(screenPos);
    }
}