using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class TapInFlour : ScalingOnPick
    {
        [SerializeField] private Transform tfRoot;
        [SerializeField] private SpriteRenderer spriteButter;
        [SerializeField] private SpriteRenderer spriteFlour;
        [SerializeField] private AudioClip rotateSfx;
        private int count = 0;


        protected override void OnMouseDown()
        {
            base.OnMouseDown();

            count++;
            spriteFlour.transform.DOPunchPosition(Vector3.up * 0.1f, 0.2f, 10, 1).SetEase(Ease.OutQuad);
            Vector3 newScale = spriteFlour.transform.localScale + new Vector3(0.03f, 0.03f, 0.03f);
            spriteFlour.transform.DOScale(newScale, 0.1f).SetEase(Ease.OutBack);
            float progress = count / 10f;
            progress = Mathf.Clamp01(progress);
            spriteButter.DOFade(Mathf.SmoothStep(1f, 0f, progress), 0.1f);
            Color startColor = Color.white;
            Color targetColor = new Color32(0xFD, 0xFD, 0xDF, 0xFF);
            spriteFlour.color = Color.Lerp(startColor, targetColor, progress);
            if (count >= 10)
            {
                // phase2.isRotateFlourDown = false;
                // phase2.CheckEndStep7_Phase2();
            }
        }


    }
}

