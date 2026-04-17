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
    private Camera cam;
    protected override void Awake()
    {
        base.Awake();
        maxLayer = 30;
        cam = Camera.main;
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
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && istap == false)
        {
            istap = true;
        }
    }
    //  [SerializeField] private List<SonSnapObject> listSnapObjectStart;
    private void SetStart()
    {
        // foreach (SonSnapObject snap in listSnapObjectStart)
        // {
        //     snap.enabled = true;
        //     snap.Col.enabled = true;
        // }
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
                //TutorialManager.Ins.SetNewTime(3f);
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
                //OnStartStep6();
                break;
            case 6:
                //OnStartStep7();
                break;
            case 7:
                // OnStartStep8();
                break;
            case 8:
                // OnStartStep9();
                break;
            case 9:
                // OnStartStep10();
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
    [SerializeField] private List<SonSnapObject> listSnapObjectEgg;
    [SerializeField] private List<SonSnapPoint> listSnapPointEgg;
    private void OnStartStep1()
    {
        foreach (SonSnapPoint point in listSnapPointEgg)
        {
            point.ChangeCanSnap(true);
        }
        for (int i = 0; i < listSnapObjectEgg.Count; i++)
        {
            SonSnapObject obj = listSnapObjectEgg[i];
            obj.OnSnap.AddListener(() =>
            {
                int removedIndex = listSnapObjectEgg.IndexOf(obj);
                if (removedIndex < 0) return;
                listSnapObjectEgg.RemoveAt(removedIndex);
                // TutorialManager.Ins.TutorialNode.RemoveAt(removedIndex);
                // TutorialManager.Ins.TfItem.RemoveAt(removedIndex);
                if (listSnapObjectEgg.Count == 0)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }
    [SerializeField] private List<SpoonIngredient> listItemSaltSugar;
    [SerializeField] private List<SnapPoint> listSnapPointItem;
    private void OnStartStep2()
    {
        foreach (SnapPoint point in listSnapPointItem)
        {
            point.ChangeCanSnap(true);
        }
        for (int i = 0; i < listItemSaltSugar.Count; i++)
        {
            SpoonIngredient obj = listItemSaltSugar[i];
            obj.onSnap.AddListener(() =>
            {
                int removedIndex = listItemSaltSugar.IndexOf(obj);
                if (removedIndex < 0) return;
                listItemSaltSugar.RemoveAt(removedIndex);
                if (listItemSaltSugar.Count == 0)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }
    [SerializeField] private List<SonSnapObject> listSnapObjectBolt;
    [SerializeField] private List<SonSnapPoint> listSnapPointBolt;
    private void OnStartStep3()
    {
        foreach (SonSnapPoint point in listSnapPointBolt)
        {
            point.ChangeCanSnap(true);
        }
        for (int i = 0; i < listSnapObjectBolt.Count; i++)
        {
            SonSnapObject obj = listSnapObjectBolt[i];
            obj.OnTrans.AddListener(() =>
            {
                int removedIndex = listSnapObjectBolt.IndexOf(obj);
                if (removedIndex < 0) return;
                listSnapObjectBolt.RemoveAt(removedIndex);
                // TutorialManager.Ins.TutorialNode.RemoveAt(removedIndex);
                // TutorialManager.Ins.TfItem.RemoveAt(removedIndex);
                if (listSnapObjectBolt.Count == 0)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }
    [SerializeField] private SonSnapPoint snapPointWhisk;
    [SerializeField] private SonSnapObject snapObjeWhisk;
    private void OnStartStep4()
    {
        snapPointWhisk.ChangeCanSnap(true);
        snapObjeWhisk.OnSnap.AddListener(() =>
           {
               DoneStep();
               TryNextStep();
           });
    }
    [SerializeField] private FlatSpoonInPan flatSpoonInPan;
    private void OnStartStep5()
    {
        flatSpoonInPan.onEndRotate.AddListener(() =>
           {
               DoneStep();
               TryNextStep();
           });
    }
    // [SerializeField] private PetOrderManager petOrderManager;
    // [SerializeField] private SonSnapObject snapObjectKiwi;

    // private void OnStartStep1()
    // {
    //     petOrderManager.OnShowOrder();
    //     StartDelayStep(0.5f);
    //     snapObjectKiwi.OnSnap.AddListener(() =>
    //     {
    //         Debug.Log("step1");
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private SonSnapPoint snapPointSpatula;
    // [SerializeField] private TapSpatula tapSpatula;
    // private void OnStartStep2()
    // {
    //     snapPointSpatula.ChangeCanSnap(true);
    //     tapSpatula.OnWin.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private SonSnapPoint snapPointMilk;
    // [SerializeField] private SonSnapObject snapObjectMilk;
    // private void OnStartStep3()
    // {
    //     snapPointMilk.ChangeCanSnap(true);
    //     snapObjectMilk.OnTrans.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private FoodItem foodItem;
    // private void OnStartStep4()
    // {
    //     foodItem.Col.enabled = true;
    //     foodItem.OnDone.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private SonSnapPoint snapPointSpatulaCut;
    // [SerializeField] private CreamCut creamCut;
    // private void OnStartStep5()
    // {
    //     snapPointSpatulaCut.ChangeCanSnap(true);
    //     creamCut.onComplete.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private SonSnapPoint snapPointSpatulaDrag;
    // [SerializeField] private GameObject objCut;
    // [SerializeField] private GameObject objRoll;
    // [SerializeField] private CreamRollController creamRollController;
    // private void OnStartStep6()
    // {
    //     snapPointSpatulaDrag.ChangeCanSnap(true);
    //     objCut.SetActive(false);
    //     objRoll.SetActive(true);
    //     creamRollController.onAllRollsCompleted.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private SonSnapObject snapObjectRollDone;
    // private void OnStartStep7()
    // {
    //     snapObjectRollDone.enabled = true;
    //     snapObjectRollDone.Col.enabled = true;
    //     snapObjectRollDone.OnSnap.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private List<SonSnapObject> listSnapObjectRollDone;
    // [SerializeField] private List<SonSnapPoint> listSnapPointRollDone;
    // [SerializeField] private SonSnapObject snapChocola;
    // private void OnStartStep8()
    // {
    //     foreach (SonSnapPoint point in listSnapPointRollDone)
    //     {
    //         point.ChangeCanSnap(true);
    //     }
    //     SetSnapObject();
    //     snapChocola.OnTrans.AddListener(() =>
    //     {
    //         isDone2 = true;
    //         CheckDoneStep8();
    //     });
    // }
    // private void SetSnapObject()
    // {

    //     foreach (SonSnapObject obj in listSnapObjectRollDone)
    //     {
    //         SonSnapObject cache = obj;
    //         UnityAction action = delegate
    //         {
    //             OnSnapHandler(cache);
    //         };
    //         snapActions[cache] = action;
    //         cache.OnSnap.AddListener(action);
    //     }
    // }
    // private void OnSnapHandler(SonSnapObject obj)
    // {
    //     if (snapActions.ContainsKey(obj))
    //     {
    //         obj.OnSnap.RemoveListener(snapActions[obj]);
    //         snapActions.Remove(obj);
    //     }
    //     int removedIndex = listSnapObjectRollDone.IndexOf(obj);
    //     if (removedIndex < 0) return;
    //     listSnapObjectRollDone.RemoveAt(removedIndex);
    //     if (removedIndex < TutorialManager.Ins.TfItem.Count)
    //         TutorialManager.Ins.TfItem.RemoveAt(removedIndex);
    //     if (listSnapObjectRollDone.Count <= 0)
    //     {
    //         isDone1 = true;
    //         CheckDoneStep8();
    //     }
    // }
    // private bool isDone1 = false;
    // private bool isDone2 = false;
    // private void CheckDoneStep8()
    // {
    //     if (isDone1 && isDone2)
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     }
    // }
    // [SerializeField] private SpriteRenderen layerr;
    // [SerializeField] private SonSnapObject snapDone;
    // [SerializeField] private SonSnapPoint snapPointPet;
    // [SerializeField] private AudioClip sfxHappy;
    // private void OnStartStep9()
    // {
    //     layerr.OnInit();
    //     snapDone.enabled = true;
    //     snapDone.Col.enabled = true;
    //     snapPointPet.ChangeCanSnap(true);
    //     snapDone.OnSnap.AddListener(() =>
    //     {
    //         TutorialManager.Ins.enableCountTime = false;
    //         petStep1.PlayDropThenIdle();
    //         SoundManager.PlaySFX(sfxHappy);
    //         HidePet();
    //     });
    // }
    // [SerializeField] private PetOrder petStep1;
    // [SerializeField] private PetOrder petStep2;
    // private void HidePet()
    // {
    //     petStep1.OnHide();
    //     DoneStep();
    //     TryNextStep();
    // }
    // [SerializeField] private List<GameObject> listObjHide;
    // private void OnStartStep10()
    // {
    //     petStep2.OnShow(1.25f);
    //     StartCoroutine(IEShowParticle());
    // }
    // private IEnumerator IEShowParticle()
    // {
    //     yield return new WaitForSeconds(1.25f);
    //     foreach (GameObject obj in listObjHide)
    //     {
    //         obj.SetActive(true);
    //     }
    //     TutorialManager.Ins.enableCountTime = true;
    //     GameManager.Ins.showEndGame();
    // }
}
