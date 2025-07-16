
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private HandCtrl handCtrl;
    [SerializeField] float TimeHint = 5f;
    private float timeCountHint = 0f;

    public float timeEndGame = 30f;
    private bool isClickTrueItem = false;



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ResetTimeHint();
            isClickTrueItem = true;
        }

        if (Input.GetMouseButton(0)) return;

        CalculateTimeHint();
        if (GameManager.Ins.isEndGame) return;
        if (!isClickTrueItem) return;
        timeEndGame -= Time.deltaTime;
        if (timeEndGame <= 0)
        {
            GameManager.Ins.isEndGame = true;
        }
    }

    private bool isShowHint = true;
    private bool enableCountTime = true;
    private void CalculateTimeHint()
    {
        if (!enableCountTime) return;
        if (isShowHint) return;
        timeCountHint -= Time.deltaTime;
        if (timeCountHint <= 0)
        {
            // PlayState();
        }
    }

    public void ResetTimeHint()
    {
        StopState();
        this.isShowHint = false;
        this.timeCountHint = TimeHint;
    }

    public void MouseDownItem()
    {
        enableCountTime = false;
        handCtrl.HideHand();

        isClickTrueItem = true;
    }

    public void MouseUpItem()
    {
        enableCountTime = true;
        ResetTimeHint();
    }

    public void StopState()
    {
        handCtrl.HideHand();
    }
}
