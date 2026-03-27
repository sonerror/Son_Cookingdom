using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class SortingOrderController : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> listSpriteRenderer;
        private int[] originalSortingOrders;
        private bool isInitialized = false;

        private void Awake()
        {
            Initialize();
        }
        public void Initialize()
        {
            if (listSpriteRenderer == null || listSpriteRenderer.Count == 0) return;
            originalSortingOrders = new int[listSpriteRenderer.Count];
            for (int i = 0; i < listSpriteRenderer.Count; i++)
            {
                if (listSpriteRenderer[i] != null)
                {
                    originalSortingOrders[i] = listSpriteRenderer[i].sortingOrder;
                }
            }
            isInitialized = true;
        }
        public void IncreaseSortingOrder(int offset)
        {
            if (!isInitialized) Initialize();

            for (int i = 0; i < listSpriteRenderer.Count; i++)
            {
                if (listSpriteRenderer[i] != null)
                {
                    listSpriteRenderer[i].sortingOrder = originalSortingOrders[i] + offset;
                }
            }
        }
        public void ResetSortingOrder()
        {
            if (!isInitialized) return;

            for (int i = 0; i < listSpriteRenderer.Count; i++)
            {
                if (listSpriteRenderer[i] != null)
                {
                    listSpriteRenderer[i].sortingOrder = originalSortingOrders[i];
                }
            }
        }
    }

}