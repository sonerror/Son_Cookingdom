using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnhPD.Tanghulu
{
  public class FlyingUnitStep : FruitPreparationStep
  {
    [SerializeField] private Transform target;
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip sfxPop;
    [SerializeField] private float force = 1f;
    [SerializeField] private float duration = .2f;
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      coll2D.enabled = false;
      // AudioManager.PlaySFxRandomPitch(sfxPop);
      spriteRenderer.sortingOrder = 20;
      Vector3 destination = target.position;

      transform.DOJump(destination, force, 1, duration)
          .OnComplete(() =>
          {
            gameObject.SetActive(false);
            target.Appear();
            OnComplete();
          });
    }

    [Button]
    private void Init()
    {
      coll2D = GetComponent<Collider2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
    }
  }
}
