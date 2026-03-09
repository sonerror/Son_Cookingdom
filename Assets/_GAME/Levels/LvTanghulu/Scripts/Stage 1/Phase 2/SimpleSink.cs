using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.Tanghulu
{
    public class SimpleSink : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer waterSink, waterFaucet;
        [SerializeField] private Transform lid;
        [SerializeField] private AudioSource sfxWatering;
        private bool _isHaveLid;
        public bool IsHaveWater { get; private set;}
        public bool IsWatering { get; private set;}

        public UnityEvent onTurnOn, onTurnOff, onHaveWater;

        public void OnHaveLid()
        {
            lid.FallAppear();
            _isHaveLid = true;
            if(IsWatering) HoldWater();
        }
        public void TurnOn()
        {
            if(IsWatering) return;
            IsWatering = true;
            waterFaucet.enabled = true;
            onTurnOn?.Invoke();
            sfxWatering.Play();
            if (_isHaveLid)
            {
                HoldWater();
            }
        }

        private void HoldWater()
        {
            onHaveWater?.Invoke();
            IsHaveWater = true;
            waterSink.DOKill();
            waterSink.DOFade(1, 1);
        }

        public void TurnOff()
        {
            if(!IsWatering) return;
            IsWatering = false;
            waterFaucet.enabled = false;
            onTurnOff?.Invoke();
            sfxWatering.Stop();
        }
    }
}
