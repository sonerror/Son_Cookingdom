using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class WhiskBowlIngredient : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Vector2 startPos;
        [SerializeField] private float startZ;
        [SerializeField] private AppearEffect.AppearType appearType;
        [Button]
        private void GetRef()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            startPos = transform.localPosition;
            startZ = transform.eulerAngles.z;
        }
        private void Update()
        {
            transform.eulerAngles = new Vector3(0,0,startZ);
        }
        public void Init()
        {
            transform.localPosition = startPos;
            gameObject.SetActive(false);
            spriteRenderer.SetAlpha(1);
        }
        public void OnAppear()
        {
            switch (appearType)
            {
                case AppearEffect.AppearType.Appear:
                    transform.Appear();
                    break;
                case AppearEffect.AppearType.FallAppear:
                    transform.FallAppear();
                    break;
                case AppearEffect.AppearType.FadeIn:
                    gameObject.SetActive(true);
                    spriteRenderer.SetAlpha(0);
                    spriteRenderer.DOFade(1, 1f);
                    break;
            }
        }

        public void FadeOut()
        {
            spriteRenderer.DOFade(0, .5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}

