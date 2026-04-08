using UnityEngine;

namespace sonnv
{
    public class Spatula : SonMonoBehaviour
    {
        [SerializeField] private bool isChange = false;
        [SerializeField] private AudioClip sfxTouch;

        private float _touchTimer = 0f;

        public void ChangeCanColor()
        {
            isChange = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (isChange == false) return;

            FoodItem item = collision.GetComponent<FoodItem>();
            if (item != null)
            {
                item.OnSpatulaHold();

                _touchTimer += Time.deltaTime;

                if (_touchTimer >= 0.5f)
                {
                    PlayTouchSound();
                    _touchTimer = 0f;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (isChange == false) return;

            FoodItem item = collision.GetComponent<FoodItem>();
            if (item != null)
            {
                item.OnSpatulaExit();

                _touchTimer = 0f;
            }
        }

        private void PlayTouchSound()
        {
            if (sfxTouch != null)
            {
                SoundManager.PlaySFX(sfxTouch);
            }
        }
    }
}