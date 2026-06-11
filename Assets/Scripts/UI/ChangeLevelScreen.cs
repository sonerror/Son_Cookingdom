using DG.Tweening;
using UnityEngine;

public class ChangeLevelScreen : UIScreen
{
    private enum ShowAnimType
    {
        FadeCanvasGroup = 0,
        ScaleRectTf = 1
    }

    [Header("Show Anim")]
    [SerializeField] private ShowAnimType showAnimType = ShowAnimType.FadeCanvasGroup;

    [Header("Fade UI")]
    [SerializeField] private CanvasGroup uiGroupCanvas;
    [Header("Scale Rect Tf")]
    [SerializeField] private RectTransform rectTF;
    [SerializeField] private RectTransform rectTFBtnNext;
    [Header("Anim Settings")]
    [SerializeField] private float animDuration = 0.45f;
    [SerializeField] private Ease animEase = Ease.OutBack;

    [SerializeField] private HandCtrlUI handCtrlUI;

    private Tween showTween;

    public override void OnCreate()
    {
        base.OnCreate();

        Debug.Log("Show ChangeLevelScreen");

        PlayShowAnim();
    }

    private void PlayShowAnim()
    {
        showTween?.Kill();

        if (showAnimType == ShowAnimType.FadeCanvasGroup)
        {
            PlayFadeCanvasGroup();
        }
        else
        {
            PlayScaleRectTf();
        }
    }

    private void PlayFadeCanvasGroup()
    {
        if (uiGroupCanvas == null)
            return;

        uiGroupCanvas.gameObject.SetActive(true);
        uiGroupCanvas.alpha = 0f;
        uiGroupCanvas.blocksRaycasts = false;

        showTween = uiGroupCanvas
            .DOFade(1f, animDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                uiGroupCanvas.blocksRaycasts = true;
                handCtrlUI.gameObject.SetActive(true);
                handCtrlUI.ShowHandAtPos(rectTFBtnNext.anchoredPosition);
            });
    }

    private void PlayScaleRectTf()
    {
        uiGroupCanvas.alpha = 1f;
        rectTF.gameObject.SetActive(true);
        rectTF.localScale = Vector3.zero;

        showTween = rectTF
            .DOScale(Vector3.one, animDuration).OnComplete(() =>
            {
                handCtrlUI.gameObject.SetActive(true);
                handCtrlUI.ShowHandAtPos(rectTFBtnNext.anchoredPosition);
                EndGame();
            }).SetEase(animEase);
    }

    public void gotoStore()
    {
        GameManager.Ins.gotoStore();
    }
    public void EndGame()
    {
        GameManager.Ins.showEndGame();
    }
    public override void Resize(Vector2 gameSize)
    {
        base.Resize(gameSize);
        rectTF.sizeDelta = gameSize;
    }

    public override void OnClose()
    {
        base.OnClose();

        showTween?.Kill();
    }
}