using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
  public class KnifeDrag : APDCookingDrag
  {
    [SerializeField] private float zAngle_cut = -60f;
    [SerializeField] private AudioClip sfxSlice;
    public void StartCutAnim(Vector3 pos, Action action = null)
    {
      gameObject.SetActive(true);
      coll2D.enabled = false;
      isDragging = false;
      Tf.DOKill();

      Tf.DORotate(new Vector3(0, 0, zAngle_cut), .3f);
      Tf.DOMove(pos + new Vector3(1.5f, 1.5f), .3f).OnComplete(() =>
      {
        // AudioManager.PlaySFX(sfxSlice);
      });
      Tf.DOMove(pos - new Vector3(1.5f, 1.5f), .15f).SetDelay(.3f).OnComplete(() =>
      {
        OnComplete();
        action?.Invoke();
        coll2D.enabled = true;
      });
    }
  }
}

