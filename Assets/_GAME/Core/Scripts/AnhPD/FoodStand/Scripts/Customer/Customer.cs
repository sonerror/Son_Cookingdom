using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.FoodStall
{
  public class Customer : MonoBehaviour
  {
    public enum State
    {
      Idle = 0,
      Talk = 1,
      Happy = 2,
      Unhappy = 3, //sad or angry
    }

    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private CustomerState[] states;
    [SerializeField] private Vector3 startLocalPosition;
    [ShowInInspector, ReadOnly] private State _state;

    private void Start()
    {
      ChangeState(State.Idle);
      states[(int)State.Talk].OnCompleteAnimationConst = () => ChangeState(State.Idle);
      states[(int)State.Unhappy].OnCompleteAnimationConst = () => ChangeState(State.Idle);
    }

    public void LocalMoveInstant(Vector2 vector)
    {
      transform.localPosition = (Vector3)vector + startLocalPosition;
    }
    public void LocalMove(Vector2 vector, float duration, Action onComplete = null, float delay = 0f)
    {
      transform.DOLocalMove(transform.localPosition + (Vector3)vector, duration)
          .SetEase(Ease.InOutBack)
          .OnComplete(() => onComplete?.Invoke())
          .SetDelay(delay);
      transform.DOPunchScale(Vector3.up * .1f, duration, 3).SetDelay(delay);
    }

    public void ChangeState(State newState, Action onComplete = null)
    {
      _state = newState;
      states[(int)_state].Enter(skeletonAnimation);
      states[(int)_state].OnCompleteAnimation = onComplete;
    }
#if UNITY_EDITOR
    // private void OnValidate()
    // {
    //     if(gameObject.scene.name == null) return;
    //     for (int i = 0; i < states.Length; i++)
    //     {
    //         states[i].SetState((State)i);
    //     }
    // }
    [Button]
    private void Init()
    {
      startLocalPosition = transform.localPosition;
    }

    [Button]
    private void TestState(State state)
    {
      ChangeState(state);
    }
#endif
  }

  [Serializable]
  public class CustomerState
  {
    [ReadOnly][SerializeField] private Customer.State state;
    [SerializeField][SpineAnimation(dataField = "skeletonAnimation")] private string anim;
    [SerializeField] private float timeScale = 1f;
    [SerializeField] private bool isLoopAnim;

    [SerializeField] private AudioClip sfx;
    [SerializeField, Range(0, 1)] private float volume = 1;
    [SerializeField] private bool isRandomPitch;

    public Action OnCompleteAnimationConst, OnCompleteAnimation;

    public void SetState(Customer.State newState)
    {
      state = newState;
    }
    public void Enter(SkeletonAnimation skeletonAnimation)
    {
      if (sfx)
      {
        if (isRandomPitch) { } // AudioManager.PlaySFxRandomPitch(sfx);
        else { }// AudioManager.PlaySFx(sfx);
      }

      if (anim.Length < 1)
      {
        OnCompleteAnimationConst?.Invoke();
        OnCompleteAnimation?.Invoke();
        return;
      }
      skeletonAnimation.timeScale = timeScale;
      TrackEntry track = skeletonAnimation.state.SetAnimation(0, anim, isLoopAnim);
      track.Complete += entry =>
      {
        OnCompleteAnimationConst?.Invoke();
        OnCompleteAnimation?.Invoke();
      };
    }
  }
}
