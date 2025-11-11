using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class SpeederWhisk : MonoBehaviour
  {
    [FoldoutGroup("References")][SerializeField] private WhiskBowl bowl;
    [FoldoutGroup("References")][SerializeField] private Transform tfRoot, tfMask;
    [FoldoutGroup("References")][SerializeField] private SpriteRenderer mixerFlour;
    [FoldoutGroup("References")][SerializeField] private AudioSource sfx;

    [FoldoutGroup("Parameters")][SerializeField] private int number = 5;
    [FoldoutGroup("Parameters")][SerializeField] private float speed_max = 10f, speed_min = 1f, angle_min = 20f;
    [FoldoutGroup("Parameters")][SerializeField] private float deltaY_mask = 1f;

    [FoldoutGroup("Event")] public UnityEvent draggingEvent, oneLapEvent, completeEvent;

    private Vector2 mouseStarPos;
    private int count = 0;
    private float deltaAngle = 0, rate;
    private float startY_mask;
    private float startLocalZ;

    public float Rate => rate;

    private void OnEnable()
    {
      transform.Appear();
      startY_mask = tfMask.localPosition.y;
      startLocalZ = (GetAngleABC(transform.position, tfRoot.position, tfRoot.position + Vector3.up * 2f) + 360f) % 360f;

      if (bowl != null)
      {
        bowl.OnStartMix();
      }
    }

    public void Init()
    {
      count = 0;
      rate = 0f;
      deltaAngle = 0f;

      UpdateByRate();
      gameObject.SetActive(false);
    }

    private void OnMouseDrag()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;

      //Calculate rotate angle
      Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
      float angle = GetAngleABC(transform.position, tfRoot.position, pos);

      //Calculate speed
      float speed = Mathf.Abs(angle) / angle_min * speed_min;
      speed = Mathf.Clamp(speed, speed_min, speed_max);

      float z = Mathf.Lerp(tfRoot.eulerAngles.z, tfRoot.eulerAngles.z + angle, Time.deltaTime * speed);
      deltaAngle += Mathf.Abs(z - tfRoot.eulerAngles.z);
      tfRoot.eulerAngles = new Vector3(0, 0, z);

      if (deltaAngle >= 360f)
      {
        OnCompleteOneLap();
      }

      rate = Mathf.Clamp01(((float)count + (deltaAngle / 360f)) / number);
      UpdateByRate();

      draggingEvent?.Invoke();

      if (bowl != null)
      {
        float localZ = Mathf.Repeat(startLocalZ + Mathf.Repeat(tfRoot.eulerAngles.z, 360f), 360f);
        bowl.OnMixing(Mathf.Clamp01(speed / speed_max), tfRoot.eulerAngles.z + startLocalZ);
      }
    }
    private void OnCompleteOneLap()
    {
      count++;
      deltaAngle %= 360f;
      if (count >= number)
      {
        gameObject.SetActive(false);
        completeEvent?.Invoke();
        if (bowl != null)
        {
          bowl.OnComplete();
        }
      }
      else
      {
        if (!sfx.isPlaying) sfx.Play();
      }
      oneLapEvent?.Invoke();
    }
    private void UpdateByRate()
    {
      mixerFlour.SetAlpha(rate + (deltaAngle / 360f) * 1 / number);
      tfMask.localPosition = new Vector3(tfMask.localPosition.x, startY_mask + deltaY_mask * rate, tfMask.localPosition.z);
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
    public void OnSpilled()
    {
      mixerFlour.DOFade(0, 1f).SetDelay(.3f);
      gameObject.SetActive(false);
      sfx.Stop();
    }
  }
}

