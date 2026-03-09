using System;
using System.Collections.Generic;
using DG.Tweening;


using UnityEngine;
using UnityEngine.Events;
namespace sonnv
{
    public class IngredientForwarder : DragController
    {
        [SerializeField] private Collider2D contactCollider;
        [SerializeField] private SpriteRenderer foodSprite;
        [SerializeField] private List<SpriteForwarderData> ingredientSpriteDataList;
        [SerializeField] private IngredientType ingredientType = IngredientType.None;
        public UnityEvent onIngredientChanged;
        [SerializeField] private FxType takeIngredientSound = FxType.None;
        [SerializeField] private FxType giveIngredientSound = FxType.None;

        private SpriteForwarderData _currentSpriteData;
        public Collider2D ContactCollider => contactCollider;

        public bool IsFilled => ingredientType != IngredientType.None;
        public IngredientType IngredientType => ingredientType;
        public IngredientType ingredient;
        [SerializeField] private SpriteRenderer imgNilong;

        public void ChangeSprite(bool toAltSprite)
        {
            if (ingredientType == IngredientType.None) return;
            foodSprite.sprite = toAltSprite ? _currentSpriteData.altSprite : _currentSpriteData.sprite;
        }
        public void SetIngredient(IngredientType iType, bool invokeOnChange = true)
        {
            ingredientType = iType;
            if (iType == IngredientType.None)
            {
                foodSprite.enabled = false;
                onIngredientChanged?.Invoke();
                SoundManager.Ins.PlayFx(giveIngredientSound);
                return;
            }
            _currentSpriteData = GetFoodSpriteData(iType);
            if (_currentSpriteData != null)
            {
                foodSprite.sprite = _currentSpriteData.sprite;
                foodSprite.enabled = true;
            }

            if (invokeOnChange)
            {
                onIngredientChanged?.Invoke();
                SoundManager.Ins.PlayFx(takeIngredientSound);
            }
        }

        private SpriteForwarderData GetFoodSpriteData(IngredientType type)
        {
            if (type is IngredientType.None) return null;

            for (int index = 0; index < ingredientSpriteDataList.Count; index++)
            {
                if (ingredientSpriteDataList[index].ingredientType == type)
                {
                    return ingredientSpriteDataList[index];
                }
            }
            return null;
        }

        public void SetOrderLayer(int index)
        {
            if (imgNilong != null)
            {
                imgNilong.sortingOrder = index;
            }
        }


    }

}
