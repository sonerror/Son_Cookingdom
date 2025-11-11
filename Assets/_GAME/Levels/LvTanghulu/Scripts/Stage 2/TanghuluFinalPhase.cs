using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.Tanghulu
{
  public class TanghuluFinalPhase : APDProgressionPhase
  {
    [SerializeField] private TanghuluData orderData;

    [SerializeField] private AudioClip sfxLaugh;
    [SerializeField] private Transform fox, capyBro, cat, dog, rabbit, snake;

    protected override void Awake()
    {
      base.Awake();
      orderData.excludeDecorTypes = new List<TanghuluData.DecorType>
            {
                TanghuluData.DecorType.Almond,
                TanghuluData.DecorType.Sesame,
                TanghuluData.DecorType.BeanCandy
            };
      orderData.excludeCoatingTypes = new List<TanghuluData.CoatingType>
            {
                TanghuluData.CoatingType.None,
                TanghuluData.CoatingType.Blue,
                TanghuluData.CoatingType.Brown,
                TanghuluData.CoatingType.White
            };


    }
    private int _count = 0;
    public void DoneCustomer()
    {
      DoneStepImageOfGroup(3);
      DoneStepText(4);
      _count++;
      if (_count == 1)
      {
        orderData.excludeCoatingTypes = new List<TanghuluData.CoatingType>
                {
                    TanghuluData.CoatingType.None,
                };
      }
      if (_count == 2)
      {
        orderData.excludeDecorTypes = new List<TanghuluData.DecorType>();
      }
    }
    public void OnRestart()
    {
      ResetAllHint();
      SetHintAccordingToGroup(0);
    }

    public override void OnComplete()
    {
      DoneStepText(5);
      LevelBase.Ins.isEndingGame = true;

      // APDLevelBase.Ins.emoji.ShowPositive();
      transform.MoveY(-10f, 1, true, .5f);

      fox.MoveX(5f, 1f, false, 1.5f);
      cat.MoveX(-5f, 1f, false, 1.5f);

      dog.MoveX(5f, 2f, false, 1.5f);
      rabbit.MoveX(-5f, 2f, false, 2f);
      snake.MoveX(5f, 2f, false, 2.5f);

      capyBro.gameObject.SetActive(true);
      capyBro.DOMoveX(capyBro.position.x + 10f, 4f).SetDelay(1f).SetEase(Ease.Linear)
          .OnStart(() =>
          {
            // AudioManager.PlaySFX(sfxLaugh);
          })
          .OnComplete(() =>
          {
            capyBro.eulerAngles = Vector3.zero;
            base.OnComplete();
          });
      capyBro.DOMoveX(capyBro.position.x, 5f).SetDelay(6f).SetEase(Ease.Linear);
    }
  }
}
