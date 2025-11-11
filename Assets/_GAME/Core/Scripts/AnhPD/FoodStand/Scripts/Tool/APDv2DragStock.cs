using System.Collections;
using System.Collections.Generic;
using AnhPD.CookV2;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class APDv2DragStock : APDv2Drag
    {
        [SerializeField] private RefillStock stock;
        [FoldoutGroup("Bool")]public bool isUseSwitchTarget;
        protected override void Start()
        {
            base.Start();
            
            onMouseDown.AddListener(stock.TakeAwayUnit);
            onRewound.AddListener(() =>
            {
                if(!IsComplete) stock.GiveBackUnit();
            });
            
            if(isUseSwitchTarget) onCompleteBaseSwitchTarget.AddListener(stock.UseUnit);
            else onComplete.AddListener(stock.UseUnit);
        }

        public override void OnReReady(bool isReady = true)
        {
            IsComplete = false;
            base.OnReReady(isReady);
        }
    }
}
