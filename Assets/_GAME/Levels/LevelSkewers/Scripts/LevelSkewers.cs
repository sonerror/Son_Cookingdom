using DG.Tweening;
using Satisgame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using sonnv;
using UnityEngine.Events;
public class LevelSkewers : LevelBase
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

    public void SetStateDoneStep(bool value)
    {
        isDoneStep = value;
    }

    public EmojiControl emoji;

    public static int maxLayer = 30;

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
                break;
            case 4:
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

    public void ShowNegativeEmojiAtPos(Vector3 position)
    {
        emoji.transform.position = position + Vector3.up * 0.5f + Vector3.left * 0.5f;
        emoji.ShowNegative();
    }

    public void ShowNegativeKnife()
    {
        emoji.ShowNegative();
    }

    public void ShowPositiveEmojiAtPos(Vector3 position)
    {
        emoji.transform.position = position + Vector3.up * 0.5f + Vector3.left * 0.5f;
        emoji.ShowPositive();
    }

    private Dictionary<SonSnapObject, UnityAction> snapActions = new Dictionary<SonSnapObject, UnityAction>();
    private Dictionary<FlourMoveToCream, UnityAction> plateActions = new Dictionary<FlourMoveToCream, UnityAction>();
    private Dictionary<FlourMoveToCream, UnityAction> broadActions = new Dictionary<FlourMoveToCream, UnityAction>();

    [SerializeField] private List<SonSnapObject> listSnapObject;
    [SerializeField] private List<SonSnapPoint> listSnapPoint;
    [SerializeField] private SonSinkWaterCleaning sink;


    void OnDisable()
    {
        snapActions.Clear();
        plateActions.Clear();
        broadActions.Clear();
    }

    #region STEP 1

    private void OnPipeInHoleHandler()
    {
        sink.DrainPipe.SetInteract(false);
        sink.DrainPipe.OnPipeInHole.RemoveListener(OnPipeInHoleHandler);
    }

    private void OnFillWaterHandler()
    {
        sink.OnFillWater.RemoveListener(OnFillWaterHandler);

        DoneStep();
        TryNextStep();
    }

    public void OnStartStep1()
    {
        Debug.Log("Step1");

        TutorialManager.Ins.enableCountTime = true;

        sink.DrainPipe.OnPipeInHole.AddListener(OnPipeInHoleHandler);
        sink.OnFillWater.AddListener(OnFillWaterHandler);
    }

    #endregion

    #region STEP 2

    private void OnStartStep2()
    {
        SetSnapObject();
    }

    private void SetSnapObject()
    {
        foreach (SonSnapPoint point in listSnapPoint)
            point.ChangeCanSnap(true);

        foreach (SonSnapObject obj in listSnapObject)
        {
            SonSnapObject cache = obj;

            UnityAction action = () => OnSnapHandler(cache);
            snapActions[cache] = action;

            cache.OnSnap.AddListener(action);
        }
    }

    private void OnSnapHandler(SonSnapObject obj)
    {
        UnityAction action;

        if (snapActions.TryGetValue(obj, out action))
        {
            obj.OnSnap.RemoveListener(action);
            snapActions.Remove(obj);
        }

        int removedIndex = listSnapObject.IndexOf(obj);
        if (removedIndex < 0) return;

        listSnapObject.RemoveAt(removedIndex);

        if (removedIndex < TutorialManager.Ins.TfItem.Count)
            TutorialManager.Ins.TfItem.RemoveAt(removedIndex);

        if (listSnapObject.Count <= 0)
        {
            DoneStep();
            TryNextStep();
        }
    }

    #endregion

    #region STEP 3
    [SerializeField] private ShowObjectEffect effectBox;

    [SerializeField] private ShowObjectEffect effectBoard;
    [SerializeField] private ShowObjectEffect effectKinfe;
    [SerializeField] private List<SonSnapObject> listSnapObjectInSink;
    [SerializeField] private List<FlourMoveToCream> listMoveObj;

    private void OnStartStep3()
    {
        effectBox.Hide();
        effectBoard.Show(1);
        effectKinfe.Show(1);
        for (int i = 0; i < listSnapObjectInSink.Count; i++)
        {
            SonSnapObject obj = listSnapObjectInSink[i];
            obj.OnSnap.AddListener(() =>
            {
                CuttingBoard.Ins.RegisterSnapDone(obj);
            });
        }
        for (int i = 0; i < listMoveObj.Count; i++)
        {
            FlourMoveToCream obj = listMoveObj[i];
            obj.onComplete.AddListener(() =>
            {
                int removedIndex = listMoveObj.IndexOf(obj);
                if (removedIndex < 0) return;

                listMoveObj.RemoveAt(removedIndex);
                if (listMoveObj.Count <= 0)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }
    #endregion
}
