using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.CookV2
{
  public class CrackNutShellStep : FruitPreparationStep
  {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip sfxCrack;
    [SerializeField, Range(-1, 1)] private int dir = 1;

    private bool _isComplete;

    public override void Setup(FruitPreparation preparation)
    {
      base.Setup(preparation);
      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || _isComplete) return;
      // AudioManager.PlaySFX(sfxCrack);
      _isComplete = true;
      spriteRenderer.DOFade(0, .5f);
      spriteRenderer.transform.DOMoveX(spriteRenderer.transform.position.x + dir * .5f, 0.5f).OnComplete(() =>
      {
        gameObject.SetActive(false);
        OnComplete();
      });
    }

#if UNITY_EDITOR
    [Button]
    private void Init()
    {
      gameObject.AddComponent<BoxCollider2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
    }
#endif
  }
}
