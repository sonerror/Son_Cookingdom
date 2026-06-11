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
    [SerializeField] private SonSnapPoint snapPointFlour;
    [SerializeField] private SonSnapObject snapObjectFlour;
    private void OnStartStep1()
    {
        TutorialManager.Ins.enableCountTime = true;
        snapPointFlour.ChangeCanSnap(true);
        snapObjectFlour.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion


    #region Step 2
    [SerializeField] private List<AnimControl> listAnimEgg;
    [SerializeField] private List<SonSnapPoint> listSnapPointEgg;
    [SerializeField] private List<SonSnapObject> listSnapObjtEgg;
    private int countSnapAnim = 0;
    private void OnStartStep2()
    {
        foreach (var snap in listSnapPointEgg)
        {
            snap.ChangeCanSnap(true);
        }
        for (int i = 0; i < listSnapObjtEgg.Count; i++)
        {
            SonSnapObject obj = listSnapObjtEgg[i];
            obj.OnSnap.AddListener(() =>
            {
                countSnapAnim++;
                if (countSnapAnim == 1)
                {
                    TutorialManager.Ins.SetNewTime(4f);
                }

            });
        }
        for (int i = 0; i < listAnimEgg.Count; i++)
        {
            AnimControl obj = listAnimEgg[i];
            obj.onDoneAnim.AddListener(() =>
            {
                int removedIndex = listAnimEgg.IndexOf(obj);
                if (removedIndex < 0) return;
                listAnimEgg.RemoveAt(removedIndex);
                TutorialManager.Ins.ListTfEgg.RemoveAt(removedIndex);
                countSnapAnim++;
                if (countSnapAnim >= 1)
                {
                    TutorialManager.Ins.SetNewTime(4f);
                }
                // TutorialManager.Ins.ListThrowClothes.RemoveAt(removedIndex);
                if (listAnimEgg.Count == 0)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }

    #endregion

    #region Step 3
    [SerializeField] private SonSnapPoint snapPointCornstack;
    [SerializeField] private SonSnapObject snapObjectCornstack;
    private void OnStartStep3()
    {
        TutorialManager.Ins.SetNewTime(0.5f);
        snapPointCornstack.ChangeCanSnap(true);
        snapObjectCornstack.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 4
    [SerializeField] private SonSnapPoint snapPointSugar;
    [SerializeField] private SonSnapObject snapObjectSugar;
    private void OnStartStep4()
    {
        TutorialManager.Ins.SetNewTime(4f);

        snapPointSugar.ChangeCanSnap(true);
        snapObjectSugar.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 5
    [SerializeField] private SonSnapPoint snapPointWhisk;
    [SerializeField] private FlatSpoonInPan flatSpoonInPan;
    private void OnStartStep5()
    {
        snapPointWhisk.ChangeCanSnap(true);
        flatSpoonInPan.onEndRotate.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 6
    [SerializeField] private SpriteButtonOnOff spriteButtonOnOff;
    [SerializeField] private GameObject objBowlDrag;
    [SerializeField] private GameObject objBowlBase;
    [SerializeField] private EmojiControl newEmoji;
    private void OnStartStep6()
    {
        SetNewEmoji(newEmoji);
        objBowlBase.SetActive(false);
        objBowlDrag.SetActive(true);
        spriteButtonOnOff.SetStateIsBlock(false);
        spriteButtonOnOff.onClickOn.AddListener(() =>
        {
            StartCoroutine(IE_DelayStartStep6());

        });
    }

    [SerializeField] private ClockTimer clockTimer;
    [SerializeField] private List<SpriteRenderer> dimSprites;
    [SerializeField] private List<SpriteRenderer> brightSprites;

    [SerializeField] private float fadeDuration = 3f;
    IEnumerator IE_DelayStartStep6()
    {
        TutorialManager.Ins.enableCountTime = false;
        yield return new WaitForSeconds(0.5f);

        clockTimer.OnTimeOut = OnStep6TimeOut;
        clockTimer.Show(fadeDuration);

        foreach (var sr in dimSprites)
            sr.DOFade(0f, fadeDuration).SetEase(Ease.InOutSine);

        foreach (var sr in brightSprites)
            sr.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
    }

    private void OnStep6TimeOut()
    {
        DoneStep();
        TryNextStep();

    }
    #endregion
    #region Step 7
    [SerializeField] private SonSnapPoint snapPointBowl;
    [SerializeField] private SonSnapObject snapObjectBowl;
    private void OnStartStep7()
    {
        TutorialManager.Ins.enableCountTime = true;

        snapPointBowl.ChangeCanSnap(true);
        snapObjectBowl.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 8
    [SerializeField] private List<SpriteRenderer> oldSprites;
    [SerializeField] private List<SpriteRenderer> newSprites;
    private void OnStartStep8()
    {
        StartCoroutine(IE_DelayStartStep8());
    }
    IEnumerator IE_DelayStartStep8()
    {
        TutorialManager.Ins.enableCountTime = false;

        yield return new WaitForSeconds(0.5f);

        clockTimer.OnTimeOut = OnStep8TimeOut;
        clockTimer.Show(fadeDuration);

        foreach (var sr in oldSprites)
            sr.DOFade(0f, fadeDuration).SetEase(Ease.InOutSine);

        foreach (var sr in newSprites)
            sr.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
    }
    private void OnStep8TimeOut()
    {
        DoneStep();
        TryNextStep();
        spriteButtonOnOff.ClickButton();
    }
    #endregion
    #region Step 9
    [SerializeField] private SonSnapPoint snapPointNilong;
    [SerializeField] private SonSnapObject snapObjectNilong;
    private void OnStartStep9()
    {
        TutorialManager.Ins.enableCountTime = true;

        snapPointNilong.ChangeCanSnap(true);
        snapObjectNilong.OnSnap.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
            TutorialManager.Ins.enableCountTime = false;

        });
    }
    #endregion
    #region Step 10
    private void OnStartStep10()
    {
        StartCoroutine(IE_DelayStartStep10());
    }
    IEnumerator IE_DelayStartStep10()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Ins.CloseUI(UIID.GamePlayScreen);
        UIManager.Ins.GetUI(UIID.ChangeLevelScreen);
    }
    #endregion
}
