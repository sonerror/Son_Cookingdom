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
    [SerializeField] private List<SonSnapObject> listSnapObject;
    public void BlockObject()
    {
        foreach (SonSnapObject obj in listSnapObject)
        {
            if (obj != null)
            {
                obj.enabled = false;
                obj.Col.enabled = false;
            }
        }
    }

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
        TutorialManager.Ins.SetNewTime(0);
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
                TutorialManager.Ins.SetFailCount();
                OnStartStep0();
                break;
            case 1:
                OnStartStep1();
                break;
            case 2:
                OnStartStep2();
                break;
            case 3:
                OnStartStep3();
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

    #region Step 0
    [SerializeField] private List<SonAutoMoveToTarget> listSonAutoMoveToTarget;
    [SerializeField] private SonTapItem sonTapItem;
    private int countStep1 = 0;
    private void OnStartStep0()
    {
        sonTapItem.eventOnPointDown.AddListener(() =>
        {
            TutorialManager.Ins.SetNewTime(100f);
        });
        for (int i = 0; i < listSonAutoMoveToTarget.Count; i++)
        {
            SonAutoMoveToTarget obj = listSonAutoMoveToTarget[i];
            obj.onComplete.AddListener(() =>
            {
                countStep1++;
                if (countStep1 >= listSonAutoMoveToTarget.Count)
                {
                    TutorialManager.Ins.SetNewTime(10f);
                    TutorialManager.Ins.ReSetFailCount();
                    DoneStep();
                    TryNextStep();
                }
            });
        }
    }


    #endregion

    #region Step 1
    [SerializeField] private List<SonSnapPoint> listSnapPointStep1FlourLeft;
    [SerializeField] private SonSnapPoint snapPointMayonaise;
    [SerializeField] private List<SonSnapObject> listSnapObjStep1FlourLeft;

    [SerializeField] private List<SonSnapPoint> listSnapPointStep1FlourR;
    [SerializeField] private SnapPoint snapPointAvocado;
    [SerializeField] private List<SonSnapObject> listSnapObjStep1FlourR;
    [SerializeField] private List<SonSnapObject> listSnapObjHint;

    [SerializeField] private SpoonIngredient snapObjAvocado;
    [SerializeField] private SonSnapObject snapObjMayonaise;

    private int countSnapLeft = 0;
    private int countSnapRight = 0;

    private bool isSnapMayonaise = false;
    private bool isSnapWasabi = false;
    private bool isSnapLeft = false;
    private bool isSnapRight = false;
    private int coutSnapHintFirst = 0;
    // Thêm [UnityEngine.Scripting.Preserve] để tránh bị strip
    [UnityEngine.Scripting.Preserve]
    private void OnCheckDoneStep1()
    {
        if (isSnapMayonaise && isSnapWasabi && isSnapLeft && isSnapRight)
        {
            DoneStep();
            TryNextStep();
        }
    }

    private void OnStartStep1()
    {
        TutorialManager.Ins.SetNewTime(0.75f);
        TutorialManager.Ins.SetFailCount();

        // Cache 'this' reference để tránh lỗi closure trong WebGL/IL2CPP
        LevelControl self = this;

        snapObjAvocado.onSnap.AddListener(() =>
        {
            TutorialManager.Ins.OnCollectSuccess();
            self.isSnapWasabi = true;
            self.OnCheckDoneStep1();
        });

        snapObjMayonaise.OnSnap.AddListener(() =>
        {
            TutorialManager.Ins.OnCollectSuccess();
            self.isSnapMayonaise = true;
            self.OnCheckDoneStep1();
            ShowHintSpoon();
            //StartCoroutine(IE_DelayShowHint());
        });
        snapObjMayonaise.OnStartTrans.AddListener(() =>
       {
           TutorialManager.Ins.SetNewTime(100f);
           TutorialManager.Ins.ReSetFailCount();
       });

        foreach (SonSnapPoint snapPoint in listSnapPointStep1FlourLeft)
        {
            snapPoint.ChangeCanSnap(true);
        }

        for (int i = 0; i < listSnapObjStep1FlourLeft.Count; i++)
        {
            SonSnapObject obj = listSnapObjStep1FlourLeft[i];
            obj.OnSnap.AddListener(() =>
            {
                coutSnapHintFirst++;
                if (coutSnapHintFirst >= 2)
                {
                    TutorialManager.Ins.OnCollectSuccess();
                }
                self.countSnapLeft++;
                if (self.countSnapLeft >= self.listSnapObjStep1FlourLeft.Count)
                {
                    self.snapPointMayonaise.ChangeCanSnap(true);
                    self.isSnapLeft = true;
                    self.OnCheckDoneStep1();
                }
            });
        }

        foreach (SonSnapPoint snapPoint in listSnapPointStep1FlourR)
        {
            snapPoint.ChangeCanSnap(true);
        }

        for (int i = 0; i < listSnapObjStep1FlourR.Count; i++)
        {
            SonSnapObject obj = listSnapObjStep1FlourR[i];
            obj.OnSnap.AddListener(() =>
            {
                TutorialManager.Ins.OnCollectSuccess();
                self.countSnapRight++;
                if (self.countSnapRight >= self.listSnapObjStep1FlourR.Count)
                {
                    TrySetIsSnapSpoon();
                    self.snapPointAvocado.ChangeCanSnap(true);
                    self.isSnapRight = true;
                    self.OnCheckDoneStep1();
                    ShowHintSpoon();
                }
            });
        }

        for (int i = 0; i < listSnapObjHint.Count; i++)
        {
            SonSnapObject obj = listSnapObjHint[i];
            obj.OnSnap.AddListener(() =>
            {
                int removedIndex = self.listSnapObjHint.IndexOf(obj);
                if (removedIndex < 0) return;
                self.listSnapObjHint.RemoveAt(removedIndex);
                TutorialManager.Ins.ListSnapObjHint.RemoveAt(removedIndex);
                TutorialManager.Ins.ListTargetStep1.RemoveAt(removedIndex);
            });
        }
    }
    private void ShowHintSpoon()
    {
        if (isSnapMayonaise && isSnapRight)
        {
            TutorialManager.Ins.SetNewTime(3f);
            TutorialManager.Ins.SetFailCount();
        }
    }
    IEnumerator IE_DelayShowHint()
    {
        yield return new WaitForSeconds(0.75f);
        if (listSnapObjHint.Count >= 5)
        {
            TutorialManager.Ins.SetNewTime(0.75f);
            TutorialManager.Ins.SetFailCount();
        }
    }
    // Tách riêng để dễ kiểm soát, tránh crash nếu method bị strip
    [UnityEngine.Scripting.Preserve]
    private void TrySetIsSnapSpoon()
    {
        try
        {
            TutorialManager.Ins.SetIsSnapSpoon();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[LevelControl] TrySetIsSnapSpoon failed: " + e.Message);
        }
    }
    #endregion
    #region Step 2
    [SerializeField] private List<ShowObjectEffect> listShowObjectEffect;
    [SerializeField] private Transform tfPlate;
    [SerializeField] private Transform target;
    [SerializeField] private float targetOrthoSize;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease easeMove = Ease.OutQuad;
    [SerializeField] private Ease easeOrtho = Ease.OutQuad;


    private void OnStartStep2()
    {
        StartCoroutine(IE_OnStartStep2());
    }

    IEnumerator IE_OnStartStep2()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (ShowObjectEffect show in listShowObjectEffect)
        {
            show.Hide();
            yield return new WaitForSeconds(0.075f);
        }
        tfPlate.DOMoveY(target.position.y, 0.3f).OnComplete(() =>
        {
            MoveToTarget();
        });
    }

    public void MoveToTarget()
    {
        transform.DOMove(target.position, duration).SetEase(easeMove);
        DOTween.To(
            () => cam.orthographicSize,
            x => cam.orthographicSize = x,
            targetOrthoSize,
            duration
        ).SetEase(easeOrtho).OnComplete(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    #endregion
    #region Step 3
    private void OnStartStep3()
    {
        StartCoroutine(IE_OnStartStep3());
    }

    IEnumerator IE_OnStartStep3()
    {
        SoundManager.Ins.Mute();
        yield return new WaitForSeconds(1.25f);
        UIManager.Instance.CloseUIGamePlay();
        UIManager.Instance.LoadUIWin();
        GameManager.Ins.showEndGame();
    }
    #endregion
}
