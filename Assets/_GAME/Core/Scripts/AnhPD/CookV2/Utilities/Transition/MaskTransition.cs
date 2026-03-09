using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.CookV2
{
    public class MaskTransition : APDProgressionTransion
    {
        [SerializeField] Transform mask;
        public override APDProgressionTransion Play()
        {
            //open
            gameObject.SetActive(true);
            mask.localScale = Vector3.zero;
            mask.DOScale(1f, transDuration).SetDelay(delay).OnComplete(() =>
            {
                onCover?.Invoke();
            });

            //close
            mask.DOScale(0f, transDuration).OnComplete(() =>
                {
                    onComplete?.Invoke();
                    gameObject.SetActive(false);
                })
                .SetDelay(stayDuration + transDuration + delay);
            return base.Play();
        }
    }
}
