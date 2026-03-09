using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class GrayScaleDisplay : DisplayBase
    {
        [SerializeField] private DisplayBase normalDisplay;
        [SerializeField] private MaterialInstancer instancer;

        [SerializeField] private float duration = 10f;
        public override void Display(FoodData foodData)
        {
            instancer.InitMaterial();
            normalDisplay.Display(foodData);
            
            DOVirtual.Float(1f, 0, duration/2, t =>
            {
                instancer.SetFloat("_GreyscaleBlend", t);
            }).SetDelay(duration/2);
        }

        public override void ClearDisplay()
        {
            instancer.ResetToDefaultMaterial();
            normalDisplay.ClearDisplay();
        }
    }
}
