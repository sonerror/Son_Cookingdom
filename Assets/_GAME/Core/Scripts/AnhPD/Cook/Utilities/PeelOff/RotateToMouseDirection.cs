using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
  public class RotateToMouseDirection : MonoBehaviour
  {
    public bool IsBlock;
    private float initialAngleOffset;

    protected virtual void OnMouseDown()
    {
      if (!LevelBase.Ins.IsAllowInteract || IsBlock) return;

      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePos.z = 0;

      Vector2 direction = mousePos - transform.position;

      // Tính góc giữa transform.up và vector đến chuột
      float angleToTransformUp = (Vector2.SignedAngle(transform.up, direction));
      initialAngleOffset = angleToTransformUp;
    }
    protected virtual void OnMouseDrag()
    {
      if (!LevelBase.Ins.IsAllowInteract || IsBlock) return;

      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePos.z = 0;

      Vector2 direction = mousePos - transform.position;

      // Tính góc giữa transform.up và vector đến chuột
      float angleToTransformUp = (Vector2.SignedAngle(transform.up, direction));

      // Giữ nguyên độ lệch ban đầu
      float newRotationZ = transform.eulerAngles.z + (angleToTransformUp - initialAngleOffset);
      newRotationZ = ClampAngleRange360(newRotationZ);
      transform.eulerAngles = new Vector3(0, 0, newRotationZ);
    }

    [SerializeField, Tooltip("Enter an angle between 0 and 360.")]
    private float angleMin = 350f;

    [SerializeField, Tooltip("Enter an angle between 0 and 360.")]
    private float angleMax = 10f;
    float ClampAngleRange360(float value)
    {
      value = Mathf.Repeat(value, 360f);

      bool IsInRange(float v, float min, float max)
      {
        if (min < max)
          return v >= min && v <= max;
        else
          return v >= min || v <= max;
      }

      if (IsInRange(value, angleMin, angleMax))
      {
        // Nằm trong khoảng thì giữ nguyên
        return value;
      }

      // Ngoài khoảng → snap về biên gần nhất
      float distToMin = Mathf.Abs(Mathf.DeltaAngle(value, angleMin));
      float distToMax = Mathf.Abs(Mathf.DeltaAngle(value, angleMax));
      return distToMin < distToMax ? angleMin : angleMax;
    }
    private void OnValidate()
    {
      angleMin = Mathf.Clamp(angleMin, 0f, 360f);
      angleMax = Mathf.Clamp(angleMax, 0f, 360f);
    }
  }
}

