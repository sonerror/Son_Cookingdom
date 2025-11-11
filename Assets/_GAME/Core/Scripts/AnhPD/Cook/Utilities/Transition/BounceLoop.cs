using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnhPD.Cook
{
    public class BounceLoop : MonoBehaviour
    {
        private void OnEnable()
        {
            Loop();
        }

        private void OnDisable()
        {
            CancelLoop();
        }

        private void OnDestroy()
        {
            CancelLoop();
        }

        private Tween _tween;
        private void Loop()
        {
            _tween = transform.DOPunchScale(Vector3.up  * Random.Range(.01f, .05f), Random.Range(1f, 2f), Random.Range(1, 3)).SetLoops(-1, LoopType.Yoyo);
        }

        private void CancelLoop()
        {
            _tween.Kill();
        }
    }
}
