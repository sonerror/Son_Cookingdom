using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace AnhPD.CookV2
{
  public class SimpleUseMeshFold : MonoBehaviour
  {
    // [SerializeField] private MeshFoldDraggable meshFoldDraggable;
    [SerializeField] private AudioClip sfxPop;

    public bool isHideAfterComplete;
    public UnityEvent onComplete;

    private void Start()
    {
      // meshFoldDraggable.onDetached += OnMeshFoldDetached;
    }

    // private void OnMeshFoldDetached(MeshFoldDraggable draggable)
    // {
    //   // AudioManager.PlaySFx(sfxPop);
    //   // MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

    //   if (isHideAfterComplete)
    //     meshFoldDraggable.gameObject.SetActive(false);
    //   onComplete?.Invoke();
    // }
  }
}
