using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AnhPD.Cook
{
  public class CleanEffect : MonoBehaviour
  {
    [SerializeField] private AudioClip sfx;
    [SerializeField][Range(0, 1)] private float volume = 1;
    [SerializeField] private float durtation = .5f;

    private void OnEnable()
    {
      // AudioManager.PlaySFX(sfx, volume);
      this.WaitToDo(() =>
      {
        gameObject.SetActive(false);
      }, durtation);
    }
    public void SpawnEffect(Vector3 pos)
    {
      transform.position = pos;
      gameObject.SetActive(true);
    }
  }
}

