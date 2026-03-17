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

    [SerializeField] private EmojiControl emoji;

    private void SetNewEmoji(EmojiControl _emoji)
    {
        emoji = _emoji;
    }

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
            case 0: OnStartStep1(); break;
            case 1: OnStartStep2(); break;
            case 2: OnStartStep3(); break;
            case 3: OnStartStep4(); break;
            case 4: OnStartStep5(); break;
            case 5: OnStartStep6(); break;
            case 6: OnStartStep7(); break;
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

    [SerializeField] private List<SonSnapObject> listSnapObject;
    [SerializeField] private List<SonSnapPoint> listSnapPoint;
    [SerializeField] private SonSinkWaterCleaning sink;

    void OnDisable()
    {
        foreach (KeyValuePair<SonSnapObject, UnityAction> pair in snapActions)
        {
            if (pair.Key != null)
                pair.Key.OnSnap.RemoveListener(pair.Value);
        }
        snapActions.Clear();

        foreach (KeyValuePair<FlourMoveToCream, UnityAction> pair in plateActions)
        {
            if (pair.Key != null)
                pair.Key.onComplete.RemoveListener(pair.Value);
        }
        plateActions.Clear();

        if (sink != null)
        {
            sink.DrainPipe.OnPipeInHole.RemoveListener(OnPipeInHoleHandler);
            sink.OnFillWater.RemoveListener(OnFillWaterHandler);
            sink.DrainPipe.OnMoveBack.RemoveListener(OnMoveBackHandler);
        }

        if (bowl != null)
        {
            bowl.OnDone.RemoveListener(OnBowlDone);
        }
    }

    #region STEP1

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

    #region STEP2

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

            UnityAction action = delegate
            {
                OnSnapHandler(cache);
            };

            snapActions[cache] = action;
            cache.OnSnap.AddListener(action);
        }
    }

    private void OnSnapHandler(SonSnapObject obj)
    {
        if (snapActions.ContainsKey(obj))
        {
            obj.OnSnap.RemoveListener(snapActions[obj]);
            snapActions.Remove(obj);
        }

        int removedIndex = listSnapObject.IndexOf(obj);
        if (removedIndex < 0) return;

        listSnapObject.RemoveAt(removedIndex);

        //if (removedIndex < TutorialManager.Ins.TfItem.Count)
        //  TutorialManager.Ins.TfItem.RemoveAt(removedIndex);

        if (listSnapObject.Count <= 0)
        {
            DoneStep();
            TryNextStep();
        }
    }

    #endregion

    #region STEP3

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

        foreach (SonSnapObject obj in listSnapObjectInSink)
        {
            SonSnapObject cache = obj;

            UnityAction action = delegate
            {
                CuttingBoard.Ins.RegisterSnapDone(cache);
                int removedIndex = listSnapObjectInSink.IndexOf(cache);
                if (removedIndex < 0) return;

                listSnapObjectInSink.RemoveAt(removedIndex);
                // if (removedIndex < TutorialManager.Ins.TfBroad.Count)
                // TutorialManager.Ins.TfBroad.RemoveAt(removedIndex);
            };

            cache.OnSnap.AddListener(action);
            snapActions[cache] = action;
        }

        foreach (FlourMoveToCream obj in listMoveObj)
        {
            FlourMoveToCream cache = obj;

            UnityAction action = delegate
            {
                int removedIndex = listMoveObj.IndexOf(cache);
                if (removedIndex < 0) return;

                listMoveObj.RemoveAt(removedIndex);

                if (listMoveObj.Count <= 0)
                {
                    TutorialManager.Ins.enableCountTime = false;

                    DoneStep();
                    TryNextStep();
                }
            };

            cache.onComplete.AddListener(action);
            plateActions[cache] = action;
        }
    }

    #endregion

    #region STEP4

    [SerializeField] private List<ShowObjectEffect> listEffectStep3;
    [SerializeField] private List<SonSnapPoint> listSnapPointSauce;
    [SerializeField] private List<SonSnapObject> listSnapObjSauce;
    [SerializeField] private EmojiControl emojiStep4;

    private int countOnMoveBack = 0;

    private void OnStartStep4()
    {
        SetNewEmoji(emojiStep4);
        effectBoard.Hide();

        foreach (ShowObjectEffect effect in listEffectStep3)
            effect.Show(1f);

        listEffectStep3[0].onShowComplete.AddListener(OnShowEffectComplete);

        TutorialManager.Ins.enableCountTime = false;

        foreach (SonSnapPoint point in listSnapPointSauce)
            point.ChangeCanSnap(true);

        foreach (SonSnapObject obj in listSnapObjSauce)
        {
            SonSnapObject cache = obj;

            UnityAction action = () => OnSnapSauce(cache);

            cache.OnTrans.AddListener(action);
            snapActions[cache] = action;
        }
    }
    private void OnShowEffectComplete()
    {
        TutorialManager.Ins.enableCountTime = true;
    }
    private void OnSnapSauce(SonSnapObject obj)
    {
        if (snapActions.TryGetValue(obj, out UnityAction action))
        {
            obj.OnSnap.RemoveListener(action);
            snapActions.Remove(obj);
        }

        listSnapObjSauce.Remove(obj);

        //TutorialManager.Ins.TfSauce.Remove(obj.transform);

        if (listSnapObjSauce.Count == 0)
        {
            DoneStep();
            TryNextStep();
        }
    }
    #endregion

    #region STEP5

    [SerializeField] private SonSnapPoint snapPointSpoon;
    [SerializeField] private BowlMixer bowl;

    private void OnStartStep5()
    {
        snapPointSpoon.ChangeCanSnap(true);
        bowl.OnDone.AddListener(OnBowlDone);
    }

    private void OnBowlDone()
    {
        DoneStep();
        TryNextStep();
    }

    #endregion

    #region STEP6

    private void OnStartStep6()
    {
        sink.DrainPipe.SetInteract(true);
        sink.DrainPipe.OnMoveBack.AddListener(OnMoveBackHandler);
    }

    private void OnMoveBackHandler()
    {
        sink.DrainPipe.OnMoveBack.RemoveListener(OnMoveBackHandler);
        DoneStep();
        TryNextStep();
    }

    #endregion

    #region STEP7

    [SerializeField] private ShowObjectEffect effectHideStep1;
    [SerializeField] private ShowObjectEffect effectShowStep2;
    [SerializeField] private List<SonSnapObject> listSnapObjIng;

    private void OnStartStep7()
    {
        effectHideStep1.Hide(1);
        effectShowStep2.Show(2f);

        foreach (SonSnapObject obj in listSnapObjIng)
        {
            SonSnapObject cache = obj;

            UnityAction action = delegate
            {
                int removedIndex = listSnapObjIng.IndexOf(cache);
                if (removedIndex < 0) return;

                listSnapObjIng.RemoveAt(removedIndex);
                //  if (removedIndex < TutorialManager.Ins.LisTfIng.Count)
                // TutorialManager.Ins.LisTfIng.RemoveAt(removedIndex);
                GameManager.Ins.showEndGame();
            };

            cache.OnSnap.AddListener(action);
            snapActions[cache] = action;
        }
    }

    #endregion

    [SerializeField] private AudioClip plateMove;

    public void OnPlaySFXPlateMove()
    {
        SoundManager.PlaySFXOneShot(plateMove);
    }

    [SerializeField] private AudioClip sfxTrans;

    public void OnPlaySFXPTransSauce()
    {
        SoundManager.PlaySFXOneShot(sfxTrans);
    }
}