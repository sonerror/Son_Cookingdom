using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
  public class Sink : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer spr_water_sink, spr_water_pour;
    [SerializeField] private Transform lid;
    [SerializeField] private AudioClip sfxPlace;
    [SerializeField] private AudioSource sfxWatering;
    [SerializeField] private Sprite[] water_sink, water_pour;

    public bool IsWatering { get; private set; }
    public bool IsContainWater { get; private set; }
    public bool IsHaveLid { get; private set; }

    [FoldoutGroup("Event")] public UnityEvent ContainWaterEvent, WateringEvent;

    float deltaY = .8f;
    Sequence anim;

    public void OnHaveLid()
    {
      IsHaveLid = true;

      // AudioManager.PlaySFX(sfxPlace);
      lid.FallAppear();
      if (IsWatering)
      {
        StartContainWater();
      }
    }
    [Button]
    public void OnRemoveLid()
    {
      IsHaveLid = false;
      IsContainWater = false;
      // AudioManager.PlaySFX(sfxPlace);

      lid.DOLocalMoveY(.5f, .3f).OnComplete(() =>
      {
        lid.gameObject.SetActive(false);
      });

      float water_sink_y = spr_water_sink.transform.localPosition.y;
      float water_pour_y = spr_water_pour.transform.localPosition.y;
      anim = DOTween.Sequence();
      anim.Append(spr_water_sink.transform.DOLocalMoveY(water_sink_y - deltaY, 1f));
      anim.Join(spr_water_pour.transform.DOLocalMoveY(-2, 1f));
      anim.Join(spr_water_sink.transform.DOScaleX(.8f, .5f));
      anim.Append(spr_water_sink.DOFade(0, .25f));
      anim.Play();
    }
    private void StartContainWater()
    {
      if (IsContainWater) return;
      IsContainWater = true;
      float water_sink_y = spr_water_sink.transform.localPosition.y;
      float water_pour_y = spr_water_pour.transform.localPosition.y;
      anim = DOTween.Sequence();
      anim.Append(spr_water_sink.DOFade(1, .25f));
      anim.Append(spr_water_sink.transform.DOLocalMoveY(water_sink_y + deltaY, 1f));
      anim.Join(spr_water_pour.transform.DOLocalMoveY(water_pour_y + deltaY, 1f));
      anim.Join(spr_water_sink.transform.DOScaleX(.9f, .5f));
      ContainWaterEvent?.Invoke();
      anim.Play();
    }
    private void DrainWater()
    {
      IsContainWater = false;
      anim.PlayBackwards();
    }
    public void StartWatering()
    {
      IsWatering = true;
      spr_water_pour.enabled = true;
      sfxWatering.Play();
      WateringEvent?.Invoke();
      if (IsHaveLid)
      {
        StartContainWater();
      }
    }
    public void StopWatering()
    {
      IsWatering = false;
      spr_water_pour.enabled = false;
      sfxWatering.Stop();
    }


    private int index_pour, index_sink;
    private float timer_pour, timer_sink;
    private void Update()
    {
      if (IsWatering)
      {
        timer_pour += Time.deltaTime;
        if (timer_pour > 0.1f)
        {
          index_pour++;
          index_pour %= water_pour.Length;
          spr_water_pour.sprite = water_pour[index_pour];

          timer_pour = 0f;
        }
      }
      if (IsContainWater && IsWatering)
      {
        timer_sink += Time.deltaTime;
        if (timer_sink > 0.2f)
        {
          index_sink++;
          index_sink %= water_sink.Length;
          spr_water_sink.sprite = water_sink[index_sink];

          timer_sink = 0f;
        }
      }
    }


    [Button]
    private void WaterOn()
    {
      spr_water_sink.SetAlpha(1);
      spr_water_sink.transform.localScale = new Vector3(.9f, 1f, 1f);
      spr_water_sink.transform.localPosition = new Vector3(-0.97f, -0.1f, 0f);
    }
    [Button]
    private void WaterOff()
    {
      spr_water_sink.SetAlpha(0);
      spr_water_sink.transform.localScale = new Vector3(.8f, 1f, 1f);
      spr_water_sink.transform.localPosition = new Vector3(-0.97f, -0.9f, 0f);
    }

  }
}

