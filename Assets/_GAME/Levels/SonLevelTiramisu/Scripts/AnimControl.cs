using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public class AnimControl : SonMonoBehaviour
    {
        [SerializeField] private Animation eggAnim;
        [SerializeField] private bool activeObj = false;
        [SerializeField] private AnimEgg animEffect;

        public UnityEvent onDoneAnim;
        public UnityEvent onStartAnim;

        private Coroutine playAnimCoroutine;

        public void PlayAnim(float delay = 0)
        {
            if (eggAnim != null)
            {
                if (playAnimCoroutine != null)
                {
                    StopCoroutine(playAnimCoroutine);
                }
                playAnimCoroutine = StartCoroutine(IE_DelayPlay(delay));
            }
        }
        IEnumerator IE_DelayPlay(float delay = 0)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            onStartAnim?.Invoke();
            eggAnim.gameObject.SetActive(true);
            yield return null;
            eggAnim.Play();
            StartCoroutine(IE_DelayEffect());
            while (eggAnim.isPlaying)
            {
                yield return null;
            }
            eggAnim.gameObject.SetActive(activeObj);
            onDoneAnim?.Invoke();
            Debug.Log("Done anim egg");
            playAnimCoroutine = null;
        }

        IEnumerator IE_DelayEffect()
        {
            yield return new WaitForSeconds(0.25f);
            animEffect.SfxKeng();
            animEffect.ShakeBowl();
            yield return new WaitForSeconds(0.25f);
            animEffect.SfxCrack();
        }

        public void StopAnim()
        {
            if (eggAnim != null && eggAnim.isPlaying)
            {
                eggAnim.Stop();
                eggAnim.gameObject.SetActive(activeObj);
                if (playAnimCoroutine != null)
                {
                    StopCoroutine(playAnimCoroutine);
                    playAnimCoroutine = null;
                }
                onDoneAnim?.Invoke();
                Debug.Log("Stop anim egg");
            }
        }

        public void AddListenerOnDoneAnim(UnityAction action)
        {
            if (onDoneAnim == null)
            {
                onDoneAnim = new UnityEvent();
            }
            onDoneAnim.AddListener(action);
        }

        public void RemoveListenerOnDoneAnim(UnityAction action)
        {
            if (onDoneAnim != null)
            {
                onDoneAnim.RemoveListener(action);
            }
        }
    }
}
