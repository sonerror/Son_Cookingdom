using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class RefillStock : MonoBehaviour
    {
        [SerializeField] private GameObject[] units;
        [SerializeField] private GameObject target;
        [SerializeField] private RefillClock clock;
        [SerializeField] private Color clockColor;
        [SerializeField] private float cooldownSpeed = 1f, cooldownTime = 10f;
        public string nameId;
        private int _index = 0;
        private bool _isHoldingOne;
        private float _timer;

        private void Start()
        {
            clock.SetColor(clockColor);
        }

        public void Setup(float speed, float cd)
        {
            cooldownSpeed = speed;
            cooldownTime = cd / 1000;
        }
        public void TakeAwayUnit()
        {
            units[_index].SetActive(false);  
            _isHoldingOne = true;
        }

        public void GiveBackUnit()
        {
            OnRefill();  
            _isHoldingOne = false;
        }
        public void UseUnit()
        {
            _isHoldingOne = false;
            _index++;
            if (_index >= units.Length)
            {
                target.SetActive(false);
            }
        }

        private void OnRefill()
        {
            units[_index].transform.FallAppear();
            target.SetActive(true);
        }
        private void Update()
        {
            // Nếu còn thiếu item (index > 0) thì:
            //   - Nếu đang giữ 1 item → chỉ refill khi còn thiếu hơn 1 (index > 0)
            //   - Nếu không giữ item → luôn refill
            // Nếu không thiếu item (index <= 0) → không refill
            bool isRefilling = _index > 0 && (!_isHoldingOne || _index >= 1);
            if (isRefilling)
            {
                _timer += Time.deltaTime;
                float rate = Mathf.Clamp01(_timer / cooldownTime);
                clock?.Show(1f - rate);
                if (_timer >= cooldownTime)
                {
                    _timer = 0;
                    if (_isHoldingOne)
                    {
                        OnRefill();
                        _index--;
                    }
                    else
                    {
                        _index--;
                        OnRefill();
                    }
                }
            }
            else clock?.Hide();
        }
        
        #if UNITY_EDITOR
        [Button]
        public void ShowClock()
        {
            clock.ShowImmediate();
        }
        [Button]
        public void HideClock()
        {
            clock.HideImmediate();
        }
        #endif
    }
}
