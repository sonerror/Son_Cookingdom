using System;
using UnityEngine;

namespace sonnv
{
    public interface IItemCooking
    {
        bool IsCanCooking { get; }

        void Cooking(float time, Action action = null);
    }
}

