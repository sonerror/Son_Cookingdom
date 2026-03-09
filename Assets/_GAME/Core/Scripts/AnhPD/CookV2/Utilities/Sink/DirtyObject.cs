using System.Collections;
using System.Collections.Generic;
using AnhPD.Cook;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace AnhPD.CookV2
{
    public class DirtyObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] renderers;
        [SerializeField] private GameObject vfxClean;
        [SerializeField] private float floatingHeight = -.1f;
        public UnityEvent onComplete;
        public bool IsSpawnVfx = true;
        private bool _isPlaced, _isHaveWater, _isCleaning;
        public bool IsPlaced => _isPlaced;
        public bool IsDone => _isCleaning;
        
        public void PutInSink()
        {
            if(_isHaveWater) Floating();
            gameObject.SetActive(true);
            _isPlaced = true;
            if (_isHaveWater)
            {
                StartCleaning();
            } 
        }

        public void OnHaveWater()
        {
            Floating();
            _isHaveWater = true;
            if (_isPlaced)
            {
                StartCleaning();
            } 
        }
        private void StartCleaning()
        {
            if(_isCleaning) return;
            _isCleaning = true;
            float duration = 3f;
            for (int i = 1; i < renderers.Length; i++)
            {
                renderers[i].DOFade(0.3f,duration);
            }
            renderers[0].DOFade(0.3f,duration).OnComplete(() =>
            {
                onComplete?.Invoke();
                if (IsSpawnVfx)
                {
                    GameObject go = Instantiate(vfxClean, transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one * .5f;
                }
                else
                {
                    vfxClean.SetActive(true);
                }
                
                foreach (SpriteRenderer r in renderers) r.SetAlpha(0f);
            });
        }

        public void Floating(bool enable = true)
        {
            if (enable) transform.Floating(floatingHeight);
            else transform.DOKill();
        }
    }
}
