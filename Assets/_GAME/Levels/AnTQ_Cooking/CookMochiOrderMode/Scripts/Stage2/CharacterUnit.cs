using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sonnv
{
    public abstract class CharacterUnit : MonoBehaviour
    {
        private Transform tf;
        public Transform TF
        {
            get
            {
                tf = tf ?? gameObject.transform;
                return tf;
            }
        }
    }
}
