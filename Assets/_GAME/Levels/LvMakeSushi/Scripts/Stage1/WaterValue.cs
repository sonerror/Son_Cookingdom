using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.MakeSushi
{
    public class WaterValue : MonoBehaviour
    {
        [SerializeField] Transform value, water;
        [SerializeField] AudioClip sfxClick;

        private bool isWatering = false;
        public bool IsWatering => isWatering;

        private bool isTurnOff = false;

        private void OnMouseDown()
        {
            if (isTurnOff) return;

            // AudioManager.PlaySfx(sfxClick);

            if (isWatering)
            {
                isWatering = false;
                water.gameObject.SetActive(isWatering);
                value.DOScaleY(1, .1f);
            }
            else
            {
                isWatering = true;
                water.gameObject.SetActive(isWatering);
                value.DOScaleY(.85f, .05f);
                LevelMakeSushi.Instance.OnWaterStart();
            }
        }
        public void OnHaveObjectInSink()
        {
            water.DOScaleY(1f, .05f);
        }
        public void OnRemovedAllObjectInSink()
        {
            water.DOScaleY(1.13f, .1f);
        }
        public void TurnOff()
        {
            isTurnOff = true;

            if (isWatering)
            {
                // AudioManager.PlaySfx(sfxClick);
                isWatering = false;
                water.gameObject.SetActive(isWatering);
                value.DOScaleY(1, .1f);
            }
        }
    }
}

