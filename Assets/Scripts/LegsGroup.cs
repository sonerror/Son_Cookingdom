using System.Collections;
using System.Collections.Generic;
using AnhPD.KingCrab;
using UnityEngine;

public class LegsGroup : MonoBehaviour
{
    public List<CrabLeg> legs = new List<CrabLeg>();

    public Vector3 getPosition()
    {
        foreach (CrabLeg leg in legs)
        {
            if (leg.gameObject.activeSelf)
            {
                return leg.transform.position;
            }
        }

        return Vector3.zero;
    }
}
