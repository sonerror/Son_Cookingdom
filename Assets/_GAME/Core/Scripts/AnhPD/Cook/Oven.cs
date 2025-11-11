using HoangHH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
  public class Oven : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer open, close;
    [SerializeField] protected Collider2D coll2D;
    // [SerializeField] protected ClockTimer clocker;
    [SerializeField] protected OvenButton button;
    [SerializeField] protected ParticleSystem vfxSmoke;
    [SerializeField] protected AudioClip sfxOpen;

    public bool IsOpen { private set; get; }
    public bool IsReady;
    protected bool IsBaked;
    protected virtual void Open()
    {
      // AudioManager.PlaySFX(sfxOpen);
      open.enabled = true;
      close.enabled = false;

      if (IsBaked)
        vfxSmoke.Play();
      IsBaked = false;
    }
    protected virtual void Close()
    {
      // AudioManager.PlaySFX(sfxOpen);
      open.enabled = false;
      close.enabled = true;
    }
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || !IsReady) return;
      IsReady = false;
      if (IsOpen)
      {
        Close();
        IsOpen = false;
      }
      else
      {
        Open();
        IsOpen = true;
      }
    }
  }
}

