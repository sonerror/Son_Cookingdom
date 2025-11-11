using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public abstract class DisplayBase : MonoBehaviour
    {
        public bool isOrder;
        public abstract void Display(FoodData foodData);
        public abstract void ClearDisplay();
    }
}
