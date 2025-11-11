using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Cook
{
    public class StackPlate : MonoBehaviour
    {
        [SerializeField] private List<Transform> foods;

        public UnityEvent completeEvent;

        public void OnPlaceFood()
        {
            if (foods.Count == 0) return;

            var last = foods[0];
            last.gameObject.SetActive(true);
            foods.RemoveAt(0);

            if (foods.Count < 1)
            {
                completeEvent?.Invoke();
            }
        }

        public void OnPlaceFoodAt(int index)
        {
            if (foods.Count == 0) return;
            
            var last = foods[index];
            last.gameObject.SetActive(true);

            for (int i = 0; i < foods.Count; i++)
            {
                if(!foods[i].gameObject.activeSelf) return;
            }
            completeEvent?.Invoke();
        }
    }
}
