using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class LiddedAppliance : MonoBehaviour
  {
    [SerializeField] private GameObject lidOpen, lidClose;
    [SerializeField] private AudioClip sfxOpen;
    public UnityEvent onOpen, onClose;

    public bool isReadyOnEnable;
    public bool isOpen;
    private bool _isReady;

    private void OnEnable()
    {
      if (isReadyOnEnable) OnReady();
    }

    public void OnReady()
    {
      _isReady = true;
    }

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || !_isReady) return;
      // AudioManager.PlaySFx(sfxOpen);

      _isReady = false;
      isOpen = !isOpen;

      if (isOpen) Open();
      else Close();

      UpdateLid();
    }

    protected virtual void Open()
    {
      onOpen?.Invoke();
    }

    protected virtual void Close()
    {
      onClose?.Invoke();
    }

    [Button]
    private void UpdateLid()
    {
      lidOpen.SetActive(isOpen);
      lidClose.SetActive(!isOpen);
    }
  }
}
