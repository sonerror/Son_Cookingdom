using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class Flour : IngredientForwarder
    {
        [SerializeField] private TapInFlour TapInFlour;
        [SerializeField] private SpriteRenderer spriteFlour;
        public void SetBoolIsTap(bool value)
        {
            TapInFlour.SetIsUpScale(!value);
        }
    }
}
