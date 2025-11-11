using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.Cook;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace AnhPD.CookV2
{
  public class APDProgressionStage : MonoBehaviour
  {
    [SerializeField] private APDProgressionPhase[] phases;
    [SerializeField] private APDProgressionTransion transition;
    [SerializeField] private bool isMoveCamera = true;
    [SerializeField] private bool isHideOnComplete = true;
    [SerializeField] private bool isUseEmojiOnComplete;
    [ShowIf("@isMoveCamera")]
    [SerializeField] private Vector3 cameraPosition = new Vector3(0, 0, -10);
    [ShowIf("@isMoveCamera")]
    [SerializeField] private float delayMoveCamera = 1f;

    [SerializeField] private float delayOnComplete = .5f;
    public Action<int> OnCompleteStage;

    public int stageIndex;

    private int _phaseIndex = 0;

    private void Start()
    {
      for (int i = 0; i < phases.Length; i++)
      {
        phases[i].phaseIndex = i;
      }
    }

    public void OnEnter()
    {
      gameObject.SetActive(true);
      if (isMoveCamera)
      {
        Camera.main?.transform.DOMove(cameraPosition, 1f).OnComplete(() =>
        {
          phases[_phaseIndex].OnStart();
        }).SetDelay(delayMoveCamera);
      }
      else
      {
        phases[_phaseIndex].OnStart();
      }
    }

    private void OnComplete()
    {
      if (isUseEmojiOnComplete) { }// APDLevelBase.Ins?.emoji.ShowPositive();
      if (transition)
      {
        transition.OnCover(() =>
            {
              gameObject.SetActive(false);
              OnCompleteStage?.Invoke(stageIndex);
            })
            .SetDelay(delayOnComplete)
            .Play();
        return;
      }
      if (isHideOnComplete) this.WaitToDo(() => gameObject.SetActive(false), delayOnComplete);
      OnCompleteStage?.Invoke(stageIndex);
    }

    private void NextPhase(int phaseIndex)
    {
      _phaseIndex = phaseIndex;
      _phaseIndex++;
      if (_phaseIndex >= phases.Length) OnComplete();
      else phases[_phaseIndex].OnStart();
    }

    public Action<Sprite> OnHintChange;

    public void Register()
    {
      foreach (APDProgressionPhase phase in phases)
      {
        phase.OnHintChange += OnHintChange;
        phase.OnCompleteAction += NextPhase;
      }
    }

    public void Unregister()
    {
      foreach (APDProgressionPhase phase in phases)
      {
        phase.OnHintChange -= OnHintChange;
        phase.OnCompleteAction -= NextPhase;
      }
    }

    //-------------For Test----------------
    public void ForceCompletePhase()
    {
      phases[_phaseIndex].OnComplete();
    }
    //-------------------------------------
  }
}
