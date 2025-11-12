using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class AppearEffect : MonoBehaviour
  {
    public enum AppearType
    {
      Appear = 0,
      FallAppear = 1,
      FallAppearAndMoveRight = 2,
      AppearFromRight = 3,
      AppearFromLeft = 4,
      FadeIn = 5,
      TossAppear = 6,
      FallAppearLocal = 7,
      AppearVertical = 8,
    }
    [ShowIf("@type == AppearType.AppearFromLeft || type == AppearType.AppearFromRight " +
            "|| type == AppearType.FallAppearAndMoveRight" +
            "|| type == AppearType.AppearVertical")]
    public float distance = 5f;

    [ShowIf("@type == AppearType.FadeIn")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public AppearType type;
    public bool isPlayOnEnable = true;

    [ShowIf("@type == AppearType.AppearFromLeft || type == AppearType.AppearFromRight" +
            "|| type == AppearType.AppearVertical")]
    public UnityEvent onAppeared;
    private void OnEnable()
    {
      if (!isPlayOnEnable) return;
      PlayEffect();
    }

    public void PlayEffect()
    {
      switch (type)
      {
        case AppearType.Appear:
          transform.Appear();
          return;
        case AppearType.FallAppear:
          transform.FallAppear();
          return;
        case AppearType.FallAppearAndMoveRight:
          transform.FallAppear(.1f, .5f, .2f, () => { transform.MoveX(distance); }, 0);
          return;
        case AppearType.AppearFromRight:
          transform.localPosition += Vector3.right * distance;
          transform.MoveX(-distance, 1f, false, 0, () => onAppeared?.Invoke());
          return;
        case AppearType.AppearFromLeft:
          transform.localPosition += Vector3.left * distance;
          transform.MoveX(distance, 1f, false, 0, () => onAppeared?.Invoke());
          return;
        case AppearType.FadeIn:
          spriteRenderer.SetAlpha(0);
          spriteRenderer.DOFade(1, 1);
          return;
        case AppearType.TossAppear:
          transform.TossAppear();
          return;
        case AppearType.FallAppearLocal:
          transform.FallAppearLocal();
          return;
        case AppearType.AppearVertical:
          transform.localPosition += Vector3.up * distance;
          transform.MoveY(-distance, 1f, false, 0, () => onAppeared?.Invoke());
          return;
      }
    }
  }
}

