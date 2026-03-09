using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public class SonLevelBase : LevelBase
    {
        private Camera cam;
        public Camera Camera => cam ? cam : cam = Camera.main;
    }

}
