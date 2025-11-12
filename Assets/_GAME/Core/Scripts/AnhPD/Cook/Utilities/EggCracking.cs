using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class EggCracking : APDCookingDrag
  {
    [SerializeField] private SpriteRenderer egg;
    [SerializeField] private Transform crack_pos, left, right;
    [SerializeField] private AudioClip sfxHit, sfxCrack;

    [SerializeField] private float height = 1.5f;

    public bool isReAppear = false;
    public UnityEvent onReReady;
    public UnityEvent onStartAnim;

    protected override void CheckTarget()
    {
      if (IsInRange(target))
      {
        isDragging = false;
        Tf.DOKill();

        OnComplete();

        IsComplete = true;
      }
      else if (isMouseUpCheck) OnIncorrectUse();
    }
    public override void OnComplete()
    {
      coll2D.enabled = false;
      isDragging = false;

      onStartAnim?.Invoke();
      //base.OnComplete();
      Tf.DOMove(crack_pos.position, .3f).OnComplete(Crack);
      Tf.DORotate(Vector3.zero, .3f);
      void Crack()
      {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Tf.DOMove(crack_pos.position + Vector3.up, .2f));
        sequence.Append(Tf.DOMove(crack_pos.position, .1f).SetEase(Ease.InBack).OnComplete(() =>
        {
          crack_pos.parent.DOPunchRotation(new Vector3(0, 0, -3f), .3f);
          // AudioManager.PlaySFX(sfxHit);
          Open();
        }));
        sequence.Append(Tf.DOMove(target.position + Vector3.up * height, .5f));
      }
      void Open()
      {
        egg.transform.DOScaleY(1.5f, .4f).OnComplete(() =>
        {
          egg.DOFade(0, .1f);
        }).SetDelay(.25f);
        left.DORotate(new Vector3(0, 0, -45f), .5f).SetDelay(.2f).OnStart(() =>
        {
          // AudioManager.PlaySFX(sfxCrack);
        });
        right.DORotate(new Vector3(0, 0, 45f), .5f).SetDelay(.2f).OnComplete(() =>
        {
          completeEvent?.Invoke();

          for (int i = 0; i < parts.Length; i++)
          {
            parts[i].DOFade(0, 1f);
          }
        });
      }
    }
    public override void OnReReady(bool isReady = true)
    {
      base.OnReReady(isReady);
      onReReady?.Invoke();
      if (isReAppear)
      {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
      }
      coll2D.enabled = true;
      left.localEulerAngles = Vector3.zero;
      right.localEulerAngles = Vector3.zero;
      egg.transform.DOScaleY(.6f, 0f);
      egg.SetAlpha(1);
      for (int i = 0; i < parts.Length; i++)
      {
        parts[i].SetAlpha(1);
        parts[i].sortingOrder = spriteRenderer.sortingOrder;
      }
    }
  }
}

