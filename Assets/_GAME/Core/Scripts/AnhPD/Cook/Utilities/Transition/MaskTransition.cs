using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.Cook;
using DG.Tweening;
using UnityEngine;

public class MaskTransition : APDTransitionBase
{
    [SerializeField] Transform mask;

    public MaskTransition StartTransition( float delay = 0f, float transDuration = .5f, float stayDuration = .25f)
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
        
        return this;
    }
}
