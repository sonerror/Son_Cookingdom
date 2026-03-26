using System;
using UnityEngine;

namespace sonnv
{
    public interface IItemCooking
    {
        public bool IsCanCooking { get; }

        public void Cooking(float time, Action action = null);
    }
}

