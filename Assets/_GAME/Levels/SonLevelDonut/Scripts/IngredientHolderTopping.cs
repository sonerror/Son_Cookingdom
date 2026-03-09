using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class IngredientHolderTopping : IngredientHolder
    {
        [SerializeField] private List<IngredientData> toppingChangeList;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool isAdd;
        [SerializeField] private Animation animChocolate;
        [SerializeField] private Transform tfChoco;

        private void Awake()
        {
            if (animChocolate != null)
            {
                animChocolate.Stop();

            }
        }
        public bool GetBoolAdd()
        {
            return isAdd;
        }
        public void PlayAnim()
        {
            if (animChocolate != null)
            {
                animChocolate.Play();
            }
        }
        public Transform GetTF()
        {
            return tfChoco;
        }
        public void SetBoolAdd(bool b)
        {
            isAdd = b;
        }
        public SpriteRenderer GetSpriteByType(IngredientType _type)
        {
            return toppingChangeList.Find(t => t.ingredientType == _type).sprite;
        }
        public SpriteRenderer GetSprite()
        {
            return spriteRenderer;
        }
    }

}
