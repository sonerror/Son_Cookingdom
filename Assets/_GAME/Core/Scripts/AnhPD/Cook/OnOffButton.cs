using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class OnOffButton : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer spr_on, spr_off;
    [SerializeField] private AudioClip sfxClick;

    public bool IsOn { get; private set; } = false;
    public UnityEvent TurnOnEvent, TurnOffEvent;
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      // AudioManager.PlaySFX(sfxClick);

      if (!IsOn)
      {
        TurnOnEvent?.Invoke();
      }
      else
      {
        TurnOffEvent?.Invoke();
      }
      IsOn = !IsOn;
      spr_on.enabled = IsOn;
      spr_off.enabled = !IsOn;
    }
  }
}
