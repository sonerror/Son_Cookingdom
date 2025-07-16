using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MGUtils : MonoBehaviour
{
    public static void KillTween(Tween twn)
    {
        if (twn != null && twn.IsActive()) twn.Kill();
    }
}
