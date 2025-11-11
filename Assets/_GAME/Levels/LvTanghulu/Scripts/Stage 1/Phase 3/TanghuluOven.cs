using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HoangHH;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace AnhPD.Tanghulu
{
  public class TanghuluOven : MonoBehaviour
  {
    [SerializeField] private GameObject open, close;
    // [SerializeField] private ClockTimer clock;
    [SerializeField] private AudioClip sfxOpen, sfxDone;
    [SerializeField] private Transform bowl;
    [SerializeField] private SortingGroup bowlSortingGroup;
    [SerializeField] private SpriteRenderer melt;

    private bool _isOpen, _isReady, _isBaked, _isHaveObject;
    public UnityEvent onOpen, onClose, onBake, onBakedOpen;

    private void Start()
    {
      _isReady = true;
    }
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || !_isReady) return;
      // AudioManager.PlaySFX(sfxOpen);
      _isReady = false;
      _isOpen = !_isOpen;
      open.SetActive(_isOpen);
      close.SetActive(!_isOpen);
      if (_isOpen)
      {
        if (_isBaked)
        {
          onBakedOpen?.Invoke();
          OnComplete();
        }
        else onOpen?.Invoke();
      }
      else
      {
        onClose?.Invoke();
      }
    }

    public void OnPutBowlIn()
    {
      bowl.Appear();
      _isReady = true;
      _isHaveObject = true;
    }

    private void OnComplete()
    {
      bowl.DOMoveY(bowl.position.y - .65f, .3f).SetDelay(.2f).OnStart(() =>
      {
        bowlSortingGroup.sortingOrder = 20;
      });
      bowl.MoveX(5f, 1, true, .5f);
    }

    public void CheckBake()
    {
      if (!_isOpen && _isHaveObject && !_isBaked) Bake();
    }
    private void Bake()
    {
      _isBaked = true;

      onBake?.Invoke();
      // clock.Show(5f);
      melt.DOFade(1, 5f);

      transform.DOShakePosition(5f, .03f, 300).OnComplete(() =>
      {
        // AudioManager.PlaySFX(sfxDone);
        _isReady = true;
      });


    }
  }
}
