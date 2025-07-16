using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class DonutInPan : SonMonoBehaviour
    {
        [SerializeField] private Transform tfTarget;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private MeshRenderer mesh;

        [SerializeField] private bool isMoveTo;
        [SerializeField] private ParticleSystem particleSmo;
        private Tween fryingShakeTween;
        private void Awake()
        {
            if (particleSmo != null)
            {
                particleSmo.gameObject.SetActive(false);

            }
        }
        public Transform GetTfTarget()
        {
            return tfTarget;
        }
        public void SetTfTarget()
        {

        }
        public bool GetIsMoveTo()
        {
            return isMoveTo;
        }
        public void SetIsMoveTo(bool value)
        {
            isMoveTo = value;
        }
        public SpriteRenderer GetspriteRenderer()
        {
            return spriteRenderer;
        }
        public void StartFryingShake()
        {
            if (fryingShakeTween != null && fryingShakeTween.IsActive() && fryingShakeTween.IsPlaying())
                return;
            if (particleSmo != null)
            {
                particleSmo.gameObject.SetActive(true);
                particleSmo.Play();
            }
            Sequence fryShake = DOTween.Sequence();

            fryShake.Join(this.transform.DOShakePosition(
                duration: 0.2f,
                strength: new Vector3(0.03f, 0.03f, 0),
                vibrato: 20,
                randomness: 90,
                fadeOut: false
            ));

            fryShake.Join(this.transform.DOShakeRotation(
                duration: 0.2f,
                strength: new Vector3(0, 0, 2),
                vibrato: 15,
                randomness: 90,
                fadeOut: false
            ));

            fryShake.SetLoops(-1)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true);

            fryingShakeTween = fryShake;
        }
        public void StopFryingShake()
        {
            if (fryingShakeTween != null && fryingShakeTween.IsActive())
            {
                fryingShakeTween.Kill();
                fryingShakeTween = null;
            }
        }
        public void hideMesh()
        {
            mesh.enabled = true;
        }

    }

}
