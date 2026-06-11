using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HandCtrlUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTf;

    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 pressScale = new Vector3(0.85f, 0.85f, 1f);

    [SerializeField] private float normalRotateZ = 0f;
    [SerializeField] private float pressRotateZ = -12f;

    [SerializeField] private float downAnimDuration = 0.12f;
    [SerializeField] private float upAnimDuration = 0.18f;

    [SerializeField] private float delayBeforeMove = 0.75f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float repeatTwoPosTime = 3f;
    [SerializeField] private float repeatThreePosTime = 4f;


    [SerializeField] private float rotateZ = -20f;
    [SerializeField] private float timerLoop = 1f;

    private Vector2 pos1;
    private Vector2 pos2;
    private Vector2 pos3;

    private Vector2 centerPos;
    private float radius;

    private Tween moveTween;
    private Tween spinTween;
    private Tween handAnimTween;

    private void Awake()
    {
        if (rectTf == null)
            rectTf = GetComponent<RectTransform>();

        normalScale = rectTf.localScale;
        normalRotateZ = rectTf.localEulerAngles.z;
    }

    public void ShowHandAtPos(Vector2 pos)
    {
        StopAllCoroutines();
        KillAllTween();

        gameObject.SetActive(true);

        rectTf.anchoredPosition = pos;

        rectTf.localEulerAngles = Vector3.zero;

        handAnimTween = rectTf
            .DOLocalRotate(new Vector3(0f, 0f, rotateZ), timerLoop)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void HideHand()
    {
        gameObject.SetActive(false);

        StopAllCoroutines();
        KillAllTween();
    }

    public void ShowHandPosToPos(Vector2 pos1, Vector2 pos2)
    {
        StopAllCoroutines();
        KillAllTween();

        gameObject.SetActive(true);

        this.pos1 = pos1;
        this.pos2 = pos2;

        ShowHandTwoPos();
    }

    private void ShowHandTwoPos()
    {
        rectTf.anchoredPosition = pos1;

        PlayHandDown();

        moveTween = rectTf
            .DOAnchorPos(pos2, moveDuration)
            .SetDelay(delayBeforeMove)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                PlayHandUp();
            });

        StartCoroutine(IEShowHandTwoPos());
    }

    private IEnumerator IEShowHandTwoPos()
    {
        yield return new WaitForSeconds(repeatTwoPosTime);

        if (gameObject.activeSelf)
            ShowHandTwoPos();
    }

    public void ShowHandPosToPosToPos(Vector2 pos1, Vector2 pos2, Vector2 pos3)
    {
        StopAllCoroutines();
        KillAllTween();

        gameObject.SetActive(true);

        this.pos1 = pos1;
        this.pos2 = pos2;
        this.pos3 = pos3;

        ShowHandThreePos();
    }

    private void ShowHandThreePos()
    {
        rectTf.anchoredPosition = pos1;

        PlayHandDown();

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(delayBeforeMove);

        sequence.Append(
            rectTf.DOAnchorPos(pos2, moveDuration)
                .SetEase(Ease.Linear)
        );

        sequence.Append(
            rectTf.DOAnchorPos(pos3, moveDuration)
                .SetEase(Ease.Linear)
        );

        sequence.OnComplete(() =>
        {
            PlayHandUp();
        });

        moveTween = sequence;

        StartCoroutine(IEShowHandThreePos());
    }

    private IEnumerator IEShowHandThreePos()
    {
        yield return new WaitForSeconds(repeatThreePosTime);

        if (gameObject.activeSelf)
            ShowHandThreePos();
    }

    public void ShowHandSpinContinuous(Vector2 center, float radius)
    {
        StopAllCoroutines();
        KillAllTween();

        gameObject.SetActive(true);

        centerPos = center;
        this.radius = radius;

        PlayHandDown();

        spinTween = DOVirtual.Float(0f, 360f, 1.5f, angle =>
        {
            float rad = angle * Mathf.Deg2Rad;

            rectTf.anchoredPosition = centerPos + new Vector2(
                Mathf.Cos(rad) * this.radius,
                Mathf.Sin(rad) * this.radius
            );
        })
        .SetLoops(-1, LoopType.Restart)
        .SetEase(Ease.Linear);
    }

    public void StopSpinOnly()
    {
        spinTween?.Kill();
        spinTween = null;

        PlayHandUp();
    }

    private void PlayHandIdle()
    {
        handAnimTween?.Kill();

        rectTf.localScale = normalScale;
        rectTf.localEulerAngles = new Vector3(0f, 0f, normalRotateZ);

        handAnimTween = rectTf
            .DOScale(normalScale * 1.06f, 0.45f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void PlayHandDown()
    {
        handAnimTween?.Kill();

        handAnimTween = DOTween.Sequence()
            .Append(rectTf.DOScale(pressScale, downAnimDuration).SetEase(Ease.OutQuad))
            .Join(rectTf.DORotate(new Vector3(0f, 0f, pressRotateZ), downAnimDuration).SetEase(Ease.OutQuad));
    }

    private void PlayHandUp()
    {
        handAnimTween?.Kill();

        handAnimTween = DOTween.Sequence()
            .Append(rectTf.DOScale(normalScale, upAnimDuration).SetEase(Ease.OutBack))
            .Join(rectTf.DORotate(new Vector3(0f, 0f, normalRotateZ), upAnimDuration).SetEase(Ease.OutBack));
    }

    private void KillAllTween()
    {
        moveTween?.Kill();
        moveTween = null;

        spinTween?.Kill();
        spinTween = null;

        handAnimTween?.Kill();
        handAnimTween = null;

        if (rectTf != null)
            rectTf.DOKill();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        KillAllTween();
    }
}