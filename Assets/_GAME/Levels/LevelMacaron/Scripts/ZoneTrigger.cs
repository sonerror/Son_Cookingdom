
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
public class ZoneTrigger : GameUnit
{
    [SerializeField] private bool canBlockTrigger = false;

    [SerializeField] private Collider2D triggerWith;
    [SerializeField] private Collider2D col;
    public Collider2D Col => col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private UnityEvent onTriggerEvent;

    [SerializeField] private UnityEvent onTriggerEventFail;
    public UnityEvent OnTriggerEventFail => onTriggerEventFail;
    [SerializeField] private bool disableTriggerWith;
    private bool _isDone;
    public UnityEvent OnTriggerEvent => onTriggerEvent;
    public Rigidbody2D Rigidbody => rb;
    public bool IsDone => _isDone;

    public void EnableCol(bool value)
    {
        col.enabled = value;
    }

    public void ReEnable(Collider2D newTriggerWith)
    {

        triggerWith = newTriggerWith;
        ReEnable();
    }

    public void ReEnable()
    {
        _isDone = false;
        col.enabled = true;
        rb.simulated = true;
    }

    public void Disable(bool removePersistentEvent = true)
    {
        _isDone = true;
        col.enabled = false;
        rb.simulated = false;
        if (removePersistentEvent) onTriggerEvent.RemoveAllListeners();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canBlockTrigger) return;
        if (other == triggerWith)
        {
            onTriggerEvent?.Invoke();
        }

    }
    public void OnTigger()
    {
        if (_isDone) return;
        _isDone = true;
        col.enabled = false;
        rb.simulated = false;
        if (disableTriggerWith) triggerWith.enabled = false;
        onTriggerEvent?.Invoke();
    }
    public void AddTriggerEvent(UnityAction action)
    {
        onTriggerEvent.AddListener(action);
    }

    public void RemoveTriggerEvent(UnityAction action)
    {
        onTriggerEvent.RemoveListener(action);
    }
#if UNITY_EDITOR

  [Sirenix.OdinInspector.Button]
  private void GetRef()
  {
    col = GetComponent<Collider2D>();
    rb = GetComponent<Rigidbody2D>();
  }

#endif
}
