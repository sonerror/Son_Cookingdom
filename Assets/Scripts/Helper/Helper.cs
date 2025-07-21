using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class Helper
{
    public static Tween DelayedCall(float delay, TweenCallback callback)
    {
        return DOTween.Sequence().AppendInterval(delay).OnStepComplete(callback)
            .SetUpdate(UpdateType.Normal, false)
            .SetAutoKill(autoKillOnCompletion: true);
    }
}
