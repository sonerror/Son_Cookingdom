using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace sonnv
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
    }
    [Serializable]
    public struct AudioData
    {
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
    }

    public static class SonUtilities
    {
        public static Tween DelayedCallScaled(float delay, TweenCallback callback)
        {
            return DOVirtual.DelayedCall(delay, callback).SetUpdate(false);
        }
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

    }


}
