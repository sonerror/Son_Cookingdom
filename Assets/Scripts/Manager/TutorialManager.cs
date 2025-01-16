
using AnhPD.KingCrab;
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
        var index = LevelKingCrab.Instance.state;

        Debug.Log("PlayState: " + index);
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
            case 11:
                PlayState11();
                break;
            case 12:
                PlayState12();
                break;
            case 50:
                PlayState50();
                break;
            default:
                break;
        }
    }

    private void PlayState0()
    {
        var pos = LevelKingCrab.Instance.legsGroup.getPosition();
        if (pos == Vector3.zero) return;
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.scissors.Tf.position,
             pos);
    }

    private void PlayState1()
    {
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.hammer.Tf.position,
             LevelKingCrab.Instance.crabBody.Tf.position);
    }

    private void PlayState2()
    {
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.pry.Tf.position,
             LevelKingCrab.Instance.crabBody.Tf.position - Vector3.up * 0.5f);
    }

    private void PlayState3()
    {
        var pos = LevelKingCrab.Instance.inDropGroup.getPosition();
        if (pos == Vector3.zero) return;
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.tweezers.Tf.position,
             pos);
    }

    private void PlayState4()
    {
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.spoon.Tf.position,
             LevelKingCrab.Instance.crabBody.Tf.position);
    }

    private void PlayState5()
    {
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.spoon.Tf.position,
             LevelKingCrab.Instance.bowl.Tf.position);
    }

    private void PlayState6()
    {
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.scissors.Tf.position,
             LevelKingCrab.Instance.crabBody.Tf.position);
    }

    private void PlayState50()
    {
        var item = LevelKingCrab.Instance.bodyPartGroup.getItemDropNotDrop();
        if (item == null) return;
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.scissors.Tf.position,
             item.Tf.position);
    }

    private void PlayState7()
    {
        var pos = LevelKingCrab.Instance.razor.maskGroupCustom.getPosition();
        if (pos == Vector3.zero) return;
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.razor.Tf.position,
             pos);
    }

    private void PlayState8()
    {
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.meat.Tf.position,
             LevelKingCrab.Instance.bowl.Tf.position);
    }

    private void PlayState9()
    {
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.tweezers.Tf.position,
             LevelKingCrab.Instance.tweezers.egg.transform.position);
    }

    private void PlayState10()
    {
        var pos = LevelKingCrab.Instance.razor.maskGroupLid.getPosition();
        if (pos == Vector3.zero) return;
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.razor.Tf.position,
             pos);
    }

    private void PlayState11()
    {
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.meat2.Tf.position,
              LevelKingCrab.Instance.bowl.Tf.position);
    }

    private void PlayState12()
    {
        var item = LevelKingCrab.Instance.crabGroup.getItemActive();
        if (item == null) return;
        handCtrl.ShowHandPosToPos(
            item.Tf.position,
             item.target.position);
    }

}
