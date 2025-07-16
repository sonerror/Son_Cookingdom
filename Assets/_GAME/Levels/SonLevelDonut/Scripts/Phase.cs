using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class Phase : SonMonoBehaviour
    {
        public void OnEndStep()
        {
            Level628.Ins.OnEndStep();
        }
        public void FinishStep()
        {
            Level628.Ins.FinishStep();
        }
    }
}