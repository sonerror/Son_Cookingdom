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
    public int CurrentStep => currentStep;
    private bool isDoneStep;
    public bool IsDoneStep => isDoneStep;
    public void SetStateDoneStep(bool _value)
    {
        isDoneStep = _value;
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
                OnStartStep5();
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
    public void OnStartStep1()
    {
        Debug.Log("Step1");

        TutorialManager.Ins.enableCountTime = true;
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
                int removedIndex = listSnapObject.IndexOf(obj);
                if (removedIndex < 0) return;
                listSnapObject.RemoveAt(removedIndex);
                TutorialManager.Ins.TfItem.RemoveAt(removedIndex);
                if (listSnapObject.Count <= 0)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }
    [SerializeField] private List<Collider2D> listBoxTapMove;
    [SerializeField] private List<FlourMoveToCream> moveToPlate;
    [SerializeField] private List<FlourMoveToCream> moveToBroad;
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
                TutorialManager.Ins.SetCanCutItem(false);
                countMoveDone++;
                if (countMoveDone >= moveToPlate.Count)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }


        for (int i = 0; i < moveToBroad.Count; i++)
        {
            FlourMoveToCream obj = moveToBroad[i];
            obj.onComplete.AddListener(() =>
            {
                TutorialManager.Ins.SetCanCutItem(true);
                int removedIndex = moveToBroad.IndexOf(obj);
                if (removedIndex < 0) return;
                moveToBroad.RemoveAt(removedIndex);
                TutorialManager.Ins.TfBroad.RemoveAt(removedIndex);
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
            GameManager.Ins.showEndGame();
            TutorialManager.Ins.enableCountTime = false;
        });
    }
    [SerializeField] private SonEffectShowObject effectHideStep1;
    [SerializeField] private SonEffectShowObject effectShowStep2;
    private void OnStartStep5()
    {
        StartCoroutine(ShowStep2Delay());
    }
    IEnumerator ShowStep2Delay()
    {
        yield return new WaitForSeconds(1f);
        effectHideStep1.Hide();
        effectShowStep2.Show(0.75f);
        TutorialManager.Ins.enableCountTime = true;


    }
}
