using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class ItemOrder : MonoBehaviour
    {
        [SerializeField] private ItemType itemType;
        public ItemType ItemType => itemType;
    }
}
