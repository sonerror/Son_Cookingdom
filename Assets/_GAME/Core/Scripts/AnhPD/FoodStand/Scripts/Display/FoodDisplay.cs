using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace AnhPD.FoodStall
{
    public class FoodDisplay : MonoBehaviour
    {
        [SerializeField] private List<DisplayBase> displays;
        [SerializeField] private OrderType orderType;
        
        public virtual void Display(FoodData foodData)
        {
            displays[(int)orderType].Display(foodData);
        }

        public virtual void ClearDisplay()
        {
            displays[(int)orderType].ClearDisplay();
        }

        public void SetOrderType(OrderType newOrderType)
        {
            this.orderType = newOrderType;
        }

        public DisplayBase GetCurrentDisplay()
        {
            return displays[(int)orderType];
        }

        public void SetOrderDisplay(bool isOrder)
        {
            foreach (var display in displays)
            {
                display.isOrder = isOrder;
            }
        }
    }
}
