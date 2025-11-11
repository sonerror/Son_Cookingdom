using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace AnhPD.CookV2
{
  public class ScoopSpoon : APDv2Drag
  {
    [SerializeField] private Transform source, spoonObject;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float sourceRangeRate = 1f, volume = 1f, pourAngle = -145f;

    [SerializeField] private bool isReturnObjectWhenMouseUp = true, isPlayRandomPitch = false;

    public UnityEvent onScoopSource, onReturnObject;

    private bool _isHaveObject;
    private bool _isSourceReady = true;
    private bool _isScooping;
    protected override void Start()
    {
      base.Start();
      isHideAfterComplete = false;
    }

    protected override void MouseDrag(BaseEventData eventData)
    {
      base.MouseDrag(eventData);
      if (!_isSourceReady) return;
      if (!_isHaveObject)
      {
        if (IsInRange(center.position, source.position, sourceRangeRate) && !_isScooping)
        {
          OnScoop(() =>
          {
            onScoopSource?.Invoke();
            _isHaveObject = true;
            spoonObject.Appear();
          });
        }
      }
    }

    protected override void CheckTarget()
    {
      if (!_isHaveObject) return;
      base.CheckTarget();
    }

    protected override void MouseUp(BaseEventData eventData)
    {
      base.MouseUp(eventData);
      if (isReturnObjectWhenMouseUp && !_isScooping)
      {
        onReturnObject?.Invoke();
        _isHaveObject = false;
        spoonObject.gameObject.SetActive(false);
      }
    }

    public override void OnComplete()
    {
      Tf.DOKill();
      isDragging = false;
      coll2D.enabled = false;
      _isScooping = true;
      Tf.DOMove(target.position + offset, .3f).OnComplete(() =>
      {
        OnScoop(() =>
              {
                _isHaveObject = false;
                spoonObject.gameObject.SetActive(false);

                coll2D.enabled = true;
                base.OnComplete();
              });
      });
    }

    public void OnSourceReady(bool enable = true)
    {
      _isSourceReady = enable;
    }

    private void OnScoop(Action action = null)
    {
      _isScooping = true;
      // Tf.DOComplete(true);
      Tf.DORotate(new Vector3(Tf.eulerAngles.x, Tf.eulerAngles.y, pourAngle), .15f).OnComplete(() =>
      {
        if (isPlayRandomPitch) { } // AudioManager.PlaySFxRandomPitch(sfx, volume * Random.Range(.8f, 1f), 1f, 2f); 

        else // AudioManager.PlaySFX(sfx, volume);
          action?.Invoke();
        Tf.DORotate(isRotateWhenPickUp ? new Vector3(Tf.eulerAngles.x, Tf.eulerAngles.y, pickupAngle) : startRotation, .15f).OnComplete(() =>
              {
                _isScooping = false;
              });
      });
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
      base.OnDrawGizmosSelected();
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(source.position, dropDistance * sourceRangeRate);
    }
#endif
  }
}
