using UnityEngine;

namespace sonnv
{

    public class SpoonTrigger : MonoBehaviour
    {
        [SerializeField] private ItemType spoonType;
        private TrayItem currentItem;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out TrayItem item)) return;
            if (item.ItemType != spoonType) return;

            if (currentItem == null)
            {
                currentItem = item;

                if (!currentItem.IsTrigger)
                {
                    item.OnSpoonTouch(this);
                    Debug.Log("trigger 2D");
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out TrayItem item)) return;

            if (currentItem == item)
                currentItem = null;
        }

        public TrayItem GetCurrentItem()
        {
            return currentItem;
        }
    }
}