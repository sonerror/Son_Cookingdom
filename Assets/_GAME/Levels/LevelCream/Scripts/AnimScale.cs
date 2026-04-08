using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace sonnv
{
    public class AnimScale : SonMonoBehaviour
    {
        public void OnPlayAnimScale()
        {
            Tf.DOScale(1f, 0.75f).SetEase(Ease.OutBack);
        }
    }
}