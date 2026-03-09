using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class InforSprite : SonMonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private int _originnalLayer;
        private void Awake()
        {
            if (spriteRenderer != null)
            {
                _originnalLayer = spriteRenderer.sortingOrder;
            }
        }

        public void ChangeLayerNew(int value)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = _originnalLayer + value;
            }
        }
        public void ChangeLayerOrig()
        {
            spriteRenderer.sortingOrder = _originnalLayer;
        }
    }

}
