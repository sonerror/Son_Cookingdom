using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class InforTFTarget : SonMonoBehaviour
    {
        [SerializeField] private bool isSnap = false;
        public bool IsSnap => isSnap;
        public void ChangeIsSnap(bool value)
        {
            isSnap = value;
        }
    }
}
