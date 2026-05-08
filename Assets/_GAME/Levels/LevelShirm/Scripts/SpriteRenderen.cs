using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    public class SpriteRenderen : MonoBehaviour
    {
        [SerializeField] private List<ShowObjectEffect> listEffectShow;
        [SerializeField] private List<SpriteRenderer> listSpriteRenderer;
        [SerializeField] private UnityEvent onDone;
        public UnityEvent OnDone => onDone;

        private List<int> _originalLayers = new List<int>();
        private void Awake()
        {
            OnInit();
        }
        public void OnInit()
        {
            foreach (SpriteRenderer sprite in listSpriteRenderer)
            {
                _originalLayers.Add(sprite.sortingOrder);
            }
        }

        public void ChangeLayerSprite(int _newLayer)
        {
            foreach (SpriteRenderer sprite in listSpriteRenderer)
            {
                sprite.sortingOrder = sprite.sortingOrder + _newLayer;
            }
        }

        public void ResetLayerSprite()
        {
            if (_originalLayers.Count == listSpriteRenderer.Count)
            {
                for (int i = 0; i < listSpriteRenderer.Count; i++)
                {
                    listSpriteRenderer[i].sortingOrder = _originalLayers[i];
                }
            }
        }

        public void ShowEffect()
        {
            StartCoroutine(IE_DelayEffect(0.1f));
        }
        public void ShowEffectDelay(float time = 0.3f)
        {
            StartCoroutine(IE_DelayShowEffect(time));
        }
        private IEnumerator IE_DelayEffect(float time)
        {
            foreach (ShowObjectEffect effrct in listEffectShow)
            {
                effrct.Show();
                yield return new WaitForSeconds(time);
            }
            onDone?.Invoke();
        }
        private IEnumerator IE_DelayShowEffect(float time)
        {
            yield return new WaitForSeconds(time);
            StartCoroutine(IE_DelayEffect(0.1f));
        }
    }
}