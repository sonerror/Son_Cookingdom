using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.Cook
{
    [RequireComponent(typeof(AppearEffect))]
    [RequireComponent(typeof(AppearSfx))]
    
    public class AppearCombo : MonoBehaviour
    {
        [SerializeField] private AppearEffect appearEffect;
        [SerializeField] private AppearSfx appearSfx;
        
        public void OnActivate()
        {
            appearEffect.PlayEffect();
            appearSfx.PlaySound();
        }

        [Button]
        private void SetPlayOnEnable(bool isPlayOnEnable = false)
        {
            appearEffect.isPlayOnEnable = isPlayOnEnable;
            appearSfx.isPlayOnEnable = isPlayOnEnable;
        }

        [Button]
        private void Init()
        {
            appearEffect = GetComponent<AppearEffect>();
            appearSfx = GetComponent<AppearSfx>();
        }
    }
}
