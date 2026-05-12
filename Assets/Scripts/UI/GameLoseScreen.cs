using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameLoseScreen : UIScreen
{
    [Header("Audio")]
    [SerializeField] private AudioClip sfxLose;

    [Header("Screenshot")]
    [SerializeField] private RawImage imgCapture;
    [SerializeField] private RenderTexture captureRT;

    [Header("Button Anim")]
    [SerializeField] private RectTransform[] animatedBtns;
    [SerializeField] private float durationMoveIn = 0.15f;
    [SerializeField] private float delayMoveIn = 0.15f;
    [SerializeField] private float delayBetweenBtns = 0.05f;

    private float[] _originalBtnPosX;
    private float _lastTimeClick;
    private const float CooldownClick = 0.5f;
    public override void OnCreate()
    {
        base.OnCreate();

        if (animatedBtns != null)
        {
            _originalBtnPosX = new float[animatedBtns.Length];
            for (int i = 0; i < animatedBtns.Length; i++)
            {
                RectTransform rect = animatedBtns[i].GetComponent<RectTransform>();
                _originalBtnPosX[i] = rect.anchoredPosition.x;
                rect.anchoredPosition = new Vector2(0f, rect.anchoredPosition.y);
            }
        }

        CaptureScreen();
        PlayLoseSound();
        AnimateButtonsIn();
    }
    public void gotoStore()
    {
        GameManager.Ins.gotoStore();
    }
    public override void OnShow()
    {
        base.OnShow();

    }

    public override void OnHide()
    {
        base.OnHide();
        DOTween.KillAll();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public override void Resize(Vector2 gameSize)
    {
        base.Resize(gameSize);
        if (RectTf == null) return;
        RectTf.sizeDelta = gameSize;
    }
    private void CaptureScreen()
    {
        if (imgCapture == null || Camera.main == null || captureRT == null) return;
        Camera.main.targetTexture = captureRT;
        Camera.main.Render();
        Camera.main.targetTexture = null;
        imgCapture.texture = captureRT;
    }
    private void AnimateButtonsIn()
    {
        if (animatedBtns == null || _originalBtnPosX == null) return;
        for (int i = 0; i < animatedBtns.Length; i++)
        {
            RectTransform rect = animatedBtns[i].GetComponent<RectTransform>();
            rect.DOAnchorPosX(_originalBtnPosX[i], durationMoveIn)
                .SetEase(Ease.OutBack)
                .SetDelay(delayMoveIn + delayBetweenBtns * i)
                .SetUpdate(true)
                .Play();
        }
    }
    private void PlayLoseSound()
    {
        SoundManager.PlaySFXOneShot(sfxLose);
    }
    public void OnClickRetry()
    {
        if (!CanClick()) return;
    }
    public void OnClickHome()
    {
        if (!CanClick()) return;
    }
    public void OnClickStore()
    {
        if (!CanClick()) return;
        GameManager.Instance.gotoStore();
    }
    private bool CanClick()
    {
        if (Time.unscaledTime < _lastTimeClick + CooldownClick) return false;
        _lastTimeClick = Time.unscaledTime;
        return true;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        DOTween.Kill(this);
    }
}
