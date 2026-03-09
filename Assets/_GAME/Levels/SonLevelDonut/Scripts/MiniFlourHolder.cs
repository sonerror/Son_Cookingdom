using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace sonnv
{
    public class MiniFlourHolder : IngredientHolder
    {
        [SerializeField] private SpriteRenderer miniFlour;
        [SerializeField] private SpriteRenderer donut;
        [SerializeField] private SpriteRenderer donutHole;
        [SerializeField] private SpriteRenderer hole;
        [SerializeField] private int maxCount;
        [SerializeField] private bool checkEnd;
        [SerializeField] private bool checkIsHole;
        [SerializeField] private Transform tfChisel;

        private void Awake()
        {
            checkEnd = false;
            checkIsHole = false;
            maxCount = GetCountFoodChangeList();
        }

        public void SetAlphaHole()
        {
            hole.SetAlpha(1);
        }
        public void HideHole(bool value)
        {
            hole.gameObject.SetActive(value);
        }
        public Transform GetTfChisel()
        {
            return tfChisel;
        }
        public int GetMaxCount()
        {
            return maxCount;
        }
        public void SetBoolCheckEnd(bool value)
        {
            checkEnd = value;
        }
        public bool GetCheckEnd()
        {
            return checkEnd;
        }
        public void SetBoolCheckEndIsHole(bool value)
        {
            checkIsHole = value;
        }
        public bool GetCheckEndIsHole()
        {
            return checkIsHole;
        }
        public int GetCountChange()
        {
            return GetCountFoodChangeList();
        }
        public void ChangeAlphaSprite()
        {
            if (GetCountFoodChangeList() <= 0) return;
            float percent = 1f - (float)GetCountFoodChangeList() / maxCount; 

            SetAlpha(miniFlour, 1 - percent);
            SetAlpha(donut, percent);        
        }
        public int TypeIngredient(IngredientType type)
        {
            return GetCountByType(type);
        }
        public void ChangeAlphaDonut()
        {
            SetAlpha(donut,0);
            SetAlpha(donutHole,1);
        }   
        private void SetAlpha(SpriteRenderer renderer, float alpha)
        {
            Color color = renderer.color;
            color.a = Mathf.Clamp01(alpha);
            renderer.color = color;
        }
        
    }

}
