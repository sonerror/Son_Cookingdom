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
          // .SetEase(Ease.InOutBack)
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
    public static void MoveYLiner(this Transform tfMove, float y, float duration = 1f, bool isDisable = true, float delay = 0f, Action action = null)
    {
      tfMove.gameObject.SetActive(true);

      tfMove.DOLocalMoveY(tfMove.localPosition.y + y, duration)
          .SetEase(Ease.Linear)
          .SetDelay(delay)
          .OnComplete(() =>
          {
            tfMove.gameObject.SetActive(!isDisable);
            action?.Invoke();
          });
    }
    public static void Appear(this Transform tfAppear, float strength = .1f)
    {
      tfAppear.gameObject.SetActive(true);

      tfAppear.DOPunchScale(Vector3.up * strength, 0.3f);
    }
    public static void FallAppear(this Transform tfAppear, float strength = .1f, float height = .5f, float duration = .2f, Action action = null, float delay = 0f)
    {
      tfAppear.position += Vector3.up * height;
      tfAppear.DOMove(tfAppear.position - Vector3.up * height, duration).SetDelay(delay).OnStart(() =>
      {
        tfAppear.gameObject.SetActive(true);
      });
      tfAppear.DOPunchScale(Vector3.up * strength, 0.3f).SetDelay(duration / 2 + delay).OnComplete(() =>
      {
        action?.Invoke();
      });
    }
    public static void FallAppearLocal(this Transform tfAppear, float strength = .1f, float height = 1f, float duration = .2f, Action action = null, float delay = 0f)
    {
      tfAppear.localPosition += Vector3.up * height;
      tfAppear.DOLocalMove(tfAppear.localPosition - Vector3.up * height, duration).SetDelay(delay).OnStart(() =>
      {
        tfAppear.gameObject.SetActive(true);
      });
      tfAppear.DOPunchScale(Vector3.up * strength, 0.3f).SetDelay(duration / 2 + delay).OnComplete(() =>
      {
        action?.Invoke();
      });
    }
    public static void TossAppear(this Transform tfAppear, float strength = .1f, float height = .5f, float duration = .2f, Action action = null, float delay = 0f)
    {
      tfAppear.gameObject.SetActive(true);
      tfAppear.DOMove(tfAppear.position + Vector3.up * height, duration / 2);
      tfAppear.DOMove(tfAppear.position, duration / 2).SetDelay(delay + duration / 2);
      tfAppear.DOPunchScale(Vector3.up * strength, 0.3f).SetDelay(duration + delay).OnComplete(() =>
      {
        action?.Invoke();
      });
    }
    public static void Floating(this Transform tf, float height = -.01f, float duration = 2f)
    {
      float y = tf.position.y;
      Sequence anim = DOTween.Sequence();
      anim.SetTarget(tf);
      anim.Append(tf.DOMoveY(y + UnityEngine.Random.Range(0.5f, 1f) * height, duration / 2).SetEase(Ease.InOutSine));
      anim.Append(tf.DOMoveY(y, duration / 2).SetEase(Ease.InOutSine));
      anim.SetLoops(-1);
    }
    public static Transform GetNearestTranform(Transform tf, List<Transform> list)
    {
      if (list.Count < 1) return null;
      int index = 0;
      float minDistance = Vector2.Distance(tf.position, list[0].position);
      for (int i = 1; i < list.Count; i++)
      {
        float distance = Vector2.Distance(tf.position, list[i].position);
        if (distance < minDistance)
        {
          minDistance = distance;
          index = i;
        }
      }
      return list[index];
    }
    public static int GetNearestTranformIndex(Transform tf, List<Transform> list)
    {
      if (list.Count < 1) return -1;
      int index = 0;
      float minDistance = Vector2.Distance(tf.position, list[0].position);
      for (int i = 1; i < list.Count; i++)
      {
        float distance = Vector2.Distance(tf.position, list[i].position);
        if (distance < minDistance)
        {
          minDistance = distance;
          index = i;
        }
      }
      return index;
    }

    public static void ReplaceByTarget(this Transform tf, Transform target, float duration = .3f, Action action = null)
    {
      tf.DOMove(target.position, duration);
      tf.DORotate(target.eulerAngles, duration);
      tf.DOScale(target.localScale, duration).OnComplete(() =>
      {
        action?.Invoke();
        tf.gameObject.SetActive(false);
        target.gameObject.SetActive(true);
      });
    }
  }
}

