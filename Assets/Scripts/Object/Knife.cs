using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sonnv;
using UnityEngine;

public class Knife : DragController, IItem
{
    public bool IsDone { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool IsActiveMove { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    [SerializeField] private ItemHolder itemTarget;

    public void OnActionEnd()
    {
        throw new System.NotImplementedException();
    }

    public void OnActionStart()
    {
        throw new System.NotImplementedException();
    }

    [SerializeField] private Animator anim;

    protected override void ActionOnMouseUp()
    {
        if (enabled)
        {

            if (itemTarget != null && itemTarget.IsOccupied)
            {
                if (Col.bounds.Intersects(itemTarget.Col.bounds))
                {
                    Col.enabled = false;
                    var time = Vector3.Distance(Tf.position, itemTarget.ItemMove.StartPoint.position + Vector3.right * 0.5f) / 3f;
                    Tf.DOMove(itemTarget.ItemMove.StartPoint.position + Vector3.right * 0.5f, time)
                        .OnComplete(() =>
                        {
                            itemTarget.ActionOnAddKnife(this);
                        });
                    _dragging = false;
                    return;
                }
            }
            OnDragStop();
            if (moveBackOnRelease)
            {
                Debug.Log("Move back on release");
                StartRelease();
            }

        }
        // TutorialManager.Ins.MouseUpItem();
    }

    public void PlayAnimCut(Transform startPoint, Transform finishPoint)
    {
        StartCoroutine(IE_PlayAnimCut(startPoint, finishPoint));
    }

    IEnumerator IE_PlayAnimCut(Transform startPoint, Transform finishPoint)
    {
        yield return Cache.GetWFS(0.5f);
        anim.SetTrigger("tomatocut");
        yield return Cache.GetWFS(0.75f);
        yield return Cache.GetWFS(1f);
        Tf.DOMove(finishPoint.position + Vector3.right * 0.5f, 1f);
        var par = itemTarget.ItemMove.parState2;
        for (int i = 0; i < 5; i++)
        {
            anim.SetTrigger("cut");
            par.Play();
            yield return Cache.GetWFS(0.2f);
        }
        yield return Cache.GetWFS(0.5f);
        StartRelease();
        itemTarget.DoneActionCut();
        Col.enabled = true;
    }


}
