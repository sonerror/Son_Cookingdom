using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace sonnv
{
    public class MiniGame : MonoBehaviour
    {
        [SerializeField] TapBar tapBar;
        [SerializeField] SpriteRenderer handSp;
        [SerializeField] Animation animation;
        [SerializeField] AudioClip ouchSound, hanSlapSound, crackSound;
        public AnimationClip tapDone, tapRollback;
        public Action onTapDone, onTapHit, onTapHitMiss;
        public void PlayAnim(AnimationClip clip)
        {
            animation.Stop();
            animation.Play(clip.name);
        }
        [Button]
        public void OnStartMiniGame(int step)
        {
            tapBar.OnInit(step);
            tapBar.OnStart();
            tapBar.OnDoneEvent.AddListener(OnStopMiniGame);
            handSp.DOFade(1, 0.3f);

        }
        [Button]
        public void OnStopMiniGame()
        {
            tapBar.OnDoneEvent.RemoveAllListeners();
            handSp.DOFade(0, 0.5f).OnComplete(() =>
            {
                onTapDone?.Invoke();
            });
        }
        public void CrackHand()
        {
            SoundManager.PlaySFXOneShot(hanSlapSound);
            PlayAnim(tapDone);
            OnEvent(0.35f, () =>
            {
                onTapHit?.Invoke();
                SoundManager.PlaySFXOneShot(crackSound);

            });
            OnEvent(0.5f, () =>
            {
                PlayAnim(tapRollback);
            });
        }
        public void FailHand()
        {
            SoundManager.PlaySFXOneShot(hanSlapSound);
            PlayAnim(tapDone);
            OnEvent(0.35f, () =>
            {
                StartBlink(0.2f, 1f);
                SoundManager.PlaySFXOneShot(ouchSound);
                onTapHitMiss?.Invoke();

            });
            OnEvent(0.5f, () =>
            {
                PlayAnim(tapRollback);
            });
            OnEvent(1f, () =>
            {
                StopBlink();
            });
        }
        public Coroutine OnEvent(float time, Action action)
        {
            return StartCoroutine(WaitForAnimation(time, action));
        }
        IEnumerator WaitForAnimation(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }
        Tween blinkTween;
        public void StartBlink(float blinkDuration, float totalTime)
        {
            handSp.color = Color.white;

            blinkTween = handSp
                .DOColor(Color.red, blinkDuration)
                .SetLoops(Mathf.RoundToInt(totalTime / blinkDuration), LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }

        public void StopBlink()
        {
            blinkTween?.Kill();
            handSp.DOColor(Color.white, 0.3f);
        }

    }
}
