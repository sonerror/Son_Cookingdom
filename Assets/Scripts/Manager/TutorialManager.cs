
using System.Collections.Generic;
using System.Linq;
using sonnv;
using UnityEngine;
using UnityEngine.XR;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private HandCtrl handCtrl;
    [SerializeField] float TimeHint = 5f;
    private float timeCountHint = 0f;

    public float timeEndGame = 30f;
    private bool isClickTrueItem = false;



    void Start()
    {

        PlayTutState();
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
            PlayTutState();
        }
    }

    private void PlayTutState()
    {
        isShowHint = true;
        var currstate = 100;
        switch (currstate)
        {
            case 0:
                PlayTutState0();
                break;
            case 1:
                PlayTutState1();
                break;
            case 2:
                PlayTutState2();
                break;
            case 3:
                PlayTutState3();
                break;
            case 4:
                PlayTutState4();
                break;
            case 5:
                PlayTutState5();
                break;
            case 6:
                PlayTutState6();
                break;
            default:
                handCtrl.gameObject.SetActive(false);
                break;
        }
    }

    private void PlayTutState0()
    {

    }
    private void PlayTutState1()
    {

    }
    private void PlayTutState2()
    {


    }
    private void PlayTutState3()
    {



    }
    private void PlayTutState3_5()
    {

    }

    public void PlayTutState4()
    {

    }

    public void PlayTutState5()
    {

    }

    public void PlayTutState6()
    {

    }

    public void PlayTutState7()
    {
        // var pos1 = phase1Donut.Tf.position;
        // var radial = phase1Donut.spoonPosToPour.position - pos1;
        // var fromAngle = Mathf.Atan2(radial.y, radial.x);

        // handCtrl.ShowHandArrow(pos1, fromAngle, 2.5f);
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
