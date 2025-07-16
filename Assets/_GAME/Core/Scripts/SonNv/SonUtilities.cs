using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace sonnv
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    [Serializable]
    public struct AudioData
    {
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
    }

    public static class SonUtilities
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

    }

}
