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
                TutorialManager.Ins.SetNewTime(3f);
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
            case 10:
                OnStartStep11();
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
        TutorialManager.Ins.enableCountTime = true;
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
                TutorialManager.Ins.ListEgg.RemoveAt(removedIndex);
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
                TutorialManager.Ins.ListBoltSaltSugar.RemoveAt(removedIndex);
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
                TutorialManager.Ins.ListBolt.RemoveAt(removedIndex);
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
                TutorialManager.Ins.enableCountTime = false;
            });
    }
    [SerializeField] private Transform tfStage2;
    [SerializeField] private float timeMoveToTargetStage2 = 0.75f;
    [SerializeField] private SonSnapObject snapObjOil;
    [SerializeField] private SonSnapPoint snapPointOil;
    [SerializeField] private EmojiControl emojiStage2;

    private void OnStartStep6()
    {
        SetNewEmoji(emojiStage2);
        StartCoroutine(IE_MoveCamToStage2());
    }
    private IEnumerator IE_MoveCamToStage2()
    {
        yield return new WaitForSeconds(1.25f);
        cam.transform.DOMoveX(tfStage2.position.x, timeMoveToTargetStage2).OnComplete(() =>
       {
           TutorialManager.Ins.enableCountTime = true;
           snapPointOil.ChangeCanSnap(true);
           snapObjOil.OnTrans.AddListener(() =>
           {
               DoneStep();
               TryNextStep();
           });
       });
    }
    [SerializeField] private SpriteButtonOnOff buttonOnOff;

    private void OnStartStep7()
    {
        buttonOnOff.enabled = true;
        buttonOnOff.Col.enabled = true;
        buttonOnOff.onClickOn.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    [SerializeField] private SonSnapObject snapObjBoltEgg;
    [SerializeField] private SonSnapPoint snapPointBoltEgg;
    private void OnStartStep8()
    {
        snapPointBoltEgg.ChangeCanSnap(true);
        snapObjBoltEgg.OnTrans.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    [SerializeField] private List<SonSnapObject> listSnapObjInPan;
    [SerializeField] private List<SonSnapPoint> listSnapPointInPan;
    private void OnStartStep9()
    {
        foreach (SonSnapPoint point in listSnapPointInPan)
        {
            point.ChangeCanSnap(true);
        }
        for (int i = 0; i < listSnapObjInPan.Count; i++)
        {
            SonSnapObject obj = listSnapObjInPan[i];
            obj.OnSnap.AddListener(() =>
            {
                int removedIndex = listSnapObjInPan.IndexOf(obj);
                if (removedIndex < 0) return;
                listSnapObjInPan.RemoveAt(removedIndex);
                TutorialManager.Ins.ListItemInPan.RemoveAt(removedIndex);
                if (listSnapObjInPan.Count == 0)
                {
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }
    [SerializeField] private SpoonIngredient spoonSalt;
    [SerializeField] private SnapPoint spoonPointSalt;
    private void OnStartStep10()
    {
        spoonPointSalt.ChangeCanSnap(true);
        spoonSalt.onSnap.AddListener(() =>
            {
                DoneStep();
                TryNextStep();
                TutorialManager.Ins.enableCountTime = false;
            });
    }
    [SerializeField] private ClockTimer time;
    [SerializeField] private List<SpriteRenderer> spriteRendererOut;
    [SerializeField] private List<SpriteRenderer> spriteRendererIn;
    private Sequence _crossfadeSeq;

    private void OnStartStep11()
    {
        TutorialManager.Ins.SetNewTime(0.5f);

        StartCoroutine(IE_DelayStep11());
    }
    IEnumerator IE_DelayStep11()
    {
        yield return new WaitForSeconds(1f);
        float duration = 3f;
        time.OnTimeOut = () =>
        {
            DoneStep();
            TryNextStep();
            buttonOnOff.ClickButton();
            GameManager.Ins.showEndGame();
            TutorialManager.Ins.enableCountTime = true;
        };
        time.Show(duration);
        CrossfadeSprites(duration);
    }
    private void CrossfadeSprites(float duration)
    {
        _crossfadeSeq?.Kill();
        _crossfadeSeq = DOTween.Sequence();
        float timeFadeIn = duration * 0.6f;
        float timeFadeOut = duration * 0.4f;
        foreach (var spr in spriteRendererIn)
        {
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0f);
            spr.gameObject.SetActive(true);
            _crossfadeSeq.Insert(0, spr.DOFade(1f, timeFadeIn).SetEase(Ease.OutQuad));
        }

        foreach (var spr in spriteRendererOut)
        {
            _crossfadeSeq.Insert(timeFadeIn, spr.DOFade(0f, timeFadeOut).SetEase(Ease.Linear));
            _crossfadeSeq.InsertCallback(duration, () => spr.gameObject.SetActive(false));
        }
    }
}
