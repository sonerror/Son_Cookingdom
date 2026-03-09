using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.CookV2
{
  public class PluckStemStep : FruitPreparationStep
  {
    [SerializeField] private GameObject stem;
    [SerializeField] private AudioClip sfxPluck;
    private void OnMouseDown()
    {
      // AudioManager.PlaySFX(sfxPluck);
      gameObject.SetActive(false);
      stem.SetActive(true);
      OnComplete();
    }

    protected void MouseDown(BaseEventData eventData)
    {
      OnMouseDown();
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void SetUpEventTrigger()
    {
      var eventTrigger = gameObject.AddComponent<EventTrigger>();
      AddEventTriggerEntry(EventTriggerType.PointerDown, MouseDown);

      void AddEventTriggerEntry(EventTriggerType eventType, UnityAction<BaseEventData> action)
      {
        var entry = new EventTrigger.Entry
        {
          eventID = eventType
        };
        UnityEditor.Events.UnityEventTools.AddPersistentListener(entry.callback, action);
        eventTrigger.triggers.Add(entry);
      }
    }
#endif
  }
}
