using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : Singleton<LevelBase>
{
  public bool isEndingGame;
  public event System.Action onBlockPlayerInteractChanged = null;
  public bool IsAllowInteract => true;
  protected Sprite _hint;

  public void SetStep(int step)
  {
    // if (onStepChanged != null)
    // {
    //     onStepChanged(step);
    // }
  }

  protected virtual void Start()
  {
  }

  protected void EndGame()
  {
    // if (onEndGame != null)
    // {
    //     onEndGame();
    // }
  }

  protected virtual void OnDestroy()
  {
  }
}
