using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
    public class IngredientBowl : MonoBehaviour
    {
        [SerializeField] private GameObject[] ingredients;

        public UnityEvent onFull;
        private int _count = 0;

        public void OnPutIngredientIn(int index)
        {
            ingredients[index].SetActive(true);
            _count++;
            if (_count >= ingredients.Length)
            {
                onFull?.Invoke();
            }
        }
    }
}
