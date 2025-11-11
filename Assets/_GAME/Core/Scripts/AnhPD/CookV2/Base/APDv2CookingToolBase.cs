using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.Cook;
using DG.Tweening;

using Satisgame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace AnhPD.CookV2
{
  public class APDv2CookingToolBase : MonoBehaviour
  {
    #region Properties

    [FoldoutGroup("Base Config")]
    [Header("References")]
    [FoldoutGroup("Base Config")][SerializeField] protected Collider2D coll2D;
    [FoldoutGroup("Base Config")][SerializeField] protected SpriteRenderer spriteRenderer;
    [FoldoutGroup("Base Config")][SerializeField] protected SortingGroup sortingGroup;
    [FoldoutGroup("Base Config")][SerializeField] protected Shadow shadow;

    [FoldoutGroup("Base Config")][SerializeField] protected AudioClip sfxPick;
    [FoldoutGroup("Base Config")][SerializeField] protected AudioClip sfxPlace;

    [FoldoutGroup("Base Config")]
    [Header("Parameters")]
    [FoldoutGroup("Base Config")][SerializeField] protected Vector3 sizeInit;
    [FoldoutGroup("Base Config")][SerializeField] protected Vector3 sizeInCrease;
    [FoldoutGroup("Base Config")][SerializeField] protected Vector2 startPos;
    [FoldoutGroup("Base Config")][SerializeField] protected Vector3 startRotation;

    [FoldoutGroup("Base Config")][SerializeField] protected int minLayer = 2;
    [FoldoutGroup("Base Config")][SerializeField] protected float dropDistance = 1f;
    [FoldoutGroup("Base Config")][SerializeField] protected float minY = -4f;
    [FoldoutGroup("Base Config")][SerializeField] protected float rewindDuration = 0.5f;

    [FoldoutGroup("Base Config")]
    [Header("Bool")]
    [FoldoutGroup("Base Config")] public bool IsReady;
    [FoldoutGroup("Base Config")] public bool IsResetPosOnReReady = true;
    [FoldoutGroup("Base Config")] public bool isGetMouseOffset = true;

    protected bool isDragging = false;

    private Transform tf;
    public Transform Tf => tf ? tf : tf = transform;

    protected Vector3 mOffset;

    protected int maxLayer => APDLevelBase.maxLayer;

    [FoldoutGroup("Event")] public UnityEvent onComplete, onMouseDown, onMouseUp, onRewound, onIncorrectUse;

    #endregion

    #region Initialization

    protected virtual void Start()
    {
      if (shadow)
      {
        onMouseDown.AddListener(shadow.OnPickUp);
        onRewound.AddListener(shadow.OnPutDown);
      }
    }
    [HorizontalGroup("Init")]
    [Button]
    public virtual void InitProperties()
    {
      coll2D = GetComponent<Collider2D>();
      sortingGroup = GetComponent<SortingGroup>();
      spriteRenderer = GetComponentInChildren<SpriteRenderer>();

#if UNITY_EDITOR
      if (!sortingGroup) sortingGroup = gameObject.AddComponent<SortingGroup>();
      if (!coll2D) AddBoxCollider();
      minLayer = spriteRenderer.sortingOrder;
#endif

      sortingGroup.sortingOrder = spriteRenderer.sortingOrder;

      sizeInit = transform.localScale;
      sizeInCrease = transform.localScale * 1.1f;

      startPos = Tf.position;
      startRotation = Tf.eulerAngles;
    }

    #endregion

    #region Mouse Handle

    protected virtual Vector3 GetMouseWorldPos()
    {
      Vector3 mousePoint = Input.mousePosition;
      return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    protected virtual void MouseDown(BaseEventData eventData)
    {
      if (!LevelBase.Ins.IsAllowInteract) return;

      onMouseDown?.Invoke();

      // AudioManager.PlaySFxRandomPitch(sfxPick);
      if (isGetMouseOffset) mOffset = Tf.position - GetMouseWorldPos();
      else ClampPosition();

      Tf.DOKill();
      Tf.DORotate(startRotation, 0.15f);
      Tf.DOScale(sizeInCrease, 0.15f);

      sortingGroup.sortingOrder = maxLayer;

      isDragging = true;
    }

    protected virtual void MouseUp(BaseEventData eventData)
    {
      if (!isDragging || !LevelBase.Ins.IsAllowInteract) return;
      onMouseUp?.Invoke();
      Rewind();
    }

    protected virtual void MouseDrag(BaseEventData eventData)
    {
      if (!isDragging || !LevelBase.Ins.IsAllowInteract) return;

      ClampPosition();
    }

    protected void ClampPosition()
    {
      Vector3 pos = GetMouseWorldPos() + mOffset;
      Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
      Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
      pos = new Vector3(
          Mathf.Clamp(pos.x, minScreenBounds.x, maxScreenBounds.x),
          Mathf.Clamp(pos.y, minY, maxScreenBounds.y), 0
      );
      Tf.position = (Vector2)pos;
    }

    #endregion

    #region Action
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
        if (sfxPlace && gameObject.activeSelf) // AudioManager.PlaySFxRandomPitch(sfxPlace);
                                               // MMVibrationManager.Haptic(HapticTypes.Selection);
          sortingGroup.sortingOrder = minLayer;

        onRewound?.Invoke();
        completeAction?.Invoke();
      });
      Tf.DOScale(sizeInit, 0.3f);
      Tf.DORotate(startRotation, .3f);
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
    protected virtual void OnIncorrectUse()
    {
      // LevelBase.Ins.LoseFullHeart(Tf.position);
      onIncorrectUse?.Invoke();
    }
    public virtual void OnReReady(bool isReady = true)
    {
      isDragging = false;
      sortingGroup.sortingOrder = minLayer;

      if (IsResetPosOnReReady)
      {
        Tf.position = startPos;
        Tf.eulerAngles = startRotation;
      }
      if (!gameObject.activeSelf)
        Tf.Appear();
      IsReady = isReady;
      Tf.localScale = sizeInit;
    }
    public void EnableCollider(bool isEnable = true)
    {
      coll2D.enabled = isEnable;
    }

    public void ReturnToStartPos()
    {
      gameObject.SetActive(true);
      Rewind();
    }
    #endregion

    #region Utilities

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

    public void SetMaskInteraction(int maskIndex)
    {
      switch (maskIndex)
      {
        case 0:
          spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
          break;
        case 1:
          spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
          break;
        case 2:
          spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
          break;
      }
    }
    #endregion


#if UNITY_EDITOR
    [HorizontalGroup("Init")]
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

    protected virtual void OnDrawGizmosSelected()
    {
      Gizmos.DrawWireSphere(startPos, APDCookConfig.INCORRECT_SAFE_DISTANCE);
    }
#endif
  }
}
