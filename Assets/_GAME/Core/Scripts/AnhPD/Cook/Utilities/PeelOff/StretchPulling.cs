using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  [RequireComponent(typeof(RotateToMouseDirection))]
  public class StretchPulling : MonoBehaviour
  {
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D coll2D;
    [SerializeField] private float detachDuration = 2f, detachDistance = 4f, startZ;
    [SerializeField] private float localScale = 1f, maxScaleUpRate = 2f, minStretchDistance = 1f;
    [SerializeField] private AudioClip sfxPick, sfxStretch, sfxComplete;

    protected bool _isDragging;
    private float _timer;

    public bool isComplete;
    public UnityEvent onComplete;

    [Button]
    private void Init()
    {
      coll2D = GetComponent<Collider2D>();
      if (!coll2D)
      {
        coll2D = gameObject.AddComponent<BoxCollider2D>();
      }
      spriteRenderer = GetComponentInChildren<SpriteRenderer>();
      localScale = transform.localScale.x;
      startZ = transform.eulerAngles.z;
    }
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      transform.DOKill();
      // AudioManager.PlaySFxRandomPitch(sfxPick);
      // AudioManager.PlaySFX(sfxStretch, .2f);

      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      _timer = 0f;
      _isDragging = true;
    }
    private void OnMouseDrag()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      if (isComplete) return;

      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      _timer += Time.deltaTime;
      if (_timer >= detachDuration)
      {
        OnComplete();
        return;
      }

      float distance = (Vector2.Distance(transform.position, mousePos));
      float rate = distance / minStretchDistance;
      rate = Mathf.Clamp(rate, 1, maxScaleUpRate);

      transform.localScale = new Vector3(localScale, localScale * (1 + rate * .25f), localScale);

      if (distance >= detachDistance)
      {
        OnComplete();
      }

    }
    protected virtual void OnMouseUp()
    {
      if (!LevelBase.Ins.IsAllowInteract || !_isDragging) return;
      _isDragging = false;
      if (!isComplete)
      {
        coll2D.enabled = false;
        transform.DOScaleY(localScale, .3f).SetEase(Ease.InOutBounce).OnComplete(() =>
        {
          coll2D.enabled = true;
        });
        transform.DORotate(new Vector3(0, 0, startZ), .3f).SetEase(Ease.InOutBack);
      }
    }
    public void OnReady()
    {
      coll2D.enabled = true;
    }
    private void OnComplete()
    {
      if (isComplete) return;
      // AudioManager.PlaySFx(sfxComplete, .5f);

      _isDragging = false;
      coll2D.enabled = false;
      transform.DOComplete();
      transform.DOScale(localScale, .3f).SetEase(Ease.InOutBounce);
      Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      transform.DOJump(mousePos, .3f, 1, .3f).OnComplete(() =>
      {
        isComplete = true;
        onComplete?.Invoke();
      });
    }
  }
}
