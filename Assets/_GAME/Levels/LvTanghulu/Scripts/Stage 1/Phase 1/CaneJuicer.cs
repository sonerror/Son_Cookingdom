using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using DG.Tweening;
using HoangHH;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class CaneJuicer : MonoBehaviour
  {
    [SerializeField] private GameObject[] cane;
    [SerializeField] private APDv2Drag lidDrag;
    [SerializeField] private GameObject juicePouring;
    [SerializeField] private SpriteRenderer caneScum;
    // [SerializeField] private ClockTimer clock;
    private int _caneCount;
    private bool _isReady;

    public Action OnFullCane;
    public Action OnComplete;

    public void OnPutCaneIn()
    {
      cane[_caneCount].SetActive(true);
      _caneCount++;
      if (_caneCount == cane.Length)
      {
        OnFullCane?.Invoke();
        lidDrag.OnReady();
      }
    }

    public void OnReady()
    {
      _isReady = true;
    }

    public void OnClick()
    {
      if (!LevelBase.Ins.IsAllowInteract || !_isReady) return;
      _isReady = false;
      juicePouring.SetActive(true);
      caneScum.DOFade(1f, 5f);
      // clock.Show(5f);
      transform.DOShakePosition(5f, Vector3.one * .01f, 500).OnComplete(() =>
      {
        juicePouring.SetActive(false);
        OnComplete?.Invoke();
      });
    }
  }
}