using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.CookV2
{
  public class KnifePeeling : FruitPreparation
  {
    [SerializeField] private Transform knife;
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private AudioClip sfxPeel;
    [SerializeField] private SpriteRenderer[] peels;
    [SerializeField] private float angle;
    private bool _isPeeling;

    public override void OnHaveTool()
    {
      base.OnHaveTool();
      knife.Appear();
      coll2D.enabled = true;
    }

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || _isPeeling) return;

      _isPeeling = true;
      // AudioManager.PlaySFX(sfxPeel);
      knife.DOPunchRotation(new Vector3(0, 0, angle), .3f, 3).OnComplete(() =>
      {
        peels[Count].DOFade(0, .3f);
        peels[Count].transform.DOMove(peels[Count].transform.position - new Vector3(.2f, .2f), .3f);
        OnDoneStep();
        _isPeeling = false;
        if (Count >= peels.Length)
        {
          knife.gameObject.SetActive(false);
          OnComplete();
        }
      });
    }
  }
}
