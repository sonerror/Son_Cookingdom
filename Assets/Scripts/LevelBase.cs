using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{

    protected Sprite _hint;

    public void SetStep(int step)
    {
        // if (onStepChanged != null)
        // {
        //     onStepChanged(step);
        // }
    }

    protected virtual void Awake()
    {
        // LevelBase.instance = this;
    }

    protected virtual void Start()
    {
        // if (onBlockPlayerInteractChanged != null)
        // {
        //     onBlockPlayerInteractChanged();
        // }
    }

    protected void EndGame()
    {
        // if (onEndGame != null)
        // {
        //     onEndGame();
        // }
    }
}
