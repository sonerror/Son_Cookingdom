using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FlipHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool canFlip = false;

    [SerializeField] private Sprite newCakeSprite;
    [SerializeField] private float flipDuration = 0.2f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] FxType soundPlay = FxType.None;

    public UnityEvent OnFlipStart;
    public UnityEvent OnSpriteChanged;
    public UnityEvent OnFlipComplete;

    private bool _isFlipped = false;
    private bool _isAnimating = false;
    private float _initialScaleX;

    private void Awake()
    {
        _initialScaleX = transform.localScale.x;
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canFlip == false) return;
        OnFlipAndChange();
    }
    public void OnFlipAndChange()
    {
        if (_isAnimating || _isFlipped) return;

        OnFlipStart?.Invoke();

        FlipAndChange();
    }
    private void FlipAndChange()
    {
        _isAnimating = true;

        transform.DOScaleX(0, flipDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            if (newCakeSprite != null)
            {
                _spriteRenderer.sprite = newCakeSprite;
            }
            SoundManager.Ins.PlayFx(soundPlay);
            OnSpriteChanged?.Invoke();

            transform.DOScaleX(_initialScaleX, flipDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _isAnimating = false;
                _isFlipped = true;

                OnFlipComplete?.Invoke();
            });
        });
    }

    public void ResetCake(Sprite originalSprite)
    {
        _isFlipped = false;
        _isAnimating = false;
        _spriteRenderer.sprite = originalSprite;
        transform.localScale = new Vector3(_initialScaleX, transform.localScale.y, transform.localScale.z);
    }
}