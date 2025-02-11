
using System.Collections.Generic;
using System.Linq;
using AnhPD.KingCrab;
using AnhPD.MakeSushi;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private HandCtrl handCtrl;
    [SerializeField] float TimeHint = 5f;

    [SerializeField] Transform sink, cuttingBroad, cooker, lid, rice1, pos1, pos2;
    [SerializeField] List<Peel> peels;

    public List<int> listState = Enumerable.Range(0, 20).ToList();
    private float timeCountHint = 0f;

    public float timeEndGame = 30f;
    private bool isClickTrueItem = false;

    public void removeState(int state)
    {
        listState.Remove(state);
    }

    public void removePeel(Peel peel)
    {
        peels.Remove(peel);
    }

    public void AddPeel(Peel peel)
    {
        if (peels.Contains(peel)) return;
        peels.Add(peel);
    }

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
            PlayState();
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

    private void Start()
    {
        PlayState();
    }

    public void StopState()
    {
        handCtrl.HideHand();
    }

    private void PlayState()
    {
        isShowHint = true;
        var index = listState[0];
        Debug.Log("PlayState: " + index);

        if (index > 4 && LevelMakeSushi.Ins.board.veg != null)
        {
            if (!LevelMakeSushi.Ins.board.veg.IsPeeled)
            {
                PlayPeeler();
                return;
            }

            if (!LevelMakeSushi.Ins.board.veg.IsCut)
            {
                PlayKnife();
                return;
            }
        }

        if (index == 9 && peels.Count > 0)
        {
            PlayPeel(peels[0]);
            return;
        }


        switch (index)
        {
            case 0:
                PlayState0();
                break;
            case 1:
                PlayState1();
                break;
            case 2:
                PlayState2();
                break;
            case 3:
                PlayState3();
                break;
            case 4:
                PlayState4();
                break;
            case 5:
                PlayState5();
                break;
            case 6:
                PlayState6();
                break;
            case 7:
                PlayState7();
                break;
            case 8:
                PlayState8();
                break;
            case 9:
                PlayState9();
                break;
            case 10:
                PlayState10();
                break;
            default:
                break;
        }
    }

    private void PlayPeeler()
    {
        handCtrl.ShowHandPosToPos(
            LevelMakeSushi.Ins.peeler.transform.position,
            cuttingBroad.position);
    }

    private void PlayKnife()
    {
        handCtrl.ShowHandPosToPos(
            LevelMakeSushi.Ins.knife.transform.position,
            cuttingBroad.position);
    }

    private void PlayPeel(Peel peel)
    {
        handCtrl.ShowHandPosToPos(
            peel.transform.position,
            peel.target.position);
    }

    private void PlayState0()
    {
        handCtrl.ShowHandPosToPos(
            LevelMakeSushi.Ins.cooker.transform.position,
             sink.position);
    }

    private void PlayState1()
    {
        handCtrl.ShowHandState1(LevelMakeSushi.Ins.value.transform.position);
    }

    private void PlayState2()
    {
        handCtrl.ShowHandPosToPos(
            LevelMakeSushi.Ins.cooker.transform.position,
             cooker.position);
    }

    private void PlayState3()
    {
        handCtrl.ShowHandPosToPos(
            lid.position,
            cooker.position);
    }

    private void PlayState4()
    {
        handCtrl.ShowHandState1(LevelMakeSushi.Ins.cookerButton.transform.position);
    }

    private void PlayState5()
    {
        handCtrl.ShowHandPosToPos(
            LevelMakeSushi.Ins.vegs[0].transform.position,
            cuttingBroad.position);
    }

    private void PlayState6()
    {
        handCtrl.ShowHandPosToPos(
            LevelMakeSushi.Ins.vegs[1].transform.position,
            cuttingBroad.position);
    }

    private void PlayState7()
    {
        handCtrl.ShowHandPosToPos(
            LevelMakeSushi.Ins.vegs[2].transform.position,
            cuttingBroad.position);
    }

    private void PlayState8()
    {
        handCtrl.ShowHandPosToPos(
            LevelMakeSushi.Ins.vegs[3].transform.position,
            cuttingBroad.position);
    }

    private void PlayState9()
    {
        var lidCpn = lid.GetComponent<Lid>();
        handCtrl.ShowHandPosToPos(
            lid.position,
            lidCpn.startPos);
    }

    private void PlayState10()
    {
        var riceCpn = rice1.GetComponent<Lid>();
        handCtrl.ShowHandPosToPos(
            rice1.position,
            riceCpn.target.position);
    }

    public void PlayStateEndGame()
    {
        handCtrl.ShowHandPosToPos(
            pos1.position,
            pos2.position);
    }


}
