using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace AnhPD.CookV2
{
  public class PrickController : MonoBehaviour
  {
    [SerializeField] private PrickObject[] targets;
    [SerializeField] private SpriteRenderer fadeTarget;
    [SerializeField] private Transform fork;
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private AudioClip sfxAppear, sfxHit;
    [SerializeField] private float moveDuration = .3f, prickDuration = .2f, prickDelay = .1f;

    private List<PrickObject> listTargets;
    private bool _isPricking;

    public bool isFallAppear = true;
    public UnityEvent onComplete;

    public void Appear()
    {
      gameObject.SetActive(true);
      if (fadeTarget) fadeTarget.SetAlpha(0f);
      for (int i = 0; i < targets.Length; i++)
      {
        targets[i].Restart();

        if (isFallAppear)
        {
          targets[i].Appear();
          targets[i].transform.FallAppear(.1f, .5f, .2f, PlaySfx, .1f * i);
        }
      }

      listTargets = targets.ToList();
    }

    public void Ready()
    {
      fork.Appear();
      coll2D.enabled = true;
    }
    private void OnMouseDown()
    {

      if (!LevelBase.Ins.IsAllowInteract || _isPricking) return;

      if (listTargets.Count > 0)
      {
        int index = Random.Range(0, listTargets.Count);
        Prick(index);
      }
    }

    private void Prick(int index)
    {
      _isPricking = true;
      fork.gameObject.SetActive(true);
      Transform target = listTargets[index].transform;

      fork.DOMove(target.position + target.up * .75f, moveDuration).SetEase(Ease.OutBack);
      int dir = target.position.x >= transform.position.x ? -1 : 1;
      fork.DOLocalRotate(new Vector3(0, 0, Random.Range(5f, 15f) * dir), moveDuration);

      fork.DOMove(target.position, prickDuration)
          .SetDelay(prickDelay)
          .SetEase(Ease.InBack)
          .OnComplete(OnPricked);

      void OnPricked()
      {
        // AudioManager.PlaySFX(sfxHit);

        _isPricking = false;

        listTargets[index].OnHit();
        listTargets.RemoveAt(index);

        if (fadeTarget)
        {
          float rate = (float)(targets.Length - listTargets.Count) / (targets.Length);
          fadeTarget.SetAlpha(rate);
        }

        if (listTargets.Count == 0)
        {
          fork.gameObject.SetActive(false);
          onComplete?.Invoke();
          coll2D.enabled = false;
        }
      }
    }
    private void PlaySfx()
    {
      // AudioManager.PlaySFX(sfxAppear, .2f);
    }
#if UNITY_EDITOR
    [Button]
    private void GetPrickObject()
    {
      targets = GetComponentsInChildren<PrickObject>(true);
    }

    [Button]
    private void Test()
    {
      Appear();
      Ready();
    }
#endif
  }
}
