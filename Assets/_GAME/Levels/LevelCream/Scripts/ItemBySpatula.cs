using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    [System.Serializable]
    public class ItemFruit
    {
        public SpriteRenderer spriteObject;
        public SpriteRenderer spriteEffect;
    }

    public class ItemBySpatula : MonoBehaviour
    {
        [Header("Items")]
        [SerializeField] private List<ItemFruit> _listItemFruit;
        public List<ItemFruit> ListItemFruit => _listItemFruit;
        [SerializeField] private BounceObjectEffect bounce;
        public void OnPlayEffect()
        {
            PlayEffect();
        }
        private void PlayEffect()
        {
            foreach (ItemFruit effect in _listItemFruit)
            {
                effect.spriteEffect.enabled = true;
                effect.spriteObject.enabled = false;

            }
            bounce.Bounce();
        }
    }
}