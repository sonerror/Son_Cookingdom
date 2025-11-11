using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class OnReadyButton : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer spriteOn, spriteOff;
    [SerializeField] private AudioClip sfxClick;
    private bool IsReady { get; set; }

    private bool _isOn = false;

    public UnityEvent onTurnOn, onTurnOff;

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || !IsReady) return;
      // AudioManager.PlaySFX(sfxClick);
      IsReady = false;
      _isOn = !_isOn;
      if (_isOn) onTurnOn?.Invoke();
      else onTurnOff?.Invoke();

      spriteOn.enabled = _isOn;
      spriteOff.enabled = !_isOn;
    }

    public void OnReady()
    {
      IsReady = true;
    }
  }
}
