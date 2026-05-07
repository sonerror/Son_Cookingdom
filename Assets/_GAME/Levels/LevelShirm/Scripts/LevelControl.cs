using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using sonnv;
using UnityEngine.Events;

public class LevelControl : LevelBase
{
    [SerializeField] private Camera cam;
    protected override void Awake()
    {
        base.Awake();
        maxLayer = 30;
        if (cam == null)
        {
            cam = Camera.main;
        }
    }
    protected virtual void Start()
    {
        StartStep();
    }
    [SerializeField] private int currentStep = 0;
    public int CurrentStep => currentStep;
    private bool isDoneStep;
    public bool IsDoneStep => isDoneStep;
    private bool istap = false;

    private void EndGame()
    {
        GameManager.Ins.showEndGame();
        TutorialManager.Ins.SetNewTime(0.5f);
        TutorialManager.Ins.enableCountTime = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && istap == false)
        {
            EventManager.TriggerEvent("ShowIconLv");
            SoundManager.Ins.PlayBgm();
            istap = true;
        }
    }
    private void SetStart()
    {
    }
    private Tween _delayTween;
    private void StartDelayStep(float delay)
    {
        _delayTween?.Kill();
        _delayTween = DOVirtual.DelayedCall(delay, () =>
        {
            CookManager.Ins.OnStartStep();
        });
    }
    IEnumerator IE_DelayStart()
    {
        yield return new WaitForSeconds(0.75f);
        SetStart();
    }
    public void SetStateDoneStep(bool value)
    {
        isDoneStep = value;
    }
    public EmojiControl emoji;
    public static int maxLayer = 30;
    private void SetNewEmoji(EmojiControl _emoji)
    {
        emoji = _emoji;
    }
    public void ShowNegative()
    {
        emoji.ShowNegative();
    }
    protected virtual void DoneStep()
    {
        emoji.ShowPositive();
        isDoneStep = true;
        EventManager.TriggerEvent(EventType.IncreaseProgress.ToString());
    }
    protected virtual void TryNextStep()
    {
        currentStep++;
        StartStep();
    }
    private void StartStep()
    {
        isDoneStep = false;

        switch (currentStep)
        {
            case 0:
                TutorialManager.Ins.SetNewTime(0.5f);
                OnStartStep1();
                break;
            case 1:
                OnStartStep2();
                break;
            case 2:
                OnStartStep3();
                break;
            case 3:
                OnStartStep4();
                break;
            case 4:
                OnStartStep5();
                break;
            case 5:
                OnStartStep6();
                break;
            case 6:
                OnStartStep7();
                break;
            case 7:
                OnStartStep8();
                break;
            case 8:
                OnStartStep9();
                break;
            case 9:
                OnStartStep10();
                break;
            case 10:
                OnStartStep11();
                break;
            case 11:
                OnStartStep12();
                break;
            case 12:
                OnStartStep13();
                break;
            case 13:
                OnStartStep14();
                break;
            case 14:
                OnStartStep15();
                break;
        }
    }
    public void OnCompleteStage(int currentStageIndex, float x)
    {
        emoji.ShowPositive();
        if (cam != null)
            cam.transform.DOMoveX(x, 1f).SetDelay(1f);
    }
    public void IncreaseMaxLayer(int increment = 4)
    {
        maxLayer += increment;
    }
    public void ShowPositiveEmojiAtPos(Vector3 position)
    {
        emoji.transform.position = position + Vector3.up * 0.5f + Vector3.left * 0.5f;
        emoji.ShowPositive();
    }
    #region Step 1
    [SerializeField] private SpriteButtonOnOff spriteButtonOnOff;
    private void OnStartStep1()
    {
        TutorialManager.Ins.enableCountTime = true;

        spriteButtonOnOff.onClickOn.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 2
    [SerializeField] private SonSnapObject snapObjectWater;
    [SerializeField] private SonSnapPoint snapPointWater;
    private void OnStartStep2()
    {
        snapPointWater.ChangeCanSnap(true);
        snapObjectWater.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
        snapObjectWater.OnStartTrans.AddListener(() =>
       {
           TutorialManager.Ins.enableCountTime = false;

       });
    }
    #endregion
    #region Step 3
    [SerializeField] private SonSnapObject snapObjectSauce;
    [SerializeField] private SonSnapPoint snapPointSauce;
    [SerializeField] private TutorialManager tutorialManager;
    private void OnStartStep3()
    {
        TutorialManager.Ins.enableCountTime = true;
        snapPointSauce.ChangeCanSnap(true);
        snapObjectSauce.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
        snapObjectSauce.OnStartTrans.AddListener(() =>
       {
           tutorialManager.enableCountTime = false;

       });
    }
    #endregion
    #region Step 4
    [SerializeField] private SonSnapObject snapObjectSiro;
    [SerializeField] private SonSnapPoint snapPointSiro;
    private void OnStartStep4()
    {
        TutorialManager.Ins.SetNewTime(4);
        TutorialManager.Ins.enableCountTime = true;
        snapPointSiro.ChangeCanSnap(true);
        snapObjectSiro.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
        snapObjectSiro.OnStartTrans.AddListener(() =>
       {
           TutorialManager.Ins.enableCountTime = false;
       });
    }
    #endregion
    #region Step 5
    [SerializeField] private ClockTimer clockTimer;
    [SerializeField] private List<SpriteRenderer> dimSprites;
    [SerializeField] private List<SpriteRenderer> brightSprites;
    [SerializeField] private float fadeDuration = 3f;
    private void OnStartStep5()
    {
        TutorialManager.Ins.enableCountTime = false;

        StartCoroutine(IE_DelayStartStep5());
    }

    IEnumerator IE_DelayStartStep5()
    {
        yield return new WaitForSeconds(0.5f);

        clockTimer.OnTimeOut = OnStep5TimeOut;
        clockTimer.Show(fadeDuration);

        foreach (var sr in dimSprites)
            sr.DOFade(0f, fadeDuration).SetEase(Ease.InOutSine);

        foreach (var sr in brightSprites)
            sr.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
    }

    private void OnStep5TimeOut()
    {
        DoneStep();
        TryNextStep();

    }
    #endregion
    #region Step 6
    [SerializeField] private SonSnapObject snapObjectFried;
    [SerializeField] private SonSnapPoint snapPointFried;
    private void OnStartStep6()
    {
        TutorialManager.Ins.enableCountTime = true;

        snapPointFried.ChangeCanSnap(true);
        snapObjectFried.OnSnap.AddListener(() =>
        {

            DoneStep();
            TryNextStep();
        });

    }
    #endregion

    #region Step 7
    [SerializeField] private SpriteRenderer friedSprites;
    [SerializeField] private SpriteRenderer friedSpritesNew;
    [SerializeField] private ShowObjectEffect lid;
    private void OnStartStep7()
    {
        TutorialManager.Ins.enableCountTime = false;

        StartCoroutine(IE_DelayStartStep7());
    }

    IEnumerator IE_DelayStartStep7()
    {
        yield return new WaitForSeconds(0.5f);
        lid.Show();
        yield return new WaitForSeconds(0.5f);
        clockTimer.OnTimeOut = OnStep7TimeOut;
        clockTimer.Show(fadeDuration);
        friedSpritesNew.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
        friedSprites.DOFade(0, fadeDuration).SetEase(Ease.InOutSine);
    }

    private void OnStep7TimeOut()
    {
        lid.Hide(0.5f);
        lid.onHide.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 8
    [SerializeField] private SonSnapObject snapObjectFriedDone;
    [SerializeField] private SonSnapPoint snapPointFriedDone;
    [SerializeField] private EmojiControl newEmoji;
    private void OnStartStep8()
    {
        SetNewEmoji(newEmoji);
        TutorialManager.Ins.enableCountTime = true;
        snapObjectFriedDone.enabled = true;
        snapObjectFriedDone.Col.enabled = true;
        snapPointFriedDone.ChangeCanSnap(true);
        snapObjectFriedDone.OnSnap.AddListener(() =>
        {
            spriteButtonOnOff.ClickButton();
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 9
    [SerializeField] private SonSnapObject snapObjectSeaweedDone;
    [SerializeField] private SonSnapPoint snapPointSeaweedDone;
    private void OnStartStep9()
    {
        snapPointSeaweedDone.ChangeCanSnap(true);
        snapObjectSeaweedDone.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 10
    [SerializeField] private SonSnapObject snapObjectBowlFood;
    [SerializeField] private SonSnapPoint snapPointBowlFood;
    private void OnStartStep10()
    {
        snapPointBowlFood.ChangeCanSnap(true);
        snapObjectBowlFood.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
        snapObjectBowlFood.OnStartTrans.AddListener(() =>
      {
          TutorialManager.Ins.enableCountTime = false;
      });
    }
    #endregion
    #region Step 11
    [SerializeField] private RollObject rollObject;
    private void OnStartStep11()
    {
        TutorialManager.Ins.enableCountTime = true;
        rollObject.enabled = true;
        rollObject.Col.enabled = true;
        rollObject.OnRollComplete.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 12
    [SerializeField] private SonSnapObject snapObjectKnife;
    [SerializeField] private SonSnapPoint snapPointKnife;
    private void OnStartStep12()
    {
        snapPointKnife.ChangeCanSnap(true);
        snapObjectKnife.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });

    }
    #endregion
    #region Step 13
    [SerializeField] private KnifeCutObject knifeCutObject;
    private void OnStartStep13()
    {
        knifeCutObject.eventDoneActionDance.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });

    }
    #endregion
    #region Step 14
    [SerializeField] private SonSnapObject snapObjectUnamiDone;
    [SerializeField] private SonSnapPoint snapPointUnamiDone;
    private void OnStartStep14()
    {
        snapPointUnamiDone.ChangeCanSnap(true);
        snapObjectUnamiDone.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
            TutorialManager.Ins.enableCountTime = false;
        });

    }
    #endregion
    #region Step 15
    [SerializeField] private ShowObjectEffect step1;
    [SerializeField] private ShowObjectEffect step2;
    private void OnStartStep15()
    {
        step1.Hide(0.5f);
        step2.Show(1.25f);
        step2.onShowComplete.AddListener(() =>
        {
            EndGame();
        });

    }
    #endregion
    // #region Step 2
    // [SerializeField] private SonSnapObject snapObjectVermicelli;
    // [SerializeField] private SonSnapPoint snapPointVermicelli;
    // private void OnStartStep2()
    // {
    //     snapPointVermicelli.ChangeCanSnap(true);
    //     snapObjectVermicelli.OnSnap.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // #endregion
    // #region Step3
    // [SerializeField] private List<SonSnapObject> listSnapObjectAllItem;
    // [SerializeField] private List<SonSnapPoint> listSnapPointItem;
    // private int countSnap = 0;
    // private void OnStartStep3()
    // {
    //     foreach (SonSnapPoint point in listSnapPointItem)
    //     {
    //         point.ChangeCanSnap(true);
    //     }
    //     for (int i = 0; i < listSnapObjectAllItem.Count; i++)
    //     {
    //         SonSnapObject obj = listSnapObjectAllItem[i];
    //         obj.OnStartTrans.AddListener(() =>
    //         {
    //             TutorialManager.Ins.enableCountTime = false;
    //         });
    //         obj.OnSnap.AddListener(() =>
    //         {
    //             int removedIndex = listSnapObjectAllItem.IndexOf(obj);
    //             if (removedIndex < 0) return;
    //             CheckSnapItem();
    //             TutorialManager.Ins.enableCountTime = true;
    //             TutorialManager.Ins.ListItem.RemoveAt(removedIndex);
    //             listSnapObjectAllItem.RemoveAt(removedIndex);
    //             if (listSnapObjectAllItem.Count == 0)
    //             {
    //                 DoneStep();
    //                 TryNextStep();
    //             }
    //         });
    //     }
    // }
    // private void CheckSnapItem()
    // {
    //     Debug.Log("Snap");
    //     countSnap++;
    //     if (countSnap == 1)
    //     {
    //         TutorialManager.Ins.SetNewTime(4f);
    //         Debug.Log("Snap Done");

    //     }
    // }
    // private bool isDoneSnapItem = false;
    // private bool isDoneSnapEgg = false;
    // private void CheckDoneStep3()
    // {
    //     if (isDoneSnapItem && isDoneSnapEgg)
    //     {

    //     }
    // }
    // #endregion
    // #region Step4
    // [SerializeField] private List<SonSnapObject> listSnapObjectIng;
    // [SerializeField] private List<SonSnapPoint> listSnapPointIng;
    // private void OnStartStep4()
    // {
    //     foreach (SonSnapPoint point in listSnapPointIng)
    //     {
    //         point.ChangeCanSnap(true);
    //     }
    //     for (int i = 0; i < listSnapObjectIng.Count; i++)
    //     {
    //         SonSnapObject obj = listSnapObjectIng[i];
    //         obj.OnTrans.AddListener(() =>
    //         {
    //             int removedIndex = listSnapObjectIng.IndexOf(obj);
    //             if (removedIndex < 0) return;
    //             listSnapObjectIng.RemoveAt(removedIndex);
    //             TutorialManager.Ins.ListItemBotl.RemoveAt(removedIndex);
    //             if (listSnapObjectIng.Count == 0)
    //             {
    //                 DoneStep();
    //                 TryNextStep();
    //             }
    //         });
    //     }
    // }
    // #endregion
    // #region Step 5
    // [SerializeField] private TriggerToRotate triggerToRotate;
    // [SerializeField] private Collider2D col;
    // [SerializeField] private Spoon spoonRotate;
    // [SerializeField] private GameObject objTrigger;

    // private void OnStartStep5()
    // {
    //     triggerToRotate.SetBlockRotate(false);
    //     triggerToRotate.enabled = true;
    //     col.enabled = true;

    //     UnityAction onStartDragAction = null;
    //     UnityAction onEndMoveBackAction = null;
    //     UnityAction onEndRotateAction = null;

    //     onStartDragAction = () =>
    //     {
    //         objTrigger.SetActive(true);
    //     };

    //     onEndMoveBackAction = () =>
    //     {
    //         objTrigger.SetActive(false);
    //     };

    //     onEndRotateAction = () =>
    //     {
    //         spoonRotate.onStartDrag.RemoveListener(onStartDragAction);
    //         spoonRotate.onEndMoveBack.RemoveListener(onEndMoveBackAction);
    //         triggerToRotate.onEndRotate.RemoveListener(onEndRotateAction);
    //         DoneStep();
    //         TryNextStep();
    //         objTrigger.SetActive(false);
    //     };

    //     spoonRotate.onStartDrag.AddListener(onStartDragAction);
    //     spoonRotate.onEndMoveBack.AddListener(onEndMoveBackAction);
    //     triggerToRotate.onEndRotate.AddListener(onEndRotateAction);
    // }
    // #endregion
    // #region Step 6
    // [SerializeField] private EmojiControl newEmoji;
    // [SerializeField] private SpriteRenderen spriteRenderen;
    // private void OnStartStep6()
    // {
    //     triggerToRotate.SetBlockRotate(true);

    //     spoonRotate.SetIsBlockTrySnap(false);
    //     SetNewEmoji(newEmoji);
    //     spriteRenderen.OnDone.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // #endregion
    // #region Step 7
    // [SerializeField] private List<SonTapItem> listTapMando;
    // private void OnStartStep7()
    // {
    //     for (int i = 0; i < listTapMando.Count; i++)
    //     {
    //         SonTapItem obj = listTapMando[i];
    //         obj.enabled = true;
    //         obj.ColD.enabled = true;
    //         obj.eventOnPointDown.AddListener(() =>
    //         {
    //             int removedIndex = listTapMando.IndexOf(obj);
    //             if (removedIndex < 0) return;
    //             listTapMando.RemoveAt(removedIndex);
    //             TutorialManager.Ins.ListMando.RemoveAt(removedIndex);
    //             if (listTapMando.Count == 0)
    //             {
    //                 DoneStep();
    //                 TryNextStep();
    //             }
    //         });
    //     }
    // }
    // #endregion
    // #region Step 8
    // [SerializeField] private ShowObjectEffect effectStep1;
    // [SerializeField] private ShowObjectEffect effectStep2;
    // private void OnStartStep8()
    // {

    //     effectStep1.Hide(0.5f);
    //     effectStep2.Show(1.5f);
    //     effectStep2.onShowComplete.AddListener(() =>
    //     {
    //         EndGame();
    //     });
    // }
    // #endregion

}
