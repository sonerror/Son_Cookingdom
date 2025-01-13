using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.KingCrab
{
    public class CrabLegPart : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public bool IsUp;

        public void OnCut()
        {
            spriteRenderer.DOFade(0, .5f);
            transform.DOMoveY(transform.position.y + .5f * (IsUp ? 1f : -1f), .5f).SetEase(Ease.Linear);
        }
    }
}

