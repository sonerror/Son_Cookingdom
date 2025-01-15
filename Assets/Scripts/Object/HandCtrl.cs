using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HandCtrl : MonoBehaviour
{
    public Animator animator;
    private Vector3 pos1;
    private Vector3 pos2;

    private bool isShowHandState1 = false;
    public void ShowHandState1(Vector3 pos)
    {
        if (isShowHandState1) return;
        isShowHandState1 = true;
        animator.gameObject.SetActive(true);
        animator.Play("Hand");
        transform.position = pos;
    }

    public void HideHand()
    {
        animator.gameObject.SetActive(false);
    }

    public void setHandPlayBox(Vector3 pos)
    {
        animator.gameObject.SetActive(true);
        animator.Play("Hand");
        transform.position = pos;
    }

    public void ShowHandPosToPos(Vector3 pos1, Vector3 pos2)
    {
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
        yield return Cache.GetWFS(3f);
        if (animator.gameObject.activeSelf)
            ShowHand();
    }
}
