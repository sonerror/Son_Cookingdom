using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace AnhPD.Cook
{
  public class DoorSlamTransition : MonoBehaviour
  {
    [SerializeField] private Transform left, right;
    [SerializeField] private AudioClip sfxStartClose, sfxEndClose;
    [SerializeField] private float duration = .5f, stayCloseDuration = 1f;

    [SerializeField] private bool isRotate, isFixedStartRotate;

    private float _leftStartX, _rightStartX;
    private Action onCover, onComplete;
    public bool isPlaySoundOpen = true, isPlaySoundClose = true;
    private void Start()
    {
      _leftStartX = left.localPosition.x;
      _rightStartX = right.localPosition.x;
    }
    public DoorSlamTransition OnCover(Action action)
    {
      onCover += action;
      return this;
    }

    public DoorSlamTransition OnComplete(Action action)
    {
      onComplete += action;
      return this;
    }

    public DoorSlamTransition ClearAction()
    {
      onComplete = null;
      onCover = null;
      return this;
    }
    [Button]
    public DoorSlamTransition StartTransition(float delay = 0f)
    {
      gameObject.SetActive(true);
      if (isFixedStartRotate) transform.eulerAngles = Vector3.zero;
      Close();
      return this;

      void Close()
      {
        left.DOLocalMoveX(0, duration).SetEase(Ease.Linear).SetDelay(delay)
            .OnStart(() =>
            {
              if (isPlaySoundClose) { } // AudioManager.PlaySFX(sfxStartClose);

            });

        right.DOLocalMoveX(0, duration).SetEase(Ease.Linear).SetDelay(delay)
            .OnComplete(() =>
            {
              // AudioManager.PlaySFX(sfxEndClose);
              onCover?.Invoke();

              if (isRotate)
              {
                transform.DORotate(new Vector3(0, 0, transform.eulerAngles.z + 90f * (UnityEngine.Random.Range(0, 2) * 2 - 1)), stayCloseDuration * .7f).SetDelay(stayCloseDuration * .1f);
              }
              Open();
            });
      }

      void Open()
      {
        left.DOLocalMoveX(_leftStartX, duration).SetEase(Ease.Linear).SetDelay(stayCloseDuration)
            .OnStart(() =>
            {
              if (isPlaySoundOpen) { }// AudioManager.PlaySFX(sfxStartClose);

            });

        right.DOLocalMoveX(_rightStartX, duration).SetEase(Ease.Linear).SetDelay(stayCloseDuration)
            .OnComplete(() =>
            {
              onComplete?.Invoke();
              gameObject.SetActive(false);
            });
      }
    }
  }
}
