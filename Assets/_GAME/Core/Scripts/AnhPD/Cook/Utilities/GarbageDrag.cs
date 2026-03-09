using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.Cook
{
    public class GarbageDrag : MonoBehaviour
    {
        [SerializeField] private Transform garbageCan;
        [SerializeField] private float offsetY = 3f;
        private float startY;
        private void Start()
        {
            startY = garbageCan.transform.position.y;
        }
        public void OnPickUp()
        {
            garbageCan.DOMoveY(startY + offsetY, .3f).SetEase(Ease.OutBack);
        }
        public void OnPutDown()
        {
            garbageCan.DOMoveY(startY, .3f).SetEase(Ease.InBack);
        }
    }
}

