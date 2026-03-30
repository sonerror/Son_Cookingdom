using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace sonnv
{
    public class ClockTimer : SonMonoBehaviour
    {
        [SerializeField] private float scaleShow = 0.6f;

        [SerializeField] private Image timerImage;
        [SerializeField] private float showTime = 0.3f;
        [SerializeField] private float timeOut = 3f;
        [SerializeField] private AudioSource sound;

        public System.Action OnTimeOut;
        private Tween _showTween;

        private bool _isTimeOut;
        private bool _isStartTimer;

        private void Update()
        {
            if (!_isStartTimer) return;
            timerImage.fillAmount -= Time.deltaTime / timeOut;
            if (!(timerImage.fillAmount <= 0)) return;
            OnTimeOut?.Invoke();
            Hide();
        }

        public void Show(float time)
        {
            timeOut = time;
            Show();
        }

        [Sirenix.OdinInspector.Button]
        private void Show()
        {
            if (_isStartTimer) return;
            gameObject.SetActive(true);
            Tf.localScale = Vector3.zero;
            timerImage.fillAmount = 1;
            _showTween?.Kill();
            _showTween = Tf.DOScale(Vector3.one * scaleShow, showTime);
            _isStartTimer = true;
            sound?.Play();
        }

        private void Hide()
        {
            _showTween?.Kill();
            _showTween = Tf.DOScale(Vector3.zero, showTime)
                .OnComplete(() => gameObject.SetActive(false));
            _isStartTimer = false;
            sound?.Stop();
        }
        public void Hide(float time = 0)
        {
            timeOut = time;
            Hide();
        }
    }

}
