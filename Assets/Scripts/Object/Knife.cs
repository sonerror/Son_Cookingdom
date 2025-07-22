using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using sonnv;
using UnityEngine;

public class Knife : DragController
{
    public bool IsDone = false;
    [SerializeField] private ItemHolder itemTarget;
    [SerializeField] private Animator anim;

    protected override void ActionOnMouseUp()
    {
        if (enabled)
        {

            if (!Level630.haveKnife && itemTarget != null && itemTarget.IsOccupied)
            {
                if (Col.bounds.Intersects(itemTarget.Col.bounds))
                {
                    Level630.haveKnife = true;
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
        yield return Cache.GetWFS(0.25f);
        anim.SetTrigger("tomatocut");
        for (int i = 0; i < 3; i++)
        {
            PoolManager.Ins.Spawn(PoolType.SfxCut2, Tf.position, Quaternion.identity);
            yield return Cache.GetWFS(0.15f);
        }
        yield return Cache.GetWFS(0.3f);
        yield return Cache.GetWFS(0.5f);
        Tf.DOMove(finishPoint.position + Vector3.right * 0.5f, 1f);
        var par = itemTarget.ItemMove.parState2;
        for (int i = 0; i < 5; i++)
        {
            anim.SetTrigger("cut");
            if (par != null) par.Play();
            PoolManager.Ins.Spawn(PoolType.SfxCut, Tf.position, Quaternion.identity);
            yield return Cache.GetWFS(0.2f);
        }
        yield return Cache.GetWFS(0.5f);
        StartRelease();
        itemTarget.DoneActionCut();
        Col.enabled = true;
        Level630.haveKnife = false;
    }


}
