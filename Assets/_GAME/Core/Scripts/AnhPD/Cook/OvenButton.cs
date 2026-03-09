using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace AnhPD.Cook
{
  public class OvenButton : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip sfxClick;
    public UnityEvent clickEvent;
    public bool IsReady;
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;

      // AudioManager.PlaySFX(sfxClick);
      spriteRenderer.color = new Color(.6f, .6f, .6f);
      this.WaitToDo(() =>
      {
        spriteRenderer.color = Color.white;
      }, 0.1f);

      if (!IsReady) return;
      IsReady = false;
      clickEvent?.Invoke();
    }
    public void OnReady()
    {
      IsReady = true;
    }
  }
}

