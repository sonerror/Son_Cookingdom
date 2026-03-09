using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.CookV2
{
  public class ScoopSphereSpoon : MonoBehaviour
  {
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private Transform tfSpoon;
    public UnityEvent onScoop;

    private bool _isScooping;

    public void MoveToPos(Vector3 pos)
    {
      coll2D.enabled = false;
      transform.DOMove(pos, .1f).OnComplete(() =>
      {
        coll2D.enabled = true;
      });
    }
    private float _startDistance;
    private void OnMouseDown()
    {
      if (_isScooping || !LevelBase.Ins.IsAllowInteract) return;
      _isScooping = true;
      coll2D.enabled = false;
      tfSpoon.DOPunchRotation(new Vector3(0, transform.eulerAngles.y, 30f), .15f, 5).OnComplete(() =>
      {
        _isScooping = false;
        onScoop?.Invoke();
      });
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
