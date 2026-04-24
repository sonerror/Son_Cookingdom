using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    public class TriggerToRotate : SonMonoBehaviour
    {
        [SerializeField] Transform tfRoot;
        [SerializeField] SpriteRenderer mixerFlour;
        [SerializeField] List<SpriteRenderer> imgList;

        [SerializeField] private AudioClip rotateSfx;
        [SerializeField] private int sovongquay = 5;
        [SerializeField] private float autoRotateDegreesPerSecond = 180f;

        [SerializeField] private bool isFlipSprite = false;
        [SerializeField] private ParticleSystem burningParticles;
        [SerializeField] private bool isBlockRotate = true;
        public bool IsBlockRotate => isBlockRotate;
        public UnityEvent onEndRotate;
        public UnityEvent onDoneOne;

        private bool isAutoRotating = false;
        private Coroutine autoRotateCoroutine;

        int count = 0;
        float deltaAngle = 0;

        private void Update()
        {
            if (!isAutoRotating)
                transform.eulerAngles = Vector3.zero;
        }

        public void SetBlockRotate(bool value)
        {
            isBlockRotate = value;
        }

        public void StartAutoRotate()
        {
            if (isBlockRotate || isAutoRotating) return;
            isAutoRotating = true;
            autoRotateCoroutine = StartCoroutine(AutoRotateCoroutine());
        }

        public void StopAutoRotate()
        {
            if (!isAutoRotating) return;
            isAutoRotating = false;
            if (autoRotateCoroutine != null)
            {
                StopCoroutine(autoRotateCoroutine);
                autoRotateCoroutine = null;
            }
        }

        private IEnumerator AutoRotateCoroutine()
        {
            while (count < sovongquay)
            {
                float step = autoRotateDegreesPerSecond * Time.deltaTime;
                deltaAngle += step;
                tfRoot.Rotate(0, 0, step);

                if (deltaAngle >= 360f)
                {
                    count++;
                    deltaAngle %= 360f;
                    SoundManager.PlaySFX(rotateSfx);

                    if (isFlipSprite)
                    {
                        mixerFlour.flipX = !mixerFlour.flipX;
                        StartParticles();
                    }

                    onDoneOne?.Invoke();

                    if (count >= sovongquay)
                    {
                        UpdateVisuals();
                        onEndRotate?.Invoke();
                        if (isFlipSprite) StopParticles();
                        break;
                    }
                }

                UpdateVisuals();
                yield return null;
            }

            isAutoRotating = false;
            autoRotateCoroutine = null;
        }

        private void UpdateVisuals()
        {
            float progress = (count + deltaAngle / 360f) / sovongquay;
            progress = Mathf.Clamp01(progress);
            float alphaMain = Mathf.SmoothStep(0f, 1f, progress);
            float alphaOthers = Mathf.SmoothStep(1f, 0f, progress);
            mixerFlour.SetAlpha(alphaMain);
            foreach (SpriteRenderer img in imgList)
                img.SetAlpha(alphaOthers);
        }

        private void StartParticles()
        {
            if (burningParticles != null)
            {
                burningParticles.gameObject.SetActive(true);
                burningParticles.Play();
            }
        }

        private void StopParticles()
        {
            if (burningParticles != null)
            {
                burningParticles.gameObject.SetActive(false);
                burningParticles.Stop();
            }
        }
    }
}