using DG.Tweening;
using Satisgame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using sonnv;
public class Level1528 : LevelBase
{
    protected override void Awake()
    {
        base.Awake();

        maxLayer = 30;
    }
    protected virtual void Start()
    {
        StartStep();
    }
    [SerializeField] private int currentStep = 0;
    private bool isDoneStep;
    public bool IsDoneStep => isDoneStep;
    public void SetStateDoneStep(bool _value)
    {
        isDoneStep = _value;
    }
    public EmojiControl emoji;
    public static int maxLayer = 30;
    protected virtual void DoneStep()
    {
        emoji.ShowPositive();
        isDoneStep = true;
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
                return;
            case 1:
                OnStartStep2();
                return;
            case 2:
                OnStartStep3();
                return;
            case 3:
                OnStartStep4();
                return;
            case 4:
                return;
            case 5:
                return;
            case 6:
                return;
            case 7:
                return;
            default:
                return;
        }
    }
    public void OnCompleteStage(int currentStageIndex, float x)
    {
        emoji.ShowPositive();
        Camera.main.transform.DOMoveX(x, 1f).SetDelay(1f);
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

    public void ShowPositiveEmojiAtPos(Vector3 position)
    {
        emoji.transform.position = position + Vector3.up * 0.5f + Vector3.left * 0.5f;
        emoji.ShowPositive();
    }
    [SerializeField] private List<SonSnapObject> listSnapObject;

    [SerializeField] private List<SonSnapPoint> listSnapPoint;

    [SerializeField] private SonSinkWaterCleaning sink;
    public void OnStartStep1()
    {
        sink.OnFillWater.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    private void OnStartStep2()
    {
        SetSnapObject();
    }
    private int countSnap = 0;
    private void SetSnapObject()
    {
        foreach (SonSnapPoint point in listSnapPoint)
        {
            point.ChangeCanSnap(true);
        }
        for (int i = 0; i < listSnapObject.Count; i++)
        {
            SonSnapObject obj = listSnapObject[i];
            obj.OnSnap.AddListener(() =>
            {
                countSnap++;
                if (countSnap >= listSnapObject.Count)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }
    [SerializeField] private List<Collider2D> listBoxTapMove;
    [SerializeField] private List<FlourMoveToCream> moveToPlate;
    private int countMoveDone = 0;
    private void OnStartStep3()
    {

        foreach (Collider2D obj in listBoxTapMove)
        {
            obj.enabled = true;
        }
        for (int i = 0; i < moveToPlate.Count; i++)
        {
            FlourMoveToCream obj = moveToPlate[i];
            obj.onComplete.AddListener(() =>
            {
                countMoveDone++;
                if (countMoveDone >= moveToPlate.Count)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }

    private void OnStartStep4()
    {
        sink.DrainPipe.SetInteract(true);
        sink.DrainPipe.OnMoveBack.AddListener(() =>
        {
            Debug.Log("Nap Move Back");
            DoneStep();
            TryNextStep();
        });
    }
}
