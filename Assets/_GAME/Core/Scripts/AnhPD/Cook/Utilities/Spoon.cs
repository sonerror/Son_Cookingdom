// using Link.PumkinPie;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static DivideGreenFade;
using Utilities;
using DG.Tweening;

namespace AnhPD.Cook
{
  public class Spoon : MonoBehaviour
  {
    [SerializeField] protected SpriteRenderer[] spices;
    [SerializeField] protected SpriteRenderer[] pours;
    [SerializeField] protected Transform[] sources;
    [SerializeField] protected float zAngle;
    [SerializeField] protected AudioClip sfx;

    private Transform tf;
    public Transform Tf => tf ? tf : tf = transform;
    public bool IsHaveSpice => currentIndex >= 0;
    private int currentIndex = -1;
    private bool _isPouring = false;
    public int sourcesNumber => sources.Length;
    public bool IsBlock = false, IsHideSource = false;
    public void CheckSource(int index = 0, float range = .5f)
    {
      if (IsBlock) return;
      if (IsHaveSpice) return;
      if (Vector2.Distance(spices[index].transform.position, sources[index].position) < range)
      {
        // AudioManager.PlaySFX(sfx);
        spices[index].transform.Appear();
        spices[index].SetAlpha(1);
        currentIndex = index;
        sources[index].Appear();

        if (IsHideSource)
        {
          sources[index].gameObject.SetActive(false);
        }
      }
    }
    public bool IsInTargetRange(Vector3 target, float range = .5f)
    {
      return Vector2.Distance(target, spices[0].transform.position) < range;
    }
    public Transform GetNearestTarget(Transform[] targets)
    {
      if (targets.Length == 0)
      {
        return null;
      }
      float distance = Vector2.Distance(spices[0].transform.position, targets[0].transform.position);
      int index = 0;
      for (int i = 1; i < targets.Length; i++)
      {
        float dis = Vector2.Distance(spices[0].transform.position, targets[i].transform.position);
        if (dis < distance)
        {
          index = i;
          distance = dis;
        }
      }
      return targets[index];
    }
    public void OnPour(Vector3 pos, Action startAction = null, Action endAction = null)
    {
      _isPouring = true;
      float duration = .3f;
      Tf.DOMove(pos, duration);
      Tf.DORotate(new Vector3(0, 0, zAngle), duration).OnComplete(start);

      void start()
      {
        // AudioManager.PlaySFX(sfx);

        startAction?.Invoke();
        pours[currentIndex].gameObject.SetActive(true);
        spices[currentIndex].DOFade(0, duration).OnComplete(end);
      }
      void end()
      {
        pours[currentIndex].gameObject.SetActive(false);
        endAction?.Invoke();
        _isPouring = false;
      }
    }
    public void TurnOffSpices()
    {
      if (IsHaveSpice && IsHideSource && !_isPouring)
      {
        sources[currentIndex].gameObject.SetActive(true);
      }
      for (int i = 0; i < spices.Length; i++)
      {
        spices[i].gameObject.SetActive(false);
      }
      currentIndex = -1;
    }
  }
}

