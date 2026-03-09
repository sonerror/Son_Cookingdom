using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMember : GameUnit
{
    private PoolType poolType;
    public PoolType PoolType
    {
        get => poolType;
        private set => poolType = value;
    }
}
