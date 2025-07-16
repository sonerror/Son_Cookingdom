using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

using UnityEngine;
namespace sonnv
{

    public class IngredientHolder : SonMonoBehaviour
    {
        [SerializeField] private bool fillHolder;
        [SerializeField] private bool isChangeScale;
        [SerializeField] private Collider2D col;
        [SerializeField] private List<IngredientForwarder> forwarder;
        [SerializeField] private List<SpriteHolderData> foodChangeList;
        [SerializeField] private bool disableContactWhenTriggered;
        private bool _canContact;
        private readonly Dictionary<Collider2D, IngredientForwarder> _cachedForwarderDict = new Dictionary<Collider2D, IngredientForwarder>();
        public Action<SpriteHolderData> OnIngredientDone;
        private const string INGREDIENT_TAG = "Ingredient";
        public Collider2D Collider => col;


        public void SetBoolChangeScale(bool value)
        {
            isChangeScale = value;
        }
        public int GetCountFoodChangeList()
        {
            return GetCountByType(IngredientType.Rolling);
        }

        public int GetCountByType(IngredientType type)
        {
            return foodChangeList.Count(item => item.ingredientType == type);
        }
        public void SetContact(bool canContact)
        {
            _canContact = canContact;
            col.enabled = canContact;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_canContact) return;
            if (!other.CompareTag(INGREDIENT_TAG)) return;
            // find in the cache
            if (!_cachedForwarderDict.TryGetValue(other, out var fwd))
            {
                fwd = other.GetComponentInParent<IngredientForwarder>();
                if (fwd == null || !forwarder.Contains(fwd)) return;
                _cachedForwarderDict.Add(other, fwd);
            }
            IngredientType ingredientType = fwd.IngredientType;
            bool actionPerformed = fillHolder ? TakeFromForwarder() : FillToForwarded();

            if (actionPerformed)
            {
                ChangeFoodAmount(ingredientType, isChangeScale);

                if (disableContactWhenTriggered)
                {
                    SetContact(false);
                }
            }
            return;

            // Helper methods
            bool FillToForwarded()
            {
                if (fwd.IsFilled) return false;
                if (foodChangeList.Count == 0) return false;
                fwd.SetIngredient(foodChangeList[0].ingredientType);
                return true;
            }

            bool TakeFromForwarder()
            {
                if (!fwd.IsFilled) return false;
                if (foodChangeList.Count == 0) return false;
                // if no Forwarder ingredient, no match with any foodChangeList ingredient, return false
                for (int i = 0; i < foodChangeList.Count; i++)
                {
                    if (foodChangeList[i].ingredientType != fwd.IngredientType) continue;
                    fwd.SetIngredient(IngredientType.None);
                    return true;
                }
                return false;
            }
        }

        private void ChangeFoodAmount(IngredientType ingredient, bool _isChangeScale)
        {
            if (foodChangeList.Count == 0)
            {
                return;
            }
            // find the food sprite data
            SpriteHolderData data = null;
            if (ingredient == IngredientType.None)
            {
                // means the forwarder take something
                if (!fillHolder) data = foodChangeList[0];
            }
            else if (fillHolder)
            {
                // means the forwarder give something
                for (int i = 0; i < foodChangeList.Count; i++)
                {
                    if (foodChangeList[i].ingredientType != ingredient) continue;
                    data = foodChangeList[i];
                    break;
                }
            }
            if (data == null)
            {
                return;
            }
            // change the food sprite
            var spriteRenderer = data.foodSprite;
            // change scale
            if (data.scale.Count > 0)
            {
                if (!_isChangeScale)
                {
                    spriteRenderer.transform.localScale = Vector3.one * data.scale[0];
                }
                // remove the first element
                data.scale.RemoveAt(0);
            }
            else
            {
                if (!_isChangeScale)
                {
                    spriteRenderer.transform.localScale = fillHolder ? Vector3.one : Vector3.zero;
                }
                // remove the data
                foodChangeList.Remove(data);
                OnIngredientDone?.Invoke(data);
            }
        }

        public void FadeFoodSprite(SpriteHolderData data, bool isFadeOut, float delay)
        {
            Color curColor = data.foodSprite.color;
            data.foodSprite.color = new Color(curColor.r, curColor.g, curColor.b, isFadeOut ? 0 : 1);
            data.foodSprite.DOFade(isFadeOut ? 0 : 1, 0.5f).SetDelay(delay);
        }
        public void ResetListFoods()
        {
            foodChangeList.Clear();
        }
        public void RemoveListFoodsByType(IngredientType type)
        {
            foodChangeList.RemoveAll(x => x.ingredientType == type);
        }
    }
    [Serializable]
    public class SpriteHolderData
    {
        public IngredientType ingredientType;
        public SpriteRenderer foodSprite;
        public List<float> scale;
    }
}
