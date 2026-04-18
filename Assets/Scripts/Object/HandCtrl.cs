using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HandCtrl : MonoBehaviour
{
  public Animator animator;
  private Vector3 pos1;
  private Vector3 pos2;
  private Vector3 _pos3;


  public void ShowHandAtPos(Vector3 pos)
  {
    gameObject.SetActive(true);
    animator.Play("Hand");
    transform.position = pos;
  }

  public void HideHand()
  {
    // Debug.Log("HideHand");
    gameObject.SetActive(false);
    StopAllCoroutines();
    transform.DOKill();
  }

  public void ShowHandPosToPos(Vector3 pos1, Vector3 pos2)
  {
    StopAllCoroutines();
    transform.DOKill();
    gameObject.SetActive(true);
    this.pos1 = pos1;
    this.pos2 = pos2;

    ShowHand();
  }

  void ShowHand()
  {
    transform.position = pos1;
    animator.SetTrigger("HandDown");
    transform.DOMove(pos2, 1f).SetDelay(0.75f)
    .OnComplete(() =>
    {
      animator.SetTrigger("HandUp");
    });
    StartCoroutine(IEShowHand());
  }

  IEnumerator IEShowHand()
  {
    yield return new WaitForSeconds(3);
    if (gameObject.activeSelf)
      ShowHand();
  }
  public void ShowHandPosToPosToPos(Vector3 pos1, Vector3 pos2, Vector3 pos3)
  {
    StopAllCoroutines();
    transform.DOKill();
    gameObject.SetActive(true);

    this.pos1 = pos1;
    this.pos2 = pos2;
    _pos3 = pos3;

    ShowHandThree();
  }

  private void ShowHandThree()
  {
    transform.position = pos1;
    animator.SetTrigger("HandDown");

    transform.DOMove(pos2, 1f)
        .SetDelay(0.75f)
        .OnComplete(() =>
        {
          transform.DOMove(_pos3, 1f).OnComplete(() =>
              {
                animator.SetTrigger("HandUp");
              });
        });

    StartCoroutine(IEShowHandThree());
  }

  private IEnumerator IEShowHandThree()
  {
    yield return new WaitForSeconds(4);
    if (gameObject.activeSelf)
      ShowHandThree();
  }
  private Vector3 _centerPos;
  private float _radius;
  private Tween _spinTween;

  public void ShowHandSpinContinuous(Vector3 center, float radius)
  {
    gameObject.SetActive(true);
    _centerPos = center;
    _radius = radius;

    animator.Play("Hand");
    animator.SetTrigger("HandDown");

    _spinTween?.Kill();

    _spinTween = DOVirtual.Float(0f, 360f, 1.5f, (angle) =>
    {
      float rad = angle * Mathf.Deg2Rad;

      transform.position = _centerPos + new Vector3(Mathf.Cos(rad) * _radius, Mathf.Sin(rad) * _radius, 0);

    })
    .SetLoops(-1, LoopType.Restart)
    .SetEase(Ease.Linear);
  }

  public void StopSpinOnly()
  {
    _spinTween?.Kill();
  }
}
