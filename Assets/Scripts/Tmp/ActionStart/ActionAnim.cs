using DG.Tweening;
using UnityEngine;

namespace Link
{
  public class ActionAnim : ActionBase
  {
    // [SerializeField] Animation anim;
    [SerializeField] AnimationClip animName;

    public override void Active()
    {
      gameObject.SetActive(startActive);
      DOVirtual.DelayedCall(delay, () =>
      {
        gameObject.SetActive(true);
        // anim.Play(animName.name);
        DOVirtual.DelayedCall(animName.length, OnDone);
        PlayFx();
      });
    }

    private void OnValidate()
    {
      // anim = GetComponent<Animation>();
      // if (animName != null && anim.GetClip(animName.name) != null)
      // {
      //   anim.AddClip(animName, animName.name);
      // }
    }

    protected override void Setup()
    {
      base.Setup();
      OnValidate();
      if (GetComponent<ItemAlpha>() == null)
      {
        gameObject.AddComponent<ItemAlpha>();
      }
    }
  }
}