using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class BeaterInBowl : MonoBehaviour
  {
    [SerializeField] Transform tfRoot;
    [SerializeField] SpriteRenderer mixerFlour;
    [SerializeField] private int number;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private FxType sfxType = FxType.None;
    // [SerializeField] private AudioSource audioSource;
    public UnityEvent draggingEvent, oneLapEvent, completeEvent;
    public bool IsLockRotation = true;
    public bool isLoopAudio;
    Vector2 mouseStarPos;
    int count = 0;
    float deltaAngle = 0, rate;

    public float Rate => rate;

    private void OnEnable()
    {
      transform.Appear();
    }

    bool isMouseDown = false;
    private void Update()
    {
      if (IsLockRotation)
      {
        transform.eulerAngles = Vector3.zero;
      }

      if (Input.GetMouseButtonDown(0) && !isMouseDown)
      {
        isMouseDown = true;
        SoundManager.Ins.PlaySoundLoop(sfxType);
      }
      else if (Input.GetMouseButtonUp(0) && isMouseDown)
      {
        isMouseDown = false;
        SoundManager.Ins.StopSoundLoop(sfxType);
      }
    }

    private void OnMouseDrag()
    {
      if (!LevelBase.Ins.IsAllowInteract) return;

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
          gameObject.SetActive(false);
          SoundManager.Ins.StopSoundLoop(sfxType);
          completeEvent?.Invoke();
        }
        else
        {
          if (!isLoopAudio)
          {
            // AudioManager.PlaySFX(sfx);
          }
          else
          {
            // if (!audioSource.isPlaying) audioSource.Play();
          }
        }
        oneLapEvent?.Invoke();
      }
      rate = Mathf.Clamp01((float)count / number);
      mixerFlour.SetAlpha(rate + (deltaAngle / 360f) * 1 / number);
      if (offset > .1f)
        draggingEvent?.Invoke();
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

    public void Restart()
    {
      count = 0;
      deltaAngle = 0;
      rate = 0;
    }
  }
}

