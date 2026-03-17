using DG.Tweening;
using Satisgame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using sonnv;
using UnityEngine.Events;

public class LevelMochi : LevelBase
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
        // EventManager.TriggerEvent(EventType.IncreaseProgress.ToString());
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
            case 2: break;
            case 3: break;
            case 4: break;
            case 5: break;
            case 6: break;
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

    [SerializeField] private TapProgressBar tapProgressBar;
    private void OnStartStep1()
    {
        tapProgressBar.EventStart();
        tapProgressBar.EventCompleteAll.AddListener(() =>
        {
            DoneStep();
            TryNextStep();
        });
    }
    [SerializeField] private ShowObjectEffect step1;
    [SerializeField] private Transform tfStep2;
    [SerializeField] private List<TrayItem> listItemColor;
    private int count = 0;
    private void OnStartStep2()
    {
        StartCoroutine(IE_DelayStep2());
        for (int i = 0; i < listItemColor.Count; i++)
        {
            TrayItem obj = listItemColor[i];
            obj.OnShow.AddListener(() =>
            {
                count++;
                if (count >= 2)
                {
                    GameManager.Ins.showEndGame();
                }
            });
        }
    }
    private void MoveCamera(
               Camera _cam,
               float targetLocalX,
               float duration,
               System.Action onComplete = null)
    {
        if (_cam == null) return;
        Sequence seq = DOTween.Sequence();
        seq.Join(_cam.transform.DOLocalMoveX(targetLocalX, duration));
        if (onComplete != null)
        {
            seq.OnComplete(() => onComplete?.Invoke());
        }
    }

    private IEnumerator IE_DelayStep2()
    {
        yield return new WaitForSeconds(1f);
        step1.Hide(0.5f);
        MoveCamera(cam, tfStep2.position.x, 1);
    }

}
