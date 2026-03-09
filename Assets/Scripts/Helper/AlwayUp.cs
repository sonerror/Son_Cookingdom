using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD.FishGrilled
{
    public class AlwayUp : MonoBehaviour
    {
        private void Update()
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}

