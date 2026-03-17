using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HandCtrl : GameUnit
{
  public Animator animator;
  private Vector3 pos1;
  private Vector3 pos2;

  private Vector3 localPosBodyHand;

  void Awake()
  {
    localPosBodyHand = animator.transform.localPosition;
  }
  private float distance = 1f;
  private Vector3[] paths = new Vector3[8];
  public void ShowHandArrow(Vector3 posRot, float fromAngle, float dist)
  {
    distance = dist;
    animator.gameObject.SetActive(true);

    for (int i = 0; i < paths.Length; i++)
    {
      paths[i] = new Vector3(
          Mathf.Cos(fromAngle + (i + 1) * Mathf.PI / 4) * distance + posRot.x,
          Mathf.Sin(fromAngle + (i + 1) * Mathf.PI / 4) * distance + posRot.y,
          posRot.z
      );
    }
    Tf.position = new Vector3(Mathf.Cos(fromAngle) * distance + posRot.x,
     Mathf.Sin(fromAngle) * distance + posRot.y, posRot.z);
    ShowHandArrowLoop();
  }

  private void ShowHandArrowLoop()
  {
    animator.SetTrigger("HandDown");
    transform.DOPath(paths, 1.5f, PathType.CatmullRom).SetLoops(2).SetDelay(0.75f).onComplete = () =>
    {
      animator.SetTrigger("HandUp");
    };
    StartCoroutine(IEShowHandRotate());
  }

  IEnumerator IEShowHandRotate()
  {
    yield return Cache.GetWFS(6f);
    if (animator.gameObject.activeSelf)
      ShowHandArrowLoop();
  }

  public void ShowHandAtPos(Vector3 pos)
  {
    animator.gameObject.SetActive(true);
    animator.Play("HandDown");
    transform.position = pos;
  }

  public void HideHand()
  {
    animator.gameObject.SetActive(false);
  }

  public void setHandPlayBox(Vector3 pos)
  {
    gameObject.SetActive(true);
    animator.gameObject.SetActive(true);
    animator.Play("Hand");
    transform.position = pos;
  }

  public void ShowHandPosToPos(Vector3 pos1, Vector3 pos2)
  {
    gameObject.SetActive(true);
    StopAllCoroutines();
    animator.gameObject.SetActive(true);
    this.pos1 = pos1;
    this.pos2 = pos2;

    ShowHand();
  }

  void ShowHand()
  {
    transform.position = pos1;
    animator.SetTrigger("HandDown");
    transform.DOMove(pos2, 1f).SetDelay(0.75f).onComplete = () =>
    {
      animator.SetTrigger("HandUp");
    };
    StartCoroutine(IEShowHand());
  }

  IEnumerator IEShowHand()
  {
    yield return new WaitForSeconds(3f);
    if (animator.gameObject.activeSelf)
      ShowHand();
  }
  public void ShowHandLoop(Vector3 pos1, Vector3 pos2)
  {
    gameObject.SetActive(true);
    StopAllCoroutines();
    animator.gameObject.SetActive(true);
    this.pos1 = pos1;
    this.pos2 = pos2;

    ShowHand();
  }
  void ShowHandLoop()
  {
    transform.position = pos1;
    animator.SetTrigger("HandDown");
    transform.DOMove(pos2, 1f).SetDelay(0.75f).onComplete = () =>
    {
      animator.SetTrigger("HandUp");
    };
    StartCoroutine(IEShowHand());
  }
  IEnumerator IEShowHandLoop()
  {
    yield return new WaitForSeconds(0.001f);
    if (animator.gameObject.activeSelf)
      ShowHandLoop();
  }

  private Coroutine circleRoutine;
  private float angle;

  public void ShowHandCircle(Vector3 center, float radius, float speed = 2f)
  {
    gameObject.SetActive(true);
    animator.gameObject.SetActive(true);
    Debug.Log("angle =============== " + angle);
    if (circleRoutine != null)
      StopCoroutine(circleRoutine);

    circleRoutine = StartCoroutine(CircleMove(center, radius, speed));
  }

  IEnumerator CircleMove(Vector3 center, float radius, float speed)
  {
    angle = 0;

    while (true)
    {
      angle += Time.deltaTime * speed;
      Debug.Log("angle " + angle);
      float x = Mathf.Cos(angle) * radius;
      float y = Mathf.Sin(angle) * radius;

      transform.position = center + new Vector3(x, y, 0);

      yield return null;
    }
  }

  public void StopHandCircle()
  {
    if (circleRoutine != null)
    {
      StopCoroutine(circleRoutine);
      circleRoutine = null;
    }

    animator.gameObject.SetActive(false);
  }



}
