using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnhPD.FoodStall
{
    public class FSTrashCan : MonoBehaviour
    {
        [SerializeField] private float startY;
        [SerializeField] private float distance = 3f;

        [Button]
        private void Init()
        {
            startY = transform.position.y;
        }
        
        public void Show()
        {
            transform.DOComplete();
            transform.DOMoveY(startY + distance, .3f);
        }

        public void Hide()
        {
            transform.DOMoveY(startY, .3f);
        }
    }
}
