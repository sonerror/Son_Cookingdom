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
                //  OnStartStep2();
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
    [SerializeField] private List<SonSnapObject> listSnapObject;
    [SerializeField] private CuttingBoard cuttingBoard;
    private void OnStartStep1()
    {
        foreach (SonSnapObject obj in listSnapObject)
        {
            SonSnapObject cache = obj;

            UnityAction action = delegate
            {
                cuttingBoard.RegisterSnapDone(cache);
                int removedIndex = listSnapObject.IndexOf(cache);
                if (removedIndex < 0) return;

                listSnapObject.RemoveAt(removedIndex);
                //if (removedIndex < TutorialManager.Ins.TfBroad.Count)
                // TutorialManager.Ins.TfBroad.RemoveAt(removedIndex);
            };

            cache.OnSnap.AddListener(action);
            snapActions[cache] = action;
        }
    }
}
