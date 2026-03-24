using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class SpriteRenderen : MonoBehaviour
    {

        [SerializeField] private List<ShowObjectEffect> listEffectShow;

        [SerializeField] private List<SpriteRenderer> listSpriteRenderer;
        public void ChangeLayerSprite(int _newLayer)
        {
            foreach (SpriteRenderer sprite in listSpriteRenderer)
            {
                sprite.sortingOrder = _newLayer;
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
        }

    }

}
