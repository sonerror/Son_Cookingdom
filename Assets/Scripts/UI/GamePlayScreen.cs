using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScreen : UIScreen
{
  [SerializeField] private TimerUI timerUI;
  public GameObject btnPlay;
  public GameObject TextTutorial;
  public GameObject objProcess, objImgCook;
  private bool istap = false;
  public void gotoStore()
  {
    GameManager.Ins.gotoStore();
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0) && istap == false)
    {
      btnPlay.SetActive(true);
      TextTutorial.SetActive(false);
      objProcess.SetActive(true);
      objImgCook.SetActive(true);
      timerUI.StartCountdown();
      SoundManager.Ins.PlayBgm();
      istap = true;
    }
  }

  public override void Resize(Vector2 gameSize)
  {
    base.Resize(gameSize);
    RectTf.sizeDelta = gameSize;
  }
}
