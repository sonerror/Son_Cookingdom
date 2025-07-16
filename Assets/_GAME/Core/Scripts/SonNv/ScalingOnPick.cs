using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class ScalingOnPick : SonMonoBehaviour
    {
        [SerializeField] private float scaleAmount = 1.1f;
        // public BoolModifierWithRegisteredSource isPreventScale;
        [SerializeField]
        private int maxLayer = 10;
        private Vector3 _originalScale;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer spriteRendererChid;
        [SerializeField] private List<InforSprite> listSpriteRendererChid;
        private int _originnalLayer;
        private int _originnalLayerChid;

        private bool isClick;
        private bool isUpScale;

        public void SetIsClick(bool _isClick)
        {
            isClick = _isClick;
        }
        public void SetIsUpScale(bool _isUpScale)
        {
            isUpScale = _isUpScale;
        }
        public void ResetScaleOrig()
        {
            _originalScale = transform.localScale;
        }
        protected virtual void Awake()
        {
            isClick = true;
            isUpScale = true;
            _originnalLayer = spriteRenderer.sortingOrder;
            if (spriteRendererChid != null)
            {
                _originnalLayerChid = spriteRendererChid.sortingOrder;
            }
            _originalScale = transform.localScale;
            // isPreventScale = new BoolModifierWithRegisteredSource(OnChangedCanScale);
        }

        public void CancelScale()
        {
            OnMouseUp();
        }

        private void OnChangedCanScale()
        {
            // if (isPreventScale.Value)
            {
                OnMouseUp();
            }
        }

        protected virtual void OnMouseDown()
        {
            if (isUpScale == true)
            {
                // if (isPreventScale.Value) return;
                Tf.localScale = _originalScale * scaleAmount;
                if (isClick == false) return;
                spriteRenderer.sortingOrder = maxLayer;
                if (spriteRendererChid != null)
                {
                    spriteRendererChid.sortingOrder = maxLayer + 1;
                }
                if (listSpriteRendererChid.Count > 0)
                {
                    foreach (var chid in listSpriteRendererChid)
                    {
                        chid.ChangeLayerNew(maxLayer + 1);
                    }
                }
            }
        }
        public void ScaleUp(float detal)
        {
            Tf.localScale += new Vector3(detal, detal, detal);
        }
        public void ScaleDown(float detal)
        {
            Tf.localScale -= new Vector3(detal, detal, detal);
        }
        public void ChangeLayerOrder(int index)
        {
            spriteRenderer.sortingOrder = index;
        }
        public void ChangeLayerChidOrder(int index)
        {
            spriteRendererChid.sortingOrder = index;
        }
        public void ChangeLayerOrderOri()
        {
            spriteRenderer.sortingOrder = _originnalLayer;
        }
        protected virtual void OnMouseUp()
        {
            if (isUpScale == true)
            {
                Tf.localScale = _originalScale;
                if (isClick == false) return;
                spriteRenderer.sortingOrder = _originnalLayer;
                if (spriteRendererChid != null)
                {
                    spriteRendererChid.sortingOrder = _originnalLayerChid;
                }
                if (listSpriteRendererChid.Count > 0)
                {
                    foreach (var chid in listSpriteRendererChid)
                    {
                        chid.ChangeLayerOrig();
                    }
                }
            }
        }

        public void ChangeLayerChid()
        {

        }
        public void ChangeOri(int index)
        {
            _originnalLayer = index;
        }
    }

}

