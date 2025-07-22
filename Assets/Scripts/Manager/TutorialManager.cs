
using System.Collections.Generic;
using System.Linq;
using sonnv;
using UnityEngine;
using UnityEngine.XR;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private HandCtrl handCtrl;
    [SerializeField] float TimeHint = 5f;
    [SerializeField] private float timeCountHint = 0f;

    public float timeEndGame = 30f;
    private bool isClickTrueItem = false;
    [SerializeField] private Level630 currLevel;


    void Start()
    {
        PlayTutState();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ResetTimeHint();
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
        if (currLevel.itemHolderBroad.IsPlaying) return;
        if (isShowHint) return;
        timeCountHint -= Time.deltaTime;
        if (timeCountHint <= 0)
        {
            PlayTutState();
        }
    }

    private void PlayTutState()
    {
        isShowHint = true;

        if (Level630.IsState2) PlayStateGame2();
        else PlayStateGame1();
    }

    void PlayStateGame1()
    {
        if (currLevel.itemHolderBroad.IsOccupied)
        {
            if (currLevel.itemHolderBroad.IsKnife)
            {
                handCtrl.ShowHandStateAtPos(currLevel.knife.Tf.position);
                return;
            }
            var pos1 = currLevel.knife.Tf.position;
            var pos2 = currLevel.itemHolderBroad.Tf.position;
            handCtrl.ShowHandPosToPos(pos1, pos2);
            return;
        }
        else
        {
            for (var i = 0; i < currLevel.itemState1.Count; i++)
            {
                if (!currLevel.itemState1[i].IsDone)
                {
                    var pos1 = currLevel.itemState1[i].Tf.position;
                    var pos2 = currLevel.itemState1[i].GetTargetPosition;
                    handCtrl.ShowHandPosToPos(pos1, pos2);
                    return;
                }
            }
        }

        if (currLevel.itemClick != null && !currLevel.itemClick.IsDone)
        {
            handCtrl.ShowHandStateAtPos(currLevel.itemClick.Tf.position);
            return;
        }
    }

    void PlayStateGame2()
    {
        for (var i = 0; i < currLevel.itemState2.Count; i++)
        {
            if (currLevel.itemState2[i].gameObject.activeSelf)
            {
                var pos1 = currLevel.itemState2[i].Tf.position;
                var pos2 = currLevel.itemState2[i].GetTargetPosition;
                handCtrl.ShowHandPosToPos(pos1, pos2);
                return;
            }
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
