using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
  public class RollingPin : MonoBehaviour
  {
    [SerializeField] private GameObject sfx;
    [SerializeField] private Transform left, right;
    [SerializeField] private float speed = 2f;

    private Vector2 center;
    private float maxDistance;
    private Transform obj_left, obj_right;

    public UnityEvent onRolling;

    private void Start()
    {
      obj_left = left;
      obj_right = right;
      maxDistance = Vector2.Distance(obj_left.position, obj_right.position) / 2;
      center = (obj_left.position + obj_right.position) / 2;
    }
    private void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      sfx.SetActive(true);
    }

    private void OnMouseDrag()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;
      Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
      float y = transform.position.y;
      float x = Mathf.Lerp(transform.position.x, mousePos.x, speed * Time.deltaTime);
      x = Mathf.Clamp(x, left.transform.position.x, right.transform.position.x);

      transform.position = new Vector3(x, y);

      if (x < obj_left.position.x || x > obj_right.position.x) return;
      float rate = Vector2.Distance(transform.position, center) / maxDistance;
      onRolling.Invoke();
    }
    private void OnMouseUp()
    {
      TurnOffSfx();
    }
    private void TurnOffSfx()
    {
      sfx.SetActive(false);
    }
    private void OnDisable()
    {
      TurnOffSfx();
    }
  }
}
