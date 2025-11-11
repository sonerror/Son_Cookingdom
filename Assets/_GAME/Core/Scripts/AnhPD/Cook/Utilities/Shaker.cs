using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
  public class Shaker : APDCookingDrag
  {
    [SerializeField] private float deltaY = .25f, offsetY = 1f, rotateAngle = 150f;
    [SerializeField] private AudioClip sfxShaker;
    protected override void Start()
    {
      isHideAfterComplete = false;
    }

    protected override void CheckTarget()
    {
      //base.CheckTarget();
      if (IsInRange(target))
      {
        isDragging = false;
        Tf.DOKill();
        coll2D.enabled = false;

        Tf.DOMove(target.position + Vector3.up * offsetY, .3f);
        Tf.DORotate(new Vector3(0, 0, rotateAngle), .3f).OnComplete(shake);

        void shake()
        {
          // AudioManager.PlaySFX(sfxShaker);

          float duration = .1f;
          spriteRenderer.transform.DOLocalMoveY(-deltaY, duration);
          spriteRenderer.transform.DOLocalMoveY(0, duration).SetDelay(duration).OnComplete(complete);
        }

        void complete()
        {
          coll2D.enabled = true;
          IsComplete = true;
          completeEvent?.Invoke();

          OnComplete();
        }
      }
      else if (isMouseUpCheck) OnIncorrectUse();
    }
  }
}

