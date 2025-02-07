using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private float rewindDuration = 1f;
    private Vector2 startOffset;
    private Vector3 startPos;

    private bool isDragging = false;
    private float startZ;
    [SerializeField] private float deltaZ = 0f;
    [SerializeField] private AudioClip sfxPick;

    public bool
        isActive = true,
        isFrezee = false,
        isFlexSortingOrder = false,
        isRewind = true,
        isFlexRewindDurationByDistance = false,
        isScaleUp = true,
        isRotate = true;

    public UnityEvent startDragEvents, draggingEvents, endDragEvents, completeRewindEvents;

    public int moveOrder, unmoveOrder;
    public SpriteRenderer[] render;

    private LevelBase _level;

    private void Awake()
    {
        ResetStartPos();
    }

    public void ResetStartPos()
    {
        startPos = transform.position;
        startZ = transform.eulerAngles.z;
    }

    private void Start()
    {
        SetUnmoveOrder();
    }

    private void OnMouseDown()
    {
        if (!isActive || isFrezee) return;

        // AudioManager.PlaySFX(sfxPick);

        SetMoveOrder();

        transform.DOKill();

        if (isScaleUp)
        {
            transform.localScale = Vector3.one * 1.05f;
        }
        startOffset = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        isDragging = true;

        if (isRotate)
        {
            transform.DORotate(new Vector3(transform.eulerAngles.x, 0, startZ + deltaZ), .2f);
        }

        startDragEvents?.Invoke();
    }

    private void OnMouseDrag()
    {
        if (isFrezee) return;

        if (isDragging)
        {
            if (!isActive)
            {
                CancelDragging();
            }
            ;

            Vector2 pos2d = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startOffset;

            transform.position = new Vector3(pos2d.x, pos2d.y, transform.position.z);
            draggingEvents?.Invoke();
        }
    }

    private void OnMouseUp()
    {
        if (!isActive || isFrezee) return;

        if (isDragging)
        {
            float rate = 1;
            if (isFlexRewindDurationByDistance)
            {
                rate = (Vector2.Distance(transform.position, startPos) / 2f);
            }

            transform.localScale = Vector3.one;
            if (isRewind)
            {
                transform.DOMove(startPos, rewindDuration * rate).OnComplete(() =>
                {
                    SetUnmoveOrder();
                    completeRewindEvents?.Invoke();
                });
            }
            else
            {
                SetUnmoveOrder();
                completeRewindEvents?.Invoke();
            }
            isDragging = false;
            if (isRotate)
            {
                transform.DORotate(new Vector3(transform.eulerAngles.x, 0, startZ), .2f);
            }
            endDragEvents?.Invoke();
        }
    }

    public void CancelDragging()
    {
        isDragging = true;
        OnMouseUp();
    }
    public void SetUnmoveOrder()
    {
        if (isFlexSortingOrder)
        {
            SetOrderLayer(unmoveOrder);
        }
    }
    public void SetMoveOrder()
    {
        if (isFlexSortingOrder)
        {
            SetOrderLayer(moveOrder);
        }
    }
    public void LockPosition()
    {
        isFrezee = true;
        isActive = false;
        transform.DOKill();
        SetUnmoveOrder();
    }
    public void UnlockPosition()
    {
        isFrezee = false;
        isActive = true;
        CancelDragging();
    }
    public void SetNewStart(Vector2 pos, float rotation)
    {
        startPos = pos;
        startZ = rotation;
    }
    public void SetOrderLayer(int order)
    {
        for (int i = 0; i < render.Length; i++)
        {
            render[i].sortingOrder = order;
        }
    }
    public void UpdateOffset()
    {
        startOffset = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
}
