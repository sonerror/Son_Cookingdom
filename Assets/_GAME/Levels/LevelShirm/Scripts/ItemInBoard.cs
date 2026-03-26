using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace sonnv
{
    public class ItemInBoard : MonoBehaviour
    {
        [SerializeField] MiniGame miniGame;

        [SerializeField] ParticleSystem[] pointPeels;
        [SerializeField] GameObject[] objHide;
        [SerializeField] Animation scaleAnim;
        [SerializeField] private UnityEvent onDone;
        public UnityEvent OnDone => onDone;

        [SerializeField] private bool isDone = false;
        public bool IsDone => isDone;

        private void PlayCrack()
        {
            if (isDone) return;

            for (int i = 0; i < pointPeels.Length; i++)
            {
                pointPeels[i].gameObject.SetActive(true);
                pointPeels[i].Play();
                objHide[i].SetActive(false);
            }

            scaleAnim.Stop();
            scaleAnim.Play();

            isDone = true;
            onDone?.Invoke();
        }

        public void OnInBoard()
        {
            miniGame.onTapHit += PlayCrack;
            miniGame.onTapDone += OnDonePeel;
            miniGame.onTapHitMiss += OnHitMissPeel;

            miniGame.OnStartMiniGame(1);
        }

        public void OnDonePeel()
        {
            miniGame.onTapHit -= PlayCrack;
            miniGame.onTapDone -= OnDonePeel;
            miniGame.onTapHitMiss -= OnHitMissPeel;
        }

        public void OnHitMissPeel()
        {
            scaleAnim.Play();
        }
    }
}