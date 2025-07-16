using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class ChangeLayer : SonMonoBehaviour
    {
        [SerializeField] private float scaleAmount = 1.1f;
        // public BoolModifierWithRegisteredSource isPreventScale;
        [SerializeField]
        private int maxLayer = 10;
        private Vector3 _originalScale;
        [SerializeField] private InforSprite spriteRenderer;
        [SerializeField] private List<InforSprite> listSpriteRendererChid;
        private bool isClick;
        [SerializeField] private bool isUpScale;
        public void SetIsUpScale(bool _isUpScale)
        {
            isUpScale = _isUpScale;
        }
        protected virtual void Awake()
        {
            isClick = false;
            _originalScale = transform.localScale;
            // isPreventScale = new BoolModifierWithRegisteredSource(OnChangedCanScale);
        }
        private void OnChangedCanScale()
        {
            // if (isPreventScale.Value)
            // {
            //     OnMouseUp();
            // }
        }
        protected virtual void OnMouseDown()
        {
            if (isClick == true) return;
            if (isUpScale == false)
            {
                // if (isPreventScale.Value) return;
                Tf.localScale = _originalScale * scaleAmount;
                spriteRenderer.ChangeLayerNew(maxLayer);
                if (listSpriteRendererChid.Count > 0)
                {
                    foreach (var chid in listSpriteRendererChid)
                    {
                        chid.ChangeLayerNew(maxLayer);
                    }
                }
            }
            else
            {
                spriteRenderer.ChangeLayerNew(maxLayer);
                if (listSpriteRendererChid.Count > 0)
                {
                    foreach (var chid in listSpriteRendererChid)
                    {
                        chid.ChangeLayerNew(maxLayer);
                    }
                }
            }
        }
        protected virtual void OnMouseUp()
        {
            if (isClick == true) return;
            if (isUpScale == false)
            {
                Tf.localScale = _originalScale;
                spriteRenderer.ChangeLayerOrig();
                if (listSpriteRendererChid.Count > 0)
                {
                    foreach (var chid in listSpriteRendererChid)
                    {
                        chid.ChangeLayerOrig();
                    }
                }
            }
            else
            {
                spriteRenderer.ChangeLayerOrig();
                if (listSpriteRendererChid.Count > 0)
                {
                    foreach (var chid in listSpriteRendererChid)
                    {
                        chid.ChangeLayerOrig();
                    }
                }
            }
        }
        public void SetValueIsClick(bool value)
        {
            isClick = value;
        }
    }
}
