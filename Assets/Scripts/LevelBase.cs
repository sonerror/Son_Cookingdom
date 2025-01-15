using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{

    protected virtual void Awake()
    {
        // LevelBase.instance = this;
    }

    protected virtual void Start()
    {
        // if (onBlockPlayerInteractChanged != null)
        // {
        //     onBlockPlayerInteractChanged();
        // }
    }
}
