using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class BlurDisplay : DisplayBase
    {
        [SerializeField] private DisplayBase normalDisplay;
        [SerializeField] private MaterialInstancer instancer;

        [SerializeField] private float duration = 10f;
        public override void Display(FoodData foodData)
        {
            instancer.InitMaterial();
            normalDisplay.Display(foodData);
            
            DOVirtual.Float(.2f, 0, duration, t =>
            {
                instancer.SetFloat("_DistortAmount", t);
                // instancer.SetColor("_Color", new Color(1, 1, 1, (1f - t)));
            });
        }

        [Button]
        public override void ClearDisplay()
        {
            instancer.ResetToDefaultMaterial();
            normalDisplay.ClearDisplay();
        }
    }
}
