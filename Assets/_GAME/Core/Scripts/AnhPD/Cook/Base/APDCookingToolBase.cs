using DG.Tweening;

using Satisgame;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AnhPD.Cook
{
  public class APDCookingToolBase : MonoBehaviour
  {
    [SerializeField] protected Collider2D coll2D;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [SerializeField] protected AudioClip sfxPick;
    [SerializeField] protected AudioClip sfxPlace;

    [SerializeField] protected Vector3 sizeInit;
    [SerializeField] protected Vector3 sizeInCrease;
    [SerializeField] protected int minLayer = 2;
    [SerializeField] protected float dropDistance = 1f;
    [SerializeField] protected float minY = -4f;
    [SerializeField] protected float rewindDuration = 0.5f;

    [SerializeField] protected Vector2 startPos;
    [SerializeField] protected Vector3 startRotation;
    [SerializeField] protected Shadow shadow;
    [SerializeField] private bool IsEnableMouseUpWarning = false;

    [ShowInInspector] public bool IsReady;

    [SerializeField] protected SpriteRenderer[] parts;

    protected bool isDragging = false, isWarned;

    private Transform tf;
    public Transform Tf => tf ? tf : tf = transform;

    protected Vector3 mOffset;

    protected EmojiControl emoji => (APDLevelBase.Ins as APDLevelBase).emoji;
    protected int maxLayer => APDLevelBase.maxLayer;

    protected Action pickupAction, landAction;

    protected virtual void Start()
    {
      if (shadow != null)
      {
        pickupAction += shadow.OnPickUp;
        landAction += shadow.OnPutDown;
      }
    }

    [Sirenix.OdinInspector.Button]
    public virtual void InitProperties()
    {
      coll2D = GetComponent<Collider2D>();
      spriteRenderer = GetComponentInChildren<SpriteRenderer>();

#if UNITY_EDITOR
      if (coll2D == null) AddBoxCollider();
      SetCurrentMinLayer();
#endif

      sizeInit = transform.localScale;
      sizeInCrease = transform.localScale * 1.1f;

      startPos = Tf.position;
      startRotation = Tf.eulerAngles;
    }

    protected virtual Vector3 GetMouseWorldPos()
    {
      Vector3 mousePoint = Input.mousePosition;
      return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    protected virtual void MouseDown(BaseEventData eventData)
    {
      // AudioManager.PlaySFxRandomPitch(sfxPick);
      mOffset = Tf.position - GetMouseWorldPos();

      Tf.DOKill();
      Tf.DORotate(startRotation, 0.15f);
      Tf.DOScale(sizeInCrease, 0.15f);

      spriteRenderer.sortingOrder = maxLayer;
      for (int i = 0; i < parts.Length; i++)
      {
        parts[i].sortingOrder = spriteRenderer.sortingOrder;
      }
      isDragging = true;
      isWarned = false;

      pickupAction?.Invoke();

      StopAllCoroutines();
      DelayWarning();
    }

    protected virtual void MouseUp(BaseEventData eventData)
    {
      if (!isDragging) return;
      if (!IsReady)
      {
        OnIncorrectUse();
        MouseUpWarning();
      }

      Rewind();
    }

    protected virtual void MouseDrag(BaseEventData eventData)
    {
      if (!isDragging || !LevelBase.Ins.IsAllowInteract) return;

      Vector3 pos = GetMouseWorldPos() + mOffset;
      Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
      Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
      pos = new Vector3(
          Mathf.Clamp(pos.x, minScreenBounds.x, maxScreenBounds.x),
          Mathf.Clamp(pos.y, minY, maxScreenBounds.y), 0
      );
      Tf.position = (Vector2)pos;
    }

    protected virtual void Rewind(Action completeAction = null)
    {
      isDragging = false;
      float duration = rewindDuration;

      float distance = Vector2.Distance(Tf.position, startPos);
      if (distance < 1f)
      {
        duration = rewindDuration * (distance / 1f);
      }
      if (Vector2.Distance(tf.position, startPos) < .2f)
      {
        duration = .1f;
      }
      Tf.DOMove(startPos, duration).OnComplete(() =>
      {
        // AudioManager.PlaySFxRandomPitch(sfxPlace);
        // MMVibrationManager.Haptic(HapticTypes.Selection);
        spriteRenderer.sortingOrder = minLayer;
        for (int i = 0; i < parts.Length; i++)
        {
          parts[i].sortingOrder = spriteRenderer.sortingOrder;
        }
        completeAction?.Invoke();
        landAction?.Invoke();
      });
      Tf.DOScale(sizeInit, 0.3f);
      Tf.DORotate(startRotation, .3f);
    }
    protected void DelayWarning()
    {
      if (!IsEnableMouseUpWarning) return;
      StartCoroutine(delay());
    }
    private IEnumerator delay()
    {
      yield return new WaitForSeconds(1f);
      if (isDragging && !isWarned && !IsReady)
      {
        emoji.ShowNegative();
        isWarned = true;
      }
    }
    protected void MouseUpWarning()
    {
      if (!isWarned && IsEnableMouseUpWarning)
      {
        emoji.ShowNegative();
        isWarned = true;
      }
    }
    public virtual void OnReady()
    {
      IsReady = true;
    }
    public virtual void OnComplete()
    {
      gameObject.SetActive(true);
      isDragging = false;
      IsReady = false;
      Rewind();
    }
    public virtual void CompleteWithAction(Action action)
    {
      gameObject.SetActive(true);
      isDragging = false;
      IsReady = false;
      Rewind(action);
    }
    protected bool IsInRange(Transform target, float rangeRation = 1f)
    {
      return (Vector2.Distance(Tf.position, target.position) < dropDistance * rangeRation);
    }
    protected bool IsInRange(Vector3 target, float rangeRation = 1f)
    {
      return (Vector2.Distance(Tf.position, target) < dropDistance * rangeRation);
    }
    protected bool IsInRange(Vector3 pos, Vector3 target, float rangeRation = 1f)
    {
      return (Vector2.Distance(pos, target) < dropDistance * rangeRation);
    }
    public void EnableCollider(bool isEnable = true)
    {
      coll2D.enabled = isEnable;
    }
    public void EnableSprite(bool isEnable = true)
    {
      spriteRenderer.enabled = isEnable;
    }
    private void OnDisable()
    {
      StopAllCoroutines();
    }
    public bool IsHalfHeart = false;
    protected virtual void OnIncorrectUse()
    {
      if (Vector2.Distance(Tf.position, startPos) > APDCookConfig.INCORRECT_SAFE_DISTANCE)
      {
        if (IsHalfHeart)
        {
          // LevelBase.Ins.LoseHalfHeart(Tf.position);
        }
        // else LevelBase.Ins.LoseFullHeart(Tf.position);
      }
    }
    protected void OnWrong()
    {
      OnIncorrectUse();
    }
    public virtual void OnReReady(bool isReady = true)
    {
      spriteRenderer.sortingOrder = minLayer;
      for (int i = 0; i < parts.Length; i++)
      {
        parts[i].sortingOrder = spriteRenderer.sortingOrder;
      }

      Tf.position = startPos;
      if (!gameObject.activeSelf)
        Tf.Appear();
      IsReady = isReady;
      Tf.eulerAngles = startRotation;
    }

    public void MoveToStartPos()
    {
      Tf.position = startPos;
      Tf.localScale = sizeInit;
    }
#if UNITY_EDITOR

    [Sirenix.OdinInspector.Button]
    private void SetUpEventTrigger()
    {
      var eventTrigger = gameObject.AddComponent<EventTrigger>();
      AddEventTriggerEntry(EventTriggerType.PointerDown, MouseDown);
      AddEventTriggerEntry(EventTriggerType.PointerUp, MouseUp);
      AddEventTriggerEntry(EventTriggerType.Drag, MouseDrag);

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

    [Sirenix.OdinInspector.Button]
    private void SetCurrentMinLayer()
    {
      minLayer = spriteRenderer.sortingOrder;
    }
    [HorizontalGroup("Add")]
    [Sirenix.OdinInspector.Button]
    private void AddShadow()
    {
      if (shadow != null) return;
      gameObject.AddComponent<CreateShadowObject>().CreateShadown();
      shadow = gameObject.AddComponent<Shadow>();
      SpriteRenderer[] spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
      foreach (var sr in spriteRenderers)
      {
        if (sr.name == "shadow")
        {
          shadow.SetSpriteRenderer(sr);
          return;
        }
      }
    }
    [HorizontalGroup("Add")]
    [Button]
    private void AddBoxCollider()
    {
      if (coll2D != null) return;
      coll2D = gameObject.AddComponent<BoxCollider2D>();
      coll2D.isTrigger = true;
    }
    [Button]
    private void GetAllSprites()
    {
      parts = GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.DrawWireSphere(Tf.position, APDCookConfig.INCORRECT_SAFE_DISTANCE);
    }
#endif
  }
}
