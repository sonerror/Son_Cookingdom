using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AnhPD.CookV2;
using AnhPD.FoodStand;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.FoodStall
{
    public class FSAPDLevelBase : MonoBehaviour
    {
        public static FSAPDLevelBase Instance;
        public static bool IsShowCooldownClock = true;
        protected void Awake()
        {
            if (!Instance)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        
        [SerializeField] private JsonDataHolder dataHolder;
        [SerializeField] private CustomerLineController lineController;
        [SerializeField] private FinishedDishDrag1 dishDrag;
        [SerializeField] private RefillStock[] refillStocks;
        public UnityEvent onRestart;
        
        private void Start()
        {
            dishDrag.OnReady();
            LoadData();
        }

        [Button]
        private void LoadData()
        {
            dataHolder.LoadJson();
            float cdSpeed = dataHolder.data.cooldownSpeed;

            for (int i = 0; i < refillStocks.Length; i++)
            {
                float cd = dataHolder.GetIngredientData(refillStocks[i].nameId).cd;
                refillStocks[i].Setup(cdSpeed, cd);
            }
            
            lineController.SetCustomerData(dataHolder.data.customers);
        }

        public void CheckOrder()
        {
            List<Transform> lineTf = lineController.GetTargets();
            int index = APDUtilities.GetNearestTranformIndex(dishDrag.transform, lineTf);
            if (dishDrag.IsInRange(lineTf[index]))
            {
                lineController.CompareData(index, dishDrag.data);
                dishDrag.OnReReady();
                onRestart?.Invoke();
            }
            else dishDrag.ReturnToStartPos();
        }

        public void Restart()
        {
            onRestart?.Invoke();
            dishDrag.OnReReady();
        }
#if UNITY_EDITOR
        [Button]
        private void GetAllRefillStocks()
        {
            refillStocks = FindObjectsOfType<RefillStock>();
        }
#endif
    }
}
