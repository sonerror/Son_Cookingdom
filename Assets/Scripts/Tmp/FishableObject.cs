using System.Collections;
using System.Collections.Generic;
using TrungNV;
using UnityEngine;

namespace AnhPD.Fishing
{
    public class FishableObject : MonoBehaviour
    {
        public enum Type
        {
            None,
            Fish = 1,
            Garbage = 2,
        }
        public Type type;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Level110Hook hook = TrufnCache<Level110Hook>.GetCol2D(collision);
            if (hook != null && hook.IsCanCatch)
            {
                OnCatched();
                hook.OnCatched(this);
            }
        }
        public virtual void OnCatched()
        {
            // ObjectPoolDictArray.Instance.ReleaseGameObject(gameObject);
        }
    }
}

