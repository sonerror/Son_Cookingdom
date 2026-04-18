using UnityEngine;
using DG.Tweening;

public class LoopScale : MonoBehaviour
{
    [SerializeField] private float scaleUp = 1.2f;
    [SerializeField] private float duration = 0.5f;

    private Tween scaleTween;
    private Vector3 baseScale;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    public void Play()
    {
        if (scaleTween != null && scaleTween.IsActive()) return;

        scaleTween = transform.DOScale(baseScale * scaleUp, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Stop()
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
            scaleTween = null;
        }

        transform.localScale = baseScale;
    }

    public void KillTween()
    {
        if (scaleTween != null)
        {
            scaleTween.Kill();
            scaleTween = null;
        }
    }

    void OnDestroy()
    {
        KillTween();
    }
}