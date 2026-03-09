using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Tanghulu
{
    public class SugarcaneBasket : MonoBehaviour
    {
        [SerializeField] private Transform[] sugarcane;

        public UnityEvent onComplete;
        
        private int _index;

        public void OnPutIn()
        {
            sugarcane[_index].FallAppear();
            _index++;
            if(_index >= sugarcane.Length) onComplete?.Invoke();
        }
    }
}
