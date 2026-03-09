using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
  public class AppearSfx : MonoBehaviour
  {
    [SerializeField] private AudioClip sfx;
    [SerializeField, Range(0, 1)] private float volume = 1f;
    public bool isPlayOnEnable = true;

    private void OnEnable()
    {
      if (!isPlayOnEnable) return;
      PlaySound();
    }

    public void PlaySound()
    {
      if (sfx != null)
      {
        // AudioManager.PlaySFX(sfx, volume);
      }
    }
  }
}

