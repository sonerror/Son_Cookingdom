using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Tanghulu
{
  public class Sickle : MonoBehaviour
  {
    [SerializeField] private Sugarcane[] sugarcane;
    [SerializeField] private Vector3 offset = new Vector3(1f, .5f, 0f);

    private int _index;
    private bool _isCutting;
    private Vector3 _startPosition;

    public UnityEvent onComplete;

    private void OnEnable()
    {
      InitPos();
    }

    private void InitPos()
    {
      if (_index >= sugarcane.Length) return;
      transform.Appear();
      transform.position = sugarcane[_index].transform.position + offset;
      _startPosition = transform.position;
    }

    private void OnCut()
    {
      _isCutting = true;
      transform.DOMove(sugarcane[_index].GetMaskPosition(), .3f).SetEase(Ease.InBack).OnComplete(() =>
      {
        sugarcane[_index].OnCut();
        if (sugarcane[_index].isComplete)
        {
          gameObject.SetActive(false);
          _index++;
          if (_index < sugarcane.Length) onComplete?.Invoke();
        }
      });
      transform.DOMove(_startPosition, .3f).SetEase(Ease.OutBack).SetDelay(.3f).OnComplete(() =>
      {
        _isCutting = false;
      });
    }

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || _isCutting) return;
      OnCut();
    }
  }
}
