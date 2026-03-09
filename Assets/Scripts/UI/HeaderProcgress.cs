using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderProcgress : Progress
{

    public override void increaseProgress(int from, int to)
    {
        base.increaseProgress(from, to);
        // thubongCtrl.PlayHappy();
    }

    void Start()
    {
        EventManager.StartListening(EventType.IncreaseProgress.ToString(), () =>
        {
            increaseProgress(40, 55);
        });
    }
}
