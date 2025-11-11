using System;
using System.Collections;
using System.Collections.Generic;
using AnhPD.FoodStand;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class CustomerLineController : MonoBehaviour
    {
        [SerializeField] private CustomerLine linePrefab;
        [SerializeField] private List<CustomerLine> lines;
        [SerializeField] private int lineNumber = 2;
        [SerializeField] private float distance = 2.24f;
        [SerializeField] private float lineScale = 1f;
        
        private void Start()
        {

        }
        public List<Transform> GetTargets()
        {
            List<Transform> targets = new List<Transform>();
            for(int i = 0; i < lineNumber; i++) targets.Add(lines[i].targetPos);
            return targets;
        }
        public void CompareData(int lineIndex, FoodData foodData)
        {
            lines[lineIndex].CompareOrder(foodData);
        }

        public void SetCustomerData(List<CustomerDataArray> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                lines[i].SetCustomerData(list[i]);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].NextCustomer(i);
            }
        }
#if UNITY_EDITOR

        [Button]
        private void InitLine()
        {
            // Clear list cũ
            foreach (var line in lines)
            {
                if (line) DestroyImmediate(line.gameObject);
            }
            lines.Clear();

            // Tạo mới lineNumber line
            for (int i = 0; i < lineNumber; i++)
            {
                CustomerLine line = (CustomerLine)PrefabUtility.InstantiatePrefab(linePrefab, transform);
                line.transform.localScale = Vector3.one * lineScale;

                float startX = -(lineNumber - 1) * distance * 0.5f;
                line.transform.localPosition = new Vector3(startX + i * distance, 0f, 0f);
                
                line.ShowTest();
                lines.Add(line);
            }
        }

        [Button]
        private void ShowCustomer()
        {
            foreach (var line in lines)
            {
                line.ShowTest();
            }
        }
        [Button]
        private void HideCustomer()
        {
            foreach (var line in lines)
            {
                line.HideTest();
            }
        }
        #endif
    }
}
