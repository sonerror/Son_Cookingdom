using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
    public class GeneralPreparation : MonoBehaviour
    {
        [SerializeField] private FruitPreparation[] preparations;
        public UnityEvent onComplete;
        public bool IsNeedTool => preparations[_index].isNeedTool;
        private int _index;

        public void StartPreparation()
        {
            gameObject.SetActive(true);
            preparations[_index].gameObject.SetActive(true);
        }
        public void OnDonePreparation()
        {
            _index++;
            if(_index < preparations.Length)
                preparations[_index].gameObject.SetActive(true);
            else
            {
                onComplete?.Invoke();
            } 
        }

        public void OnHaveTool()
        {
            preparations[_index].OnHaveTool();
        }
        
    #if UNITY_EDITOR
        [Button]
        private void Setup()
        {
            preparations = GetComponentsInChildren<FruitPreparation>(true);
            foreach (FruitPreparation step in preparations) step.SetGeneral(this);
        }
    #endif
    }
}
