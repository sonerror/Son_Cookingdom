using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public class TapToMiniFlour : SonMonoBehaviour
    {
        [SerializeField] private float scaleAmount = 1.1f;
        // public BoolModifierWithRegisteredSource isPreventScale;
        [SerializeField]
        private int maxLayer = 10;
        private Vector3 _originalScale;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private int _originnalLayer;
        private int countTap;
        private bool isTap;
        [SerializeField] private List<GameObject> listObj;
        [SerializeField] private List<GameObject> listObjCircle;
        [SerializeField] private AudioClip tapSfx1;
        [SerializeField] private AudioClip tapSfx2;
        // [SerializeField] private Phase2Donut phase2;

        protected virtual void Awake()
        {
            isTap = false;
            countTap = 0;
            _originnalLayer = spriteRenderer.sortingOrder;
            _originalScale = transform.localScale;
            // isPreventScale = new BoolModifierWithRegisteredSource(OnChangedCanScale);
        }
        public void SetBoolIsTap(bool isTap)
        {
            this.isTap = isTap;
            scaleAmount = 1.1f;
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
            // if (isPreventScale.Value) return;

            Tf.localScale = _originalScale * scaleAmount;
            spriteRenderer.sortingOrder = maxLayer;
            if (!isTap) return;
            countTap++;
            //.PlaySFX(tapSfx1);
            if (countTap == 7)
            {
                //.PlaySFX(tapSfx2);
                StartCoroutine(IE_HideObj());
                // phase2.CheckEndStep11Phase2();
                scaleAmount = 1.01f;
                isTap = false;
            }
        }

        IEnumerator IE_HideObj()
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < listObj.Count; i++)
            {
                HideObj(listObj[i], false);
                HideObj(listObjCircle[i], true);
            }
        }


        private void HideObj(GameObject obj, bool value)
        {
            obj.SetActive(value);
            obj.transform.DOShakeScale(0.15f, strength: 0.05f, vibrato: 8, randomness: 60, fadeOut: true);
        }
        protected virtual void OnMouseUp()
        {
            Tf.localScale = _originalScale;
            spriteRenderer.sortingOrder = _originnalLayer;
        }

    }
}


