using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public class WaterCleaningSnapObject : SonMonoBehaviour
    {
        [Header("Water Cleaning")]
        [SerializeField] private SonSinkWaterCleaning sink;
        [SerializeField] private Sprite cleanSprite;
        [SerializeField] private ParticleSystem cleanEffect;
        [SerializeField] private AudioClip soundClean;
        [SerializeField] private AudioClip splashSound;

        public System.Action onManualEnable;
        public UnityEvent onCleanEvent;

        private void OnEnable()
        {
            SoundManager.PlaySFXOneShot(soundClean);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                // Sprite.sprite = cleanSprite;
                cleanEffect.Play();
                //Col.enabled = true;
            });
        }

        public void OnActive()
        {
            gameObject.SetActive(true);
            if (sink.HasWater)
            {
                SoundManager.PlaySFXOneShot(splashSound);
                ManualEnable();
            }
            else
            {
                sink.needCleanSnapObjects.Add(this);
            }
        }

        public bool TryEnableByFillWater()
        {
            if (!gameObject.activeSelf) return false;
            //if (IsSnap) return false;
            ManualEnable();
            return true;
        }

        private void ManualEnable()
        {
            enabled = true;
            onManualEnable?.Invoke();
            onCleanEvent?.Invoke();
        }
        public void AddCleanEvent(UnityAction action)
        {
            onCleanEvent.AddListener(action);
        }

    }

}
