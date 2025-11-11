using System;
using UnityEngine;
using Luna.Unity;
using System.Collections.Generic;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
  public List<ShowItem> gameActiveState1 = new List<ShowItem>();

  public List<ShowItem> gameActiveState2 = new List<ShowItem>();

  public void ChangeState()
  {
    StartCoroutine(IE_ChangeState());
  }

  protected IEnumerator IE_ChangeState()
  {
    yield return new WaitForSeconds(0.75f);
    gameActiveState1.ForEach(item =>
    {
      item.MoveObjHide();
    });
    gameActiveState2.ForEach(item =>
    {
      item.gameObject.SetActive(true);
      item.MoveObjShow();
    });
  }

  public void gotoStore()
  {
    Debug.Log("Goto Store");
    LifeCycle.GameEnded();
    Playable.InstallFullGame();
    SoundManager.Ins.Mute();
  }

  private void Update()
  {
    if (isEndGame && Input.GetMouseButtonDown(0))
    {
      gotoStore();
    }
  }

  public bool isEndGame = false;

  public void showEndGame()
  {
    Debug.Log("End Game");
    isEndGame = true;
  }

  public void PlaySoundWater()
  {
    // SoundManager.Ins.PlayFxAfterTime(FxType.DropWater, 0.3f);
  }

  public void PlaySoundDrop()
  {
    // SoundManager.Ins.PlayFxAfterTime(FxType.ShakeDrop, 0.3f);
  }
}