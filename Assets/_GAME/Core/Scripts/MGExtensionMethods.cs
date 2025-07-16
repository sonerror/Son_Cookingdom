using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MGExtensionMethods
{
    public static void SetAlpha(this SpriteRenderer sr, float alpha)
    {
        var tmpColor = sr.color;
        tmpColor.a = alpha;
        sr.color = tmpColor;
    }
}
