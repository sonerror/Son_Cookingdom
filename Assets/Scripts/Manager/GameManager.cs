using System;
using UnityEngine;
using Luna.Unity;
using System.Collections.Generic;
using System.Collections;

public class GameManager : Singleton<GameManager>
{

    public void GotoStore()
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
            GotoStore();
        }
    }

    public bool isEndGame = false;

    public void ActiveListenToStore()
    {
        Debug.Log("End Game - ActiveListenToStore");
        isEndGame = true;
    }
}