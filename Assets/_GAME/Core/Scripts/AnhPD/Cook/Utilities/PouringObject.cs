using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AnhPD.Cook
{
  public class PouringObject : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer sprite_default, sprite_pour, sprite_pour_complete, sprite_liquid_pouring;
    [SerializeField] private float zAngle, fadeTime = .3f;
    [SerializeField] private AudioClip sfxPouring;

    private Transform tf;
    public Transform Tf => tf ? tf : tf = transform;

    public void Pouring(Vector3 pos, Action startAction = null, Action endAction = null)
    {
      Tf.DOMove(pos, fadeTime);
      Tf.DORotate(new Vector3(0, 0, zAngle), fadeTime).OnComplete(start);

      ActiveSprite(sprite_pour);
      sprite_default.DOFade(0, fadeTime);
      sprite_pour.DOFade(1, fadeTime);

      void start()
      {
        startAction?.Invoke();
        // AudioManager.PlaySFX(sfxPouring);
        sprite_liquid_pouring.enabled = true;

        if (sprite_pour_complete != null)
        {
          sprite_pour_complete.enabled = true;
          sprite_pour.DOFade(0, .5f).OnComplete(end);
        }
        else
        {
          this.WaitToDo(end, .5f);
        }
      }
      void end()
      {
        sprite_liquid_pouring.enabled = false;
        endAction?.Invoke();
        if (sprite_pour_complete == null)
        {
          sprite_default.DOFade(1, fadeTime);
          sprite_pour.DOFade(0, fadeTime);
        }
      }

    }
    private void ActiveSprite(SpriteRenderer sprd)
    {
      sprd.enabled = true;
      //sprd.SetAlpha(1);
    }
  }
}

