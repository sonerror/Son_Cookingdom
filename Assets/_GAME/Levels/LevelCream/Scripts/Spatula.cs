using UnityEngine;

namespace sonnv
{
    public class Spatula : SonMonoBehaviour
    {
        [SerializeField] private bool isChange = false;
        [SerializeField] private AudioClip sfxTouch;

        private const float SOUND_COOLDOWN = 0.5f;
        private float _lastSoundTime = -999f; // realtimeSinceStartup của lần play gần nhất

        public void ChangeCanColor()
        {
            isChange = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!isChange) return;

            FoodItem item = collision.GetComponent<FoodItem>();
            if (item == null) return;

            item.OnSpatulaHold();

            TryPlayTouchSound();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!isChange) return;

            FoodItem item = collision.GetComponent<FoodItem>();
            if (item == null) return;

            item.OnSpatulaExit();
        }

        private void TryPlayTouchSound()
        {
            if (sfxTouch == null) return;

            float now = Time.realtimeSinceStartup;

            if (now - _lastSoundTime >= SOUND_COOLDOWN)
            {
                _lastSoundTime = now;
                SoundManager.PlaySFX(sfxTouch);
            }
        }
    }
}