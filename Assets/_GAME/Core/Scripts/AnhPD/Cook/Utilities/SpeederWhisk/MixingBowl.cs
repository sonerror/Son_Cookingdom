
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class MixingBowl : MonoBehaviour
  {
    [SerializeField] protected GameObject[] ingredients;
    [SerializeField] private BeaterInBowl whisk;

    public UnityEvent fullIngredientEvent;

    protected int count = 0;
    public void OnPutIngredientIn(int index)
    {
      count++;
      ingredients[index].SetActive(true);
      if (count >= ingredients.Length)
      {
        fullIngredientEvent?.Invoke();
      }
    }
    public void OnMixing()
    {
      float angle = Angle360ToTarget(transform, whisk.transform);
      //Debug.Log(angle);
      float dir = AngleToWaveNormalized(angle);
      float z;
      float speed = 20;
      z = Mathf.LerpAngle(
      transform.eulerAngles.z,
          (3f * dir),
          //speedRate > 0.5f ?(-20f * dir * speedRate) : 0, 
          speed * Time.deltaTime
          );
      transform.eulerAngles = new Vector3(0, 0, z);
    }
    public void OnComplete()
    {
      transform.eulerAngles = Vector3.zero;
    }
    float AngleToWaveNormalized(float angle)
    {
      angle = Mathf.Repeat(angle, 360f); // đảm bảo 0–360
      float rad = angle * Mathf.Deg2Rad;
      return Mathf.Sin(rad);
    }
    public static float Angle360ToTarget(Transform self, Transform target)
    {
      Vector3 toTarget = (target.position - self.position).normalized;
      Vector3 localUp = self.up;

      // Dựng mặt phẳng tính toán: trục pháp tuyến là trục Z (nếu bạn làm game 2D theo XY)
      float angle = Vector3.SignedAngle(localUp, toTarget, Vector3.forward); // mặt phẳng XY

      if (angle < 0)
        angle += 360f;

      return angle;
    }
  }
}

