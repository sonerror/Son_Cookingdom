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
            if (leg.isCut == false)
            {
                return leg.transform.position;
            }
        }

        return Vector3.zero;
    }
}
