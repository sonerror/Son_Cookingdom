using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class RollingPinV2 : MonoBehaviour
  {
    [FoldoutGroup("References")][SerializeField] private Collider2D coll2D;
    [FoldoutGroup("References")][SerializeField] private GameObject sfx;

    [FoldoutGroup("Paramaters")][SerializeField] private float speed = 2f, timer = 0f, duration = 2f, rate_validRange = .5f;
    [FoldoutGroup("Paramaters")][SerializeField] private RollAxis axis = RollAxis.Horizontal;

    public UnityEvent rollingEvent, completeEvent;
    public float DistanceRate => centerDistanceRate;
    public float DurationRate => Mathf.Clamp01(timer / duration);

    public enum RollAxis { Horizontal, Vertical };

    private Vector2 center;
    private float maxDistance, centerDistanceRate;
    private Transform obj_left, obj_right;

    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      sfx.SetActive(true);
    }

    private void OnMouseDrag()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      Vector3 targetPosition = transform.position;

      if (axis == RollAxis.Horizontal)
      {
        float x = Mathf.Lerp(transform.position.x, mousePos.x, speed * Time.deltaTime);
        x = Mathf.Clamp(x, obj_left.position.x, obj_right.position.x);
        targetPosition.x = x;
      }
      else // Vertical
      {
        float y = Mathf.Lerp(transform.position.y, mousePos.y, speed * Time.deltaTime);
        y = Mathf.Clamp(y, obj_left.position.y, obj_right.position.y);
        targetPosition.y = y;
      }

      transform.position = targetPosition;

      // Optional: rate calculation (cho animation, fx, etc.)
      if (axis == RollAxis.Horizontal && (targetPosition.x < obj_left.position.x || targetPosition.x > obj_right.position.x)) return;
      if (axis == RollAxis.Vertical && (targetPosition.y < obj_left.position.y || targetPosition.y > obj_right.position.y)) return;

      float distance = Vector2.Distance(transform.position, center);
      centerDistanceRate = distance / maxDistance;

      rollingEvent?.Invoke();

      if (centerDistanceRate > rate_validRange) return; // Prevents from triggering too early or too late

      timer += Time.deltaTime;
      if (timer >= duration)
      {
        coll2D.enabled = false;
        gameObject.SetActive(false);
        completeEvent?.Invoke();
      }
    }
    private void OnMouseUp()
    {
      sfx.SetActive(false);
    }
    public void Init(Transform start, Transform end, RollAxis axis = RollAxis.Vertical, float duration = 2f)
    {
      transform.Appear();
      coll2D.enabled = false;
      obj_left = start;
      obj_right = end;

      timer = 0f;
      this.duration = duration;

      transform.DOMove(start.position, .3f);
      this.axis = axis;
      transform.DORotate(new Vector3(0, 0, axis == RollAxis.Vertical ? 0 : -90f), .3f).OnComplete(() =>
      {
        coll2D.enabled = true;
      });

      maxDistance = Vector2.Distance(obj_left.position, obj_right.position) / 2;
      center = (obj_left.position + obj_right.position) / 2;
    }
    private void OnDisable()
    {
      sfx.SetActive(false);
    }
  }
}
