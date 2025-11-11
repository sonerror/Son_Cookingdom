using AnhPD.Cook;
using AnhPD.CookV2;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Tanghulu
{
  public class TanghuluChocoPot : ClockerStoveBase
  {

    [SerializeField] private SpriteRenderer[] ingredients;
    [SerializeField] private TanghuluChocoConfig[] chocoConfig;
    [SerializeField] private SpriteRenderer choco, melt, pour;
    [SerializeField] private APDv2Drag drag;
    [SerializeField] private BeaterInBowl whisk;

    public Action OnRestart, OnDone;
    public UnityEvent onFullIngredient;

    public bool isHaveChoco;
    public bool isBrown = true;

    private int _ingredientCount;
    private int _chocoCount;

    private void Start()
    {
      drag.onCompleteBaseSwitchTarget.AddListener(Restart);
    }
    public void PutInIngredient(int index)
    {
      if (index == 1)
      {
        ingredients[1].SetAlpha(1f);
        ingredients[1].gameObject.SetActive(true);

        TutorialManager.Ins.RemoveStep(3);
      }
      else
      {
        isHaveChoco = true;
        SetupChoco(index);
        ingredients[0].SetAlpha(1f);
        ingredients[0].gameObject.SetActive(true);
        drag.SwitchTarget(index);
        TutorialManager.Ins.RemoveStep(2);
      }

      _ingredientCount++;
      if (_ingredientCount >= 2)
      {
        if (!isOn)
        {
          IsReady = true;
          OnClick();
        }
        else onFullIngredient?.Invoke();
      }
    }
    protected override void OnTurnOn()
    {
      base.OnTurnOn();
      onFullIngredient?.Invoke();
    }

    public void OnMixDone()
    {
      for (int i = 0; i < ingredients.Length; i++) ingredients[i].gameObject.SetActive(false);
      drag.EnableCollider();
      drag.OnReady();
    }
    public void OnMixing()
    {
      for (int i = 0; i < ingredients.Length; i++) ingredients[i].SetAlpha(1 - whisk.Rate);
    }

    public void SetupChoco(int index)
    {
      isBrown = index == 0;
      choco.sprite = chocoConfig[index].choco;
      melt.sprite = chocoConfig[index].melt;
      pour.sprite = chocoConfig[index].pour;


      drag.EnableCollider(false);
    }
    private void Restart()
    {
      _chocoCount++;
      _ingredientCount = 0;
      isHaveChoco = false;
      melt.SetAlpha(0);

      if (_chocoCount > 1)
      {
        IsReady = true;
        return;
      }
      OnRestart?.Invoke();
    }
    protected override void OnTurnOff()
    {
      base.OnTurnOff();
      vfxSmoke.Stop();
      OnDone?.Invoke();
    }
  }
  [Serializable]
  public class TanghuluChocoConfig
  {
    public Sprite choco, melt, pour;
  }
}
