using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.CookV2
{
    public class GarbageDrag : APDv2Drag
    {
        [SerializeField]private GarbageCan garbageCan;
        protected override void Start()
        {
            base.Start();
            isHideAfterComplete = true;
            if (!garbageCan)
            {
                Debug.LogWarning("GarbageDrag needs a GarbageCan");
                return;
            }
            onMouseDown.AddListener(() =>
            {
                OnReady();
                garbageCan.Show();
            });
            onMouseUp.AddListener(garbageCan.Hide);
            onComplete.AddListener(garbageCan.ThrowGarbage);
        }

        public void Setup(GarbageCan can)
        {
            garbageCan = can;
            target = can.center;
        }
    }
}
