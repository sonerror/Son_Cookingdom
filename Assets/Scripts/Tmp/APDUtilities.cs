using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnhPD
{
    public static class APDUtilities
    {
        public static void MoveX(this Transform tfMove, float x, float duration = 1f, bool isDisable = true, float delay = 0f, Action action = null)
        {
            tfMove.gameObject.SetActive(true);
            tfMove.DOLocalMoveX(tfMove.localPosition.x + x, duration)
                .SetEase(Ease.InOutBack)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    tfMove.gameObject.SetActive(!isDisable);
                    action?.Invoke();
                });
        }
        public static void MoveY(this Transform tfMove, float y, float duration = 1f, bool isDisable = true, float delay = 0f, Action action = null)
        {
            tfMove.gameObject.SetActive(true);
            tfMove.DOLocalMoveY(tfMove.localPosition.y + y, duration)
                .SetEase(Ease.InOutBack)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    tfMove.gameObject.SetActive(!isDisable);
                    action?.Invoke();
                });
        }
    }
}

