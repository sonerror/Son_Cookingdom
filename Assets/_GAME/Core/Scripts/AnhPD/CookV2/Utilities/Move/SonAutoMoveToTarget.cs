using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class SonAutoMoveToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float delay;
    [SerializeField] private AudioClip vfxDone;
    public UnityEvent onComplete;
    public void Move()
    {
        transform.DOMove(target.position, duration).SetDelay(delay).SetEase(Ease.InBack);
        transform.DOScale(target.localScale, duration).SetDelay(delay).SetEase(Ease.Linear);
        transform.DORotate(target.eulerAngles, duration).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.localScale = Vector3.zero;
            SoundManager.PlaySFXOneShot(vfxDone);
            onComplete?.Invoke();
        });
    }
}
