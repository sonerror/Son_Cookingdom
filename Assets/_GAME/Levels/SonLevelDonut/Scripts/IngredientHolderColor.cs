using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    [Serializable]
    public class IngredientData
    {
        public IngredientType ingredientType;
        public SpriteRenderer sprite;
    }
    public class IngredientHolderColor : IngredientHolder
    {
        [SerializeField] private List<IngredientData> ingredientChangeList;
        [SerializeField] private Transform tf;
        [SerializeField] private Transform tfNilong;
        [SerializeField] private Transform tfSpoon;
        [SerializeField] private Transform tfCenter;
        [SerializeField] private ChangeLayer changeLayer;
        [SerializeField] private SpriteRenderer spriteNilong;
        [SerializeField] private IngredientType typeData = IngredientType.None;
        public FlatSpoonIbBowlColor flatSpoonIbBowlColor;
        [SerializeField] private List<IngredientData> listIngredientMix;


        public void HiedeObjSpoon(bool f)
        {
            flatSpoonIbBowlColor.gameObject.SetActive(f);
        }
        public void SetAlphaSpriteSpatula(float index)
        {
            flatSpoonIbBowlColor.SetAlpha(index);

        }
        public void HideCol(bool value)
        {
            flatSpoonIbBowlColor.HideCol(value);
        }
        public SpriteRenderer GetIngredientMixByType()
        {
            return listIngredientMix.Find(e => e.ingredientType == typeData).sprite;
        }
        public Transform GetTFCenter()
        {
            return tfCenter;
        }
        public Transform GetTF()
        {
            return tf;
        }
        public Transform GetTFNilong()
        {
            return tfNilong;
        } public Transform GetTFSpoon()
        {
            return tfSpoon;
        }
        public IngredientData GetIngredientByType(IngredientType _type)
        {
            return ingredientChangeList.Find(t => t.ingredientType == _type);
        }
        public SpriteRenderer GetSpriteNilong()
        {
            return spriteNilong;
        }
        public void SetTypeData(IngredientType type)
        {
            typeData = type;
        }
        public IngredientType GetTypeData()
        {
            return typeData;
        }
        public void SetValueIsClick(bool value)
        {
            changeLayer.SetValueIsClick(value);
        }
    }
}
