using DG.Tweening;
using Satisgame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using sonnv;
using UnityEngine.Events;

public class Level1528 : LevelBase
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
    public EmojiControl emojiKnife;

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
                OnStartStep4();
                break;
            case 4:
                OnStartStep5();
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
        emojiKnife.transform.position = position + Vector3.up * 0.5f + Vector3.left * 0.5f;
        emojiKnife.ShowNegative();
    }

    public void ShowNegativeKnife()
    {
        emojiKnife.ShowNegative();
    }

    public void ShowPositiveEmojiAtPos(Vector3 position)
    {
        emoji.transform.position = position + Vector3.up * 0.5f + Vector3.left * 0.5f;
        emoji.ShowPositive();
    }

    [SerializeField] private List<SonSnapObject> listSnapObject;
    [SerializeField] private List<SonSnapPoint> listSnapPoint;
    [SerializeField] private SonSinkWaterCleaning sink;

    private Dictionary<SonSnapObject, UnityAction> snapActions = new Dictionary<SonSnapObject, UnityAction>();
    private Dictionary<FlourMoveToCream, UnityAction> plateActions = new Dictionary<FlourMoveToCream, UnityAction>();
    private Dictionary<FlourMoveToCream, UnityAction> broadActions = new Dictionary<FlourMoveToCream, UnityAction>();

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

    [SerializeField] private List<Collider2D> listBoxTapMove;
    [SerializeField] private List<FlourMoveToCream> moveToPlate;
    [SerializeField] private List<FlourMoveToCream> moveToBroad;

    private int countMoveDone = 0;

    private void OnStartStep3()
    {
        foreach (Collider2D obj in listBoxTapMove)
            obj.enabled = true;

        foreach (FlourMoveToCream obj in moveToPlate)
        {
            FlourMoveToCream cache = obj;

            UnityAction action = () => OnMoveToPlateComplete(cache);
            plateActions[cache] = action;

            cache.onComplete.AddListener(action);
        }

        foreach (FlourMoveToCream obj in moveToBroad)
        {
            FlourMoveToCream cache = obj;

            UnityAction action = () => OnMoveToBroadComplete(cache);
            broadActions[cache] = action;

            cache.onComplete.AddListener(action);
        }
    }

    private void OnMoveToPlateComplete(FlourMoveToCream obj)
    {
        UnityAction action;

        if (plateActions.TryGetValue(obj, out action))
        {
            obj.onComplete.RemoveListener(action);
            plateActions.Remove(obj);
        }

        countMoveDone++;

        TutorialManager.Ins.SetCanItemInBroad(false);

        if (countMoveDone >= moveToPlate.Count)
        {
            DoneStep();
            TryNextStep();
        }
    }

    private void OnMoveToBroadComplete(FlourMoveToCream obj)
    {
        UnityAction action;

        if (broadActions.TryGetValue(obj, out action))
        {
            obj.onComplete.RemoveListener(action);
            broadActions.Remove(obj);
        }

        if (CuttingBoard.Instance != null)
        {
            CuttingBoard.Instance.RegisterMoveDone(obj);
        }

        int removedIndex = moveToBroad.IndexOf(obj);
        if (removedIndex < 0) return;

        moveToBroad.RemoveAt(removedIndex);

        if (removedIndex < TutorialManager.Ins.TfBroad.Count)
            TutorialManager.Ins.TfBroad.RemoveAt(removedIndex);
    }

    #endregion

    #region STEP 4

    private void OnStartStep4()
    {
        sink.DrainPipe.SetInteract(true);
        sink.DrainPipe.OnMoveBack.AddListener(OnMoveBackHandler);
    }

    private void OnMoveBackHandler()
    {
        sink.DrainPipe.OnMoveBack.RemoveListener(OnMoveBackHandler);

        Debug.Log("Nap Move Back");

        DoneStep();
        TryNextStep();

        GameManager.Ins.showEndGame();
        TutorialManager.Ins.enableCountTime = false;
    }

    #endregion

    #region STEP 5

    [SerializeField] private SonEffectShowObject effectHideStep1;
    [SerializeField] private SonEffectShowObject effectShowStep2;

    private void OnStartStep5()
    {
        StartCoroutine(ShowStep2Delay());
    }

    IEnumerator ShowStep2Delay()
    {
        yield return null;
        yield return new WaitForSeconds(1f);

        effectHideStep1.Hide();
        effectShowStep2.Show(0.75f);

        TutorialManager.Ins.enableCountTime = true;
    }

    #endregion
}