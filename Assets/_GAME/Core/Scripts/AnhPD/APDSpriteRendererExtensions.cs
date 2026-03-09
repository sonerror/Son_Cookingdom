using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD
{
    public static class APDSpriteRendererExtensions
    {
        /// <summary>
        /// Flip flipX liên tục theo duration và delay.
        /// </summary>
        /// <param name="sr">SpriteRenderer cần flip</param>
        /// <param name="duration">Thời gian giữ trạng thái flip</param>
        /// <param name="gap">Khoảng delay giữa lần flip</param>
        /// <returns>Sequence của DOTween (có thể dùng chain tiếp)</returns>
        public static void DOFlipLoop(this SpriteRenderer sr, float duration, float gap)
        {
            Sequence seq = DOTween.Sequence();

            int flipCount = Mathf.FloorToInt(duration / gap);
            for (int i = 0; i < flipCount; i++)
            {
                seq.AppendCallback(() => sr.flipX = !sr.flipX);
                seq.AppendInterval(gap);
            }

            seq.Play();
        }
    }
}

