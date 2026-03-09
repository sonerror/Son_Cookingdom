using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.FoodStall
{
    [CreateAssetMenu(menuName = "AnhPD/FoodStall/ColorConfigSO")]
    public class ColorConfigSO : ScriptableObject
    {
        public Color[] colors;

        public Color GetColor(ColorTag colorTag)
        {
            return colors[(int)colorTag];
        }

        public Color GetColor(int index)
        {
            return colors[index];
        }
    }
}
