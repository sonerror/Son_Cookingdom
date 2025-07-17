using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class Phase : SonMonoBehaviour
    {
        public void OnEndStep(bool playSound = false)
        {
            Level628.Ins.OnEndStep(playSound);
            EventManager.TriggerEvent(EventType.IncreaseProgress.ToString());
        }
        public void FinishStep()
        {
            Level628.Ins.FinishStep();
        }
    }
}