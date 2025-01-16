using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropGroup : MonoBehaviour
{

    [SerializeField] private List<ItemDrop> itemDrops = new List<ItemDrop>();

    public Vector3 getPosition()
    {
        foreach (ItemDrop item in itemDrops)
        {
            if (item != null && item.gameObject.activeSelf)
            {
                return item.Tf.position;
            }
        }

        return Vector3.zero;
    }

    public ItemDrop getItemDropNotDrop()
    {
        foreach (ItemDrop item in itemDrops)
        {
            if (item != null && !item.isDrop)
            {
                return item;
            }
        }

        return null;
    }

}
