using System.Collections;
using System.Collections.Generic;
using AnhPD.KingCrab;
using UnityEngine;

public class ItemCrabGroup : MonoBehaviour
{
    public List<CrabPart> itemInCrabs = new List<CrabPart>();

    public CrabPart getItemActive()
    {
        foreach (var item in itemInCrabs)
        {
            if (item.gameObject.activeSelf)
            {
                return item;
            }
        }

        return null;
    }

    public void resetMask()
    {
        foreach (var item in itemInCrabs)
        {
            item.gameObject.SetActive(false);
        }
    }
}
