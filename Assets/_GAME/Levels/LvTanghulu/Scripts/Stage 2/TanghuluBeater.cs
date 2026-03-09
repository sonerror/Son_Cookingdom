using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Tanghulu
{
  public class TanghuluBeater : MonoBehaviour
  {
    [SerializeField] Transform tfRoot;
    [SerializeField] private Collider2D coll2D;
    [SerializeField] private GameObject sprite;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private int number;
    [SerializeField] private float speed = 2.5f;
    public UnityEvent draggingEvent, oneLapEvent, completeEvent;

    Vector2 mouseStarPos;
    int count = 0;
    float deltaAngle = 0, rate;
    private bool _isDone;

    public void Active()
    {
      transform.Appear();
      sprite.SetActive(true);
      coll2D.enabled = true;

      count = 0;
      deltaAngle = 0;
      rate = 0;

      _isDone = false;
    }
    private void Update()
    {
      transform.eulerAngles = Vector3.zero;
    }

    private void OnMouseDrag()
    {
      if (!LevelBase.Ins.IsAllowInteract || _isDone) return;

      Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
      float angle = GetAngleABC(transform.position, tfRoot.position, pos);

      float z = Mathf.Lerp(tfRoot.eulerAngles.z, tfRoot.eulerAngles.z + angle, Time.deltaTime * speed);

      float offset = Mathf.Abs(z - tfRoot.eulerAngles.z);
      deltaAngle += offset;

      tfRoot.eulerAngles = new Vector3(0, 0, z);

      if (deltaAngle >= 360f)
      {
        count++;
        deltaAngle %= 360f;
        if (count >= number)
        {
          _isDone = true;
          sprite.SetActive(false);
          coll2D.enabled = false;
          audioSource.Stop();
          completeEvent?.Invoke();
        }
        else
        {
          if (!audioSource.isPlaying) audioSource.Play();
        }
        oneLapEvent?.Invoke();
      }
      rate = Mathf.Clamp01((float)count / number);
      if (offset > .1f)
        draggingEvent?.Invoke();
    }

    private void OnMouseUp()
    {
      audioSource.Stop();
    }

    private float GetAngleABC(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
      Vector2 BA = pointA - pointB;
      Vector2 BC = pointC - pointB;

      float cosTheta = Vector2.Dot(BA.normalized, BC.normalized);

      float angleRad = Mathf.Acos(Mathf.Clamp(cosTheta, -1f, 1f));

      float angleDeg = angleRad * Mathf.Rad2Deg;

      float crossZ = BA.x * BC.y - BA.y * BC.x;
      if (crossZ < 0)
      {
        angleDeg = -angleDeg;
      }

      return angleDeg;
    }
  }
}
