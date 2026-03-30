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
                OnStartStep1();
                break;
            case 1:
                OnStartStep2();
                break;
            case 2:
                OnStartStep3();
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
    [SerializeField] private TapPaper tapPaper;

    [SerializeField] private List<SonSnapObject> listSnapObjItem;
    [SerializeField] private List<SonSnapObject> listSnapObjItemInList;
    [SerializeField] private bool isSetCanSnap = false;
    private int countSnapItemSnap = 0;
    private void OnStartStep1()
    {
        for (int i = 0; i < listSnapObjItemInList.Count; i++)
        {
            SonSnapObject obj = listSnapObjItemInList[i];
            obj.OnSnap.AddListener(() =>
            {
                countSnapItemSnap++;
                if (countSnapItemSnap >= listSnapObjItemInList.Count)
                {

                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }
    public void SetCanSnapObject()
    {
        if (isSetCanSnap == false)
        {
            foreach (SonSnapObject obj in listSnapObjItem)
            {
                obj.enabled = true;
                obj.Col.enabled = true;
            }
            isSetCanSnap = true;
        }
    }
    [SerializeField] private EmojiControl newEmoji;
    [SerializeField] private Transform tfStep2;
    [SerializeField] private float timeMove = 1.75f;
    [SerializeField] private List<TriggerWithCertainCollider> listTriggerWithCertainCollider1;
    [SerializeField] private List<TriggerWithCertainCollider> listTriggerWithCertainCollider2;
    private int countTrigger1 = 0;
    private int countTrigger2 = 0;

    private void OnStartStep2()
    {
        SetNewEmoji(newEmoji);
        tapPaper.EventCloseShoppingList();
        StartCoroutine(IE_DelayStartStep2());
        for (int i = 0; i < listTriggerWithCertainCollider1.Count; i++)
        {
            TriggerWithCertainCollider trigger = listTriggerWithCertainCollider1[i];
            trigger.AddTriggerEvent(() =>
            {
                countTrigger1++;
                if (countTrigger1 >= listTriggerWithCertainCollider1.Count)
                {
                    isDone1 = true;
                    CheckDoneStep2();
                }
            });
        }
        for (int i = 0; i < listTriggerWithCertainCollider2.Count; i++)
        {
            TriggerWithCertainCollider trigger = listTriggerWithCertainCollider2[i];
            trigger.AddTriggerEvent(() =>
            {
                countTrigger2++;
                if (countTrigger2 >= listTriggerWithCertainCollider2.Count)
                {
                    isDone2 = true;
                    CheckDoneStep2();
                }
            });
        }
    }
    private bool isDone1 = false;
    private bool isDone2 = false;

    private void CheckDoneStep2()
    {
        if (isDone1 && isDone2)
        {
            DoneStep();
            TryNextStep();
        }
    }
    IEnumerator IE_DelayStartStep2()
    {
        yield return new WaitForSeconds(0.75f);
        cam.transform.DOMoveX(tfStep2.position.x, timeMove);

    }
    [SerializeField] private SonSnapObject snapObjectTray;

    [SerializeField] private SonTurnOnOff lidOnOff;
    [SerializeField] private SpriteButtonOnOff btnOnOffOven;

    private void OnStartStep3()
    {
        snapObjectTray.enabled = true;
        snapObjectTray.Col.enabled = true;
        StartCoroutine(IE_DelayOpenOven());
        snapObjectTray.OnSnap.AddListener(() =>
        {
            StartCoroutine(IE_DelayCloseOven());
        });

    }

    [SerializeField] private SonSnapPoint snapPointTray;
    IEnumerator IE_DelayOpenOven()
    {
        yield return new WaitForSeconds(0.5f);
        snapPointTray.ChangeCanSnap(true);
        lidOnOff.ClickButton();
    }
    [SerializeField] private float timeOven = 3;
    [SerializeField] private ClockTimer timerLoNuong;
    [SerializeField] private OvenVibratorDOTween effectOven;
    [SerializeField] private ParticleSystem fvxFlour;
    [SerializeField] private ShowObjectEffect hideStep1;
    [SerializeField] private ShowObjectEffect showStep2;
    [SerializeField] private List<SpriteRenderer> listSpriteCakePink;
    [SerializeField] private List<SpriteRenderer> listSpriteCakeGreen;
    [SerializeField] private Sprite spriteCakePinkNew;
    [SerializeField] private Sprite spriteCakeGreenNew;
    [SerializeField] private SonSnapObject snapObjectTrayInOven;


    IEnumerator IE_DelayCloseOven()
    {
        yield return new WaitForSeconds(0.5f);
        lidOnOff.ClickButton();
        yield return new WaitForSeconds(0.5f);
        effectOven.StartVibration();
        btnOnOffOven.ClickButton();
        hideStep1.Hide();
        showStep2.Show(0.5f);
        timerLoNuong.OnTimeOut = () =>
            {
                foreach (SpriteRenderer sprite in listSpriteCakePink)
                {
                    sprite.sprite = spriteCakePinkNew;
                }
                foreach (SpriteRenderer sprite in listSpriteCakeGreen)
                {
                    sprite.sprite = spriteCakeGreenNew;
                }
                btnOnOffOven.ClickButton();
                fvxFlour.gameObject.SetActive(true);
                fvxFlour.Play();
                lidOnOff.ClickButton();
                effectOven.StopVibration();
                snapObjectTrayInOven.enabled = true;
                snapObjectTrayInOven.Col.enabled = true;


            };
        timerLoNuong.Show(timeOven);
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
