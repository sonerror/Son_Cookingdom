using AnhPD.Cook;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.HotDog
{
  public class ShakerSpices : APDCookingDrag
  {
    [SerializeField] private int number = 2;
    [SerializeField] private Vector3 offset = new Vector3(0, 1, 0);
    [SerializeField] private float deltaY = 1f, rotateAngle = 150f;
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] private AudioClip sfxShaker;

    public UnityEvent onStartAnim;

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

        onStartAnim?.Invoke();
        Tf.DOMove(target.position + offset, .3f);
        Tf.DORotate(new Vector3(0, 0, rotateAngle), .3f).OnComplete(shake);

        void shake()
        {
          float duration = .1f;
          Sequence sequence = DOTween.Sequence();
          sequence.Append(spriteRenderer.transform.DOLocalMoveY(-deltaY, duration));
          sequence.Append(spriteRenderer.transform.DOLocalMoveY(0, duration));

          sequence.SetLoops(number);
          sequence.OnStepComplete(() =>
          {
            // AudioManager.PlaySFX(sfxShaker);
            vfx.transform.SetParent(null);
            vfx.Play();
          });
          sequence.OnComplete(complete);
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
