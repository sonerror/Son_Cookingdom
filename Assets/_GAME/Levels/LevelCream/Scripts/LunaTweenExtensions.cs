using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace sonnv
{
    public static class LunaTweenExtensions
    {
        public static void DOColorLuna(this MaskableGraphic graphic, Color targetColor, float duration)
        {
            MonoBehaviour runner = graphic.GetComponent<MonoBehaviour>();
            if (runner == null) return;
            runner.StopAllCoroutines();
            runner.StartCoroutine(ColorRoutine(graphic, targetColor, duration));
        }
        private static IEnumerator ColorRoutine(MaskableGraphic graphic, Color target, float duration)
        {
            Color startColor = graphic.color;
            float elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float smoothT = t * t * (3f - 2f * t);
                graphic.color = Color.Lerp(startColor, target, smoothT);
                yield return null;
            }
            graphic.color = target;
        }
    }
}