using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TrungKien
{
    public class Clock : MonoBehaviour
    {
        Transform tf;
        public Transform TF { get { return tf ??= transform; } }
        [SerializeField] private Image timerImage;
        [SerializeField] private float showTime = 0.3f;
        [SerializeField] private float timeOut = 3f;
        [SerializeField] private AudioSource sound;

        public UnityEvent _OnTimeStart;
        public UnityEvent _OnTimeOut;
        private Tween _showTween;

        private bool _isTimeOut;
        private bool _isStartTimer;
        float timer;

        private void Update()
        {
            if (!_isStartTimer) return;
            timerImage.fillAmount -= Time.deltaTime / timeOut;
            timer += Time.deltaTime;
            timerImage.color = (timer / timeOut > 0.7f) ? Color.red : Color.green;
            if (!(timerImage.fillAmount <= 0)) return;
            timer = 0;
            _OnTimeOut?.Invoke();
            Hide();
        }

        public void Show(float time)
        {
            timer = 0;
            timeOut = time;
            Show();
        }

        [Sirenix.OdinInspector.Button]
        private void Show()
        {
            if (_isStartTimer) return;
            _OnTimeStart?.Invoke();
            gameObject.SetActive(true);
            TF.localScale = Vector3.zero;
            timerImage.fillAmount = 1;
            _showTween?.Kill();
            _showTween = TF.DOScale(Vector3.one, showTime);
            _isStartTimer = true;
            if (sound != null)
            {
                sound.Play();
            }
        }

        public void Hide()
        {
            _showTween?.Kill();
            _showTween = TF.DOScale(Vector3.zero, showTime)
                .OnComplete(() => gameObject.SetActive(false));
            _isStartTimer = false;
            if (sound != null)
            {
                sound.Stop();
            }
        }
    }
}