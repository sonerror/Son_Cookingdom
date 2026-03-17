using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace sonnv
{
    [System.Serializable]
    public enum ItemType
    {
        None,
        Flour,

        Matcha,
        Strawberry,
        Chocolate,
        Mango,
        Bean,

        MochiMatcha,
        MochiStrawberry,
        MochiChocolate,
        MochiMango,
        MochiBean
    }
    [System.Serializable]
    public class MochiRecipe
    {
        public ItemOrder flour;
        public ItemOrder color;
        public ItemOrder result;
    }
    public class MixFlourManager : Singleton<MixFlourManager>
    {

        [SerializeField] private List<MochiRecipe> recipes;
        private MochiRecipe currentRecipe;
        [SerializeField] private List<TrayItem> listTrayItem;
        [SerializeField] private TrayItem trayFlour;
        [SerializeField] private ItemType itemTypeColor;
        private int recipeIndex = 0;


        [SerializeField] private List<InforTFTarget> listInforTarget = new List<InforTFTarget>();
        public InforTFTarget GetSnapItem()
        {
            for (int i = 0; i < listInforTarget.Count; i++)
            {
                if (listInforTarget[i] != null && listInforTarget[i].IsSnap)
                {
                    return listInforTarget[i];
                }
            }
            return null;
        }
        private void Awake()
        {
            CreateRandomOrder();
        }
        public void CreateRandomOrder()
        {
            if (recipes == null || recipes.Count == 0) return;

            HideAll();

            if (recipeIndex >= recipes.Count)
            {
                recipeIndex = 0;
            }

            currentRecipe = recipes[recipeIndex];
            recipeIndex++;
            currentRecipe.flour.gameObject.SetActive(true);
            currentRecipe.color.gameObject.SetActive(true);
            itemTypeColor = currentRecipe.color.ItemType;
            currentRecipe.result.gameObject.SetActive(true);
        }

        private void HideAll()
        {
            foreach (var recipe in recipes)
            {
                recipe.flour.gameObject.SetActive(false);
                recipe.color.gameObject.SetActive(false);
                recipe.result.gameObject.SetActive(false);
            }
        }

        public ItemOrder Mix(ItemOrder flour, ItemOrder color)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.flour.ItemType == flour.ItemType &&
                    recipe.color.ItemType == color.ItemType)
                {
                    return recipe.result;
                }
            }

            return null;
        }
        public void SetItemColorOrder()
        {
            ResetTrayItemByType(itemTypeColor);
        }
        private void ResetTrayItemByType(ItemType type)
        {
            foreach (var tray in listTrayItem)
            {
                if (tray.ItemType == type)
                {
                    tray.ResetTrigger(false);
                }
                else
                {
                    tray.ResetTrigger(true);
                }
            }
        }
        public bool CheckOrder(ItemOrder result)
        {
            return result.ItemType == currentRecipe.result.ItemType;
        }

        public void ResetAllState()
        {
            itemTypeColor = ItemType.None;
            ResetTrayItemByType(itemTypeColor);
            trayFlour.ResetTrigger(false);
            CreateRandomOrder();
        }
    }
}