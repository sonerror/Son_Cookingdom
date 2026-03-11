using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace sonnv
{
    public class ObjectShaking : MonoBehaviour
    {
        [SerializeField] private Transform tfToShake;
        [SerializeField] private float rotStrength = 1f;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private int vibrato = 10;
        private Tween rotTween;

        public void DoNormalShake()
        {
            StartShake(tfToShake);
            SonUtilities.DelayedCallScaled(duration, () =>
            {
                StopShake(tfToShake);
            });
        }

        public void ShakingObj(Transform tf, float duration = 0.2f, float rotStrength = 1f)
        {
            tfToShake = tf;
            this.rotStrength = rotStrength;
            StartShake(tf);
            SonUtilities.DelayedCallScaled(duration, () =>
            {
                StopShake(tf);
            });
        }

        public void StartShake(Transform tf)
        {
            StopShake(tf);
            tfToShake = tf;
            rotTween = tfToShake.DOShakeRotation(duration, new Vector3(0, 0, rotStrength), vibrato, 90f)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }

        public void StopShake(Transform tf)
        {
            if (rotTween != null && rotTween.IsActive()) rotTween.Kill();

            tf.localRotation = Quaternion.identity;
        }
    }
}