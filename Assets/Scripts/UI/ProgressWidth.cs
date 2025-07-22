using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressWidth : MonoBehaviour
{
    public RectTransform rectTransform;
    [SerializeField] private int maxWidth;
    [SerializeField] private int minWidth;
    [SerializeField] private float progressSpeed = 0.5f;
    [SerializeField] private Vector2 sizeRandom = new Vector2(0, 0);

    // [SerializeField] private Vector2 IqRange = new Vector2(0, 0);
    // public TextMeshProUGUI textIq;

    public bool isFillUp = true;
    public float currWidth;
    private float targetWidth;
    [SerializeField] private Image progressImage;

    private void Start()
    {
        targetWidth = currWidth;

        EventManager.StartListening(EventType.IncreaseProgress.ToString(), () =>
        {
            IncreseProgress();
        });

        EventManager.StartListening(EventType.IncreaseProgressScaled.ToString(), () =>
        {
            IncreaseProgressScaled();
        });
    }

    public void IncreseProgress()
    {
        isFillUp = true;
        targetWidth += Random.Range(sizeRandom.x, sizeRandom.y);
        if (targetWidth > maxWidth) targetWidth = maxWidth;
    }

    public void IncreaseProgressScaled()
    {
        isFillUp = true;
        targetWidth += 10f;
        if (targetWidth > maxWidth) targetWidth = maxWidth;
    }

    public bool enableFill = true;
    private void Update()
    {
        if (!enableFill) return;
        FillAmount();
    }

    public virtual void FillAmount()
    {
        if (targetWidth - currWidth < 0.25f) return;
        currWidth += progressSpeed * Time.deltaTime;

        updateFill();
    }

    private void updateFill()
    {
        progressImage.fillAmount = currWidth / maxWidth;
    }
}
