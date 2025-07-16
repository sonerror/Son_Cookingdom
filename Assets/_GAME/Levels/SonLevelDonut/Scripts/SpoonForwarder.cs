using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class SpoonForwarder : IngredientForwarder
    {
        [SerializeField] private SpriteRenderer IngredientImg;
        public void HideImg(bool isActive)
        {
            IngredientImg.enabled = isActive;
            if(isActive == true)
            {
                IngredientImg.gameObject.transform.DOPunchScale(Vector3.up * 0.05f, 0.15f);
            }
        }
    }
}

