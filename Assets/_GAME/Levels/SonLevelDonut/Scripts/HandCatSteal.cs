using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

namespace sonnv
{
    public class HandCatSteal : SonMonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteDonut;
        [SerializeField] private AudioClip catSfx;

        private Vector3 startPos;
        private void Start()
        {
            startPos = this.Tf.position;
        }
        public void MoveToTarget(Transform tfTarget, SpriteRenderer spriteDonutOnPlate, Action action)
        {
            StartCoroutine(IE_Steal(tfTarget, spriteDonutOnPlate, action));
        }
        private IEnumerator IE_Steal(Transform tfTarget, SpriteRenderer spriteDonutOnPlate, Action action)
        {
            Vector3 midPoint = Vector3.Lerp(startPos, tfTarget.position, 0.7f);
            this.Tf.DOMove(midPoint, 0.8f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(0.8f);
            this.Tf.DOMove(tfTarget.position, 0.15f).SetEase(Ease.InQuad);
            yield return new WaitForSeconds(0.15f);
            spriteDonutOnPlate.SetAlpha(0);
            spriteDonut.SetAlpha(1);
            yield return new WaitForSeconds(0.2f);
            //.PlaySFX(catSfx);
            this.Tf.DOMove(midPoint, 0.4f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.4f);
            this.Tf.DOMove(startPos, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                action?.Invoke();
            });
        }
    }
}
