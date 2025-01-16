using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInCrab : MonoBehaviour
{
    public Transform Tf;

    public Transform itemTarget;

    private void Awake()
    {
        Tf = transform;
    }
}
