using DG.Tweening;
using Satisgame;
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
    [SerializeField] private GameObject objSpriteRBlack;
    [SerializeField] private GameObject objMaskMorter;
    [SerializeField] private SonSnapObject snapObjStraw;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && istap == false)
        {
            //TutorialManager.Ins.IncreaseCountHintStep1();
            //objSpriteRBlack.SetActive(false);
            // objMaskMorter.SetActive(true);
            ///snapObjStraw.enabled = true;
            //snapObjStraw.Col.enabled = true;
            istap = true;
        }
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
                //OnStartStep1();
                break;
            case 1:
                //OnStartStep2();
                break;
            case 2:
                // OnStartStep3();
                break;
            case 3:
                // OnStartStep4();
                break;
            case 4:
                // OnStartStep5();
                break;
            case 5:
                // OnStartStep6();
                break;
            case 6:
                //OnStartStep7();
                break;
            case 7:
                //OnStartStep8();
                break;
            case 8:
                // OnStartStep9();
                break;
            case 9:
                //OnStartStep10();
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
    private Dictionary<SonSnapObject, UnityAction> snapActions = new Dictionary<SonSnapObject, UnityAction>();
    // private Dictionary<FlourMoveToCream, UnityAction> plateActions = new Dictionary<FlourMoveToCream, UnityAction>();
    //  private Dictionary<FlourMoveToCream, UnityAction> broadActions = new Dictionary<FlourMoveToCream, UnityAction>();
    void OnDisable()
    {
        foreach (var kvp in snapActions)
        {
            if (kvp.Key != null)
            {
                kvp.Key.OnSnap.RemoveListener(kvp.Value);
            }
        }
        snapActions.Clear();
        //   plateActions.Clear();
        //  broadActions.Clear();
    }








































































    // [SerializeField] private List<SonSnapObject> listSnapObject;
    // [SerializeField] private CuttingBoard cuttingBoard;
    // private void OnStartStep1()
    // {
    //     foreach (SonSnapObject obj in listSnapObject)
    //     {
    //         SonSnapObject cache = obj;

    //         UnityAction action = delegate
    //         {
    //             cuttingBoard.RegisterSnapDone(cache);
    //             int removedIndex = listSnapObject.IndexOf(cache);
    //             if (removedIndex < 0) return;

    //             listSnapObject.RemoveAt(removedIndex);
    //             //if (removedIndex < TutorialManager.Ins.TfBroad.Count)
    //             // TutorialManager.Ins.TfBroad.RemoveAt(removedIndex);
    //         };

    //         cache.OnSnap.AddListener(action);
    //         snapActions[cache] = action;
    //     }

    //     snapObjStraw.OnSnap.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private FlourMoveToCream strawMove;

    // private void OnStartStep2()
    // {
    //     strawMove.onComplete.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }

    // [SerializeField] private SonSnapObject snapObjMango;

    // private void OnStartStep3()
    // {
    //     snapObjMango.enabled = true;
    //     snapObjMango.Col.enabled = true;
    //     snapObjMango.OnSnap.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private FlourMoveToCream mangoMove;

    // private void OnStartStep4()
    // {
    //     mangoMove.onComplete.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }


    // [SerializeField] private SonSnapObject snapObjMacca;

    // private void OnStartStep5()
    // {
    //     snapObjMacca.enabled = true;
    //     snapObjMacca.Col.enabled = true;
    //     snapObjMacca.OnSnap.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }

    // [SerializeField] private FlourMoveToCream maccaMove;
    // private void OnStartStep6()
    // {
    //     maccaMove.onComplete.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }

    // [SerializeField] private SonSnapObject snapObjWalnut;

    // private void OnStartStep7()
    // {
    //     snapObjWalnut.enabled = true;
    //     snapObjWalnut.Col.enabled = true;
    //     snapObjWalnut.OnSnap.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private FlourMoveToCream walnutMove;
    // private void OnStartStep8()
    // {
    //     walnutMove.onComplete.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });
    // }
    // [SerializeField] private SonSnapObject snapObjPistachio;
    // [SerializeField] private SpriteButtonOnOff btnOnOffOven;
    // [SerializeField] private EmojiControl newEmoji;

    // private void OnStartStep9()
    // {
    //     SetNewEmoji(newEmoji);
    //     snapObjPistachio.enabled = true;
    //     snapObjPistachio.Col.enabled = true;
    //     btnOnOffOven.enabled = true;
    //     btnOnOffOven.Col.enabled = true;
    //     btnOnOffOven.onClickOn.AddListener(() =>
    //     {
    //         DoneStep();
    //         TryNextStep();
    //     });

    // }
    // private void OnStartStep10()
    // {
    //     snapObjPistachio.OnSnap.AddListener(() =>
    //             {
    //                 GameManager.Ins.showEndGame();
    //                 DoneStep();
    //                 TryNextStep();
    //             });
    // }
}
