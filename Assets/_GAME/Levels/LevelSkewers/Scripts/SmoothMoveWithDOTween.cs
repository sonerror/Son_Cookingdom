using UnityEngine;
using DG.Tweening;
using Satisgame;
public class SmoothMoveWithDOTween : MonoBehaviour
{
    [SerializeField] private EmojiControl emoji;
    [SerializeField] private Transform tfTarget;

    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease easeType = Ease.InOutSine;
    private Tween moveTween;

    public void OnMove()
    {
        if (tfTarget == null) return;
        moveTween?.Kill();
        moveTween = transform.DOMove(tfTarget.position, duration)
            .SetEase(easeType).OnComplete(() =>
            {
                emoji.ShowPositive();
            });
    }
    public void StopMove()
    {
        moveTween?.Kill();
    }
}