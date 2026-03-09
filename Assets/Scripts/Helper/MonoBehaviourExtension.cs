using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
  public static class MonoBehaviourExtension
  {
    public static Coroutine WaitOneFrame(this MonoBehaviour monoBehaviour, System.Action action)
    {
      return monoBehaviour.StartCoroutine(DelayOneFrame(action));
    }
    private static IEnumerator DelayOneFrame(System.Action action)
    {
      yield return new WaitForEndOfFrame();
      action?.Invoke();
    }

    public static Coroutine WaitFrames(this MonoBehaviour monoBehaviour, System.Action action, int numFrames)
    {
      return monoBehaviour.StartCoroutine(DelayFrames(action, numFrames));
    }
    private static IEnumerator DelayFrames(System.Action action, int numFrames)
    {
      for (int i = 0; i < numFrames; i++)
      {
        yield return null;
      }
      action?.Invoke();
    }

    public static Coroutine WaitToDo(this MonoBehaviour monoBehaviour, System.Action action, float waitDuration)
    {
      if (waitDuration <= 0)
      {
        action?.Invoke();
        return null;
      }
      else
      {
        return monoBehaviour.StartCoroutine(DelayToDo(action, waitDuration));
      }
    }
    public static Coroutine WaitToDoUnscaled(this MonoBehaviour monoBehaviour, System.Action action, float waitDuration)
    {
      if (waitDuration <= 0)
      {
        action?.Invoke();
        return null;
      }
      else
      {
        return monoBehaviour.StartCoroutine(DelayToDoUnscaled(action, waitDuration));
      }
    }

    private static IEnumerator DelayToDo(System.Action action, float waitDuration)
    {
      yield return new WaitForSeconds(waitDuration);
      action?.Invoke();
    }
    private static IEnumerator DelayToDoUnscaled(System.Action action, float waitDuration)
    {
      yield return new WaitForSecondsRealtime(waitDuration);
      action?.Invoke();
    }

    public static Coroutine WaitToDoUntil(this MonoBehaviour monoBehaviour, System.Action action, System.Func<bool> predicate)
    {
      if (predicate())
      {
        action?.Invoke();
        return null;
      }
      else
      {
        return monoBehaviour.StartCoroutine(DelayToDoUntil(action, predicate));
      }
    }

    private static IEnumerator DelayToDoUntil(System.Action action, System.Func<bool> predicate)
    {
      yield return new WaitUntil(predicate);
      action?.Invoke();
    }
    public delegate string TextFormatDelegateInt(int value);
    public static Coroutine PlayTextTransitionUnscaledTime(this Text text, TextFormatDelegateInt textFormat, int start, int end, float duration, float delay = 0, System.Action onDone = null)
        => text.StartCoroutine(IEPlayTextTransitionUnscaledTime(text, textFormat, start, end, duration, delay, onDone));
    private static IEnumerator IEPlayTextTransitionUnscaledTime(Text text, TextFormatDelegateInt textFormat, int start, int end, float duration, float delay, System.Action onDone)
    {
      yield return new WaitForSecondsRealtime(delay);
      float time = 0f;
      while (time < duration)
      {
        time += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(time / duration);
        int value = Mathf.RoundToInt(Mathf.Lerp(start, end, t));
        text.text = textFormat(value);
        yield return null;
      }
      text.text = textFormat(end);
      onDone?.Invoke();
    }
  }
}