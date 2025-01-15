
using AnhPD.KingCrab;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private HandCtrl handCtrl;
    [SerializeField] float TimeHint = 5f;
    private float timeCountHint = 0f;
    private void Update()
    {
        CalculateTimeHint();
    }

    private bool isShowHint = true;
    private void CalculateTimeHint()
    {
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
        var index = LevelKingCrab.Instance.state;
        switch (index)
        {
            case 0:
                PlayState1();
                break;
            default:
                break;
        }
    }

    public void PlayState1()
    {
        var pos = LevelKingCrab.Instance.legsGroup.getPosition();
        if (pos == Vector3.zero) return;
        handCtrl.ShowHandPosToPos(
            LevelKingCrab.Instance.scissors.Tf.position,
             pos);
    }
}
