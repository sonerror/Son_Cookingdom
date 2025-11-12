using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
  public class FreeSpoon : APDCookingToolBase
  {
    [SerializeField] private List<SpiceData> allSpices;
    [SerializeField] private Transform target;
    [SerializeField] private Transform centerPos;
    [SerializeField] private float zAngle;
    [SerializeField] private Vector3 offset = new Vector2(1, 1);
    private List<Transform> _allSource;
    private SpiceData _currentSpice;

    private Func<bool> _conditionFunc;

    public bool IsHaveSpice => _currentSpice != null;

    public UnityEvent onStartAnim;
    protected override void Start()
    {
      base.Start();
      _allSource = allSpices.Select(s => s.source).ToList();
      if (!centerPos)
      {
        centerPos = allSpices[0].spiceInSpoon;
      }
      IsReady = true;
    }

    protected void CheckSource()
    {
      int index = APDUtilities.GetNearestTranformIndex(Tf, _allSource);
      Transform source = _allSource[index];

      if (IsInRange(source.position, centerPos.position) && !allSpices[index].isBlock)
      {
        _currentSpice = allSpices[index];
        _currentSpice.ShowSpice();
      }
    }

    protected override void MouseDrag(BaseEventData eventData)
    {
      base.MouseDrag(eventData);
      if (!IsHaveSpice) CheckSource();
    }

    protected override void MouseUp(BaseEventData eventData)
    {
      base.MouseUp(eventData);
      if (!IsHaveSpice) return;

      if (IsInRange(target.position, centerPos.position))
      {
        _conditionFunc = _currentSpice.ConditionFunc;
        if (_conditionFunc != null && !_conditionFunc())
        {
          OnIncorrectUse();
          CleanupCurrentSpice();
          return;
        }
        OnPourSpice();
      }
      else
      {
        CleanupCurrentSpice();
      }

      void CleanupCurrentSpice()
      {
        _currentSpice.HideSpice();
        _currentSpice = null;
      }
    }

    private void OnPourSpice()
    {
      coll2D.enabled = false;
      isDragging = false;
      Tf.DOKill();

      onStartAnim?.Invoke();
      Tf.DOMove(target.position + offset, .5f);
      Tf.DORotate(new Vector3(0, 0, zAngle), .3f).SetDelay(.5f).OnComplete(Pour);

      void Pour()
      {
        _currentSpice.StartPourSpice();
        Tf.DORotate(startRotation, .3f)
            .SetDelay(.1f)
            .OnComplete(End)
            .OnStart(() =>
            {
              _currentSpice.CompletePourSpice();
            });
      }

      void End()
      {
        coll2D.enabled = true;
        _currentSpice = null;
        Rewind();
      }
    }

    public void SetupCondition(int index, Func<bool> condition)
    {
      allSpices[index].SetupCondition(condition);
    }
  }

  [Serializable]
  public class SpiceData
  {
    public Transform source;
    public Transform spiceInSpoon;
    public GameObject spicePouring;
    public AudioClip sfx;
    public bool isHideSource;
    public bool isBlock = false;
    [Range(0, 1)] public float volume = 1f;

    public UnityEvent onComplete;

    public Func<bool> ConditionFunc;
    public void ShowSpice()
    {
      // AudioManager.PlaySFxRandomPitch(sfx, volume);
      spiceInSpoon.Appear();

      if (isHideSource) source.gameObject.SetActive(false);
      else source.Appear();
    }

    public void StartPourSpice()
    {
      // AudioManager.PlaySFxRandomPitch(sfx, volume);
      spiceInSpoon.gameObject.SetActive(false);
      spicePouring.SetActive(true);
    }

    public void CompletePourSpice()
    {
      spicePouring.SetActive(false);
      onComplete?.Invoke();
    }
    public void HideSpice()
    {
      spiceInSpoon.gameObject.SetActive(false);
      if (isHideSource) source.gameObject.SetActive(true);
    }
    public void SetupCondition(Func<bool> condition)
    {
      ConditionFunc = condition;
    }
  }
}
