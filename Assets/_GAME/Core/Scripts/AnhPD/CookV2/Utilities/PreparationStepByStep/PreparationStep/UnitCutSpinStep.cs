using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
    public class UnitCutSpinStep : FruitPreparationStep
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject afterCut;
        [SerializeField] private float duration = .5f;

        public void OnCut()
        {
            StartCoroutine(PlaySequenceRoutine());
        }

        private IEnumerator PlaySequenceRoutine()
        {
            if (sprites == null || sprites.Length == 0 || spriteRenderer == null)
                yield break;

            float timePerSprite = duration / sprites.Length;

            for (int i = 0; i < sprites.Length; i++)
            {
                yield return new WaitForSeconds(timePerSprite);
                spriteRenderer.sprite = sprites[i];
                transform.DOComplete();
                transform.Appear();
            }
            afterCut.SetActive(true);
            gameObject.SetActive(false);
            OnComplete();
        }
    }
}
