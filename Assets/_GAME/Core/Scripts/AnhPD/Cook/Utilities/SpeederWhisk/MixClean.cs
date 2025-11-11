using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class MixClean : DirtyClean
    {
        public override void Appear()
        {
            //base.Appear();
            gameObject.SetActive(true);
            duration -= timer;
            timer = 0f;
            isReady = true;
            spriteRenderer.DOFade(1, 1);
            duration += .5f;
        }
    }
}

