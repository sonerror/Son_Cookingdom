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
            _isSliced = true;
            Slice();
        }

        public void OnDrag(PointerEventData eventData) { }

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
    }
}