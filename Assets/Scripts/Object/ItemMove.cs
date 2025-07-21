using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sonnv;
using UnityEngine;

public class ItemMove : DragController, IItem
{
    private bool isDone = false;
    public bool IsDone { get => isDone; }
    [SerializeField] private ItemHolder itemTarget;
    public Vector3 GetTargetPosition => itemTarget ? itemTarget.Tf.position : Vector3.zero;

    public void OnActionEnd()
    {
        throw new System.NotImplementedException();
    }

    public void OnActionStart()
    {
        throw new System.NotImplementedException();
    }

    public bool isTofu = false;
    public ParticleSystem parState1;
    public ParticleSystem parState2;
    [SerializeField] GameObject State0;
    [SerializeField] GameObject State1;
    [SerializeField] GameObject State2;
    [SerializeField] GameObject State3;

    public Transform StartPoint;
    public Transform FinishPoint;

    [SerializeField] private Transform mask1;
    [SerializeField] private Transform mask2;

    protected override void ActionOnMouseUp()
    {
        if (enabled)
        {
            OnDragStop();
            if (itemTarget != null && !itemTarget.IsOccupied)
            {
                if (Col.bounds.Intersects(itemTarget.Col.bounds))
                {
                    Col.enabled = false;

                    itemTarget.AddItem(this);
                    var time = Vector3.Distance(Tf.position, itemTarget.Tf.position) / 3f;
                    Tf.DOMove(itemTarget.Tf.position, time)
                        .OnComplete(() =>
                        {
                            level.PlayEmojiHeart();
                            itemTarget.ActionOnAddItem();
                        });
                    return;
                }
            }

            if (moveBackOnRelease)
            {
                Debug.Log("Move back on release");
                StartRelease();
            }

        }
        // TutorialManager.Ins.MouseUpItem();
    }

    public void PlayAnimCut()
    {
        StartCoroutine(IE_PlayAnimCut());
    }

    IEnumerator IE_PlayAnimCut()
    {
        yield return Cache.GetWFS(0.25f);
        parState1.Play();
        yield return Cache.GetWFS(0.75f);
        State0.SetActive(false);
        State1.SetActive(true);
        yield return Cache.GetWFS(0.5f);
        State2.SetActive(true);
        mask1.DOMove(FinishPoint.position, 1f);
        mask2.DOMove(FinishPoint.position, 1f).OnComplete(() =>
        {
            State1.SetActive(false);
            mask1.gameObject.SetActive(false);
        });
        yield return Cache.GetWFS(1f);
        level.PlayEmojiHeart();
        yield return Cache.GetWFS(0.5f);
        StartRelease();

        if (isTofu)
        {
            yield return Cache.GetWFS(0.3f);
            State3.SetActive(true);
            State2.SetActive(false);
        }

        yield return Cache.GetWFS(0.5f);
        isDone = true;
        level.OnItemState0Done();
    }

}
