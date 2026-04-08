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

        private List<int> _originalLayers = new List<int>();

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

        private IEnumerator IE_DelayEffect(float time)
        {
            foreach (ShowObjectEffect effrct in listEffectShow)
            {
                effrct.Show();
                yield return new WaitForSeconds(time);
            }
            onDone?.Invoke();
        }
    }
}