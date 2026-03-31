using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace sonnv
{
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> listSprites;
        [SerializeField] private float timeChangeSprite = 0.2f;

        [SerializeField] private bool isPlaying = false;

        private int currentIndex = 0;
        private float timer = 0f;

        private void Update()
        {
            if (!isPlaying || listSprites == null || listSprites.Count == 0) return;
            timer += Time.deltaTime;
            if (timer >= timeChangeSprite)
            {
                timer -= timeChangeSprite;
                SetSpriteAlpha(listSprites[currentIndex], 0f);
                currentIndex++;
                if (currentIndex >= listSprites.Count)
                {
                    currentIndex = 0;
                }
                SetSpriteAlpha(listSprites[currentIndex], 1f);
            }
        }
        public void Play()
        {
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
        }
        public void ResetToDefault()
        {
            timer = 0f;
            currentIndex = 0;

            for (int i = 0; i < listSprites.Count; i++)
            {
                SetSpriteAlpha(listSprites[i], i == 0 ? 1f : 0f);
            }
        }

        private void SetSpriteAlpha(SpriteRenderer sr, float alphaValue)
        {
            if (sr == null) return;
            Color c = sr.color;
            c.a = alphaValue;
            sr.color = c;
        }
    }
}