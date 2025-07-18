using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilExtensions
{
    public static T Find<T>(this T[] items, Predicate<T> predicate)
    {
        return Array.Find(items, predicate);
    }
    public static int IndexOf<T>(this T[] array, T element)
    {
        int result = -1;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(element))
            {
                result = i;
                break;
            }
        }
        return result;
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

    private static IEnumerator DelayToDo(System.Action action, float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        action?.Invoke();
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T item in source)
        {
            action(item);
        }

        return source;
    }

    public static bool IsNullOrEmpty(this IEnumerable @this)
    {
        if (@this != null)
        {
            return !@this.GetEnumerator().MoveNext();
        }

        return true;
    }
}
