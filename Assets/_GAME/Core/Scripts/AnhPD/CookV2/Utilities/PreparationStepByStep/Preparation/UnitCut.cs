using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.CookV2
{
  public class UnitCut : FruitPreparation
  {
    [FoldoutGroup("References")][SerializeField] private GameObject[] afterCutFruits;
    [FoldoutGroup("References")][SerializeField] private Transform knife;
    [FoldoutGroup("References")][SerializeField] private Collider2D coll2D;
    [FoldoutGroup("References")][SerializeField] private AudioClip sfxKnifeAppear, sfxKnifeCut;
    [FoldoutGroup("References")][SerializeField] private ParticleSystem vfxCut;
    [FoldoutGroup("Parameters")][SerializeField] private Vector3 knifeOffset;
    [FoldoutGroup("Parameters")][SerializeField] private float cutOffset = .5f;
    private bool _isReady = false;

    public override void OnHaveTool()
    {
      base.OnHaveTool();
      // AudioManager.PlaySFX(sfxKnifeAppear);
      knife.Appear();
      MoveKnifeToCurrentPos();
      coll2D.enabled = true;
    }

    private void MoveKnifeToCurrentPos()
    {
      if (Count >= steps.Length) return;
      knife.DOMove(steps[Count].transform.position + knifeOffset, .5f).OnComplete(() =>
      {
        _isReady = true;
      });
    }

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || !_isReady) return;

      _isReady = false;
      knife.DOComplete();
      knife.DOMoveY(knife.position.y + cutOffset, .1f);
      knife.DOMoveY(knife.position.y - cutOffset * 2, .1f).SetDelay(.1f).OnComplete(Cut);

      void Cut()
      {
        vfxCut.transform.position = steps[Count].transform.position;
        if (vfxCut) vfxCut.Play();
        // AudioManager.PlaySFX(sfxKnifeCut);
        steps[Count].gameObject.SetActive(false);
        afterCutFruits[Count].transform.FallAppear();
        knife.DOMoveY(knife.position.y + cutOffset, .1f).OnComplete(() =>
        {
          OnDoneStep();
          MoveKnifeToCurrentPos();
        });
      }
    }
  }
}
