using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace AnhPD.Cook
{
  public class WhiskBowl : MonoBehaviour
  {
    [SerializeField] private WhiskBowlIngredient[] ingredients;
    [SerializeField] private SpeederWhisk whisk;
    [SerializeField] private MixClean dirty;
    [SerializeField] private Slider slider_correct, slider_wrong;
    [SerializeField] private AudioClip sfxSpilled, sfxHit;

    [SerializeField] private float duration_spilled = 2f;
    [ShowInInspector, ReadOnly] private float timer_spilled;
    private float last_hit_dir;
    private int count;
    public UnityEvent eventSpilled, eventSpilledComplete, eventComplete, eventInit, eventHaveAllIngredient;
    public void Init()
    {
      for (int i = 0; i < ingredients.Length; i++)
      {
        ingredients[i].Init();
      }
      whisk.Init();
      transform.eulerAngles = Vector3.zero;
      count = 0;
      last_hit_dir = 0;
      timer_spilled = 0f;

      slider_correct.value = 0;
      slider_wrong.value = 0;

      slider_correct.gameObject.SetActive(false);
      slider_wrong.gameObject.SetActive(false);

      eventInit?.Invoke();
    }
    public void OnPutIngredientIn(int index)
    {
      ingredients[index].OnAppear();
      count++;
      if (count == ingredients.Length)
      {
        eventHaveAllIngredient?.Invoke();
      }
    }
    public void OnStartMix()
    {
      slider_correct.gameObject.SetActive(true);
      slider_wrong.gameObject.SetActive(true);
    }
    public void OnMixing(float speedRate, float angle)
    {
      float dir = AngleToWaveNormalized(angle);
      //Debug.Log(speedRate);
      float z;
      float speed = 20;
      z = Mathf.LerpAngle(
          transform.eulerAngles.z,
          (-20f * dir * speedRate),
          //speedRate > 0.5f ?(-20f * dir * speedRate) : 0, 
          speed * Time.deltaTime
          );
      transform.eulerAngles = new Vector3(0, 0, z);
      if (speedRate >= .7f)
      {
        int i = Mathf.CeilToInt(Mathf.Abs(dir) / dir);
        if (last_hit_dir != i)
        {
          last_hit_dir = i;
          // AudioManager.PlaySFX(sfxHit);
        }
        timer_spilled += Time.deltaTime;
        if (timer_spilled >= duration_spilled)
        {
          OnSpilled();
        }
      }

      slider_correct.value = whisk.Rate;
      slider_wrong.value = (float)timer_spilled / duration_spilled;
    }
    private void OnSpilled()
    {
      // AudioManager.PlaySFX(sfxSpilled);
      whisk.OnSpilled();
      transform.DORotate(new Vector3(0, 0, 30f), .3f).SetEase(Ease.InOutBack).OnComplete(() =>
      {
        for (int i = 0; i < ingredients.Length; i++)
        {
          ingredients[i].FadeOut();
        }
        dirty.Appear();
        eventSpilled?.Invoke();
      });

      this.WaitToDo(() =>
      {
        transform.MoveX(-5f, 1, false, 0, () =>
              {
            transform.eulerAngles = Vector3.zero;
            transform.MoveX(5f, 1, false, 0, () =>
                  {
                Init();
                eventSpilledComplete?.Invoke();
              });
          });
      }, 1.5f);
    }
    public void OnComplete()
    {
      slider_correct.gameObject.SetActive(false);
      slider_wrong.gameObject.SetActive(false);

      transform.DORotate(Vector3.zero, .3f).OnComplete(() =>
      {
        eventComplete?.Invoke();
      });
    }
    /// <summary>
    /// Chuyển đổi góc 0-360 thành giá trị -1 đến 1 theo đường cong hình sin.
    /// Đỉnh tại 90 (trả 1), đỉnh tại 270 (trả -1), 0/180/360 trả về 0.
    /// </summary>
    float AngleToWaveNormalized(float angle)
    {
      angle = Mathf.Repeat(angle, 360f); // đảm bảo 0–360
      float rad = angle * Mathf.Deg2Rad;
      return Mathf.Sin(rad);
    }

  }
}

