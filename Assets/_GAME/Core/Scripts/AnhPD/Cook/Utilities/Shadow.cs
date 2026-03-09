using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class Shadow : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer shadow;
        [SerializeField] private float alpha = .27f;
        public void OnPickUp()
        {
            shadow.DOFade(0, .2f);
        }
        public void OnPutDown()
        {
            shadow.DOFade(alpha, .2f);
        }
        public void SetAlpha(float alpha)
        {
            this.alpha = alpha;
            shadow.SetAlpha(alpha);
        }

        public void SetSpriteRenderer(SpriteRenderer render)
        {
            shadow = render;
        }
    }
}

