using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.KingCrab
{
    public class CrabLeg : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Transform target;

        public bool isCut = false;
        private void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnCut()
        {
            isCut = true;
            spriteRenderer.DOFade(0f, .5f);
            transform.DOMove(transform.position + transform.right * 1f, .5f).SetEase(Ease.Linear).OnComplete(delay);
            void delay()
            {
                target.gameObject.SetActive(true);
                target.transform.DOPunchScale(Vector3.one * .1f, .3f);
            }
        }
    }
}

